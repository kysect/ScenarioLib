# Kysect.ScenarioLib

Kysect.ScenarioLib is a NuGet library for parsing of YAML files containing descriptions of specific steps and the execution of these steps. The primary purpose of this library is to streamline the process of defining and running scenarios within an application.

Key features of Kysect.ScenarioLib include:

- YAML Parsing: The library includes robust YAML parsing capabilities, allowing developers to define scenarios in a human-readable format. This is particularly useful for creating structured and easily maintainable descriptions of various steps in a workflow.

- Scenario Definition: With Kysect.ScenarioLib, users can define scenarios by specifying a sequence of steps in a YAML file. Each step is outlined in the YAML file, providing a clear and organized representation of the desired workflow.

- Step Execution: The library executes the steps defined in the YAML file, automating the process and reducing the need for manual intervention. This feature is especially beneficial for automating repetitive tasks or complex workflows.

- Customization: Kysect.ScenarioLib is designed to be flexible and customizable. Developers can extend the library to incorporate custom step types or behaviors, allowing for tailored solutions that meet specific project requirements.

## How to use

### How to define scenario steps

Scenario step arguments definition:

```csharp
[ScenarioStep("Environment.BuildSolution")]
public record EnvironmentBuildSolution(string SolutionPath) : IScenarioStep;
```

- ScenarioStepAttribute and IScenarioStep implementation mark type for parsing as ScenarioStep
- ScenarioStepAttribute allow to define user-friendly name
- Type properties allow to define input arguments

Scenario step execution logic definition:

```csharp
public class EnvironmentBuildSolutionExecutor : IScenarioStepExecutor<EnvironmentBuildSolution>
{
    // fields that can be assigned via DI

    public EnvironmentBuildSolutionExecutor(/* arguments */)
    {
        // field initialization
    }

    public void Execute(EnvironmentBuildSolution request)
    {
        _logger.LogDebug("Building solution {name}", request.SolutionPath);
        _msbuild.BuildSolution(request.SolutionPath);
    }
}
```

Scenario file definition:

```yaml
- Name: Git.Pull
  Parameters:
    Path: C:\path\to\repository

- Name: Environment.BuildSolution
  Parameters:
    SolutionPath: $/Backup/Development/9.5-Updates
```

### Depencency injection container configuration

Configuration sample for `Microsoft.Extensions.DependencyInjection`:

```csharp
public static IServiceCollection AddCloudextScenarioExecutionServices(this IServiceCollection serviceCollection)
{
    const string pathToDirectoryWithScenarions = @"C:\scenarios";
    Assembly[] assemblies = new[]
    {
        // Definition of assembly with IScenarioStep implemenration
        // Definition of assembly with IScenarioStepExecutor implemenration
    };

    return serviceCollection
        .AddSingleton<IScenarioSourceProvider>(sp => new ScenarioSourceProvider(pathToDirectoryWithScenarions))
        .AddSingleton<IScenarioSourceCodeParser, YamlScenarioSourceCodeParser>()
        .AddSingleton<IScenarioStepParser, ScenarioStepReflectionParser>(_ => ScenarioStepReflectionParser.Create(assemblies))
        .AddAllImplementationOf<IScenarioStepExecutor>(assemblies)
        .AddSingleton<IScenarioStepHandler, ScenarioStepReflectionHandler>(sp => ScenarioStepReflectionHandler.Create(sp, assemblies))
        .AddSingleton<ScenarioExecutor>();
}
```
