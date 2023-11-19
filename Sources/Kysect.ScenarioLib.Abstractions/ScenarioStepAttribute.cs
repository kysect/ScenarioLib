using System;

namespace Kysect.ScenarioLib.Abstractions;

[AttributeUsage(AttributeTargets.Class)]
public sealed class ScenarioStepAttribute : Attribute
{
    public string ScenarioName { get; }

    public ScenarioStepAttribute(string scenarioName)
    {
        ScenarioName = scenarioName;
    }
}