namespace Kysect.ScenarioLib.Abstractions;

public interface IScenarioSourceProvider
{
    IReadOnlyCollection<string> GetScenarioNames();
    string GetScenarioSourceCode(string scenarioName);
}