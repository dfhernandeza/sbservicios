using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using AppWebSantaBeatriz.Models;
using AppWebSantaBeatriz.Models.Clientes;
using AppWebSantaBeatriz.Models.Error;
using static BibliotecaDeClases.Logica.Clientes.ProcesadorClientes;
using static BibliotecaDeClases.Logica.Funciones;
using static BibliotecaDeClases.Logica.Traducción.TraductorProcesador;
using System.Threading.Tasks;
namespace AplicacionWebSB.Controllers
{
    [Authorize]
    public class ClientesController : Controller
    {
       
        public async Task<ActionResult> Detalles(int id)
        {
            var contactos = await ListaDeContactosAsync(id);
            List<ContactoModel> listacontactos = new List<ContactoModel>();
            foreach(var datos in contactos)
            {
                listacontactos.Add(new ContactoModel { ID = datos.ID, Nombre = datos.Nombre, Apellido = datos.Apellido, Cargo = datos.Cargo, Fono = datos.Fono, Email = datos.Email });
            }


            var ubicaciones = await ListaUbicacionesAsync(id);
            List<UbicacionesModel> listaubicaciones = new List<UbicacionesModel>();
            foreach(var fila in ubicaciones)
            {
                listaubicaciones.Add(new UbicacionesModel { ID = fila.ID, Nombre = fila.Nombre, Direccion = fila.Direccion, Ciudad = fila.Ciudad, Alias = fila.Alias });
            }

            var data = await LoadClienteAsync(id);
            ClienteModel model = new ClienteModel
            {
                IDCliente = FormatRutView(data.IDCliente),
                RazonSocial = data.RazonSocial,
                Alias = data.Alias,
                Giro = data.Giro,
                IDSinFormato = data.IDCliente,
                ID = data.ID,
                Contactos = listacontactos,
                Ubicaciones = listaubicaciones


            };


            return View(model);
        }
        public ActionResult Crear()
        {
            ClienteModel cliente = new ClienteModel
            {
                Giro = null
            };
            return View(cliente);
        }

        [HttpPost]
        public ActionResult Crear(ClienteModel modelo)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    CrearCliente(GetRut(modelo.IDCliente), modelo.RazonSocial.ToUpperCheckForNull(), modelo.Alias.ToUpperCheckForNull(), modelo.Giro.ToUpperCheckForNull(), modelo.Arauco, User.Identity.Name);

