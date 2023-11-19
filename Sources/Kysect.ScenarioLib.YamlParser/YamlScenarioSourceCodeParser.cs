using Kysect.ScenarioLib.Abstractions;
using System.Collections.Generic;
using System.Linq;
using YamlDotNet.Serialization;

namespace Kysect.ScenarioLib.YamlParser;

public class YamlScenarioSourceCodeParser : IScenarioSourceCodeParser
{
    private readonly IDeserializer _deserializer;

    public YamlScenarioSourceCodeParser()
    {
        _deserializer = YamlDeserializerBuilderFactory.Create();
    }

    public IReadOnlyCollection<ScenarioStepArguments> Parse(string content)
    {
        var result = _deserializer
            .Deserialize<List<ScenarioStepArguments>>(content)
            .ToList();

        return result;
    }
}