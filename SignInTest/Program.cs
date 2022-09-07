using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using SignInTest.Data;
using System.Security.Claims;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddSingleton<WeatherForecastService>();
builder.Services.AddAuthentication("Cookies").AddCookie(opt =>
    {
        opt.Cookie.Name = "HAHAY";
        opt.LoginPath = "/auth/signin-google";
    }
).AddGoogle(opt =>
{
    opt.ClientId = builder.Configuration["Google:Id"];
    opt.ClientSecret = builder.Configuration["Google:Secret"];
    opt.Scope.Add("profile");
    opt.Events.OnCreatingTicket = context =>
    {
        string picuri = context.User.GetProperty("picture").ToString();
        context.Identity.AddClaim(new Claim("picture", picuri));
        return Task.CompletedTask;
    };
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
app.UseAuthentication();
app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
    endpoints.MapBlazorHub();
    endpoints.MapFallbackToPage("/_Host");
});

app.Run();
