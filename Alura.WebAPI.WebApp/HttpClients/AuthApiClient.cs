using Alura.ListaLeitura.Seguranca;
using Alura.ListaLeitura.WebApp.Models;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace Alura.ListaLeitura.HttpClients
{
    //Classe que representa o resultado login do Identity
    public class LoginResult
    {
        public bool Succeeded { get; set; }
        public string Token { get; set; }

        public LoginResult(string token, HttpStatusCode statusCode)
        {
            Token = token;
            Succeeded = (statusCode == HttpStatusCode.OK);
        }
    }

    //Classe que gerencia a autenticação via token JWT
    public class AuthApiClient
    {
        private readonly HttpClient _httpClient;

        public AuthApiClient(HttpClient httpClient)
        {
            this._httpClient = httpClient;
        }

        public async Task<LoginResult> PostLoginAsync(LoginModel model)
        {
            var resposta = await _httpClient.PostAsJsonAsync<LoginModel>("login", model);
            return new LoginResult(await resposta.Content.ReadAsStringAsync(), resposta.StatusCode);
        }

        public async Task PostRegisterAsync(RegisterViewModel model)
        {
            var resposta = await _httpClient.PostAsJsonAsync<RegisterViewModel>("usuarios", model);
            resposta.EnsureSuccessStatusCode();
        }
    }
}