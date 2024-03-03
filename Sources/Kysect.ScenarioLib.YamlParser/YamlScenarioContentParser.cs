using Kysect.ScenarioLib.Abstractions;
using YamlDotNet.Serialization;

namespace Kysect.ScenarioLib.YamlParser;

public class YamlScenarioContentParser : IScenarioContentParser
{
    private readonly IDeserializer _deserializer;

    public YamlScenarioContentParser()
    {
        _deserializer = YamlDeserializerBuilderFactory.Create();
    }

    public IReadOnlyCollection<ScenarioStepArguments> Parse(string content)
    {
        List<ScenarioStepArguments> result = _deserializer
            .Deserialize<List<ScenarioStepArguments>>(content)
            .ToList();

        foreach (ScenarioStepArguments scenarioStepArguments in result)
        {
            // KB: it can be null
            if (scenarioStepArguments.Parameters is null)
                scenarioStepArguments.Parameters = new Dictionary<string, object>();
        }

        return result;
    }
}