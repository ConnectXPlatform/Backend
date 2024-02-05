using Firebase;
using FirebaseAdmin;
using FirebaseAdminAuthentication.DependencyInjection.Extensions;
using DatabaseServer.Endpoints;
using DatabaseServer.Repositories;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton(FirebaseApp.Create());
builder.Services.AddSingleton<FirebaseService>();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSingleton<IDevicesRepository, DevicesRepository>();
builder.Services.AddSingleton<IUsersRepository, UsersRepository>();
builder.Services.AddSingleton<IControlPanelsRepository, ControlPanelsRepository>();
builder.Services.AddSingleton<ITopicsRepository, TopicsRepository>();
builder.Services.AddSingleton<IComponentsRepository, ComponentsRepository>();
builder.Services.AddSingleton<IPositionedComponentsRepository, PositionedComponentsRepository>();

builder.Services.AddFirebaseAuthentication();
builder.Services.AddAuthorization();

var app = builder.Build();
var a = builder.Configuration.GetValue<string>("Firebase_ProjectName");
await app.Services.GetRequiredService<FirebaseService>()
    .Initialize(builder.Configuration.GetValue<string>("Firebase_ProjectName")!);

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.MapGroup("devices")
    .WithTags("Devices")
    .MapDevicesEndpoints()
    .MapDeviceTopicsEndpoints()
    .RequireAuthorization();
app.MapGroup("users")
    .WithTags("Users")
    .MapUsersEndpoints()
    .MapUsersComponentsEndpoints()
    .RequireAuthorization();
app.MapGroup("control_panels")
    .WithTags("Control Panels")
    .MapControlPanelsEndpoints()
    .MapControlPanelComponentsEndpoints()
    .RequireAuthorization();
app.MapGroup("components")
    .WithTags("Components")
    .MapComponentsEndpoints()
    .RequireAuthorization();
app.MapGroup("positioned_components")
    .WithTags("Positioned Components")
    .MapPositionedComponentsEndpoints()
    .RequireAuthorization();
app.MapGroup("topics")
    .WithTags("Topics")
    .MapTopicsEndpoints()
    .RequireAuthorization();

app.Run();