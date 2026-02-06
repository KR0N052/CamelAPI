using Microsoft.EntityFrameworkCore;

namespace CamelAPI
{
    public class CamelDb : DbContext
    {
        public CamelDb(DbContextOptions<CamelDb> options) 
            : base(options) { }

        public DbSet<Camel> Camels => Set<Camel>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Camel>(entity =>
            {
                entity.HasKey(c => c.Id);

                entity.Property(c => c.Name)
                    .IsRequired();

            });
        }

    }
}
