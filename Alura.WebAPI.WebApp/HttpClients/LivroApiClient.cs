using Alura.ListaLeitura.Modelos;
using System;
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






        private string EnvolveComAspasDuplas(string valor)
        {
            return $"\"{valor}\"";
        }
        private HttpContent CreateMultipartFormDataContent(LivroUpload model)
        {
            var content = new MultipartFormDataContent();
            content.Add(new StringContent(model.Titulo), EnvolveComAspasDuplas("titulo"));
            content.Add(new StringContent(model.Subtitulo), EnvolveComAspasDuplas("subtitulo"));
            content.Add(new StringContent(model.Autor), EnvolveComAspasDuplas("autor"));
            content.Add(new StringContent(model.Resumo), EnvolveComAspasDuplas("resumo"));

            if (model.Capa != null)
            {
                var imagemContent = new ByteArrayContent(model.Capa.ConvertToBytes());
                imagemContent.Headers.Add("content-type", "image/png"); //poderia ser um arquivo (PDF, Excel etc.)
                content.Add(
                    imagemContent,
                    EnvolveComAspasDuplas("capa"), //campo
                    EnvolveComAspasDuplas("capa.png") //nome do arquivo                  
                );
            }

            content.Add(new StringContent(model.Lista.ParaString()), EnvolveComAspasDuplas("lista"));            
            return content;
        }

        public async Task PostLivroAsync(LivroUpload model)
        {
            HttpContent content = CreateMultipartFormDataContent(model);
            var resposta = await _httpClient.PostAsync($"/api/Livros", content);
            resposta.EnsureSuccessStatusCode(); //200
        }        
    }
}
