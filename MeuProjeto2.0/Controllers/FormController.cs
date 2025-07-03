using Microsoft.AspNetCore.Mvc;
using MeuProjeto2._0.Models;
using MeuProjeto2._0.Services;
using System;
using System.Threading.Tasks;
using System.Xml;

namespace MeuProjeto2._0.Controllers
{
    public class FormController : Controller
    {
        private readonly OpenRouterService _openAIService;
        private readonly XmlParserService _xmlParser;

        public FormController(OpenRouterService openAIService, XmlParserService xmlParser)
        {
            _openAIService = openAIService;
            _xmlParser = xmlParser;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ProcessText([FromBody] TextRequestModel request)
        {
            if (string.IsNullOrWhiteSpace(request.Text))
                return BadRequest("Texto inválido.");

            try
            {
                // 1. Obtemos a resposta da IA (em XML, mas como string)
                string xmlString = await _openAIService.GetXmlResponseAsync(request.Text);

                // (DEBUG) Mostra o XML no console
                Console.WriteLine("===== XML Gerado pela IA =====");
                Console.WriteLine(xmlString);
                Console.WriteLine("================================");

                // 2. Convertendo string em XmlDocument
                var xmlDoc = new XmlDocument();
                xmlDoc.LoadXml(xmlString); // Pode lançar exceção se XML malformado

                // 3. Fazendo o parsing para um modelo
                var formData = _xmlParser.Parse(xmlDoc);

                // 4. Retornando os dados como JSON para preencher o formulário
                return Json(formData);
            }
            catch (Exception ex)
            {
                // Mostrar erro no console
                Console.WriteLine("ERRO AO PROCESSAR:");
                Console.WriteLine(ex.ToString());

                // Devolver erro visível no frontend
                return StatusCode(500, $"Erro interno: {ex.Message}");
            }
        }
    }
}





