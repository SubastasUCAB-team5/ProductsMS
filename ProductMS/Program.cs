using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.Security.Claims;
using ProductMS.Application.Handlers.Commands;
using ProductMS.Application.Handlers.Queries;
using ProductMS.Core.DataBase;
using ProductMS.Core.Repositories;
using ProductMS.Core.Service;
using ProductMS.Infrastructure.DataBase;
using ProductMS.Infrastructure.Repositories;
using ProductMS.Infrastructure.Setings;
using System.Configuration;
using MassTransit;
using ProductMS.Infrastructure.Messaging.Consumers;
using ProductMS.Infrastructure.Service;
using Microsoft.Extensions.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var _appSettings = new AppSettings();
var appSettingsSection = builder.Configuration.GetSection("AppSettings");
_appSettings = appSettingsSection.Get<AppSettings>();
builder.Services.Configure<AppSettings>(appSettingsSection);

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
        options.JsonSerializerOptions.Converters.Add(new System.Text.Json.Serialization.JsonStringEnumConverter());
    });

builder.Services.AddHttpClient();


builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(CreateProductCommandHandler).Assembly));
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(UpdateProductCommandHandler).Assembly));
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(DeleteProductCommandHandler).Assembly));
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(ChangeProductStateCommandHandler).Assembly));


builder.Services.AddScoped<IEventPublisher, EventPublisher>();
builder.Services.AddTransient<IProductsDbContext, ProductsDbContext>();
builder.Services.AddTransient<IProductRepository, ProductRepository>();
builder.Services.AddTransient<IProductsDbContext, ProductsDbContext>();

System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Tls12;

builder.Services.AddSingleton<MongoDbContext>();
builder.Services.AddSingleton(provider =>
{
    var context = provider.GetRequiredService<MongoDbContext>();
    return context.Products;
});

var dbConnectionString = builder.Configuration.GetValue<string>("DefaultConnection");
builder.Services.AddDbContext<ProductsDbContext>(options =>
options.UseSqlServer(dbConnectionString));
builder.Services.AddSingleton<MongoDbContext>();

builder.Services.AddMassTransit(x =>
{
    x.AddConsumer<ProductCreatedConsumer>();
    x.AddConsumer<ProductUpdatedConsumer>();
    x.AddConsumer<ProductDeletedConsumer>();

    x.UsingRabbitMq((context, cfg) =>
    {
        cfg.Host("localhost", "/", h => {
            h.Username("guest");
            h.Password("guest");
        });

        cfg.ReceiveEndpoint("product-created-queue", e =>
        {
            e.ConfigureConsumer<ProductCreatedConsumer>(context);
        });
        cfg.ReceiveEndpoint("product-updated-queue", e =>
        {
            e.ConfigureConsumer<ProductUpdatedConsumer>(context);
        });
        cfg.ReceiveEndpoint("product-deleted-queue", e =>
        {
            e.ConfigureConsumer<ProductDeletedConsumer>(context);
        });
    });
});


builder.Services.AddEndpointsApiExplorer();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthentication(); 
app.UseAuthorization();  
app.MapControllers();
app.Run();

