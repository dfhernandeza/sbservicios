using BibliotecaDeClases.Modelos.Traductor;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;


namespace BibliotecaDeClases.Logica.Traducción
{
  public  class TraductorProcesador
    {
        private static readonly string subscriptionKey = "2ee83e89a85247c08134a9cfd7f5c088";
        private static readonly string endpoint = "https://api.cognitive.microsofttranslator.com/";

        // Add your location, also known as region. The default is global.
        // This is required if using a Cognitive Services resource.
        private static readonly string location = "Global";


      public static async Task<TranslationResult[]> Traducir(string texto)
        {
            // Input and output languages are defined as parameters.
            string route = "/translate?api-version=3.0&from=en&to=es";
            string textToTranslate = texto;
            object[] body = new object[] { new { Text = textToTranslate } };
            var requestBody = JsonConvert.SerializeObject(body);

            using (var client = new HttpClient())
            using (var request = new HttpRequestMessage())
            {
                // Build the request.
                request.Method = HttpMethod.Post;
                request.RequestUri = new Uri(endpoint + route);
                request.Content = new StringContent(requestBody, Encoding.UTF8, "application/json");
                request.Headers.Add("Ocp-Apim-Subscription-Key", subscriptionKey);
                request.Headers.Add("Ocp-Apim-Subscription-Region", location);

                // Send the request and get response.
                HttpResponseMessage response = await client.SendAsync(request).ConfigureAwait(false);
                // Read response as a string.
                string result = await response.Content.ReadAsStringAsync();
                
                TranslationResult[] deserializedOutput = JsonConvert.DeserializeObject<TranslationResult[]>(result);
                return deserializedOutput;
            }
        }
    
     


        public static Error FuncionError(string mensaje)
        {
            var datos =  Traducir(mensaje).Result.FirstOrDefault().Translations[0];

            Error error = new Error
            {
                Mensaje = datos.Text
            };

            return error;
        }
    
    
    }
}
