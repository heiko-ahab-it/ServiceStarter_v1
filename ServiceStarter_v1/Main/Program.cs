using ConfigLoader_v2;
using Microsoft.Extensions.Options;
using ServiceStarter_v1;
using ServiceStarter_v1.DomainEntitys_MonitoredItems;
using Serilog;
using Serilog.Events;
using Serilog.Extensions;
using Serilog.Enrichers;
using Serilog.Enrichers.CallerInfo;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using ServiceStarter_v1.Validation_Config;
using ServiceStarter_v1.Main;

string configFilePath = @"D:\Workspaces\1_TestData\2026-02-26_Konfig_V3.json";



string currentDir = AppContext.BaseDirectory;

var builder = Host.CreateApplicationBuilder(args);
builder.Services.AddHostedService<Worker>();

builder.ConfigureCustomSerilog();
builder.Logging.ClearProviders();
builder.Logging.AddSerilog();
Log.Information($"Application started from {currentDir}");

//READ CONFIG
Log.Information($"Reading Config-File from {configFilePath}");
var loader = new JsonConfigFileLoader();
ConfigDTO config = loader.getRootObject<ConfigDTO>(configFilePath);
Log.Verbose($"ConfigDTO: {config} created from ConfigFile{configFilePath}");

//VALIDATE CONFIG
var failedValidations = new List<ValidationResult>();
bool isRecursivValid = RecursiveValidator.TryValidateObjectRecursive(config!, failedValidations);
Log.Verbose($"{config} object is validated: {isRecursivValid}");
failedValidations.ForEach(err => Log.Error(err.ErrorMessage!));
if(!isRecursivValid) { throw new ValidationException($"Validation of Config File failed: {string.Join(",",failedValidations.Select(v=>v.ErrorMessage))}"); }

//INJECT CONFIG
var configWrapper = Options.Create(config);
builder.Services.AddSingleton( configWrapper);

//DOMAIN OBJECT FACTORY
builder.Services.AddSingleton<DomainObjectFactory>(); // Das Object muss nachfolgend untermehredn Schlüsseln registriert werden damit Objekte welche nur das Interface kennen das Objekt finden
builder.Services.AddSingleton<IDomainEntitySource>(sp => sp.GetRequiredService<DomainObjectFactory>());
builder.Services.AddSingleton<IMonitoredItemSource>(sp => sp.GetRequiredService<DomainObjectFactory>());

//var serviceProvider = builder.Services.BuildServiceProvider();
/*var domainFactory = serviceProvider.GetRequiredService<DomainObjectFactory>();*/



//StartUpHandler
builder.Services.AddSingleton<StartUpHandler>();
//serviceProvider = builder.Services.BuildServiceProvider();
/*var startHandler = serviceProvider.GetRequiredService<StartUpHandler>();
CancellationTokenSource cts = new CancellationTokenSource();
var token = cts.Token;
startHandler.StartAllDomainEntities(token);*/

// MonitoringHandler
builder.Services.AddSingleton<MonitoringHandler>();
//serviceProvider = builder.Services.BuildServiceProvider();
/*var monitorHandler = serviceProvider.GetRequiredService<MonitoringHandler>();
monitorHandler.ProcessAllMonitoredItems(cts.Token);*/


var host = builder.Build();
host.Run();



