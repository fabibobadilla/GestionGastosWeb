using Blazorise;
using Blazorise.Bootstrap;
using GestionGastosWeb;
using GestionGastosShared.Services;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Blazored.LocalStorage;
using Syncfusion.Blazor;
using MudBlazor.Services;
using CurrieTechnologies.Razor.SweetAlert2;
using OfficeOpenXml;


var builder = WebAssemblyHostBuilder.CreateDefault(args);

builder.Services.AddMudServices();

builder.Services.AddBlazoredLocalStorage();//linea de localstorage

builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

// Configurar la lectura del archivo appsettings.json
var configuration = builder.Configuration;
//builder.Services.AddSingleton(new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
builder.Services.AddSingleton(new HttpClient { BaseAddress = new Uri(configuration["ApiUrl"]) });
//builder.Services.AddSingleton(new HttpClient { BaseAddress = new Uri(configuration["ApiUrlLocal"]) });
builder.Services.AddSyncfusionBlazor();

builder.Services.AddScoped<AuthService>();
builder.Services.AddScoped<CategoriasService>();
builder.Services.AddScoped<GastosService>();
builder.Services.AddScoped<IngresosService>();
builder.Services.AddScoped<PresupuestoService>();
builder.Services.AddScoped<UsuariosService>();
builder.Services.AddSweetAlert2();
builder.Services.AddScoped<ExcelReportService>();


await builder.Build().RunAsync();


