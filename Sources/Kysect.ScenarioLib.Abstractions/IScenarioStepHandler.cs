namespace Kysect.ScenarioLib.Abstractions;

public interface IScenarioStepHandler
{
    void Handle(ScenarioContext scenarioContext, IScenarioStep scenarioStep);
}