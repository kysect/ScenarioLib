using Kysect.ScenarioLib.Abstractions;

namespace Kysect.ScenarioLib;

public class ScenarioSourceProvider : IScenarioSourceProvider
{
    private readonly string _directory;

    public ScenarioSourceProvider(string directory)
    {
        _directory = directory;
    }

    public IReadOnlyCollection<string> GetScenarioNames()
    {
        return Directory
            .EnumerateFiles(_directory)
            .Select(f => new FileInfo(f).Name)
            .ToList();
    }

    public string GetScenarioSourceCode(string scenarioName)
    {
        string fullPath = Path.Combine(_directory, scenarioName);
        return File.ReadAllText(fullPath);
    }
}