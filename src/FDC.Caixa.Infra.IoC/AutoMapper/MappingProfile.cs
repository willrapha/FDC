using AutoMapper;
using FDC.Caixa.Domain.Caixas.Dtos;
using FDC.Caixa.Domain.Caixas.Entities;
using FDC.Generics.Domain;

namespace FDC.Caixa.Infra.IoC.AutoMapper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<FluxoDeCaixa, FluxoDeCaixaDto>()
                .ForMember(d => d.Id, dto => dto.MapFrom(s => s.Id))
                .ForMember(d => d.Saldo, dto => dto.MapFrom(s => s.Saldo))
                .ForMember(d => d.Situacao, dto => dto.MapFrom(s => s.Situacao))
                .ForMember(d => d.Movimentacoes, dto => dto.MapFrom(s => s.Movimentacoes.Select(m => new MovimentacaoDto
                {
                    Descricao = m.Descricao,
                    Tipo = m.Tipo,
                    FluxoDeCaixaId = m.FluxoDeCaixaId,
                    Id = m.Id,
                    Valor = m.Valor,
                    DataHora = m.DataHora
                })))
                .ForMember(d => d.Data, dto => dto.MapFrom(s => s.Data));

            CreateMap<FluxoDeCaixa, FluxoDeCaixaImprimirDto>()
               .ForMember(d => d.Id, dto => dto.MapFrom(s => s.Id))
               .ForMember(d => d.Movimentacoes, dto => dto.MapFrom(s => s.Movimentacoes.Select(m => new MovimentacaoImprimirDto
               {
                   Descricao = m.Descricao,
                   Tipo = m.Tipo.GetEnumDescription(),
                   DataHora = m.DataHora,
                   Valor = m.Valor
               })));
        }
    }
}
