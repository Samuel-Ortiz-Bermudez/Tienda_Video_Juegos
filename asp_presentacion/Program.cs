using asp_presentacion;

var builder = WebApplication.CreateBuilder(args);

var startup = new Startup(builder.Configuration);
startup.ConfigureServices(builder, builder.Services);

var app = builder.Build();
startup.Configure(app, app.Environment);
app.Run();