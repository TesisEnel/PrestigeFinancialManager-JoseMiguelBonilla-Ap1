using Microsoft.EntityFrameworkCore;
using PrestigeFinancial.Shared.Models;

namespace PrestigeFinancial.Server.DAL 
{
    public class Contexto : DbContext
    {
        public Contexto(DbContextOptions<Contexto> options) : base(options) {}
        public DbSet<Clientes> Clientes {get;set;}        
        public DbSet<ClientesDetalle> ClientesDetalle {get;set;}
        public DbSet<Pagos> Pagos {get;set;}        
        public DbSet<PagosDetalle> PagosDetalle {get;set;}
        public DbSet<Prestamos> Prestamos {get;set;}
        public DbSet<TiposTelefonos> TiposTelefonos {get;set;}
        public DbSet<TiposPrestamos> TiposPrestamos {get;set;}
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<TiposTelefonos>().HasData(new List<TiposTelefonos>()
        {
            new TiposTelefonos(){TiposTelefonoId=1,  Descripcion="Celular"},
            new TiposTelefonos(){TiposTelefonoId=2,  Descripcion="Residencial"},
            new TiposTelefonos(){TiposTelefonoId=3,  Descripcion="Oficina"},
        });
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<TiposPrestamos>().HasData(new List<TiposPrestamos>()
        {
            new TiposPrestamos(){TiposPrestamoId=1,  DescripcionPrestamo="Personal"},
            new TiposPrestamos(){TiposPrestamoId=2,  DescripcionPrestamo="Automotriz"},
            new TiposPrestamos(){TiposPrestamoId=3,  DescripcionPrestamo="Hipotecario"},
        });
        
        }
    }
}