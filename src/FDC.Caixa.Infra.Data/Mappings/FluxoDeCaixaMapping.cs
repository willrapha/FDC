using FDC.Caixa.Domain.Caixas.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FDC.Caixa.Infra.Data.Mappings
{
    internal class FluxoDeCaixaMapping : IEntityTypeConfiguration<FluxoDeCaixa>
    {
        public void Configure(EntityTypeBuilder<FluxoDeCaixa> builder)
        {
            builder.HasKey(c => c.Id);

            builder.Property(c => c.Data)
                .IsRequired();

            builder.Ignore(_ => _.Saldo);

            builder.Ignore(_ => _.CascadeMode);

            builder.ToTable(nameof(FluxoDeCaixa));
        }
    }
}
