using BibliotecaDeClases.Modelos.ServicioAutomotriz;
using System;
using System.Threading.Tasks;
using System.Web.Mvc;
using static BibliotecaDeClases.Logica.ServicioAutomotriz.ProcesadorNeumaticos;
using static BibliotecaDeClases.Logica.Traducción.TraductorProcesador;
using static BibliotecaDeClases.Logica.ServicioAutomotriz.ProcesadorFotoLogo;
using static BibliotecaDeClases.Logica.ServicioAutomotriz.ProcesadorPromociones;
using static BibliotecaDeClases.Logica.ServicioAutomotriz.ProcesadorFotos;
using static BibliotecaDeClases.Logica.ServicioAutomotriz.ProcesadorCorreoContacto;
using static BibliotecaDeClases.Logica.ServicioAutomotriz.ProcesadorProductos;
using static BibliotecaDeClases.Logica.ServicioAutomotriz.ProcesadorMarcaProducto;
using static BibliotecaDeClases.Logica.ServicioAutomotriz.ProcesadorCategoriaFoto;
using static BibliotecaDeClases.Logica.ServicioAutomotriz.ProcesadorSecciones;
using static BibliotecaDeClases.Logica.ServicioAutomotriz.ProcesadorCategoriaMarca;
using static BibliotecaDeClases.Logica.ServicioAutomotriz.ProcesadorModelos;
using static BibliotecaDeClases.Logica.ServicioAutomotriz.ProcesadorMarcaAutos;
using static BibliotecaDeClases.Logica.ServicioAutomotriz.ProcesadorModelosAutos;
using static BibliotecaDeClases.Logica.ServicioAutomotriz.ProcesadorAñosAutos;
using static BibliotecaDeClases.Logica.ServicioAutomotriz.ProcesadorModeloAñoAuto;
using System.Collections.Generic;

