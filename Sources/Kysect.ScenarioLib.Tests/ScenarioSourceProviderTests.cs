using FluentAssertions;
using System.IO.Abstractions.TestingHelpers;

namespace Kysect.ScenarioLib.Tests;

public class ScenarioSourceProviderTests
{
    private readonly MockFileSystem _mockFileSystem;
    private readonly ScenarioSourceProvider _sourceProvider;

    public ScenarioSourceProviderTests()
    {
        _mockFileSystem = new MockFileSystem();
        string fullPath = _mockFileSystem.Path.GetFullPath(".");
        _sourceProvider = new ScenarioSourceProvider(_mockFileSystem, fullPath);
    }

    [Fact]
    public void GetScenarioNames_ReturnAllItems()
    {
        _mockFileSystem.AddFile("first.yaml", new MockFileData("Some text"));
        _mockFileSystem.AddFile("second.yaml", new MockFileData("Some text"));

        IReadOnlyCollection<string> scenarios = _sourceProvider.GetScenarioNames();

        scenarios.Should().BeEquivalentTo(["first.yaml", "second.yaml"]);
    }

    [Fact]
    public void GetScenarioSourceCode_ReturnContent()
    {
        _mockFileSystem.AddFile("first.yaml", new MockFileData("Some text"));

        string scenarioSourceCode = _sourceProvider.GetScenarioSourceCode("first.yaml");

        scenarioSourceCode.Should().Be("Some text");
    }
}