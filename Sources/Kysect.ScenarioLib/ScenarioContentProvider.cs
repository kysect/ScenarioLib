using Kysect.ScenarioLib.Abstractions;
using System.IO.Abstractions;

namespace Kysect.ScenarioLib;

public class ScenarioContentProvider : IScenarioContentProvider
{
    private readonly IFileSystem _fileSystem;
    private readonly string _directory;

    public ScenarioContentProvider(IFileSystem fileSystem, string directory)
    {
        _fileSystem = fileSystem;
        _directory = directory;
    }

    public IReadOnlyCollection<string> GetScenarioNames()
    {
        return _fileSystem
            .Directory
            .EnumerateFiles(_directory)
            .Select(f => _fileSystem.FileInfo.New(f).Name)
            .ToList();
    }

    public string GetScenarioSourceCode(string scenarioName)
    {
        string fullPath = _fileSystem.Path.Combine(_directory, scenarioName);
        return _fileSystem.File.ReadAllText(fullPath);
    }
}