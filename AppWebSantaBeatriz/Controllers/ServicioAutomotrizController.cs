using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using static BibliotecaDeClases.Logica.ServicioAutomotriz.ProcesadorFotos;
using BibliotecaDeClases.Modelos.ServicioAutomotriz;
using static BibliotecaDeClases.Logica.ServicioAutomotriz.ProcesadorCorreoContacto;
using static BibliotecaDeClases.Logica.ServicioAutomotriz.AccesoMyBusiness;
using Google.Apis.MyBusiness.v4.Data;
using static BibliotecaDeClases.Logica.ServicioAutomotriz.ProcesadorProductos;
using static BibliotecaDeClases.Logica.ServicioAutomotriz.ProcesadorNeumaticos;
namespace AppWebSantaBeatriz.Controllers
{
    public class ServicioAutomotrizController : Controller
    {
        // GET: ServicioAutomotriz
        public async Task<ActionResult> Home()
        {
  
            //var reviewsTask =  getReviews();
            var fotosTask =  getFotosAsync();
            //var rankingtask = getRanking();
            var productsTask =  getProductosAsync();
            var neumaticosTask =  getListNeumaticosAsync();
            await Task.WhenAll(fotosTask, productsTask, neumaticosTask);
            var productos = await productsTask;
            var neumas = await neumaticosTask;
            foreach (var item in neumas)
            {
                productos.Add(new ProductoModel { 
                    Nombre = item.Ancho + "/" + item.Perfil + "R" + item.Aro + " " + item.Modelo  + " " + item.Marca,
                    Fotos = item.Fotos
                });
            }
            var fotos = await fotosTask;
            //var reviews = await reviewsTask;
            //var ranking = await rankingtask;
            //List<Review> list = new List<Review>();
            //list.Add(reviews[0]);
            return View(new FrontPageModel { Fotos = fotos, Productos = productos });
        }

        public  ActionResult FormularioContacto()
        {

            return View();
        }
        
        [HttpPost]
        public async Task<ActionResult> FormularioContactos(ContactoCorreoModel modelo)
        {
            await nuevoContactoAsync(modelo);
            return RedirectToAction("Home");
        }

        public async Task<ActionResult> Tienda()
        {
            var products = await getProductosAsync();
            var secciones = await getFotosSeccionOpcionesAsync();
            return View(new TiendaModel { Productos = products, FotosOpciones = secciones });
        }
        public async Task<ActionResult> Neumaticos()
        {
            var dataTask = getListNeumaticosAsync();
            var arosTask = getArosAsync();
            var anchosask = getAnchosAsync();
            var perfilesTask = getPerfilesAsync();
            await Task.WhenAll(dataTask, arosTask, anchosask, perfilesTask);
            var data = await dataTask;
            var aros = await arosTask;
            var anchos = await anchosask;
            var perfiles = await perfilesTask;
            return View(new PagNeumaticosModel {Neumaticos = data, Aros = aros, Anchos = anchos, Perfiles = perfiles });
        }
        public ActionResult Carrito()
        {
            return View();
        }    
    }
    
   
}