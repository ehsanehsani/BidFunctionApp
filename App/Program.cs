using BidFunctionApp.Repository;
using Microsoft.Azure.Functions.Worker;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RabbitMQ.Client;

var host = new HostBuilder()
    .ConfigureFunctionsWebApplication()
        .ConfigureAppConfiguration(config =>
        {
            config.AddJsonFile("local.settings.json", optional: true, reloadOnChange: true);
        })
    .ConfigureServices((context, services) =>
    {
        services.AddApplicationInsightsTelemetryWorkerService();
        services.ConfigureFunctionsApplicationInsights();

        services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblyContaining<Program>());
        services.AddScoped<IBidRepository, BidRepository>();
        services.AddSingleton<IConnectionFactory>(new ConnectionFactory { HostName = "localhost" });

        var connectionString = context.Configuration.GetConnectionString("DefaultConnection");
        services.AddDbContext<AppDbContext>(options =>
            options.UseSqlServer(connectionString));
    })
    .Build();

host.Run();