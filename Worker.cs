using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using WorkerServicePlusEFCore.Models;
using WorkerServicePlusEFCore.Services;

namespace WorkerExample;

public class Worker : BackgroundService
{
    private readonly ILogger<Worker> _logger;

    public Worker(ILogger<Worker> logger)
    {
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);

            DbHelper dbHelper = new DbHelper();

	    // fetch user data
	    List<User> users = dbHelper.GetUsers();

            if (users.Count == 0)
            {
               dbHelper.SeedUsers();
            }
            else
            {
               DisplayUserInformation(users);
            }
	    
            await Task.Delay(1000, stoppingToken);
        }
    }

	private void DisplayUserInformation(List<User> users)
        {
            users?.ForEach(user =>
            {
                _logger.LogInformation($"User Information\nUser: {user.Name}\t Email: {user.Email}");
            });
        }

}
