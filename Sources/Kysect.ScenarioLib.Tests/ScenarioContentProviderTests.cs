using FluentAssertions;
using System.IO.Abstractions.TestingHelpers;

namespace Kysect.ScenarioLib.Tests;

public class ScenarioContentProviderTests
{
    private readonly MockFileSystem _mockFileSystem;
    private readonly ScenarioContentProvider _contentProvider;

    public ScenarioContentProviderTests()
    {
        _mockFileSystem = new MockFileSystem();
        string fullPath = _mockFileSystem.Path.GetFullPath(".");
        _contentProvider = new ScenarioContentProvider(_mockFileSystem, fullPath);
    }

    [Fact]
    public void GetScenarioNames_ReturnAllItems()
    {
        _mockFileSystem.AddFile("first.yaml", new MockFileData("Some text"));
        _mockFileSystem.AddFile("second.yaml", new MockFileData("Some text"));

        IReadOnlyCollection<string> scenarios = _contentProvider.GetScenarioNames();

        scenarios.Should().BeEquivalentTo(["first.yaml", "second.yaml"]);
    }

    [Fact]
    public void GetScenarioSourceCode_ReturnContent()
    {
        _mockFileSystem.AddFile("first.yaml", new MockFileData("Some text"));

        string scenarioSourceCode = _contentProvider.GetScenarioSourceCode("first.yaml");

        scenarioSourceCode.Should().Be("Some text");
    }
}