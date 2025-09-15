var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

builder.Services.AddMudServices();
builder.Services.AddRazorComponents().AddInteractiveServerComponents();
var options = builder.Configuration.GetSection("Configuration").Get<Configuration>();
builder.Services.AddHttpClient("Default", client =>
{
    client.BaseAddress = options.ApiBaseAddress;
});

var app = builder.Build();

app.MapDefaultEndpoints();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
}

app.UseAntiforgery();
app.MapStaticAssets(); app.MapRazorComponents<App>().AddInteractiveServerRenderMode();

app.Run();
