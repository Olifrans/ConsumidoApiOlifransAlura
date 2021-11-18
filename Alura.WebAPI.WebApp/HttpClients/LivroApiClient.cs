using Alura.ListaLeitura.Modelos;
using Alura.ListaLeitura.Seguranca;
using Microsoft.AspNetCore.Http;
using System;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Lista = Alura.ListaLeitura.Modelos.ListaLeitura;

namespace Alura.ListaLeitura.HttpClients
{
    //Classe que faz as requisições para as API's
    public class LivroApiClient
    {
        private readonly HttpClient _httpClient;
        private readonly IHttpContextAccessor _accessor;  //(IHttpContextAccessor) de acessar o classse que ficam fora Context de requisição 

        public LivroApiClient(HttpClient httpClient, IHttpContextAccessor accessor)
        {
            this._httpClient = httpClient;
            this._accessor = accessor;
        }

        //Metódo de autenticação e validação via Token
        private void AddBearerToken()
        {
            //Autenticação via token
            var token = _accessor.HttpContext.User.Claims.First(c => c.Type == "Token").Value;         

            //Adção do cabeçalho de autorização
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        }



        public async Task<Lista> GeListaLeituraAsync(TipoListaLeitura tipo)
        {
            //Liberação do serviço de autenticação do JWT passando o Token via cabeçalho e setando o cabeçalho
            AddBearerToken();         
            var resposta = await _httpClient.GetAsync($"/api/ListasLeitura/{ tipo}");
            resposta.EnsureSuccessStatusCode(); //200
            return await resposta.Content.ReadAsAsync<Lista>();
        }



        public async Task<byte[]> GetCapaLivroAsync(int id)
        {
            AddBearerToken();
            HttpResponseMessage resposta = await _httpClient.GetAsync($"/api/Livros/{id}/capa");
            resposta.EnsureSuccessStatusCode();
            return await resposta.Content.ReadAsByteArrayAsync();
        }



        public async Task<LivroApi> GetLivroAsync(int id)
        {
            AddBearerToken();
            HttpResponseMessage resposta = await _httpClient.GetAsync($"/api/Livros/{id}");
            resposta.EnsureSuccessStatusCode();
            return await resposta.Content.ReadAsAsync<LivroApi>();
        }



        public async Task DeleteLivroAsync(int id)
        {
            AddBearerToken();
            var resposta = await _httpClient.DeleteAsync($"/api/Livros/{id}");
            resposta.EnsureSuccessStatusCode(); //200
        }



        public async Task PostLivroAsync(LivroUpload model)
        {
            AddBearerToken();
            HttpContent content = CreateMultipartFormDataContent(model);
            var resposta = await _httpClient.PostAsync($"/api/Livros/", content);
            resposta.EnsureSuccessStatusCode(); //200
        }




        public async Task PutLivroAsync(LivroUpload model)
        {
            AddBearerToken();
            HttpContent content = CreateMultipartFormDataContent(model);
            var resposta = await _httpClient.PutAsync($"/api/Livros/", content);
            resposta.EnsureSuccessStatusCode(); //200
        }







        private string EnvolveComAspasDuplas(string valor)
        {
            return $"\"{valor}\"";
        }
        private HttpContent CreateMultipartFormDataContent(LivroUpload model)
        {
            var content = new MultipartFormDataContent();

            content.Add(new StringContent(model.Titulo), EnvolveComAspasDuplas("titulo"));
            content.Add(new StringContent(model.Lista.ParaString()), EnvolveComAspasDuplas("lista"));

            if (!string.IsNullOrEmpty(model.Subtitulo))
            {
                content.Add(new StringContent(model.Subtitulo), EnvolveComAspasDuplas("subtitulo"));
            }

            if (!string.IsNullOrEmpty(model.Resumo))
            {
                content.Add(new StringContent(model.Resumo), EnvolveComAspasDuplas("resumo"));
            }

            if (!string.IsNullOrEmpty(model.Autor))
            {
                content.Add(new StringContent(model.Autor), EnvolveComAspasDuplas("autor"));
            }        
           
            if (model.Id > 0)
            {
                content.Add(new StringContent(model.Id.ToString()), EnvolveComAspasDuplas("id"));
            }

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
            return content;
        }










    }
}
