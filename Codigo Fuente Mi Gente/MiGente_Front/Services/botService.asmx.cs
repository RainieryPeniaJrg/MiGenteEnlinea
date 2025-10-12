using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Script.Services;
using System.Web.Services;

namespace MiGente_Front.Services
{
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    [ScriptService]
    public class botService : System.Web.Services.WebService
    {

        [WebMethod]
        public async Task<string> GetChatResponse(string userMessage)
        {
            BotServices bs = new BotServices();
            var openAi = bs.getOpenAI();



            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Clear(); // Limpia cualquier encabezado anterior
            client.DefaultRequestHeaders.Add("Authorization", $"Bearer {openAi.OpenAIApiKey}");

            var requestBody = new
            {
                model = "gpt-3.5-turbo",
                messages = new[]
                {
            new { role = "system", content = "Eres un abogado virtual especializado en derecho laboral de la República Dominicana." },
            new { role = "user", content = userMessage }
        }
            };

            string jsonRequestBody = JsonConvert.SerializeObject(requestBody);
            StringContent content = new StringContent(jsonRequestBody, Encoding.UTF8, "application/json");

            try
            {
                HttpResponseMessage response = await client.PostAsync(openAi.OpenAIApiUrl, content);

                // Verifica si la respuesta es exitosa
                if (response.IsSuccessStatusCode)
                {
                    string jsonResponse = await response.Content.ReadAsStringAsync();
                    dynamic parsedResponse = JsonConvert.DeserializeObject(jsonResponse);
                    return parsedResponse.choices[0].message.content.ToString().Trim();
                }
                else
                {
                    response.EnsureSuccessStatusCode();
                    return string.Empty;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al llamar la API: {ex.Message}");
                throw;
            }
        }
        }
}
