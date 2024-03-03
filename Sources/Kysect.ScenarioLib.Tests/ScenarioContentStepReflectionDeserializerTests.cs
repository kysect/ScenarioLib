using FluentAssertions;
using Kysect.CommonLib.BaseTypes.Extensions;
using Kysect.ScenarioLib.Abstractions;
using Kysect.ScenarioLib.Tests.Mocks;
using Kysect.ScenarioLib.Tests.Tools;

namespace Kysect.ScenarioLib.Tests;

public class ScenarioContentStepReflectionDeserializerTests
{
    [Fact]
    public void Create_ForTestAssembly_ReturnInitializedInstance()
    {
        var scenarioStepReflectionParser = ScenarioContentStepReflectionDeserializer.Create(TestConstants.CurrentAssembly);

        var scenarioStepArguments = new ScenarioStepArguments("First.Scenario", new Dictionary<string, object> { { "Name", "Value" } });
        IScenarioStep scenarioStep = scenarioStepReflectionParser.ParseScenarioStep(scenarioStepArguments);

        scenarioStep.Should().BeOfType<FirstScenarioStepHandler.Arguments>();
        scenarioStep.To<FirstScenarioStepHandler.Arguments>().Name.Should().Be("Value");
    }
}