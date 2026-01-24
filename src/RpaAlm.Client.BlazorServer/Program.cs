using RpaAlm.Client.BlazorServer.Components;
using RpaAlm.Shared.ApiClient.Clients;
using MudBlazor.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

// Add MudBlazor services
builder.Services.AddMudServices();

// Register API Clients (all 22 entities)
builder.Services.AddScoped<StatusApiClient>();
builder.Services.AddScoped<ComplexityApiClient>();
builder.Services.AddScoped<MedalApiClient>();
builder.Services.AddScoped<RegionApiClient>();
builder.Services.AddScoped<SegmentApiClient>();
builder.Services.AddScoped<FunctionApiClient>();
builder.Services.AddScoped<SlaItemTypeApiClient>();
builder.Services.AddScoped<AutomationEnvironmentTypeApiClient>();
builder.Services.AddScoped<ADDomainApiClient>();
builder.Services.AddScoped<EnhancementApiClient>();
builder.Services.AddScoped<StoryPointCostApiClient>();
builder.Services.AddScoped<AutomationApiClient>();
builder.Services.AddScoped<SlaMasterApiClient>();
builder.Services.AddScoped<SlaItemApiClient>();
builder.Services.AddScoped<VirtualIdentityApiClient>();
builder.Services.AddScoped<ViAssignmentsApiClient>();
builder.Services.AddScoped<EnhancementUserStoryApiClient>();
builder.Services.AddScoped<AutomationEnvironmentApiClient>();
builder.Services.AddScoped<AutomationLogEntryApiClient>();
builder.Services.AddScoped<SlaLogEntryApiClient>();
builder.Services.AddScoped<JjedsHelperApiClient>();
builder.Services.AddScoped<CmdbHelperApiClient>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
