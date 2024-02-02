namespace Kysect.ScenarioLib.Abstractions;

public class ScenarioStepArguments
{
    public string Name { get; set; }
#pragma warning disable CA2227 // Collection properties should be read only
    public Dictionary<string, object> Parameters { get; set; }
#pragma warning restore CA2227 // Collection properties should be read only

    public ScenarioStepArguments(string name, Dictionary<string, object> parameters)
    {
        Name = name;
        Parameters = parameters;
    }

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    public ScenarioStepArguments()
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    {
    }
}