                    return RedirectToAction("VerClientes");
                }
                catch(Exception e)
                {

                    return RedirectToAction("VistaError", "Errores", FuncionError(e.Message));
                }
            }
            else
            {
                return View(modelo);
            }
         
        }

        public JsonResult CreaCliente(string idcliente, string razonsocial, string alias, string giro, bool arauco)
        {
            try
            {
                var modelo = CrearCliente(GetRut(idcliente), razonsocial.ToUpperCheckForNull(), alias.ToUpperCheckForNull(), giro.ToUpperCheckForNull(), arauco,  User.Identity.Name);
                return Json(modelo);
            }
            catch(Exception e)
            {
                return Json(RedirectToAction("VistaError", "Errores", FuncionError(e.Message)));
            }
           
          
            


        }

        public async Task<ActionResult> Edit(int id)
        {
            var data = await LoadClienteAsync(id);
            ClienteModel model = new ClienteModel
            {
                IDCliente = FormatRutView(data.IDCliente),
                RazonSocial = data.RazonSocial,
                Giro = data.Giro,
                Alias = data.Alias
              
            };


            return View(model);
          
        }

        [HttpPost]
        public async Task<ActionResult> Edit(ClienteModel modelo)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    // TODO: Add update logic here
                   await EditarClienteAsync(modelo.ID, GetRut(modelo.IDCliente), modelo.RazonSocial.ToUpperCheckForNull(),modelo.Alias, modelo.Giro.ToUpperCheckForNull(), User.Identity.Name);

                    return RedirectToAction("VerClientes");
                }
                catch(Exception e)
                {
                    return RedirectToAction("VistaError", "Errores", FuncionError(e.Message));
                }


            }
            else
            {
                return View(modelo);
            }
         
        }

        public async Task<ActionResult> Delete(int id)
        {
            var data = await LoadClienteAsync(id);
            ClienteModel model = new ClienteModel
            {
                IDCliente = data.IDCliente,
                Alias = data.Alias,
                RazonSocial = data.RazonSocial,
                Giro = data.Giro,
                ID = data.ID
             
            };


            return View(model);
        }

        [HttpPost]
        public ActionResult Delete(ClienteModel modelo)
        {

            try
            {
                BorrarClienteAsync(modelo.ID);
                return RedirectToAction("VerClientes");
            }
            catch(Exception e)
            {
                return RedirectToAction("VistaError", "Errores", FuncionError(e.Message));
            }
     
           
        }

        [HttpGet]
        public async Task<ActionResult> ListaClientes()
        {
            var lista = await LoadClientesAsync();
            List<ClienteModel> modelo = new List<ClienteModel>();

            foreach(var row in lista)
            {
                ClienteModel cliente = new ClienteModel
                {
                    IDCliente = row.IDCliente,
                    RazonSocial = row.RazonSocial,
                    Giro = row.Giro
                  
                };
                modelo.Add(cliente);
            }

            return View(modelo);

        }
        public async Task<ActionResult> VerClientes()
        {
            List<ClienteModel> CLientes = new List<ClienteModel>();
            var lista = await LoadClientesAsync();
            foreach(var row in lista)
            {
                ClienteModel cliente = new ClienteModel
                {
                    ID = row.ID,
                    IDSinFormato = row.IDCliente,
                    IDCliente = FormatRutView(row.IDCliente),
                    RazonSocial = row.RazonSocial,
                    Giro = row.Giro
                    

                };

                CLientes.Add(cliente);
            }

            return View(CLientes);

        }

        public async Task<JsonResult> FiltroCliente(string palabra)
        {
            
            return Json(await FiltroCLienteAsync(palabra));
        }

        public async Task<JsonResult> Ubicaciones(int idcliente)
        {
            var info = await ListaUbicacionesAsync(idcliente);
           return Json(info);
            
        }

        public async Task<JsonResult> Contactos (int idcliente)
        {
            return Json( await ListaDeContactosAsync(idcliente));

        }

        public async Task<JsonResult> CrearContacto(string idcontacto, string alias, string cargo, string nombre, string apellido, string email, string fono, string notas, int idcliente, string area )

        {
            try
            {
                var data = await CreaContacto(GetRut(idcontacto), alias.ToUpperCheckForNull(), cargo.ToUpperCheckForNull(), nombre.ToUpperCheckForNull(), apellido.ToUpperCheckForNull(), email.ToUpperCheckForNull(), fono, notas.ToUpperCheckForNull(), idcliente, area.ToUpper(), User.Identity.Name);

                return Json(data);
            }
            catch(Exception e)
            {
                return Json(RedirectToAction("VistaError", "Errores", FuncionError(e.Message)));
            }
      

        }
         public async Task<JsonResult> CrearUbicacion(string nombreubicacion, int idcliente, string direccion, string ciudad, string alias)
        {
            try
            {
               await CreaUbicacionAsync(nombreubicacion.ToUpperCheckForNull(), idcliente, direccion.ToUpperCheckForNull(), ciudad.ToUpperCheckForNull(), alias.ToUpperCheckForNull(), User.Identity.Name);
                return Json("La ubicación ha sido guardada.", JsonRequestBehavior.AllowGet);
            }
           catch(Exception e)
            {
                return Json(RedirectToAction("VistaError", "Errores", FuncionError(e.Message)));
            }

        }

        public async Task<JsonResult> EditarUbicacion(int id, string nombreubicacion, string direccion, string ciudad, string alias)
        {
            try
            {
              await  EditaUbicacionAsync(id, nombreubicacion.ToUpperCheckForNull(), direccion.ToUpperCheckForNull(), ciudad.ToUpperCheckForNull(), alias.ToUpperCheckForNull(), User.Identity.Name);
                return Json("La ubicación ha sido editada.", JsonRequestBehavior.AllowGet);
            }
            catch(Exception e)
            {
                return Json(RedirectToAction("VistaError", "Errores", FuncionError(e.Message)));
            }
          

        }
    
         public async Task<JsonResult> VerDetalleContacto(int idcontacto)
        {
            var info = await VerContactoAsync(idcontacto);
            return Json(info);
        }


        public async Task<JsonResult> EditarContacto(int id ,string idcontacto, string alias, string cargo, string nombre, string apellido, string email, string fono, string notas, string area)
        {
            try
            {
               await EditaContactoAsync(id, GetRut(idcontacto), alias.ToUpper(), cargo.ToUpper(), nombre.ToUpper(), apellido.ToUpper(), email.ToUpper(), fono, notas.ToUpper(), area.ToUpper(), User.Identity.Name);
                return Json("El contacto ha sido editado.", JsonRequestBehavior.AllowGet);
            }
            catch(Exception e)
            {
                return Json(RedirectToAction("VistaError", "Errores", FuncionError(e.Message)));
            }
            

        }

        public async Task<JsonResult> AjaxEliminaContacto(int idcontacto)
        {
            try
            {
               await BorrarContactoAsync(idcontacto);
                return Json("Contacto Eliminado", JsonRequestBehavior.AllowGet);
            }
            catch(Exception e)
            {
                return Json(RedirectToAction("VistaError", "Errores", FuncionError(e.Message)));
            }
            
        }


        public async Task<JsonResult> AjaxEliminaUbicacion(int idubicacion)
        {
            try
            {
              await  BorraUbicacionAsync(idubicacion);
                return Json("Ubicación Eliminada", JsonRequestBehavior.AllowGet);
            }
            catch(Exception e)
            {
                return Json(RedirectToAction("VistaError", "Errores", FuncionError(e.Message)));
            }
           
        }

    }
}
