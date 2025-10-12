using ClassLibrary_CSharp.Encryption;
using MiGente_Front.Data;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using RestSharp;

namespace MiGente_Front.Services
{
    public class PaymentService
    {
        public async Task<dynamic> consultarIdempotency(string url)
        {


            // Configura HttpClient
            using (HttpClient client = new HttpClient())
            {
                // Configura el encabezado "Accept" con "text/plain"
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("text/plain"));

                // Realiza la solicitud POST al servicio web
                HttpResponseMessage response = await client.PostAsync(url.Replace("/transactions/", "/idenpotency-keys"), null);

                // Verifica si la solicitud fue exitosa
                if (response.IsSuccessStatusCode)
                {
                    // Lee la respuesta en formato de texto
                    string plainTextResponse = await response.Content.ReadAsStringAsync();
                    return new
                    {
                        success = true,
                        message = "Llave Generada",
                        result = plainTextResponse
                    };

                }
                else
                {
                    return null;
                }
            }

        }
        Crypt crypt = new Crypt();
        public async Task<PaymentResponse> Payment(string cardNumber, string cvv,
         decimal amount, string clientIP, string expirationDate,
       string referenceNumber, string invoiceNumber)
        {

            var gatewayParams = getPaymentParameters();
            string url = "";
            if (gatewayParams.test == true)
            {
                url = gatewayParams.testURL;
            }
            else
            {
                url = gatewayParams.productionURL;

            }

            var result = await consultarIdempotency(url);
            string idempotency = "";
            if ((bool)result.success)
            {
                idempotency = result.result?.Substring("ikey:".Length);
            }
            else
            {
                return null;
            }

            var client = new RestClient(url + "sales");

            var jsonBody = $@"
                  {{
                        ""amount"": {amount},
                        ""card-number"":""{crypt.Decrypt(cardNumber)}"",
                        ""client-ip"": ""{clientIP}"",
                        ""currency"": ""214"",
                        ""cvv"": ""{cvv}"",
                        ""environment"": ""ECommerce"",
                        ""expiration-date"": ""{expirationDate}"",
                        ""idempotency-key"": ""{idempotency}"",
                        ""merchant-id"": ""{gatewayParams.merchantID}"",
                        ""reference-number"": ""{referenceNumber}"",
                        ""terminal-id"": ""{gatewayParams.terminalID}"",
                        ""token"": ""454500350001"",
                        ""invoice-number"":""{invoiceNumber}""
                  }}";


            var request = new RestRequest(url + "sales", Method.Post);
            request.AddHeader("Content-Type", "application/json");
            request.AddParameter("application/json", jsonBody, ParameterType.RequestBody);

            RestResponse response = client.Execute(request);

            Console.WriteLine(response.Content);

            PaymentResponse paymentResponse = JsonConvert.DeserializeObject<PaymentResponse>(response.Content);

            return paymentResponse;
        }

        public class PaymentResponse
        {
            [JsonProperty(PropertyName = "idempotency-key")]
            public string IdempotencyKey { get; set; }
            [JsonProperty(PropertyName = "response-code")]
            public string ResponseCode { get; set; }
            [JsonProperty(PropertyName = "internal-response-code")]

            public string InternalResponseCode { get; set; }
            [JsonProperty(PropertyName = "response-code-desc")]

            public string ResponseCodeDesc { get; set; }
            [JsonProperty(PropertyName = "response-code-source")]

            public string ResponseCodeSource { get; set; }
            [JsonProperty(PropertyName = "approval-code")]

            public string ApprovalCode { get; set; }
            [JsonProperty(PropertyName = "pnRef")]

            public string PnRef { get; set; }
        }

        public PaymentGateway getPaymentParameters()
        {
            using (migenteEntities db = new migenteEntities())
            {

                var result = db.PaymentGateway.FirstOrDefault();
                return result;


            };
        }

    }
}