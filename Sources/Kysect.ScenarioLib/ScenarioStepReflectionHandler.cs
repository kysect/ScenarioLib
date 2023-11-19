using Kysect.CommonLib.BaseTypes.Extensions;
using Kysect.CommonLib.Reflection;
using Kysect.ScenarioLib.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Kysect.ScenarioLib;

public class ScenarioStepReflectionHandler : IScenarioStepHandler
{
    private readonly Dictionary<Type, IScenarioStepExecutor> _scenarioSteps;

    public static ScenarioStepReflectionHandler Create(IServiceProvider serviceProvider, params Assembly[] assemblies)
    {
        var executors = new Dictionary<Type, IScenarioStepExecutor>();

        foreach (Type type in AssemblyReflectionTraverser.GetAllImplementationOf(assemblies, typeof(IScenarioStepExecutor<>)))
        {
            Type? scenarioStepExecutorImplementation = AssemblyReflectionTraverser.FindInterfaceImplementationByGenericTypeDefinition(type, typeof(IScenarioStepExecutor<>));
            if (scenarioStepExecutorImplementation == null)
                continue;

            // TODO: GetRequired
            IScenarioStepExecutor handlerType = serviceProvider.GetService(type).To<IScenarioStepExecutor>();
            Type requestType = scenarioStepExecutorImplementation.GetGenericArguments().Single();
            executors.Add(requestType, handlerType);
        }

        return new ScenarioStepReflectionHandler(executors);
    }

    public ScenarioStepReflectionHandler(Dictionary<Type, IScenarioStepExecutor> scenarioSteps)
    {
        _scenarioSteps = scenarioSteps;
    }

    public void Handle(IScenarioStep scenarioStep)
    {
        if (!_scenarioSteps.TryGetValue(scenarioStep.GetType(), out IScenarioStepExecutor? handler))
            throw new ArgumentException($"Cannot find handler for {scenarioStep.GetType().FullName}");

        handler.ExecuteUnsafe(scenarioStep);
    }
}