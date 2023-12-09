using Microsoft.EntityFrameworkCore;
using WorkerServicePlusEFCore.Models;
using WorkerExample;

public class Program
    {
        public static void Main(string[] args)
        {
            IHost host = CreateHostBuilder(args).Build();
	        CreateDbIfNoneExist(host);
		    host.Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureServices((hostContext, services) =>
                {
        	   IConfiguration configuration = hostContext.Configuration;
	           AppSettings.ConnectionString = configuration.GetConnectionString("DefaultConnection");
	           var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();

	          optionsBuilder.UseSqlServer(AppSettings.ConnectionString);
	           services.AddScoped<AppDbContext>(db => new AppDbContext(optionsBuilder.Options));
                   services.AddHostedService<Worker>();
                });

	private static void CreateDbIfNoneExist(IHost host)
        {
            using (var scope = host.Services.CreateScope())
            {
                var service = scope.ServiceProvider;

                try
                {
                    var context = service.GetRequiredService<AppDbContext>();
                    context.Database.EnsureCreated();
                }
                catch (Exception)
                {

                    throw;
                }
            }
        }

}

/**
IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices(hostContext, services =>
    {
        services.AddHostedService<Worker>();
    })
    .Build();

await host.RunAsync();
*/
