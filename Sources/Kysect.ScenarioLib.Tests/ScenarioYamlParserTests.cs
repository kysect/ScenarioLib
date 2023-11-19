using Kysect.CommonLib.BaseTypes.Extensions;
using Kysect.ScenarioLib.Abstractions;
using Kysect.ScenarioLib.Tests.Mocks;
using Kysect.ScenarioLib.YamlParser;
using NUnit.Framework;
using YamlDotNet.Serialization;

namespace Kysect.ScenarioLib.Tests;

public class ScenarioYamlParserTests
{
    [Test]
    public void Scenario_yaml_should_deserialize_to_dictionary()
    {
        const string content = """
                               Repository.Create:
                                   Name: Repository name
                                   RepositoryPath: V:\
                               Repository.Remove:
                                   Name: Repository name
                               """;

        IDeserializer deserializer = YamlDeserializerBuilderFactory.Create();

        Dictionary<string, Dictionary<string, string>> result = deserializer.Deserialize<Dictionary<string, Dictionary<string, string>>>(content);

        Assert.IsNotNull(result);
        Assert.That(result.Count, Is.EqualTo(2));

        KeyValuePair<string, Dictionary<string, string>> firstElement = result.First();
        Assert.That(firstElement.Key, Is.EqualTo("Repository.Create"));
        Assert.That(firstElement.Value["Name"], Is.EqualTo("Repository name"));
        Assert.That(firstElement.Value["RepositoryPath"], Is.EqualTo(@"V:\"));
    }

    [Test]
    public void YamlScenarioSourceReader_should_deserialize_correctly()
    {
        const string content = """
                               - Name: Repository.Create
                                 Parameters:
                                    Name: Repository name
                                    RepositoryPath: V:\
                               
                               - Name: Repository.Remove
                                 Parameters:
                                    Name: Repository name
                               """;

        var yamlScenarioSourceReader = new YamlScenarioSourceCodeParser();

        IReadOnlyCollection<ScenarioStepArguments> nodes = yamlScenarioSourceReader.Parse(content);

        Assert.IsNotNull(nodes);
        Assert.That(nodes.Count, Is.EqualTo(2));

        var firstElement = nodes.First();
        Assert.That(firstElement.Name, Is.EqualTo("Repository.Create"));
        Assert.That(firstElement.Parameters["Name"], Is.EqualTo("Repository name"));
        Assert.That(firstElement.Parameters["RepositoryPath"], Is.EqualTo(@"V:\"));
    }

    [Test]
    public void ArgumentList_should_parsed_successfully()
    {
        const string content = """
                               Some.Action:
                                   Names: [first, second, third]
                                   RepositoryPath: V:\
                               """;

        IDeserializer deserializer = YamlDeserializerBuilderFactory.Create();
        var scenarioStepNameToTypeMapping = new Dictionary<string, Type>() { { "Some.Action", typeof(FakeScenarioNodeWithMultiArgument) } };
        var scenarioStepReflectionParser = new ScenarioStepReflectionParser(scenarioStepNameToTypeMapping);

        Dictionary<string, Dictionary<string, object>> result = deserializer.Deserialize<Dictionary<string, Dictionary<string, object>>>(content);
        KeyValuePair<string, Dictionary<string, object>> firstCommand = result.First();
        IScenarioStep scenarioStep = scenarioStepReflectionParser.ParseScenarioStep(new ScenarioStepArguments(firstCommand.Key, firstCommand.Value));

        Assert.IsNotNull(scenarioStep);
        Assert.That(scenarioStep, Is.TypeOf<FakeScenarioNodeWithMultiArgument>());
        Assert.That(scenarioStep.To<FakeScenarioNodeWithMultiArgument>().Names, Is.EqualTo(new[] { "first", "second", "third" }));
        Assert.That(scenarioStep.To<FakeScenarioNodeWithMultiArgument>().RepositoryPath, Is.EqualTo(@"V:\"));
    }
}