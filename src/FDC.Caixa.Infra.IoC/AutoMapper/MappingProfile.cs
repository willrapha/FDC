using AutoMapper;
using FDC.Caixa.Domain.Caixas.Dtos;
using FDC.Caixa.Domain.Caixas.Entities;

namespace FDC.Caixa.Infra.IoC.AutoMapper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<FluxoDeCaixaDto, FluxoDeCaixa>()
                .ForMember(d => d.Id, dto => dto.MapFrom(s => s.Id))
                .ForMember(d => d.Saldo, dto => dto.MapFrom(s => s.Saldo))
                .ForMember(d => d.Situacao, dto => dto.MapFrom(s => s.Situacao))
                .ForMember(d => d.Movimentacoes, dto => dto.MapFrom(s => s.Movimentacoes))
                .ForMember(d => d.Data, dto => dto.MapFrom(s => s.Data));

            CreateMap<Movimentacao, MovimentacaoDto>()
                .ForMember(d => d.FluxoDeCaixaId, dto => dto.MapFrom(s => s.FluxoDeCaixaId))
                .ForMember(d => d.Tipo, dto => dto.MapFrom(s => s.Tipo))
                .ForMember(d => d.Descricao, dto => dto.MapFrom(s => s.Descricao))
                .ForMember(d => d.Id, dto => dto.MapFrom(s => s.Id))
                .ForMember(d => d.Valor, dto => dto.MapFrom(s => s.Valor));
        }
    }
}
