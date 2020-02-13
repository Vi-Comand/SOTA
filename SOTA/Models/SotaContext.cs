using Microsoft.EntityFrameworkCore;
using SOTA.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace Sota.Models
{
    public class SotaContext : DbContext
    {
        public DbSet<Zadanie> Zadanie { get; set; }


        public SotaContext(DbContextOptions<SotaContext> options)
            : base(options)
        {
            //    Database.EnsureCreated();
        }
    }
}