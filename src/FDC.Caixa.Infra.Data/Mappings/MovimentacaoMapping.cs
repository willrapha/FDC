using FDC.Caixa.Domain.Caixas.Entities;
using FDC.Caixa.Resource;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FDC.Caixa.Infra.Data.Mappings
{
    public class MovimentacaoMapping : IEntityTypeConfiguration<Movimentacao>
    {
        public void Configure(EntityTypeBuilder<Movimentacao> builder)
        {
            builder.HasKey(m => m.Id);

            builder.Property(m => m.DataHora)
                .IsRequired();

            builder.Property(m => m.Descricao)
                .IsRequired()
                .HasMaxLength(Constantes.QuantidadeDeCaracteres200);

            builder.Property(m => m.Valor)
                .IsRequired();

            builder.Property(m => m.Tipo)
                .IsRequired();

            builder.Property(m => m.FluxoDeCaixaId)
                .IsRequired();

            builder.HasOne(m => m.FluxoDeCaixa)
                .WithMany(m => m.Movimentacoes)
                .HasForeignKey(m => m.FluxoDeCaixaId);

            builder.Ignore(_ => _.CascadeMode);

            builder.ToTable(nameof(Movimentacao));
        }
    }
}
