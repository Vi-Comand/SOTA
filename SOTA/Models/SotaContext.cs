using Microsoft.EntityFrameworkCore;
using SOTA.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace SOTA.Models
{
    public class SotaContext : DbContext
    {
        public DbSet<Zadanie> Zadanie { get; set; }
        public DbSet<Otvet> Otvet { get; set; }
        public DbSet<Specific> Specific { get; set; }
        public DbSet<Predm> Predm { get; set; }
        public DbSet<TipSpec> TipSpec { get; set; }
        public SotaContext(DbContextOptions<SotaContext> options)
            : base(options)
        {
            Database.EnsureCreated();
        }
    }
}