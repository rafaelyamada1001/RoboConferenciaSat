using System.Text.Json.Serialization;

namespace Domain.Models
{
    public class Usuario
    {
        public Usuario(string token, string email)
        {
            Token = token;
            Email = email;
        }

        [JsonPropertyName("autorizado")]
        public bool Autorizado { get; set; }

        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("nome")]
        public string Nome { get; set; }

        [JsonPropertyName("senha")]
        public string Token { get; set; }

        [JsonPropertyName("msg")]
        public string? Mensagem { get; set; }

        [JsonPropertyName("status")]
        public string Status { get; set; }
        public string Email { get; set; }

        [JsonPropertyName("empresa")]
        public List<Empresa> Empresas { get; set; }
    }
}
