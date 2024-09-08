using QuartzWebScheduler.DataAccess.DbContext;
using QuartzWebScheduler.DataAccess.Repository;
using QuartzWebScheduler.DataAccess.Repository.IRepository;
using QuartzWebScheduler.Models;
using QuartzWebScheduler.Utility;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using NToastNotify;

var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("WebDbContextConnection")
                       ?? throw new InvalidOperationException("Connection string 'WebDbContextConnection' not found.");

builder.Services.AddDbContext<WebDbContext>(options =>
    options.UseSqlServer(connectionString,
        optionsAction => optionsAction.MigrationsAssembly("QuartzWebScheduler.Web")));

builder.Services.AddIdentity<WebUser, IdentityRole>(options =>
{
    options.SignIn.RequireConfirmedAccount = false;
})
    .AddEntityFrameworkStores<WebDbContext>()
    .AddDefaultTokenProviders();

// Add services to the container.
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
//builder.Services.AddScoped<ILogController, LogController>();
builder.Services.AddScoped<UserManager<WebUser>>();
builder.Services.AddSingleton<IEmailSender, EmailSender>();
builder.Services.AddScoped<RoleManager<IdentityRole>>();
builder.Services.AddRazorPages().AddNToastNotifyToastr(new ToastrOptions
{
    ProgressBar = true,
    TimeOut = 5000,
    PositionClass = "toast-bottom-right"
});
builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = $"/Identity/Account/Login";
    options.LogoutPath = $"/Identity/Account/Logout";
    options.AccessDeniedPath = $"/Identity/Account/AccessDenied";
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseAuthentication(); ;
app.UseAuthorization();
app.UseNToastNotify();
app.MapRazorPages();

app.Run();