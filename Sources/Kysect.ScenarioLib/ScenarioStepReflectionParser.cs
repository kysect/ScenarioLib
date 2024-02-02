using Kysect.CommonLib.BaseTypes.Extensions;
using Kysect.CommonLib.Reflection;
using Kysect.ScenarioLib.Abstractions;
using System.Reflection;
using System.Text.Json.Serialization;
using System.Text.Json;

namespace Kysect.ScenarioLib;

public class ScenarioStepReflectionParser : IScenarioStepParser
{
    private readonly ReflectionJsonInstanceCreator _instanceCreator;

    private readonly Dictionary<string, Type> _scenarioSteps;

    public ScenarioStepReflectionParser(Dictionary<string, Type> scenarioSteps)
    {
        _scenarioSteps = scenarioSteps;
        var jsonSerializerOptions = new JsonSerializerOptions()
        {
            NumberHandling = JsonNumberHandling.AllowReadingFromString
        };

        jsonSerializerOptions.Converters.Add(BooleanConverter.Instance);
        _instanceCreator = new ReflectionJsonInstanceCreator(jsonSerializerOptions);
    }

    public static ScenarioStepReflectionParser Create(params Assembly[] assemblies)
    {
        var attributeFinder = new ReflectionAttributeFinder();

        IReadOnlyCollection<Type> scenarioSteps = AssemblyReflectionTraverser.GetAllImplementationOf<IScenarioStep>(assemblies);

        var result = new Dictionary<string, Type>();

        foreach (Type scenarioStep in scenarioSteps)
        {
            var scenarioStepAttribute = attributeFinder.GetAttributeFromType<ScenarioStepAttribute>(scenarioStep);
            result.Add(scenarioStepAttribute.ScenarioName, scenarioStep);
        }

        return new ScenarioStepReflectionParser(result);
    }

    public IScenarioStep ParseScenarioStep(ScenarioStepArguments scenarioStepArguments)
    {
        scenarioStepArguments.ThrowIfNull();

        if (!_scenarioSteps.TryGetValue(scenarioStepArguments.Name, out Type? handlerType))
            throw new ArgumentException("Cannot find scenario with name " + scenarioStepArguments.Name);

        object scenarioStep = _instanceCreator.Create(handlerType, scenarioStepArguments.Parameters);
        return scenarioStep.To<IScenarioStep>();
    }
}