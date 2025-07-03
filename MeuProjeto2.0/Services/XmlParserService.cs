using System.Xml;
using MeuProjeto2._0.Models;

namespace MeuProjeto2._0.Services
{
    public class XmlParserService
    {
        public FormDataModel Parse(XmlDocument xmlDoc)
        {
            var root = xmlDoc.DocumentElement ?? throw new ArgumentException("Documento XML inválido: root é nulo.");

            return new FormDataModel
            {
                Data = root.SelectSingleNode("//Data")?.InnerText ?? string.Empty,
                HoraI = root.SelectSingleNode("//HoraI")?.InnerText ?? string.Empty,
                HoraFim = root.SelectSingleNode("//HoraFim")?.InnerText ?? string.Empty,
                Ausente = root.SelectSingleNode("//Ausente")?.InnerText ?? string.Empty,
                Reg = root.SelectSingleNode("//Reg")?.InnerText ?? string.Empty
            };
        }
    }
}
