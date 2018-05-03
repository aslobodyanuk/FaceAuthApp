using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using SmartFaceAuthAPI.Models;

namespace SmartFaceAuthAPI.Context
{
    public class ApiContext : DbContext
    {
        public DbSet<Log> Logs { get; set; }

        public DbSet<Group> Groups { get; set; }

        public DbSet<Person> Persons { get; set; }
    }
}