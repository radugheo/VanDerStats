using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json.Linq;
using System.Net;
using WorldCupWrapped.Data;
using WorldCupWrapped.Helpers.Extensions;
using WorldCupWrapped.Helpers.Seeders;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
builder.Services.AddDbContext<ProjectContext>(opts => { opts.UseNpgsql(builder.Configuration.GetConnectionString("AppDb")); }, ServiceLifetime.Transient);
builder.Services.AddControllersWithViews();
builder.Services.AddRepositories();
builder.Services.AddServices();
builder.Services.AddSeeders();
builder.Services.AddUtils();

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "My API", Version = "v1" });
});

var app = builder.Build();
SeedDataAsync(app);

app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
});

if (!app.Environment.IsDevelopment())
{
    app.UseHsts();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

//app.MapGet("/trophies", async (ProjectContext db) => await db.Trophies.ToListAsync()).Produces<List<Trophy>>;

app.MapControllerRoute(
    name: "default",
    pattern: "{controller}/{action=Index}/{id?}");

app.MapFallbackToFile("index.html"); ;

app.Run();


string WCLogin()
{
    var url = "http://api.cup2022.ir/api/v1/user/login";
    var httpRequest = (HttpWebRequest)WebRequest.Create(url);
    httpRequest.Method = "POST";
    httpRequest.Accept = "application/json";
    httpRequest.ContentType = "application/json";
    var data = @"{
              ""email"": ""radu2002ro@gmail.com"",
              ""password"": ""worldCupApi2022""
            }";
    using (var streamWriter = new StreamWriter(httpRequest.GetRequestStream()))
    {
        streamWriter.Write(data);
    }

    var httpResponse = (HttpWebResponse)httpRequest.GetResponse();
    var streamReader = new StreamReader(httpResponse.GetResponseStream());
    var response = streamReader.ReadToEnd();
    JObject joResponse = JObject.Parse(response);
    var token = joResponse["data"]["token"];

    return (string)token;
}
async Task SeedDataAsync(IHost app)
{
    var scope = app.Services.GetService<IServiceScopeFactory>().CreateScope();
    using (var context = new ProjectContext(
            scope.ServiceProvider.GetRequiredService
            <DbContextOptions<ProjectContext>>()))
    {
        var token = WCLogin();
        var trophyService = new TrophySeeder(context);
        trophyService.SeedInitialTrophies();

        var teamService = new TeamSeeder(context);
        await teamService.SeedInitialTeamsAsync(token);
    }
}
