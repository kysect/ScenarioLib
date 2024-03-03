using Kysect.CommonLib.Logging;
using Kysect.ScenarioLib.Abstractions;
using Microsoft.Extensions.Logging;

namespace Kysect.ScenarioLib;

public class ScenarioExecutor
{
    private readonly IScenarioContentProvider _contentProvider;
    private readonly IScenarioContentDeserializer _contentDeserializer;
    private readonly IScenarioStepHandler _scenarioStepHandler;
    private readonly ILogger _logger;

    public ScenarioExecutor(
        IScenarioContentProvider contentProvider,
        IScenarioStepHandler scenarioStepHandler,
        ILogger logger,
        IScenarioContentDeserializer scenarioContentDeserializer)
    {
        _contentProvider = contentProvider;
        _contentDeserializer = scenarioContentDeserializer;
        _scenarioStepHandler = scenarioStepHandler;
        _logger = logger;
    }

    public void Execute(string scenarioName)
    {
        _logger.LogInformation($"Run scenario {scenarioName}");

        string scenarioContent = _contentProvider.GetScenarioSourceCode(scenarioName);
        IReadOnlyCollection<IScenarioStep> steps = _contentDeserializer.Deserialize(scenarioContent);

        _logger.LogInformation("Steps:");
        foreach (IScenarioStep scenarioStep in steps)
            _logger.LogTabInformation(1, $"{scenarioStep.GetType().Name}");

        foreach (IScenarioStep scenarioStep in steps)
        {
            _logger.LogInformation($"Execute step {scenarioStep.GetType().Name}");
            _scenarioStepHandler.Handle(new ScenarioContext(), scenarioStep);
        }
    }
}