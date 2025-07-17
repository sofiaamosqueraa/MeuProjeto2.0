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
            // Obter hora atual do servidor no formato HH:mm
            string horaAtual = DateTime.Now.ToString("HH:mm");

            // Substituir expressões vagas pela hora real
            inputText = inputText
                .Replace("agora", horaAtual, StringComparison.OrdinalIgnoreCase)
                .Replace("neste momento", horaAtual, StringComparison.OrdinalIgnoreCase)
                .Replace("nesta altura", horaAtual, StringComparison.OrdinalIgnoreCase);

            string prompt = $@"
Transforma o seguinte texto numa estrutura XML com os campos abaixo.

Texto:
""{inputText}""

Formato esperado:
<Atividade>
  <Empresa>Nome da empresa</Empresa>
  <EmpresaDescricao>Breve descrição da empresa</EmpresaDescricao>
  <PedidoPor>Nome da pessoa que fez o pedido</PedidoPor>
  <Pedido>Resumo da tarefa principal a executar</Pedido>
  <Data>YYYY-MM-DD</Data>
  <HoraI>HH:MM</HoraI>
  <HoraFim>HH:MM</HoraFim>
  <Ausente>HH:MM</Ausente>
  <Gastos>Valor total em euros</Gastos>
  <TempoDeslocacao>Duração da deslocação no formato HH:MM</TempoDeslocacao>
  <Reg>Descrição clara e profissional da tarefa</Reg>
</Atividade>



- Preenche os campos com base no conteúdo do texto.
- No campo <Empresa>, escreve exatamente o nome da empresa mencionada no texto, mesmo que venha após as palavras ""na empresa"", ""em"", ""visitei"", ""trabalhei para"", etc.
- Mesmo que o nome da empresa esteja no meio da frase, extrai-o tal como aparece.
- Se não for possível identificar algo, deixa-o vazio.
- Para EmpresaDescricao, escreve uma descrição genérica da empresa com base no nome, se for conhecida.
- Para PedidoPor, tenta identificar o nome da pessoa associada ao pedido, ou que confirmou, ou que interagiu diretamente no processo como clientes ou funcionários.
- A descrição das tarefas deve ser clara, profissional, com frases completas e bem estruturadas.
- Responde apenas com o XML.
- Gastos: valor total estimado em euros.
- TempoDeslocacao: duração real da deslocação no formato HH:MM.
- O campo Pedido deve ser um resumo claro da principal tarefa.
";



            var requestBody = new
            {
                model = "openrouter/auto", // modelo gratuito e válido
                messages = new[]
                {
                    new { role = "system", content = "Tu és um assistente que extrai informação de texto e devolve XML estruturado." },
                    new { role = "user", content = prompt }
                },
                max_tokens = 1000
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

            Console.WriteLine("XML RECEBIDO DA IA:\n" + xmlOutput);

            return xmlOutput.Trim();
        }
    }
}
