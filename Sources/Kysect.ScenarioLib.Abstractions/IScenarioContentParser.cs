namespace Kysect.ScenarioLib.Abstractions;

public interface IScenarioContentParser
{
    IReadOnlyCollection<ScenarioStepArguments> Parse(string content);
}