using Core.Services;
using Domain.Models;
using Infrasitructure;
using Infrasitructure.Data;
using Infrasitructure.IRepository;
using Infrasitructure.Repository;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

//builder.Services.AddTransient(); // new object for evry request
builder.Services.AddScoped(typeof(IBaseRepository<>),typeof(BaseRepository<>)); // new object for One request
builder.Services.AddScoped(typeof(IUnitOfWork),typeof(UintOfWork)); // new object for One request
builder.Services.AddScoped(typeof(IPostRepo),typeof(PostRepo)); // new object for One request
//builder.Services.AddSingleton<PostService>(); // one object for all request
builder.Services.AddScoped<PostService>(); 
builder.Services.AddScoped<CategoryService>(); 

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("Con"))
);
// add identity

builder.Services.AddIdentity<User, IdentityRole>(
    options =>
    {
        options.Password.RequiredLength = 4;
        options.Password.RequireDigit = false; // 02165
        options.Password.RequireLowercase = false; // gfhkf
        options.Password.RequireUppercase = true; // GHFJKL
        options.Password.RequireNonAlphanumeric = false; // @#$%^&*()_+
        options.Password.RequiredUniqueChars = 0;
        options.SignIn.RequireConfirmedEmail = false; // email confirmation
        options.User.RequireUniqueEmail = true; // unique email
        // admin#123
        options.User.AllowedUserNameCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+"; // allowed characters
        
        options.Lockout.AllowedForNewUsers = true; // lockout
        options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromSeconds(30); // lockout time
        options.Lockout.MaxFailedAccessAttempts = 3; // max failed attempts

    }
    ).AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<AppDbContext>()
    .AddDefaultTokenProviders();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");



using (var scope = app.Services.CreateScope())
{
    var srvices = scope.ServiceProvider;
    await DbInitalizer.SeedAdminData(srvices);// IServesProvider
};




    app.Run();
