using Kysect.ScenarioLib.Abstractions;

namespace Kysect.ScenarioLib;

public class ScenarioContentDeserializer(IScenarioContentParser contentParser, IScenarioContentStepDeserializer stepDeserializer) : IScenarioContentDeserializer
{
    public IReadOnlyCollection<IScenarioStep> Deserialize(string content)
    {
        IReadOnlyCollection<ScenarioStepArguments> arguments = contentParser.Parse(content);
        return arguments
            .Select(stepDeserializer.ParseScenarioStep)
            .ToList();
    }
}