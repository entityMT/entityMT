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
services.AddScoped<IDataTranscriptor<Pet>, PetWithTutorDataTranscriptor>();
services.AddScoped<IDataTranscriptor<PetMedicalHistory>, PetMedicalHistoryDataTranscriptor>();

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
    
    Console.WriteLine("Query: " + query?.Content);
    
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

    Console.WriteLine("Query: " + query?.Content);
    
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
    
    Console.WriteLine("Query: " + query?.Content);
    
    Console.ForegroundColor = ConsoleColor.White;
}
catch (Exception ex)
{
    Console.WriteLine("Error on tutors query with contains: " + ex.Message);
}

Console.WriteLine("Pet query with tutor without filters...");

var petTutorQueryBuilder = serviceProvider.GetService<IQueryBuilder<Pet>>();
var petQueryHandler = serviceProvider.GetService<IQueryHandler<Pet>>();

query = petTutorQueryBuilder?.Build();

try
{
    var pets = await petQueryHandler!.HandleAsync(query!, default);
    
    Console.ForegroundColor = ConsoleColor.DarkCyan;
    Console.WriteLine($"Id - Name - Years");
    
    foreach (var pet in pets)
    {
        Console.WriteLine($"{pet.Id} - {pet.Name} - {pet.Years}");
    }
    
    Console.WriteLine("Query: " + query?.Content);
    
    Console.ForegroundColor = ConsoleColor.White;
}
catch (Exception ex)
{
    Console.WriteLine("Error on pet with tutor query: " + ex.Message);
}

Console.WriteLine("Pet query with tutor and medical history (with filter anonymous class)...");

var petMedicalHistoryQueryBuilder = serviceProvider.GetService<IQueryBuilder<PetMedicalHistory>>();

var filter = new
{
    InitialDate = new DateTime(2023, 3, 1),
    FinalDate = DateTime.Now.Date
};

// Dont use 'new' inside the expressions because this afect performance of query translation.
petMedicalHistoryQueryBuilder?.SetFilter(mh => mh.AppointmentDate > filter.InitialDate);
petMedicalHistoryQueryBuilder?.SetFilter(mh => mh.AppointmentDate < filter.FinalDate);

query = petMedicalHistoryQueryBuilder?.Build();
var petMedicalHistoryQueryHandler = serviceProvider.GetService<IQueryHandler<PetMedicalHistory>>();

try
{
    var petMedicalHistory = await petMedicalHistoryQueryHandler!.HandleAsync(query!, default);
    
    Console.ForegroundColor = ConsoleColor.DarkCyan;
    Console.WriteLine($"Id - Pet Name - Tutor Name - Comments");
    
    foreach (var medicalHistory in petMedicalHistory)
        Console.WriteLine($"{medicalHistory.Id} - {medicalHistory.Pet.Name} - {medicalHistory.Pet.Tutor.Name} - {medicalHistory.Comments}");
    
    Console.WriteLine("Query: " + query?.Content);
    
    Console.ForegroundColor = ConsoleColor.White;
}
catch (Exception ex)
{
    Console.WriteLine("Error on pet medical history query: " + ex.Message);
}

Console.WriteLine("Pet query with tutor and medical history ordered by appointment date...");

petMedicalHistoryQueryBuilder?.Reset();
petMedicalHistoryQueryBuilder?.SetOrder(mh => mh.AppointmentDate);

query = petMedicalHistoryQueryBuilder?.Build();

try
{
    var petMedicalHistory = await petMedicalHistoryQueryHandler!.HandleAsync(query!, default);
    
    Console.ForegroundColor = ConsoleColor.DarkCyan;
    Console.WriteLine($"Id - Pet Name - Tutor Name - Comments");
    
    foreach (var medicalHistory in petMedicalHistory)
        Console.WriteLine($"{medicalHistory.Id} - {medicalHistory.Pet.Name} - {medicalHistory.Pet.Tutor.Name} - {medicalHistory.Comments}");
    
    Console.WriteLine("Query: " + query?.Content);
    
    Console.ForegroundColor = ConsoleColor.White;
}
catch (Exception ex)
{
    Console.WriteLine("Error on pet medical history ordered by appointment date query: " + ex.Message);
}
