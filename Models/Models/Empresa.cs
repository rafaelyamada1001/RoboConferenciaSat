using System.Text.Json.Serialization;

namespace Domain.Models
{
    public class Empresa
    {
        [JsonPropertyName("acao")]
        public int Acao { get; set; }

        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("status")]
        public string Status { get; set; }

        [JsonPropertyName("cnpjcpf")]
        public string CnpjCpf { get; set; }

        [JsonPropertyName("nome")]
        public string Nome { get; set; }

        public Perfil Perfil { get; set; } // Perfil da empresa
    }
    public class Perfil
    {
        [JsonPropertyName("nfeDownload")]
        public string NfeDownload { get; set; }

        [JsonPropertyName("cteDownload")]
        public string CteDownload { get; set; }

        [JsonPropertyName("nfseEmissao")]
        public string NfseEmissao { get; set; }
    }
}
