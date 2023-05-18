using CW21.DAL.Context;
using CW21.Repositories;
using CW21.Repositories.Doctors;
using Microsoft.EntityFrameworkCore;
using Serilog;
using Serilog.Sinks.MSSqlServer;
using System.Collections.ObjectModel;
using System.Data;
using System.Diagnostics;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews().AddRazorRuntimeCompilation();
builder.Services.AddDbContext<AppDbContext>(option =>
option.UseSqlServer("Server=.;Database=DoctorDb;User Id=sa;Password=<YourStrong@Passw0rd>;TrustServerCertificate=True;"));

builder.Services.AddScoped<IPatientRepository, PatientRepository>();
builder.Services.AddScoped<IDoctorRepository, DoctorRepository>();
builder.Services.AddScoped<IAppointmentRepository, AppointmentRepository>();

builder.Logging.ClearProviders();
//builder.Logging.AddConsole();
//Log.Logger = new LoggerConfiguration()
//.WriteTo.Seq("http://localhost:5341")
//.CreateLogger();

var config = new ConfigurationBuilder()
.AddJsonFile("appsettings.Development.json", optional: false)
.Build();

var connectionString = config.GetSection("LogConfigs:ConnectionString").Value;
var filePath = config.GetSection("LogConfigs:FilePath").Value;
var serverUrl = config.GetSection("LogConfigs:ServerUrlSeq").Value;



var sinkOpts = new MSSqlServerSinkOptions()
{
    AutoCreateSqlDatabase = true,
    AutoCreateSqlTable = true,
    TableName = "Log",
    SchemaName = "Base",
};


var columnOpts = new ColumnOptions()
{
    AdditionalColumns = new Collection<SqlColumn>()
    {
        new SqlColumn()
        {
        ColumnName="CrudState" , DataType=SqlDbType.NVarChar
        },
        new SqlColumn()
        {
        ColumnName="ProductId" , DataType=SqlDbType.Int
        }
    }
};


//Serilog.Debugging.SelfLog.Enable(msg =>
//{
//    Debug.Print(msg);
//    Debugger.Break();
//});



Log.Logger = new LoggerConfiguration()
    .WriteTo.Logger(x =>
    {
        x.WriteTo.Seq(serverUrl);
    })
    .WriteTo.Logger(x =>
    {
        x.WriteTo.File(filePath);
        x.MinimumLevel.Error();
    })
    .WriteTo.Logger(x =>
    {
        x.WriteTo.MSSqlServer(connectionString: connectionString, sinkOptions: sinkOpts, columnOptions: columnOpts);
    })
    .CreateLogger();





var app = builder.Build();


// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();


app.MapAreaControllerRoute(
    areaName: "DoctorArea",
    name: "areas",
    pattern: "{area:exists}/{controller=Account}/{action=LoginDoctor}/{id?}");



app.MapControllerRoute(
name: "default",
pattern: "{controller=Home}/{action=Index}/{id?}");




app.Run();

