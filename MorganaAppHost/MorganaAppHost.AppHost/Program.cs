var builder = DistributedApplication.CreateBuilder(args);

var umbracoCms = builder.AddProject<Projects.UmbracoCMS>("umbraco-cms")
    .WithEnvironment("ASPNETCORE_ENVIRONMENT", "Development");

var umbracoBridge = builder.AddProject<Projects.UmbracoBridge>("umbraco-bridge")
    .WithEnvironment("ASPNETCORE_ENVIRONMENT", "Development")
    .WithReference(umbracoCms);

builder.Build().Run();
