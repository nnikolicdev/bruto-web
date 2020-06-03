using bruto_web.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System;
using System.Configuration;

namespace bruto_web.Data
{
    public class DbUserContext : DbContext
    {

        private readonly IOptions<ConfigModel> _config;


        public DbUserContext(DbContextOptions<DbUserContext> options, IOptions<ConfigModel> config)
            :base(options)
        {
            _config = config;
        }

        public DbSet<UserModel> Users { get; set; }

        public DbSet<BrutoModel> models { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            // Obracun I Dohodak.
            optionsBuilder.UseSqlServer(_config.Value.ConnectionString);
        }
    }
}
