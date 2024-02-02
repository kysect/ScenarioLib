using Kysect.CommonLib.BaseTypes.Extensions;
using Kysect.CommonLib.Reflection;
using Kysect.ScenarioLib.Abstractions;
using System.Reflection;

namespace Kysect.ScenarioLib;

public class ScenarioStepReflectionHandler : IScenarioStepHandler
{
    private static readonly Type ScenarioStepExecutorType = typeof(IScenarioStepExecutor<>);

    private readonly Dictionary<Type, ScenarioStepExecutorReflectionDecorator> _scenarioSteps;

    public static ScenarioStepReflectionHandler Create(IServiceProvider serviceProvider, params Assembly[] assemblies)
    {
        serviceProvider.ThrowIfNull();

        var executors = new Dictionary<Type, ScenarioStepExecutorReflectionDecorator>();

        foreach (Type type in AssemblyReflectionTraverser.GetAllImplementationOf(assemblies, ScenarioStepExecutorType))
        {
            Type? scenarioStepExecutorImplementation = AssemblyReflectionTraverser.FindInterfaceImplementationByGenericTypeDefinition(type, ScenarioStepExecutorType);
            if (scenarioStepExecutorImplementation == null)
                continue;

            object scenarioStepExecutor = serviceProvider.GetService(type);
            if (scenarioStepExecutor is null)
                throw new ArgumentException($"Scenario step executor for type {type.FullName} is not registered in service provider.");

            Type argumentType = scenarioStepExecutorImplementation.GetGenericArguments().Single();
            executors.Add(argumentType, new ScenarioStepExecutorReflectionDecorator(scenarioStepExecutor.To<IScenarioStepExecutor>()));
        }

        return new ScenarioStepReflectionHandler(executors);
    }

    public ScenarioStepReflectionHandler(Dictionary<Type, ScenarioStepExecutorReflectionDecorator> scenarioSteps)
    {
        _scenarioSteps = scenarioSteps;
    }

    public void Handle(ScenarioContext scenarioContext, IScenarioStep scenarioStep)
    {
        scenarioStep.ThrowIfNull();

        if (!_scenarioSteps.TryGetValue(scenarioStep.GetType(), out ScenarioStepExecutorReflectionDecorator? handler))
            throw new ArgumentException($"Cannot find handler for {scenarioStep.GetType().FullName}");

        handler.Execute(scenarioContext, scenarioStep);
    }
}