using MiGente_Front.Services;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace MiGente_Front
{
    public partial class abogadoVirtual : System.Web.UI.Page
    {
   
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BotServices bs = new BotServices();
                var openAi = bs.getOpenAI();
                Session["key"] = openAi.OpenAIApiKey;
                Session["apiURL"] = openAi.OpenAIApiUrl;
         
            }
        }
        protected async void btnSendMessage_Click(object sender, EventArgs e)
        {
            string userMessage = txtUserMessage.Text.Trim();
            if (!string.IsNullOrEmpty(userMessage))
            {

                userMessage = $"<div class='message-user'>{userMessage}</div>";
                // Mostrar mensaje del usuario en el chat
                AddMessageToChat(userMessage, "user");

                // Obtener respuesta del bot desde OpenAI
                string botResponse = await GetChatGPTResponse(userMessage);

                // Mostrar respuesta del bot
                AddMessageToChat(botResponse, "bot");

                // Limpiar cuadro de texto
                txtUserMessage.Text = "";
            }
        }
        // Crea un HttpClient estático para usarlo durante toda la vida útil de la aplicación
        private static readonly HttpClient client = new HttpClient();

        private async Task<string> GetChatGPTResponse(string userMessage)
        {
            client.DefaultRequestHeaders.Clear();
            client.DefaultRequestHeaders.Add("Authorization", $"Bearer {Session["key"]}");

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
                HttpResponseMessage response = await client.PostAsync(Session["apiURL"].ToString(), content);

                if (response.IsSuccessStatusCode)
                {
                    string jsonResponse = await response.Content.ReadAsStringAsync();
                    dynamic parsedResponse = JsonConvert.DeserializeObject(jsonResponse);

                    if (parsedResponse?.choices != null && parsedResponse.choices.Count > 0)
                    {
                        string responseMessage = parsedResponse.choices[0].message.content.ToString().Trim();

                        // Asignamos la clase 'message-lawyer' para el mensaje del abogado
                        string messageHtml = $"<div class='message-lawyer'>{responseMessage}</div>";
                        return messageHtml;
                    }
                    else
                    {
                        return "<div class='message-error'>Error: No se obtuvo respuesta válida.</div>";
                    }
                }
                else
                {
                    Console.WriteLine($"Error en la respuesta de la API: {response.StatusCode}");
                    return "<div class='message-error'>Error al contactar el servicio.</div>";
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return "<div class='message-error'>Error inesperado.</div>";
            }
        }




        private void AddMessageToChat(string message, string sender)
        {
            string cssClass = sender == "bot" ? "chat-bubble bot-message" : "chat-bubble user-message";
            chatMessages.InnerHtml += $"<div class='{cssClass}'>{message}</div>";
        }

    }
}