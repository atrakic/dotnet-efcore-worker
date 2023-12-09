using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using WorkerServicePlusEFCore.Models;

namespace WorkerServicePlusEFCore.Services
{
    public class DbHelper
    {
        private AppDbContext? _dbContext;

        private DbContextOptions<AppDbContext> GetAllOptions()
        {
            DbContextOptionsBuilder<AppDbContext> optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();
            if (AppSettings.ConnectionString != null)
            {
                optionsBuilder.UseSqlServer(AppSettings.ConnectionString);
            }
            return optionsBuilder.Options;
        }

        public List<User> GetUsers()
        {
            using (_dbContext = new AppDbContext(GetAllOptions()))
            {
                try
                {
                    var users = _dbContext.Users?.ToList();

                    if (users == null)
                        throw new InvalidOperationException("No user data is found!");

                    return users;
                }
                catch (Exception)
                {
                    throw;
                }
            }
        }


        // Seed Data - When no data is in the db, we want to populate with data
        public void SeedUsers()
        {
            using (_dbContext = new AppDbContext(GetAllOptions()))
            {
                _dbContext.Users?.AddRange(ListOfUsers());
                _dbContext.SaveChanges();
            }
        }

        private List<User> ListOfUsers()
        {
            List<User> users = new List<User> {
                new User
                {
                    Name = "Jay Jay",
                    Email = "jayjay@gmail.com"
                },

                new User
                {
                    Name = "Kanu Nwankwo",
                    Email = "kanunwankwo@gmail.com"
                },

                new User
                {
                    Name = "Taribo West",
                    Email = "taribowest@gmail.com"
                }

            };

            return users;
        }
    }
}

