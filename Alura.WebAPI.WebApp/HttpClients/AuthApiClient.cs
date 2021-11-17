using Alura.ListaLeitura.Seguranca;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Alura.ListaLeitura.HttpClients
{
    //Classe que representa o resultado login do Identity
    public class LoginResul
    {
        public bool Succeeded { get; set; }
        public string Token { get; set; }
    }


    //Classe que gerencia a autenticação via token JWT
    public class AuthApiClient
    {
        private readonly HttpClient _httpClient;
        public AuthApiClient(HttpClient httpClient)
        {
            this._httpClient = httpClient;
        }

        public async Task<LoginResul> PostLoginAsync(LoginModel model)
        {
            var resposta = await _httpClient.PostAsJsonAsync("login", model);
            return new LoginResul
            {
                Succeeded = resposta.IsSuccessStatusCode,
                Token = await resposta.Content.ReadAsStringAsync(),
            };
        }
    }
}
