using Kysect.CommonLib.BaseTypes.Extensions;
using Kysect.CommonLib.Reflection;
using Kysect.ScenarioLib.Abstractions;
using System.Reflection;
using System.Text.Json.Serialization;
using System.Text.Json;

namespace Kysect.ScenarioLib;

public class ScenarioContentStepReflectionDeserializer : IScenarioContentStepDeserializer
{
    private readonly ReflectionJsonInstanceCreator _instanceCreator;

    private readonly Dictionary<string, Type> _scenarioSteps;

    public ScenarioContentStepReflectionDeserializer(Dictionary<string, Type> scenarioSteps)
    {
        _scenarioSteps = scenarioSteps;
        var jsonSerializerOptions = new JsonSerializerOptions()
        {
            NumberHandling = JsonNumberHandling.AllowReadingFromString
        };

        jsonSerializerOptions.Converters.Add(BooleanConverter.Instance);
        _instanceCreator = new ReflectionJsonInstanceCreator(jsonSerializerOptions);
    }

    public static ScenarioContentStepReflectionDeserializer Create(params Assembly[] assemblies)
    {
        var attributeFinder = new ReflectionAttributeFinder();

        IReadOnlyCollection<Type> scenarioSteps = AssemblyReflectionTraverser.GetAllImplementationOf<IScenarioStep>(assemblies);

        var result = new Dictionary<string, Type>();

        foreach (Type scenarioStep in scenarioSteps)
        {
            var scenarioStepAttribute = attributeFinder.GetAttributeFromType<ScenarioStepAttribute>(scenarioStep);
            result.Add(scenarioStepAttribute.ScenarioName, scenarioStep);
        }

        return new ScenarioContentStepReflectionDeserializer(result);
    }

    public IScenarioStep ParseScenarioStep(ScenarioStepArguments arguments)
    {
        arguments.ThrowIfNull();

        if (!_scenarioSteps.TryGetValue(arguments.Name, out Type? handlerType))
            throw new ArgumentException("Cannot find scenario with name " + arguments.Name);

        object scenarioStep = _instanceCreator.Create(handlerType, arguments.Parameters);
        return scenarioStep.To<IScenarioStep>();
    }
}