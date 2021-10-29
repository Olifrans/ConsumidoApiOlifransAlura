using Alura.ListaLeitura.Modelos;
using System.Net.Http;
using System.Threading.Tasks;

using Lista = Alura.ListaLeitura.Modelos.ListaLeitura;

namespace Alura.ListaLeitura.HttpClients
{
    public class LivroApiClient
    {
        private readonly HttpClient _httpClient;
        public LivroApiClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }       

        public async Task<byte[]> GetCapaLivroAsync(int id)
        {
            HttpResponseMessage resposta = await _httpClient.GetAsync($"/api/Livros/{id}/capa");
            resposta.EnsureSuccessStatusCode();
            return await resposta.Content.ReadAsByteArrayAsync();
        }

        public async Task<LivroApi> GetLivroAsync(int id)
        {
            HttpResponseMessage resposta = await _httpClient.GetAsync($"/api/Livros/{id}");
            resposta.EnsureSuccessStatusCode();
            return await resposta.Content.ReadAsAsync<LivroApi>();
        }

        public async Task DeleteLivroAsync(int id)
        {
            var resposta = await _httpClient.DeleteAsync($"/api/Livros/{id}");
            resposta.EnsureSuccessStatusCode(); //200
        }



        public async Task<Lista> GeListaLeituraAsync(TipoListaLeitura tipo)
        {
            var resposta = await _httpClient.GetAsync($"/api/ListasLeitura/{ tipo}");
            resposta.EnsureSuccessStatusCode(); //200
            return await resposta.Content.ReadAsAsync<Lista>();
        }
    }
}
