namespace Kysect.ScenarioLib.Abstractions;

public interface IScenarioContentProvider
{
    IReadOnlyCollection<string> GetScenarioNames();
    string GetScenarioSourceCode(string scenarioName);
}