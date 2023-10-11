using aws_s3_digitalocean_spaces_basic;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

var startup = new Startup(builder.Configuration);

startup.configurationServices(builder.Services);

var app = builder.Build();

startup.configuration(app,app.Environment);

// Configure the HTTP request pipeline.

app.Run();
