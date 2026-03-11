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

//INJECT CONFIG
var configWrapper = Options.Create(config);
builder.Services.AddSingleton(configWrapper);
builder.Services.AddSingleton<DomainObjectFactory>();
var fac = builder.Services.BuildServiceProvider().GetRequiredService<DomainObjectFactory>();


/*var host = builder.Build();
host.Run();
*/


