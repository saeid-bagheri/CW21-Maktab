using CW21.DAL.Context;
using CW21.Repositories;
using CW21.Repositories.Doctors;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews().AddRazorRuntimeCompilation();
builder.Services.AddDbContext<AppDbContext>(option =>
option.UseSqlServer("Server=.;Database=DoctorDb;User Id=sa;Password=<YourStrong@Passw0rd>;TrustServerCertificate=True;"));

builder.Services.AddScoped<IPatientRepository, PatientRepository>();
builder.Services.AddScoped<IDoctorRepository, DoctorRepository>();
builder.Services.AddScoped<IAppointmentRepository, AppointmentRepository>();


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

