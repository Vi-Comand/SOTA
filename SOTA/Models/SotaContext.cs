using Microsoft.EntityFrameworkCore;

namespace SOTA.Models
{
    public class SotaContext : DbContext
    {
        public DbSet<Zadanie> Zadanie { get; set; }
        public DbSet<Otvet> Otvet { get; set; }
        public DbSet<Oo> Oo { get; set; }
        public DbSet<Mo> Mo { get; set; }
        public DbSet<Users> Users { get; set; }
        public DbSet<Klass> Klass { get; set; }
        public DbSet<Specific> Specific { get; set; }
        public DbSet<Predm> Predm { get; set; }
        public DbSet<TipSpec> TipSpec { get; set; }
        public DbSet<Rabota> Rabota { get; set; }
        public DbSet<NaznachMo> NaznachMo { get; set; }
        public DbSet<NaznachOo> NaznachOo { get; set; }
        public DbSet<SaveImg> SaveImg { get; set; }
        public DbSet<AnswerUser> AnswerUser { get; set; }
        public DbSet<VariantUser> VariantUser { get; set; }
        public DbSet<UsersBalls> UsersBalls { get; set; }
        public DbSet<Kriterocen> Kriterocen { get; set; }
        public SotaContext(DbContextOptions<SotaContext> options)
            : base(options)
        {
            //Database.EnsureCreated();
        }
    }
}
