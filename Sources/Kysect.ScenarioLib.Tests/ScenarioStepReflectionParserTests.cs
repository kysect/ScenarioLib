using FluentAssertions;
using Kysect.CommonLib.BaseTypes.Extensions;
using Kysect.ScenarioLib.Abstractions;
using Kysect.ScenarioLib.Tests.Mocks;
using Kysect.ScenarioLib.Tests.Tools;
using NUnit.Framework;

namespace Kysect.ScenarioLib.Tests;

public class ScenarioStepReflectionParserTests
{
    [Test]
    public void Create_ForTestAssembly_ReturnInitializedInstance()
    {
        var scenarioStepReflectionParser = ScenarioStepReflectionParser.Create(TestConstants.CurrentAssembly);

        var scenarioStepArguments = new ScenarioStepArguments("First.Scenario", new Dictionary<string, object> { { "Name", "Value" } });
        IScenarioStep scenarioStep = scenarioStepReflectionParser.ParseScenarioStep(scenarioStepArguments);

        scenarioStep.Should().BeOfType<FirstScenarioStepHandler.Arguments>();
        scenarioStep.To<FirstScenarioStepHandler.Arguments>().Name.Should().Be("Value");
    }
}