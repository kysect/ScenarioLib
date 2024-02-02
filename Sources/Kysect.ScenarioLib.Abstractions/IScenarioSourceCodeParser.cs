namespace Kysect.ScenarioLib.Abstractions;

public interface IScenarioSourceCodeParser
{
    IReadOnlyCollection<ScenarioStepArguments> Parse(string content);
}