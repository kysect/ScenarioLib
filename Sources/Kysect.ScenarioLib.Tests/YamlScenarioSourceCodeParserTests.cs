using FluentAssertions;
using Kysect.CommonLib.BaseTypes.Extensions;
using Kysect.ScenarioLib.Abstractions;
using Kysect.ScenarioLib.Tests.Mocks;
using Kysect.ScenarioLib.Tests.Tools;
using Kysect.ScenarioLib.YamlParser;

namespace Kysect.ScenarioLib.Tests;

public class YamlScenarioSourceCodeParserTests
{
    private readonly YamlScenarioSourceCodeParser _yamlScenarioSourceReader;
    private readonly IScenarioStepParser _scenarioStepParser;

    public YamlScenarioSourceCodeParserTests()
    {
        _yamlScenarioSourceReader = new YamlScenarioSourceCodeParser();
        _scenarioStepParser = ScenarioStepReflectionParser.Create(TestConstants.CurrentAssembly);
    }

    [Fact]
    public void Parse_ForScenarioWithTwoStep_ReturnBothStep()
    {
        const string content = """
                               - Name: First.Scenario
                                 Parameters:
                                    Name: Some name
                               - Name: Second.Scenario
                                 Parameters:
                                    Name: Other name
                               """;

        IReadOnlyCollection<ScenarioStepArguments> scenarioStepArguments = _yamlScenarioSourceReader.Parse(content);
        IReadOnlyCollection<IScenarioStep> steps = scenarioStepArguments.Select(_scenarioStepParser.ParseScenarioStep).ToList();

        steps.Should().HaveCount(2);
        steps.ElementAt(0).Should().BeOfType<FirstScenarioStepHandler.Arguments>();
        steps.ElementAt(0).To<FirstScenarioStepHandler.Arguments>().Name.Should().Be("Some name");
        steps.ElementAt(1).Should().BeOfType<SecondScenarioStepHandler.Arguments>();
        steps.ElementAt(1).To<SecondScenarioStepHandler.Arguments>().Name.Should().Be("Other name");
    }

    [Fact]
    public void Parse_ArgumentWithArray_ParseArray()
    {
        const string content = """
                               - Name: Scenario.WithArray
                                 Parameters:
                                    Values: ["first", "second"]
                               """;

        IReadOnlyCollection<ScenarioStepArguments> scenarioStepArguments = _yamlScenarioSourceReader.Parse(content);
        IReadOnlyCollection<IScenarioStep> steps = scenarioStepArguments.Select(_scenarioStepParser.ParseScenarioStep).ToList();

        steps.Should().HaveCount(1);
        steps.ElementAt(0).Should().BeOfType<ScenarioWithArrayStepHandler.Arguments>();
        steps.ElementAt(0).To<ScenarioWithArrayStepHandler.Arguments>().Values.Should().BeEquivalentTo("first", "second");
    }

    [Fact]
    public void Parse_BoolArgument_ParseWithoutError()
    {
        const string content = """
                               - Name: Scenario.WithBool
                                 Parameters:
                                    Value: true
                               """;

        IReadOnlyCollection<ScenarioStepArguments> scenarioStepArguments = _yamlScenarioSourceReader.Parse(content);
        IReadOnlyCollection<IScenarioStep> steps = scenarioStepArguments.Select(_scenarioStepParser.ParseScenarioStep).ToList();

        steps.Should().HaveCount(1);
        steps.ElementAt(0).Should().BeOfType<ScenarioWithBoolStepHandler.Arguments>();
        steps.ElementAt(0).To<ScenarioWithBoolStepHandler.Arguments>().Value.Should().BeTrue();
    }

    [Fact]
    public void Parse_WithoutArguments_ParseWithoutError()
    {
        const string content = """
                               - Name: Scenario.WithoutArguments
                                 Parameters:
                               """;

        IReadOnlyCollection<ScenarioStepArguments> scenarioStepArguments = _yamlScenarioSourceReader.Parse(content);
        IReadOnlyCollection<IScenarioStep> steps = scenarioStepArguments.Select(_scenarioStepParser.ParseScenarioStep).ToList();

        steps.Should().HaveCount(1);
        steps.ElementAt(0).Should().BeOfType<ScenarioWithoutArgumentsStepHandler.Arguments>();
        steps.ElementAt(0).To<ScenarioWithoutArgumentsStepHandler.Arguments>().Should().NotBeNull();
    }

    [Fact]
    public void Parse_WithDictionaryArguments_ParseWithoutError()
    {
        const string content = """
                               - Name: Scenario.WithDictionaryArguments
                                 Parameters:
                                   Values:
                                     Key: Value
                               """;

        IReadOnlyCollection<ScenarioStepArguments> scenarioStepArguments = _yamlScenarioSourceReader.Parse(content);
        IReadOnlyCollection<IScenarioStep> steps = scenarioStepArguments.Select(_scenarioStepParser.ParseScenarioStep).ToList();

        steps.Should().HaveCount(1);
        steps.ElementAt(0).Should().BeOfType<ScenarioWithDictionaryArgumentsStepHandler.Arguments>();
        Dictionary<string, string> values = steps.ElementAt(0).To<ScenarioWithDictionaryArgumentsStepHandler.Arguments>().Values;
        values.Should().HaveCount(1);
        values.Single().Should().Be(new KeyValuePair<string, string>("Key", "Value"));
    }
}