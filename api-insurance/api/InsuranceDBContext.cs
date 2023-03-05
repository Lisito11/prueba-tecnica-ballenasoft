using System;
using api.Models;
using Microsoft.EntityFrameworkCore;

namespace api
{
	public class InsuranceDBContext : DbContext
    {
        public InsuranceDBContext(DbContextOptions options) : base(options)
        {
            this.Database.EnsureCreated();
        }

        public DbSet<Insurance>? Insurances { get; set; }

    }
}

