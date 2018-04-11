using Microsoft.EntityFrameworkCore;

namespace DeskDexCore.Models
{
    public class DeskContext : DbContext
    {
        public DeskContext(DbContextOptions<DeskContext> options) : base(options)
        {
        }

        public DbSet<Checkin> Checkins { get; set; }
        public DbSet<Station> Stations { get; set; }
        public DbSet<WorkStyle> WorkStyles { get; set; }
        public DbSet<Equipment> Equipment { get; set; }
        public DbSet<StationEquipment> StationEquipments { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Checkin>().ToTable("Checkins");
            modelBuilder.Entity<Station>().ToTable("Stations");
            modelBuilder.Entity<WorkStyle>().ToTable("WorkStyles");
            modelBuilder.Entity<Equipment>().ToTable("Equipments");
            modelBuilder.Entity<StationEquipment>().ToTable("StationEquipments");

            modelBuilder.Entity<StationEquipment>()
                .HasKey(se => new { se.EquipmentId, se.StationId });

            modelBuilder.Entity<StationEquipment>()
                .HasOne(se => se.Station)
                .WithMany(p => p.StationEquipments)
                .HasForeignKey(se => se.StationId);

            modelBuilder.Entity<StationEquipment>()
                .HasOne(se => se.Equipment)
                .WithMany(c => c.StationEquipments)
                .HasForeignKey(se => se.EquipmentId);
        }
    }
}
