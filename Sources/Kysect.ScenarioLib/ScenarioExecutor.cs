using Kysect.CommonLib.Logging;
using Kysect.ScenarioLib.Abstractions;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;

namespace Kysect.ScenarioLib;

public class ScenarioExecutor
{
    private readonly IScenarioSourceProvider _scenarioProvider;
    private readonly IScenarioSourceCodeParser _scenarioSourceCodeParser;
    private readonly IScenarioStepParser _scenarioStepParser;
    private readonly IScenarioStepHandler _scenarioStepHandler;
    private readonly ILogger _logger;

    public ScenarioExecutor(
        IScenarioSourceProvider sourceProvider,
        IScenarioSourceCodeParser sourceCodeParser,
        IScenarioStepParser stepParser,
        IScenarioStepHandler scenarioStepHandler,
        ILogger logger)
    {
        _scenarioProvider = sourceProvider;
        _scenarioSourceCodeParser = sourceCodeParser;
        _scenarioStepParser = stepParser;
        _logger = logger;
        _scenarioStepHandler = scenarioStepHandler;
    }

    public void Execute(string scenarioName)
    {
        _logger.LogInformation($"Run scenario {scenarioName}");

        string scenarioContent = _scenarioProvider.GetScenarioSourceCode(scenarioName);
        IReadOnlyCollection<ScenarioStepArguments> scenarioStepNodes = _scenarioSourceCodeParser.Parse(scenarioContent);
        IReadOnlyCollection<IScenarioStep> steps = scenarioStepNodes.Select(_scenarioStepParser.ParseScenarioStep).ToList();

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