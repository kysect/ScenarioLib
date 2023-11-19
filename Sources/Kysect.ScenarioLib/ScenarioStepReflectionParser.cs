using Kysect.CommonLib.BaseTypes.Extensions;
using Kysect.CommonLib.Reflection;
using Kysect.ScenarioLib.Abstractions;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace Kysect.ScenarioLib;

public class ScenarioStepReflectionParser : IScenarioStepParser
{
    private readonly Dictionary<string, Type> _scenarioSteps;

    public ScenarioStepReflectionParser(Dictionary<string, Type> scenarioSteps)
    {
        _scenarioSteps = scenarioSteps;
    }

    public static ScenarioStepReflectionParser Create(params Assembly[] assemblies)
    {
        var attributeFinder = new ReflectionAttributeFinder();

        IReadOnlyCollection<Type> scenarioSteps = AssemblyReflectionTraverser.GetAllImplementationOf<IScenarioStep>(assemblies);

        var result = new Dictionary<string, Type>();

        foreach (Type scenarioStep in scenarioSteps)
        {
            ScenarioStepAttribute scenarioStepAttribute = attributeFinder.GetAttributeFromType<ScenarioStepAttribute>(scenarioStep);
            result.Add(scenarioStepAttribute.ScenarioName, scenarioStep);
        }

        return new ScenarioStepReflectionParser(result);
    }

    public IScenarioStep ParseScenarioStep(ScenarioStepArguments scenarioStepArguments)
    {
        if (!_scenarioSteps.TryGetValue(scenarioStepArguments.Name, out Type? handlerType))
            throw new ArgumentException("Cannot find scenario with name " + scenarioStepArguments.Name);

        object scenarioStep = ReflectionJsonInstanceCreator.Create().Create(handlerType, scenarioStepArguments.Parameters);
        return scenarioStep.To<IScenarioStep>();
    }
}