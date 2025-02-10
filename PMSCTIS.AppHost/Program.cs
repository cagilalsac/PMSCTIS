var builder = DistributedApplication.CreateBuilder(args);

builder.AddProject<Projects.API_Projects>("api-projects");

builder.Build().Run();
