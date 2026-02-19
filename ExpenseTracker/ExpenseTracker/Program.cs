using ExpenseTracker.ConnectionsFactory;
using ExpenseTracker.Services;
using Microsoft.AspNetCore.Builder;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();

builder.Services.AddSingleton<AIConnectionFactory>();
builder.Services.AddSingleton<SqlConnectionFactory>();
builder.Services.AddScoped<UserService>();// Configure the HTTP request pipeline.
builder.Services.AddScoped<AIService>();// Configure the HTTP request pipeline.
builder.Services.AddHttpClient<AIService>();


builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromSeconds(5); // short timeout
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

var app = builder.Build();


if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();

    
}
else
{
    app.UseDeveloperExceptionPage();
}

app.UseHttpsRedirection();

app.UseRouting();

app.UseAuthorization();

app.UseSession();
app.Use(async (context, next) =>
{
    var path = context.Request.Path.Value ?? string.Empty;

    if (string.IsNullOrEmpty(path) || path == "/")
    {
        context.Response.Redirect("/ExpenseTracker/Login");
        return;
    }

    await next();
});

app.Use(async (context, next) =>
{
    var path = context.Request.Path.Value ?? string.Empty;

    if (path.StartsWith("/Login", StringComparison.OrdinalIgnoreCase) ||
        path.EndsWith(".css", StringComparison.OrdinalIgnoreCase) ||
        path.EndsWith(".js", StringComparison.OrdinalIgnoreCase) ||
        path.EndsWith(".png", StringComparison.OrdinalIgnoreCase) ||
        path.EndsWith(".jpg", StringComparison.OrdinalIgnoreCase) ||
        path.EndsWith(".jpeg", StringComparison.OrdinalIgnoreCase) ||
        path.EndsWith(".gif", StringComparison.OrdinalIgnoreCase) ||
        path.EndsWith(".ico", StringComparison.OrdinalIgnoreCase))
    {
        await next();
        return;
    }

    var isLoggedIn = context.Session.GetString("IsLoggedIn");
    if (isLoggedIn != "true")
    {
        context.Response.Redirect("/Login?timeout=true");
        return;
    }

    await next();
});


app.MapStaticAssets();
app.MapRazorPages()
   .WithStaticAssets();

app.Run();
