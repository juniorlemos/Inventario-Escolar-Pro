using InventarioEscolar.Api.Configurations;
using InventarioEscolar.Api.Filters;
using InventarioEscolar.Application;
using InventarioEscolar.Infrastructure;
using InventarioEscolar.Infrastructure.DataSeeder;
using Serilog;

var builder = WebApplication.CreateBuilder(args);


LoggerConfigurationFactory.ConfigureSerilog(builder);
builder.Host.UseSerilog();


builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigins", policy =>
    {
        policy.WithOrigins("https://inventario360-front.onrender.com")
              .AllowAnyMethod()
              .AllowAnyHeader()
              .AllowCredentials()
              .WithExposedHeaders("*");
    });

    options.AddPolicy("DevCors", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});


builder.Services.AddControllers(options =>
{
    options.Filters.Add<TrimStringsFilter>();
})
.AddJsonOptions(options =>
{
    options.JsonSerializerOptions.ReferenceHandler =
        System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;

    options.JsonSerializerOptions.DefaultIgnoreCondition =
        System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull;

    options.JsonSerializerOptions.Converters.Add(
        new System.Text.Json.Serialization.JsonStringEnumConverter()
    );
});


builder.Services.AddOpenApi();
builder.Services.AddSwaggerGen();

builder.Services.AddRouting(options => options.LowercaseUrls = true);
builder.Services.AddMvc(options =>
{
    options.Filters.Add(typeof(ExceptionFilter));
});


builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddJwtAuthentication(builder.Configuration);
builder.Services.AddApplication(builder.Configuration);

var app = builder.Build();


if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI();

    app.UseCors("DevCors");
}
else
{
    app.UseHttpsRedirection();
    app.UseCors("DevCors");
}

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    await DataSeeder.SeedDatabaseAsync(services);
}


app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

await app.RunAsync();
