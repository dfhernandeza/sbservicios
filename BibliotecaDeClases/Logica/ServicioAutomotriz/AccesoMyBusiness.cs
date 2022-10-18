using Google.Apis.Auth.OAuth2;
using Google.Apis.MyBusiness.v4;
using Google.Apis.MyBusiness.v4.Data;
using Google.Apis.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static BibliotecaDeClases.Logica.Traducción.TraductorProcesador;
namespace BibliotecaDeClases.Logica.ServicioAutomotriz
{
   public class AccesoMyBusiness
    {
        public static async Task<IList<Review>> getReviews()
        {
            var scopes = new List<string>()
              {
               "https://www.googleapis.com/auth/plus.business.manage",
              };
            string jsonPath = System.Web.Hosting.HostingEnvironment.MapPath("~/Credenciales/sbservicios-92245bae555d.json");
            var stream = new FileStream(jsonPath, FileMode.Open, FileAccess.Read);
            var credential = GoogleCredential.FromStream(stream).CreateScoped(scopes).CreateWithUser("dahernandez@sbservicios.cl");
            var service = new MyBusinessService(new BaseClientService.Initializer
            {
                HttpClientInitializer = credential,
                ApplicationName = "ASP.NET MVC Sample"
            });
            var locations = service.Accounts.Locations.List("accounts/103209606980336178639");
            var ubicaciones = locations.Execute();
            var list = service.Accounts.Locations.Reviews.List("accounts/103209606980336178639/locations/6491098300554863706");
            var response = await list.ExecuteAsync();
            foreach (var item in response.Reviews)
            {
                if (item.Comment != null)
                {
                    var end = item.Comment.IndexOf("\n");
                    if (end > -1)
                    {
                        var newstring = item.Comment.Substring(0, end);
                        item.Comment = newstring;
                    }
        
                }
               
            }
            foreach (var item in response.Reviews)
            {
                if (item.Comment != null)
                {
                    if (item.Comment.Contains("(Translated by Google)"))
                    {
                        var texto = item.Comment.Substring(22);
                        var traduccion = await Traducir(texto);
                        item.Comment = traduccion.FirstOrDefault().Translations.FirstOrDefault().Text;
                    }
                }
              
            }
            return response.Reviews;
        }
        public static async Task<string> getRanking()
        {
            var reviews = await getReviews();
            decimal total = 0;
            foreach (var item in reviews)
            {
                
                switch (item.StarRating)
                {
                    case "FIVE":
                        total = total + 5;
                        break;
                    case "FOUR":
                        total = total + 4;
                        break;
                    case "THREE":
                        total = total + 3;
                        break;
                    case "TWO":
                        total = total + 2;
                        break;
                    case "ONE":
                        total = total + 1;
                        break;

                }
            }
            var promedio = total / reviews.Count;
            return promedio.ToString("0.#");
        }

        
    }
}
