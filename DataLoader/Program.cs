using Data;
using Data.Hash;
using DataLoader;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;

var builder = new ConfigurationBuilder();

BuildConfig(builder);

Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Build())
    .Enrich.FromLogContext()
    .WriteTo.Console()
    .CreateLogger();

Log.Logger.Information("Application Starting");

var host = Host.CreateDefaultBuilder()
    .ConfigureServices((context, services) =>
    {
        services.AddSingleton<IHashingHelper, HashingHelper>();
        services.AddTransient<IRepository, SQlPhoneBookRepository>();
        services.AddTransient<IPhoneBookProcessor, PhoneBookProcessor>();    
    })
    .UseSerilog()
    .Build();

ApiHelper.InitializeClient();

var phoneBookProcessor = ActivatorUtilities.CreateInstance<PhoneBookProcessor>(host.Services); 

await phoneBookProcessor.LoadPhoneBook();

Log.Logger.Information("Application Stopped");

static void BuildConfig(IConfigurationBuilder builder)
{
    builder.SetBasePath(Directory.GetCurrentDirectory())
        .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
        .AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIROMENT") ?? "Production"}.json"
        , optional: true)
        .AddEnvironmentVariables();
}