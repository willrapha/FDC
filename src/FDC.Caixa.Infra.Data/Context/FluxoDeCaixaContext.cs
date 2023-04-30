using FDC.Caixa.Domain.Caixas.Entities;
using FluentValidation.Results;
using Microsoft.EntityFrameworkCore;

namespace FDC.Caixa.Infra.Data.Context
{
    public class FluxoDeCaixaContext : DbContext
    {
        public FluxoDeCaixaContext(DbContextOptions<FluxoDeCaixaContext> options)
            : base(options)
        {
        }

        public DbSet<FluxoDeCaixa> FluxoDeCaixa { get; set; }
        public DbSet<Movimentacao> Movimentacao { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Ignore<ValidationResult>();

            foreach (var relationship in modelBuilder.Model.GetEntityTypes().SelectMany(e => e.GetForeignKeys()))
                relationship.DeleteBehavior = DeleteBehavior.Restrict;

            modelBuilder.ApplyConfigurationsFromAssembly(typeof(FluxoDeCaixaContext).Assembly);

        }
    }
}
