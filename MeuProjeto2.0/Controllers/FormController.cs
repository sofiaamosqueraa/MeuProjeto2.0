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
        private readonly MeuProjetoContext _context;  // **injetar contexto**

        public FormController(OpenRouterService openAIService, XmlParserService xmlParser, MeuProjetoContext context)
        {
            _openAIService = openAIService;
            _xmlParser = xmlParser;
            _context = context;  // guardar no campo privado
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
                string xmlString = await _openAIService.GetXmlResponseAsync(request.Text);
                var xmlDoc = new XmlDocument();
                xmlDoc.LoadXml(xmlString);
                var formData = _xmlParser.Parse(xmlDoc);
                return Json(formData);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return StatusCode(500, $"Erro interno: {ex.Message}");
            }
        }

        // NOVO: método para guardar o formulário no banco
        [HttpPost]
        public async Task<IActionResult> Guardar([FromBody] FormDataModel formData)
        {
            if (!ModelState.IsValid)
                return BadRequest("Dados inválidos.");

            try
            {
                _context.Formularios.Add(formData);
                await _context.SaveChangesAsync();

                return Ok("Formulário guardado com sucesso.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Erro ao guardar dados: " + ex.Message);
            }
        }
    }
}
