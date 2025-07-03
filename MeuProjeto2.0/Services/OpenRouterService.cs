using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace MeuProjeto2._0.Services
{
    public class OpenRouterService
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiKey;

        public OpenRouterService(IConfiguration configuration)
        {
            _httpClient = new HttpClient();
            _apiKey = configuration["OpenRouter:ApiKey"] ?? throw new ArgumentNullException("OpenRouter:ApiKey não definido no appsettings.json");
        }

        public async Task<string> GetXmlResponseAsync(string inputText)
        {
            string prompt = $@"
Transforma o seguinte texto numa estrutura XML com os campos abaixo:

Texto:
""{inputText}""

Formato esperado:
<Atividade>
  <Data>YYYY-MM-DD</Data>
  <HoraI>HH:MM</HoraI>
  <HoraFim>HH:MM</HoraFim>
  <Ausente>HH:MM</Ausente>
  <Reg>Descrição resumida das tarefas</Reg>
</Atividade>

Responde apenas com o XML.";

            var requestBody = new
            {
                model = "openrouter/auto", // modelo gratuito e válido
                messages = new[]
                {
                    new { role = "system", content = "Tu és um assistente que extrai informação de texto e devolve XML estruturado." },
                    new { role = "user", content = prompt }
                }
            };

            var content = new StringContent(JsonSerializer.Serialize(requestBody), Encoding.UTF8, "application/json");
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _apiKey);
            _httpClient.DefaultRequestHeaders.Remove("HTTP-Referer");
            _httpClient.DefaultRequestHeaders.Remove("X-Title");

            _httpClient.DefaultRequestHeaders.Add("HTTP-Referer", "https://teuprojeto.com");
            _httpClient.DefaultRequestHeaders.Add("X-Title", "PreenchimentoFormulario");

            var response = await _httpClient.PostAsync("https://openrouter.ai/api/v1/chat/completions", content);

            if (!response.IsSuccessStatusCode)
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                throw new HttpRequestException($"Erro da API: {response.StatusCode} - {errorContent}");
            }

            var responseBody = await response.Content.ReadAsStringAsync();

            using var jsonDoc = JsonDocument.Parse(responseBody);
            string? xmlOutput = jsonDoc.RootElement
                .GetProperty("choices")[0]
                .GetProperty("message")
                .GetProperty("content")
                .GetString();

            if (string.IsNullOrWhiteSpace(xmlOutput))
                throw new Exception("Resposta da API não contém conteúdo válido.");

            return xmlOutput.Trim();
        }
    }
}
