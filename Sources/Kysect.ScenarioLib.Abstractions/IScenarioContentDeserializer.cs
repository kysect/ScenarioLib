namespace Kysect.ScenarioLib.Abstractions;

public interface IScenarioContentDeserializer
{
    IReadOnlyCollection<IScenarioStep> Deserialize(string content);
}