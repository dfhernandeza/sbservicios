using AppWebSantaBeatriz.Models;
using BibliotecaDeClases.Logica;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using static BibliotecaDeClases.Logica.ProcesadorMateriales;
using static BibliotecaDeClases.Logica.Funciones;
using static BibliotecaDeClases.Logica.Traducción.TraductorProcesador;
using System.Threading.Tasks;
namespace AppWebSantaBeatriz.Controllers
{
    [Authorize]
    public class CotMaterialController : Controller
    {
        // GET: CotMaterial
        public ActionResult Index()
        {
            return View();
            
        
        }

        public async Task<ActionResult> VerMateriales(int id)




        {
            ViewBag.id = id;
            ViewBag.Total = TotalMaterialAsync(id);
            var data = await LoadCotMaterialesAsync(id);
            if (data.Count == 0)
            {
                List<CotizacionMaterialesModel> materiales = new List<CotizacionMaterialesModel>();
                return View(materiales);

            }
            else
            {

                List<CotizacionMaterialesModel> materiales = new List<CotizacionMaterialesModel>();
                foreach (var row in data)
                {
                    CotizacionMaterialesModel material = new CotizacionMaterialesModel
                    {
                        ID = row.ID,
                        IDCot = row.IDCot,
                        Cantidad = row.Cantidad,
                        Item = row.Item,
                        PUnitario = row.PUnitario,
                        Unidad = row.Unidad

                    };

                    materiales.Add(material);

                }


                return View(materiales);

            }

        }
      
        public async Task<JsonResult> BorrarMaterial(int id)
        {
            // TODO: Add delete logic here
            try
            {
              await BorraMaterialAsync(id);

                return Json("Material eliminado", JsonRequestBehavior.AllowGet);
            }
            catch(Exception e)
            {
                return Json(RedirectToAction("VistaError", "Errores", FuncionError(e.Message)));
            }
          



        }

        [HttpPost]
        public async Task<ActionResult> CotiMaterial(CotizacionMaterialesModel modelo)
        {
     
            try
            {
               await CreaCotMaterialAsync(modelo.IDCot, modelo.Item, modelo.Unidad, (decimal)modelo.Cantidad, (decimal)modelo.PUnitario, modelo.IDCategoria, User.Identity.Name);

                return RedirectToAction("VerMateriales", "CotMaterial", new { id = modelo.IDCot });
            }
            catch
            {
                return View();
            }
        }

        public async Task<JsonResult> NuevoMaterial(int idcot, string item,  string unidad,  decimal punitario, decimal cantidad, int idcategoria)
        {
            try
            {
              await  CreaCotMaterialAsync(idcot, item.ToUpperCheckForNull(), unidad.ToUpperCheckForNull(), cantidad, punitario, idcategoria, User.Identity.Name);
                return Json("Material ingresado", JsonRequestBehavior.AllowGet);
            }
            catch(Exception e)
            {
                return Json(RedirectToAction("VistaError", "Errores", FuncionError(e.Message)));
            }
            
            
        }


        [HttpGet]
        public ActionResult CotiMaterial(int id)
        {

            CotizacionMaterialesModel modelo = new CotizacionMaterialesModel
            {
                IDCot = id,
                PUnitario = null,
                Cantidad = null,


            };


            ViewBag.id = id;
            return PartialView(modelo);

        }  
       
        public async Task<JsonResult> EditaMaterial(int idcot, string item, string unidad, decimal punitario, decimal cantidad, int idcategoria)
        {
            try
            {
              await  EditarMaterialAsync(idcot, item.ToUpperCheckForNull(), unidad.ToUpperCheckForNull(), cantidad, punitario, idcategoria, User.Identity.Name);

                return Json("Material editado", JsonRequestBehavior.AllowGet);
            }
            catch(Exception e)
            {
                return Json(RedirectToAction("VistaError", "Errores", FuncionError(e.Message)));
            }
                   


        }

        public JsonResult DetalleMateriales(int idcotizacion)
        {
            return Json(LoadCotMaterialesAsync(idcotizacion));
        }

        public async Task<JsonResult> ListadoMaterialesCompletoAsync(int idcot)
        {
           
            return Json(await ListaTodosMaterialesAsync(idcot));
        }


    }


}