using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using WebApplication8.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("WebsiteDbConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddEntityFrameworkStores<ApplicationDbContext>();

builder.Services.AddDbContext<GiftOfGiversContext>(options =>
   options.UseSqlServer(builder.Configuration.GetConnectionString("WebsiteDbConnection")));
builder.Services.AddControllersWithViews();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<IdentityUser>>();
    var signInManager = scope.ServiceProvider.GetRequiredService<SignInManager<IdentityUser>>();

    string dummyEmail = "Bonno@gmail.com";
    string dummyPassword = "Password123@";

    var user = await userManager.FindByEmailAsync(dummyEmail);
    if (user == null)
    {
        user = new IdentityUser { UserName = dummyEmail, Email = dummyEmail, EmailConfirmed = true };
        await userManager.CreateAsync(user, dummyPassword);
    }


    var httpContextAccessor = scope.ServiceProvider.GetRequiredService<IHttpContextAccessor>();
    var httpContext = httpContextAccessor.HttpContext;

    // HttpContext may not be available at startup, so we handle login elsewhere
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Use(async (context, next) =>
{
    var signInManager = context.RequestServices.GetRequiredService<SignInManager<IdentityUser>>();
    var userManager = context.RequestServices.GetRequiredService<UserManager<IdentityUser>>();

    if (!context.User.Identity.IsAuthenticated)
    {
        var dummyUser = await userManager.FindByEmailAsync("Bonno@gmail.com");
        if (dummyUser != null)
        {
            await signInManager.SignInAsync(dummyUser, isPersistent: false);
        }
    }

    await next();
});
app.MapRazorPages();

app.Run();
