using DataAccess.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using System.Configuration;
using System.Data.Common;

namespace AktifVehiclePlanningSystem.Context
{
    public class ApplicationDbContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
    {
        public ApplicationDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
            optionsBuilder.UseSqlServer("Server=.\\SQLEXPRESS;Database=VPSDb;Trusted_Connection=True;TrustServerCertificate=True;");
                        
            return new ApplicationDbContext(optionsBuilder.Options);
        }
    }
}
