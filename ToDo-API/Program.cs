using ToDo_API.Configurations;

var builder = WebApplication.CreateBuilder(args);

ServicesConfiguration.ConfigureServices(builder.Services, builder.Configuration);

var app = builder.Build();

ApplicationConfiguration.ConfigureApplication(app);

app.Run();