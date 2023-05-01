using FDC.Caixa.Domain.Caixas.Dtos;
using FDC.Caixa.Domain.Caixas.Interfaces;
using FDC.Generics.Domain;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using RestSharp;

namespace FDC.Caixa.Infra.Data.Rest.Caixas
{
    public class ImprimirFluxoDeCaixaRest : IImprimirFluxoDeCaixaRest
    {
        private readonly RestClient _restClient;

        public ImprimirFluxoDeCaixaRest(IConfiguration configuration)
        {
            _restClient = new RestClient(configuration["Appsettings:RelatorioUrl"]);
        }

        public async Task<ArquivoDto> ObterPorId(FluxoDeCaixaImprimirDto dto, string token)
        {
            var arquivo = new ArquivoDto();

            var request = new RestRequest("/ObterRelatorioFluxoDeCaixa", Method.Post);
            request.AddHeader("Authorization", token);
            request.AddJsonBody(dto);

            var response = await _restClient.ExecuteAsync(request);

            if (response.IsSuccessful)
                arquivo = JsonConvert.DeserializeObject<ArquivoDto>(response?.Content);

            return arquivo;
        }
    }
}
