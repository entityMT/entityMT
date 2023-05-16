// See https://aka.ms/new-console-template for more information

using Microsoft.Extensions.DependencyInjection;
using MTQueries.Abstractions;
using MTQueries.SqlServer.NetCore;
using MtTenants.Abstractions;
using Queries.SqlServer.Customs;
using Queries.SqlServer.DataTranscriptors;
using Queries.SqlServer.Entities;

Console.WriteLine("********** Examples EntityMt Sql Server queries ORM modules **********");

// Injecting queries resources of entityMt...
Console.WriteLine("Injecting queries resources with 'AddSqlServerQueries()' entityMt extension...");

var services = new ServiceCollection();

services.AddSqlServerQueries();

// Adding demo tenant.
Console.WriteLine("Injecting DEMO tenant and connection string providers for ORM queries examples...");

services.AddScoped<ITenantProvider, DemoTenantProvider>();
services.AddScoped<IConnectionStringProvider, DemoConnectionStringProvider>();

Console.WriteLine("Injecting data transcriptors...");

// Data Transcriptors transforms data tables to objects.
services.AddScoped<IDataTranscriptor<Tutor>, TutorOnlyDataTranscriptor>();

Console.WriteLine("Building service provider...");

// Build service provider.
var serviceProvider = services.BuildServiceProvider();

// Initiating queries.

Console.WriteLine("Initiating queries...");

Console.WriteLine("Tutor query without any filter...");

var petQueryBuilder = serviceProvider.GetService<IQueryBuilder<Tutor>>();
var query = petQueryBuilder?.Build();
var queryHandler = serviceProvider.GetService<IQueryHandler<Tutor>>();

try
{
    var tutors = await queryHandler?.HandleAsync(query!, default)!;

    Console.ForegroundColor = ConsoleColor.DarkCyan;
    Console.WriteLine($"Id - Name - Document");
    
    foreach (var tutor in tutors)
    {
        Console.WriteLine($"{tutor.Id} - {tutor.Name} - {tutor.Document}");
    }
    
    Console.ForegroundColor = ConsoleColor.White;
}
catch (Exception ex)
{
    Console.WriteLine("Error in tutors only query: " + ex.Message);
}

Console.WriteLine("Tutor query with filter by id (resetting too)...");

// resetting.
petQueryBuilder!.Reset();

petQueryBuilder.SetFilter(t => t.Id == 2);
query = petQueryBuilder.Build();

try
{
    var tutor = (await queryHandler?.HandleAsync(query!, default)!).First();
    
    Console.ForegroundColor = ConsoleColor.DarkCyan;
    
    Console.WriteLine($"Id: {tutor.Id} - Name: {tutor.Name} - Document: {tutor.Document}");

    Console.ForegroundColor = ConsoleColor.White;
}
catch (Exception ex)
{
    Console.WriteLine("Error in tutor by id query: " + ex.Message);
}

Console.WriteLine("Tutor query with filter by name (contains)...");

// resetting.
petQueryBuilder!.Reset();

petQueryBuilder.SetFilter(t => t.Name.Contains("E"));
query = petQueryBuilder.Build();

try
{
    var tutors = await queryHandler?.HandleAsync(query!, default)!;
    
    Console.ForegroundColor = ConsoleColor.DarkCyan;
    Console.WriteLine($"Id - Name - Document");
    
    foreach (var tutor in tutors)
    {
        Console.WriteLine($"{tutor.Id} - {tutor.Name} - {tutor.Document}");
    }
    
    Console.ForegroundColor = ConsoleColor.White;
}
catch (Exception ex)
{
    Console.WriteLine("Error on tutors query with contains: " + ex.Message);
}