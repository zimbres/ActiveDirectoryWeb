var builder = DistributedApplication.CreateBuilder(args);

builder.AddProject<Projects.ActiveDirectoryWebApi>("Api");

builder.AddProject<Projects.ActiveDirectoryWebFrontend>("Frontend");

builder.Build().Run();