namespace AppWebSantaBeatriz.Controllers
{
    [Authorize]
    public class AdminServAutoController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }
        public async Task<ActionResult> NuevoNeumatico(string iditem)
        {
            var dataTask = getItemByIDAsync(iditem);
            var anchoTask = getAnchosAsync();
            var arostask = getArosAsync();
            var marcasTask = getMarcasNeumaticosAsync();
            var perfilesTask = getPerfilesAsync();
            var fotosTask = getListFotosAsync();
            await Task.WhenAll(anchoTask, arostask, marcasTask, perfilesTask, fotosTask, dataTask);
            var data = await dataTask;
            var anchos = await anchoTask;
            var aros = await arostask;
            var marcas = await marcasTask;
            var perfiles = await perfilesTask;
            var fotos = await fotosTask;
            NeumaticosModel modelo = new NeumaticosModel
            {
                Item = data,
                Anchos = anchos,
                Aros = aros,
                Marcas = marcas,
                Perfiles = perfiles,
                Fotos = fotos
            };
            return View(modelo);
        }
        [HttpPost]
        public async Task<ActionResult> NuevoNeumatico(NeumaticosModel modelo)
        {
            
                try
                {
                    await nuevoNeumaticoAsync(modelo);
                    return Json("OK");
                }
                catch (Exception e)
                {

                    return Json(e.Message);
                }
            
        }
        [HttpGet]
        public async Task<ActionResult> EditarNeumatico(string id)
        {
            var anchoTask = getAnchosAsync();
            var arostask = getArosAsync();
            var marcasTask = getMarcasAsync();
            var perfilesTask = getPerfilesAsync();
            var dataTask =  getNeumaticoAsync(id);            
            var fotosTask = getFotosNeumaticoAsync(id);
            var fotossTask = getListFotosAsync();
            await Task.WhenAll(anchoTask, arostask, marcasTask, perfilesTask, dataTask, fotosTask, fotossTask);
            var data = await dataTask;
            var anchos = await anchoTask;
            var aros = await arostask;
            var marcas = await marcasTask;
            var perfiles = await perfilesTask;
            var modelos = await getModelosAsync(data.IDMarca);
            var fotos = await fotosTask;
            var fotoss = await fotossTask;
            data.Anchos = anchos;
            data.Aros = aros;
            data.Marcas = marcas;
            data.Perfiles = perfiles;
            data.Modelos = modelos;
            data.NeumaticoFotoList = fotos;
            data.Fotos = fotoss;
            return View(data);
        }
        [HttpPost]
        public async Task<ActionResult> EditarNeumatico(NeumaticosModel modelo)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    await editorNeumaticoAsync(modelo);
                    return Json("OK");
                }
                catch (Exception e)
                {

                    return Json(e.Message);
                }
            }
            else
            {
                return View(modelo);
            }
        }
        public async Task<ActionResult> ListaNeumaticos()
        {
            
            var data = await getListNeumaticosAsync();
            foreach (var item in data)
            {
                var fotos = await getListFotosAsync(item.ID);
                item.Fotos = fotos;
            }
            return View(data);
        }
        public async Task<JsonResult> getModelsAsync(string idmarca)
        {
            try
            {
               return Json(await getModelosAsync(idmarca));
            }
            catch (Exception e)
            {
                return Json(RedirectToAction("VistaError", "Errores", FuncionError(e.Message)));
            }
            
        }


        //Foto Logo
        //===================================================================================================
        public async Task<ActionResult> NuevaFotoLogo()
        {
            var marcas = await getMarcasAsync();
            FotoLogoModel modelo = new FotoLogoModel { Marcas = marcas };
            return View(modelo);
        }
        [HttpPost]
        public async Task<ActionResult> NuevaFotoLogo(FotoLogoModel modelo)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    await nuevaFotoLogoAsync(modelo);
                    return RedirectToAction("VerFotosLogos");
                }
                catch (Exception e)
                {

                    return RedirectToAction("VistaError", "Errores", FuncionError(e.Message));
                }
            }
            else
            {
                return View(modelo);
            }
            
        }
        public async Task<ActionResult> VerFotosLogos()
        {
            var data = await getFotoLogoListAsync();
            return View(data);
        }
        //Promociones
        //===================================================================================================
        public async Task<ActionResult> NuevaPromocion()
        {
            var fotos = await getFotosAsync();
            return View(new PromocionModel { Fotos = fotos});
        }
        [HttpPost]
        public async Task<ActionResult> NuevaPromocion(PromocionModel modelo)
        {

            await nuevaPromocionAsync(modelo);

            return RedirectToAction("VerPromociones");
        }
        public async Task<ActionResult> VerPromociones()
        {

            var data = await getPromociones();

            return View(data);
        }
        //Fotos
        //===================================================================================================
        public async Task<ActionResult> NuevaFoto()
        {
           var catsTask =  getListCategoriasFotoAsync();
            var seccionesTask =  getSeccionesListAsync();
            await Task.WhenAll(catsTask, seccionesTask);
            var cats = await catsTask;
            var secciones = await seccionesTask;

            return View(new FotoModel { CategoriasFotos = cats, Secciones = secciones });
        }
        [HttpPost]
        public async Task<ActionResult> NuevaFoto(FotoModel modelo)
        {
            await nuevaFotoSubidaAsync(modelo);
            return RedirectToAction("VerFotos");
        }
        public async Task<JsonResult> DisponibilidadNombreFoto(string nombre)
        {

          return Json(await checkNombreDisponibleAsync(nombre));
        }
        public async Task<ActionResult> EditarFoto(string id)
        {
            var catsTask =  getListCategoriasFotoAsync();
            var dataTask =  getFotoAsync(id);
            var seccionesTask = getSeccionesListAsync();
            await Task.WhenAll(catsTask, seccionesTask, dataTask);
            var cats = await catsTask;
            var secciones = await seccionesTask;
            var data = await dataTask;
            data.CategoriasFotos = cats;
            data.Secciones = secciones;
            return View(data);
        }
        [HttpPost]
        public async Task<ActionResult> EditarFoto(FotoModel modelo)
        {
            try
            {
                await editorFotoAsync(modelo);
                return RedirectToAction("VerFotos");
            }
            catch (Exception e)
            {
                return RedirectToAction("VistaError", "Errores", FuncionError(e.Message));
            }
        }
        public async Task<ActionResult> VerFotos()
        {
            var data = await getFotosAsync();
            return View(data);
        }
        public async Task<ActionResult> EliminarFoto(string id)
        {

            await eliminarFotoAsync(id);
            return RedirectToAction("VerFotos");
        }
        public  ActionResult NuevaCategoriaFoto()
        {
            return View();
        }
        [HttpPost]
        public async Task<ActionResult> NuevaCategoriaFoto(CategoriaFotoModel modelo)
        {
            await insertarCategoriaFotoAsync(modelo);
            return RedirectToAction("VerCategoriasFoto");

        }
        public async Task<ActionResult> VerCategoriasFoto()
        {
            var data = await getCategoriasFotoAsync();
            return View(data);
        }
        //Contacto
        //===================================================================================================
        public async Task<ActionResult> VerSolicitudesContacto()
        {
            var data = await getSolicitudesAsync();
            return View(data);
        }
        //Productos
        //===================================================================================================
        public async Task<ActionResult> VerInventarioAsync(bool neuma = false)
        {
            Task<List<InventarioModel>> listaTask;
            if (neuma)
            {
                listaTask =  getInventarioNeumaticosAsync();
            }
            else
            {
              listaTask =   getInventarioItemsAsync();
            }
            var catTask = selectCategorias();           
            await Task.WhenAll(catTask, listaTask);
            var data = await listaTask;
            var cat = await catTask;
            ViewBag.Categorias = cat;
            return View(data);
        }
        public async Task<JsonResult> filtroItemAsync(string text, string categoria)
        {
           
            List<InventarioModel> model;
            if (categoria == "")
            {
                model = await getItemsAsync(text);
            }
            else if (text == "")
            {
                model = await getItemsCategoriaAsync(categoria);
            }
            else
            {
                model = await getItemsAsync(text, categoria);
            }
            return Json(model);
        }
        public async Task<ActionResult> NuevoProducto(string id)
        {
            var dataTask = getItemByIDAsync(id);
            var fotosTask = getFotosSeccionAsync("PRODUCTOS");
            var catTask = getListCategoriasFotoAsync();
            var marcasTask = getListaMarcasAsync();
            await Task.WhenAll(dataTask, fotosTask, catTask, marcasTask);
            var data = await dataTask;
            var fotos = await fotosTask;
            var cats = await catTask;
            var marcas = await marcasTask;
            data.Fotos = fotos;
            data.CatsFotos = cats;
            data.Marcas = marcas;
            return View(data);
        }
        [HttpPost]
        public async Task<ActionResult> NuevoProducto(ProductoModel modelo) 
        {
            try
            {
               await nuevoProductoAsync(modelo);
               return Json("OK");
            }
            catch (Exception e)
            {
            
                return Json(e.Message);
            }
            
        }
        public async Task<JsonResult> filtrarFotosCatAsync(string id)
        {
            return Json(await getFotosAsync(id,"PRODUCTOS"));
        }
        public async Task<ActionResult> VerProductos()
        {
            var data = await getProductosAsync();
            return View(data);
        }
        public async Task<ActionResult> VerProducto(string id)
        {
            var productoTask = getProductoAsync(id);
            var fotosTask = getFotosProducto(id);
            await Task.WhenAll(productoTask, fotosTask);
            var producto = await productoTask;
            var fotos = await fotosTask;
            producto.FotosProducto = fotos;
            return View(producto);
        }
        public async Task<ActionResult> EditarProducto(string id)
        {
            var productTask =  getProductoAsync(id);
            var fotosTask = getFotosSeccionAsync("PRODUCTOS");
            var catTask = getListCategoriasFotoAsync();
            var marcasTask = getListaMarcasAsync();
            var fotosProductoTask = getFotosProductoList(id);
            await Task.WhenAll(productTask, fotosTask, catTask, marcasTask, fotosProductoTask);
            var product = await productTask;
            var fotos = await fotosTask;
            var cats = await catTask;
            var marcas = await marcasTask;
            var fotosProducto = await fotosProductoTask;
            product.Fotos = fotos;
            product.CatsFotos = cats;
            product.Marcas = marcas;
            product.FotoProductoList = fotosProducto;
            return View(product);
        }
        [HttpPost]
        public async Task<ActionResult> EditarProducto(ProductoModel modelo)
        {

            try
            {
                await edicionProductoAsync(modelo);
                return Json("OK");
            }
            catch (Exception e)
            {

                return Json(e.Message);
            }
        }
        
        //Marcas
        //===================================================================================================
        public async Task<ActionResult> NuevaMarcaProducto()
        {
            var categorias = await getCategoriasMarcaListAsync();

            return View(new MarcaProductoModel {Categorias = categorias });
        }
        [HttpPost]
        public async Task<ActionResult> NuevaMarcaProducto(MarcaProductoModel modelo)
        {
            try
            {
                await insertarMarcaPrductoAsync(modelo);
                return RedirectToAction("VerMarcas");
            }
            catch (Exception e)
            {

                return RedirectToAction("VistaError", "Errores", FuncionError(e.Message));
            }
         
        }

        public async Task<ActionResult> EditarMarcaProducto(string id)
        {
            var categoriasTask = getCategoriasMarcaListAsync();            
            var dataTask = getMarcaProducto(id);
            await Task.WhenAll(categoriasTask, dataTask);
            var data = await dataTask;
            var categorias = await categoriasTask;
            data.Categorias = categorias;
            return View(data);
        }
        [HttpPost]
        public async Task<ActionResult> EditarMarcaProducto(MarcaProductoModel modelo)
        {
            try
            {
                await updateMarcaProductoAsync(modelo);
                return RedirectToAction("VerMarcas");
            }
            catch (Exception e)
            {

                return RedirectToAction("VistaError", "Errores", FuncionError(e.Message));
            }
         
        }
        public async Task<ActionResult> VerMarcas()
        {
            var data = await getMarcasProductos();
            return View(data);
        }
        public async Task<JsonResult> getPicturesProdcuts()
        {
            var data = await getFotosAsync();
            return Json(data);
        }

        public async Task<ActionResult> VerMarca(string id)
        {
            var marca = await getMarcaProducto(id);
            var data = await getModelosMarcaAsync(id);
            marca.Modelos = data;
            return View(marca);
        }

        //Categorias Marcas
        //===================================================================================================
        public  ActionResult NuevaCategoriaMarca()
        {
            return View();
            
        }
        [HttpPost]
        public async Task<ActionResult> NuevaCategoriaMarca(CategoriaMarcaModel modelo)
        {
            try
            {
                await insertarNuevaCategoriaMarcaAsync(modelo);
                return RedirectToAction("VerCategoriasMarcas");
            }
            catch (Exception e)
            {
                return RedirectToAction("VistaError", "Errores", FuncionError(e.Message));

            }
          

        }
        public async Task<ActionResult> EditarCategoriaMarca(string id)
        {
            var data = await getCategoriaMarcaAsync(id);
            return View(data);
        }
        [HttpPost]
        public async Task<ActionResult> EditarCategoriaMarca(CategoriaMarcaModel modelo)
        {
            try
            {
                await updateCategoriaMarcaAsync(modelo);
                return RedirectToAction("VerCategoriasMarcas");
            }
            catch (Exception e)
            {

                return RedirectToAction("VistaError", "Errores", FuncionError(e.Message));
            }
            
            
        }
        public async Task<ActionResult> VerCategoriasMarcas()
        {
            var data = await getCategoriasMarcaAsync();
            return View(data);
        }

        //Secciones
        //===================================================================================================
        public ActionResult NuevaSeccion()
        {
            return View();
        }
        [HttpPost]
        public async Task<ActionResult> NuevaSeccion(SeccionesModel modelo)
        {
            try
            {
                await insertarSeccionAsync(modelo);
                return RedirectToAction("VerSecciones");
            }
            catch (Exception e)
            {
                return RedirectToAction("VistaError", "Errores", FuncionError(e.Message));
            }
         
        }
        public async Task<ActionResult> EditarSeccion(string id)
        {
            var data = await getSeccionAsync(id);
            return View(data);
        }
        [HttpPost]
        public async Task<ActionResult> EditarSeccion(SeccionesModel modelo)
        {
            try
            {
                await editarSeccionAsync(modelo);
                return RedirectToAction("VerSecciones");
            }
            catch (Exception e)
            {
                return RedirectToAction("VistaError", "Errores", FuncionError(e.Message));
            }

        }
        public async Task<ActionResult> VerSecciones()
        {
            var data = await getSeccionesAsync();
            return View(data);
        }
        //Modelos
        //=====================================================================================================
        public ActionResult NuevoModelo(string idmarca, string marca)
        {
            return View(new ModeloModel {IDMarca = idmarca, Marca = marca });
        }
        [HttpPost]
        public async Task<ActionResult> NuevoModelo(ModeloModel model)
        {
            try
            {
                await insertarModeloAsync(model);
                return RedirectToAction("VerMarca", new { id = model.IDMarca });
            }
            catch (Exception e)
            {
                return RedirectToAction("VistaError", "Errores", FuncionError(e.Message));
            }
        }

        //Administración Marcas, Modelos y Años de Vehículos
        //
        public async Task<ActionResult> VerMarcasAutos()
        {
            var data = await selectMarcasAutosAsync();
            return View(data);
        }
        public  async Task<ActionResult> VerMarcaAuto(string id)
        {
            var data = await selectModelosAutosAsync(id);
            var auto = await selectMarcaAutoAsync(id);
            PaginaModelosAuto modelo = new PaginaModelosAuto { 
            IDMarca = id,
            MarcaAuto = auto.MarcaAuto,
            Modelos = data
            };
            return View(modelo);
           
        }
        public async Task<ActionResult> VerModeloAuto(string id)
        {
            var años = await selectListItemAñosAutosAsync();
            var auto = await selectModeloAutoAsync(id);
            var añosSeleccionados = await selectAñosModeloAsync(id);
            foreach(var item in años)
            {
                foreach (var elemento in añosSeleccionados)
                {
                    if (item.ID == elemento.IDAño)
                    {
                        item.Selected = true;

                    }
                   
                }

            }




            AñadirAñosPaginaModel modelo = new AñadirAñosPaginaModel
            {
               IDModeloAuto = id,
               Años = años,
               ModeloAuto = auto.ModeloAuto
            };
            List<string> lista = new List<string>();

            foreach (var item in añosSeleccionados)
            {
                lista.Add(item.IDAño);
            }
            modelo.AñosSeleccionados = lista;
            return View(modelo);

        }
  
        public async Task<JsonResult> AñadirAñosAsync(List<ModeloAutoAñoModel> modelo)
        {
  
            try
            {
                await insertModeloAñoAutoAsync(modelo);
                return Json("OK");
            }
            catch (Exception e)
            {

                return Json(e.Message);
            }
         
        }
    
    }
}