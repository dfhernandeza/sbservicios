using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using AppWebSantaBeatriz.Models.Servicios;
using static BibliotecaDeClases.Logica.Servicios.ProcesadorServicios;
using static BibliotecaDeClases.Logica.ProcesadorCotizaciones;
using static BibliotecaDeClases.ProcesadorEmpleados;
using static BibliotecaDeClases.Logica.Clientes.ProcesadorClientes;
using static BibliotecaDeClases.Logica.Servicios.ProcesadorEstadoServicios;
using static BibliotecaDeClases.Logica.Funciones;
using static BibliotecaDeClases.Logica.Servicios.ProcesadorTipoServicio;
using static BibliotecaDeClases.Logica.Traducción.TraductorProcesador;
using AppWebSantaBeatriz.Models.Error;
using static BibliotecaDeClases.Logica.Cotizaciones.ProcesadorCotizacionesArauco;
using System.Threading.Tasks;
using static BibliotecaDeClases.Logica.Servicios.ProcesadorTareas;
using System.Net.Http;
using System.IO;
using Azure.Identity;
using Azure.ResourceManager;
using Azure.ResourceManager.Resources;
using Azure.ResourceManager.Compute;

namespace AppWebSantaBeatriz.Controllers
{
    [Authorize]
    public class ServiciosController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }     
        public async Task<ActionResult> Detalles(int id)
        {
            var datos =  LoadServicio(id);
            var catgs =  CategoriasEntregables();
            var empls =  ListaEmpleadosAsync();
            var ubis = UbicacionesServicio(id);
            var resumendiaTask = getResumenDiaServicioAsync(id);
            await  Task.WhenAll(datos, catgs, empls, ubis);
            var data = await datos;
            var emps = await empls;
            var cats = await catgs;
            var ubics = await ubis;
            var eys = await BibliotecaDeClases.Logica.Servicios.ProcesadorTareas.getItemEyS(data.IDCotizacion);
            var resumendia = await resumendiaTask;
            ServicioModel servicio = new ServicioModel
            {
                ID = data.ID,
                Encargado = data.Encargado,
                NombreServicio = data.NombreServicio,
                IDCotizacion = data.IDCotizacion,
                CLiente = data.CLiente,
                Contacto = data.Contacto,
                Estado = data.Estado,
                CostoMaterial = data.CostoMaterial,
                CostoPersonal = data.CostoPersonal,
                CostoEquiposServicios = data.CostoEquiposServicios,
                CostoSeguridad = data.CostoSeguridad,
                Empleados = emps ,
                FechaInicio = data.FechaInicio,
                FechaTermino = data.FechaTermino,
                Ubicacion = data.Ubicacion,
                NumPedido = data.NumPedido,
                CategoriasEntregables = cats,
                UbisTareas = ubics,
                EySList = eys,
                ResumenDiaList = resumendia


            };
         
            return View(servicio);
        }      
        public async Task<ActionResult> NuevoServicioArauco(int? idcot)
        {
            if(idcot == null)
            {
                var cot =  ListaCotizacionesAsync();
                var emps =  ListaEmpleadosAsync();
                var clients =  ListaClientesAraucoAsync();
                var tipos =  TiposServiciosAsync();
                var ubis =  UbicacionesAraucoAsync();
              await  Task.WhenAll(cot, emps, clients, tipos, ubis);
                var coti = await cot;
                var empls = await emps;
                var clientes = await clients;
                var tips = await tipos;
                var locs = await ubis;
                BibliotecaDeClases.Modelos.Servicios.ServicioModel modelo = new BibliotecaDeClases.Modelos.Servicios.ServicioModel
                {
                    Cotizaciones = coti,
                    Empleados = empls ,
                    Clientes = clientes ,
                    TiposServicios = tips ,
                    Ubicaciones = locs                 

                };

                return View(modelo);
            }
            else
            {
                var idservicio =  IDServicioAsync();
                var cot =  ListaCotizacionesAsync();
                var emps =  ListaEmpleadosAsync();
                var clients =  ListaClientesAraucoAsync();
                var tipos =  TiposServiciosAsync();
                var ubis =  UbicacionesAraucoAsync();
                await Task.WhenAll(idservicio, cot, emps, clients, tipos, ubis);
                var idserv = await idservicio;
                var coti = await cot;
                var empls = await emps;
                var clientes = await clients;
                var tips = await tipos;
                var ubics = await ubis;

                BibliotecaDeClases.Modelos.Servicios.ServicioModel modelo = new BibliotecaDeClases.Modelos.Servicios.ServicioModel
                {
                    ID = idserv ,
                    Cotizaciones = coti,
                    Empleados = empls,
                    Clientes = clientes,
                    IDCotizacion = (int)idcot,
                    TiposServicios = tips,
                    Ubicaciones = ubics
                };
                return View(modelo);
            
            }
          
        }
        public async Task<JsonResult> GetContactos(int id)
        {
            
            return Json(await ListaContactosAsync(id));
        }
        public async Task<ActionResult> Edit(int id)
        {
          var data = await CargarServicioAsync(id);      

            var cot = await ListaCotizacionesAsync();
            var emps = await ListaEmpleadosAsync();
            var clients = await ListaClientesAraucoAsync();
            var tipos = await TiposServiciosAsync();
            var ubis = await UbicacionesAraucoAsync();
            var contacts = await ListaContactosAsync(data.IDCliente);
            ServicioModel servicio = new ServicioModel
            {
                ID = data.ID,
                Cotizaciones = cot,
                Empleados = emps,
                Clientes = clients,        
                NombreServicio = data.NombreServicio,
                Encargado = data.Encargado,
                FechaInicio = data.FechaInicio,
                FechaTermino = data.FechaTermino,
                Descripcion = data.Descripcion,
                HorasTotales = data.HorasTotales,           
                TiposServicios = tipos,
                Ubicaciones = ubis,
                Contactos = contacts,
                NumPedido = data.NumPedido
                


            };
            servicio.TiposServicios.Where(item => item.Value == data.IDTipoServicio.ToString()).ToList().ForEach(s => s.Selected = true);
            servicio.Cotizaciones.Where(item => item.Value == data.IDCotizacion.ToString()).ToList().ForEach(s => s.Selected = true);
            servicio.Empleados.Where(item => item.Value == data.IDEncargado.ToString()).ToList().ForEach(s => s.Selected = true);
            servicio.Clientes.Where(item => item.Value == data.IDCliente.ToString()).ToList().ForEach(s => s.Selected = true);
            servicio.Ubicaciones.Where(item => item.Value == data.IDUbicacion.ToString()).ToList().ForEach(s => s.Selected = true);
            servicio.Contactos.Where(item => item.Value == data.IDContacto.ToString()).ToList().ForEach(s => s.Selected = true);
            return View(servicio);
        }  
        [HttpPost]
        public async Task<ActionResult> Edit(BibliotecaDeClases.Modelos.Servicios.ServicioModel modelo)
        {

            if (ModelState.IsValid)
            {
                modelo.EditadoEn = DateTime.Now;
                modelo.EditadoPor = User.Identity.Name;
                modelo.NombreServicio = modelo.NombreServicio.ToUpperCheckForNull();
                modelo.Descripcion = modelo.Descripcion.ToUpperCheckForNull();
                try
                {
                   await EditarServicioAsync(modelo);
                    return RedirectToAction("VerServicios");
                }
                catch(Exception e)
                {
                    return RedirectToAction("VistaError", "Errores", FuncionError(e.Message));
                }
            }
            else
            {
                var cot = await ListaCotizacionesAsync();
                var emps = await ListaEmpleadosAsync();
                var clients = await ListaClientesAraucoAsync();
                var tipos = await TiposServiciosAsync();
                var ubis = await UbicacionesAraucoAsync();


                modelo.Cotizaciones = cot;
                modelo.Empleados = emps;
                modelo.Clientes = clients;
                modelo.TiposServicios = tipos;
                modelo.Ubicaciones = ubis;
                return View(modelo);
            }


        
          
        }
        public async Task<ActionResult> Delete(int id)
        {

            var data = await LoadServicio(id);

            ServicioModel servicio = new ServicioModel
            {
                ID = id,
                NombreServicio = data.NombreServicio,
                Encargado = data.Encargado,
                Estado = data.Estado,
                FechaInicio = data.FechaInicio,
                FechaTermino = data.FechaTermino,
                Descripcion = data.Descripcion,
                HorasTotales = data.HorasTotales,
                CLiente = data.CLiente,
                Contacto = data.Contacto,
                NumPedido = data.NumPedido,
                Ubicacion = data.Ubicacion,
                TipoServicio = data.TipoServicio


            };

            return View(servicio);

        }
        [HttpPost]
        public async Task<ActionResult> Delete(ServicioModel modelo)
        {
            
                // TODO: Add delete logic here
               await BorrarServicioAsync(modelo.ID);
                return RedirectToAction("VerServicios");
            
         
        }
        public async Task<ActionResult> VerServicios()
        {
            ViewBag.Titulo = "Servicios";
            var servicio = await LoadServiciosAsync();
            List<ServicioModel> Servicios = new List<ServicioModel>();
            foreach(var row in servicio)
            {
                ServicioModel model = new ServicioModel
                {
                    ID = row.ID,
                    Encargado = row.Encargado,
                    NombreServicio = row.NombreServicio,
                    Estado= row.Estado,
                    CLiente = row.CLiente,
                    FechaInicio = row.FechaInicio,
                    FechaTermino = row.FechaTermino
                    

                };

                Servicios.Add(model);
            }

            return View(Servicios);
        }
        public async Task<ActionResult> GetPresupuesto(int idcot)
        {
            var info = await PresupuestoAsync(idcot);

            return RedirectToAction("Create", new { id = idcot, pptoseguridad = info.PresupuestoSeguridad, pptoequipos = info.PresupuestoEquiposServicios, pptopersonal = info.PresupuestoPersonal, pptomateriales = info.PresupuestoMateriales, idcliente = info.IDCliente, idcontacto = info.IDContacto, servicio = info.NombreServicio, descripcion = info.Descripcion });
        }
        public async Task<JsonResult> ObtenerPresupuesto(int idcot)
        {
            var data = await PresupuestoAsync(idcot);
            return Json(data);
        }
        public async Task<JsonResult> getPresupuestoServicio(int idcot)
        {
            var data = await PresupuestoServicioAsync(idcot);
            return Json(data);
        }
        public async Task<ActionResult> ResumenHHServicio(int idservicio)
        {
            var data = await getCostoHHDiaEmpleadoAsync(idservicio);
            return View(data);
        }         

        [HttpPost]
        public async Task<ActionResult> NuevoServicioArauco(BibliotecaDeClases.Modelos.Servicios.ServicioModel modelo)
        {

            try
            {
                modelo.CreadoEn = modelo.EditadoEn = DateTime.Now;
                modelo.EditadoPor = modelo.CreadoPor = User.Identity.Name;
                modelo.Descripcion= modelo.Descripcion.ToUpperCheckForNull();
                modelo.NombreServicio = modelo.NombreServicio.ToUpperCheckForNull();
                var idservicio = await CrearServicio(modelo);

                return RedirectToAction("Detalles", new { id = idservicio });
            }
            catch (Exception e)
            {
                return RedirectToAction("VistaError", "Errores", FuncionError(e.Message));
            }

       

        }
        public async Task<ActionResult> VerEstadoServiciosAsync()


        {
            var data = await ListaEstadoServicioAsync();
            List<EstadoServicioModel> lista = new List<EstadoServicioModel>();
                foreach(var row in data)
            {
                EstadoServicioModel modelo = new EstadoServicioModel
                {
                    Estado = row.Estado,
                    Descripcion = row.Descripcion,
                    ID = row.ID
                };

                lista.Add(modelo);

            }

            return View(lista);



        }
        public ActionResult CrearEstadoServicio()
        {
            return View();
        }
        [HttpPost]
        public async Task<ActionResult> CrearEstadoServicio(EstadoServicioModel modelo)
        {

          await  CreaEstadoServicioAsync(modelo.Estado.ToUpper(), modelo.Descripcion.ToUpper(), User.Identity.Name);



            return RedirectToAction("VerEstadoServicios");
        }
        public async Task<ActionResult> EditaEstadoServicio(int id)
        {
            var data = await LoadEstadoServicioAsync(id);

            EstadoServicioModel modelo = new EstadoServicioModel
            {
                Estado = data.Estado,
                ID = data.ID,
                Descripcion = data.Descripcion
            };

            return View(modelo);
        }
        [HttpPost]
        public async Task<ActionResult> EditaEstadoServicio(EstadoServicioModel modelo)
        {
          await  ActualizaEstadoServicioAsync(modelo.ID, modelo.Estado.ToUpper(), modelo.Descripcion.ToUpper(), User.Identity.Name);

            return RedirectToAction("VerEstadoServicios");
        }
        public async Task<ActionResult> BorraEstadoServicio(int id)
        {
            var data = await LoadEstadoServicioAsync(id);

            EstadoServicioModel modelo = new EstadoServicioModel
            {
                Estado = data.Estado,
                ID = data.ID,
                Descripcion = data.Descripcion
            };

            return View(modelo);
        }
        [HttpPost]
        public async Task<ActionResult> BorraEstadoServicio(EstadoServicioModel modelo)
        {
          await  EliminaEstadoServicioAsync(modelo.ID);

            return RedirectToAction("VerEstadoServicios");
        }
        public JsonResult ObtenerCotizaciones(int idcliente)
        {
          return Json(CotizacionesPorClienteAsync(idcliente));
        }
        public async Task<JsonResult> ObtenerPptoVsCostosAsync(int idcotizacion)
        {
            
            return Json(await PresupuestoVsCostos(idcotizacion));
        }

        //=================================================================================================================================

        public ActionResult CreaTipoServicio()
        {
            return View();
        }
        [HttpPost]
        public async Task<ActionResult> CreaTipoServicio(TipoServicioModel modelo)
        {
            if (ModelState.IsValid)
            {
                try
                {
                   await NuevoTipoServicioAsync(modelo.TipoServicio.ToUpperCheckForNull(), modelo.Descripcion.ToUpperCheckForNull(), User.Identity.Name);
                    return RedirectToAction("VerTiposServicios");
                }
                catch(Exception e)
                {
                    var datos = Traducir(e.Message).Result.FirstOrDefault().Translations[0];

                    ErrorModel error = new ErrorModel
                    {
                        Mensaje = datos.Text
                    };

                    return RedirectToAction("VistaError", "Errores", error);
                }
            }
            else
            {
                return View(modelo);
            }

           
        }
        
        public async Task<ActionResult> EditarTipoServicio(int id)
        {
            var data = await LoadTipoServicioAsync(id);
            TipoServicioModel modelo = new TipoServicioModel { ID = data.ID, TipoServicio = data.TipoServicio, Descripcion = data.Descripcion };
            return View(modelo);
        }
        [HttpPost]
        public async Task<ActionResult> EditarTipoServicio(TipoServicioModel modelo)
        {
            if (ModelState.IsValid)
            {
                try
                {
                  await  EditaTipoServicioAsync(modelo.ID, modelo.TipoServicio.ToUpperCheckForNull(), modelo.Descripcion.ToUpperCheckForNull(), User.Identity.Name);
                    return RedirectToAction("VerTiposServicios");
                }
                catch(Exception e)
                {
                    var datos = Traducir(e.Message).Result.FirstOrDefault().Translations[0];

                    ErrorModel error = new ErrorModel
                    {
                        Mensaje = datos.Text
                    };

                    return RedirectToAction("VistaError", "Errores", error);
                }
               
            }
            else
            {
                return View(modelo);
            }
            


            
        }

        public async Task<ActionResult> EliminarTipoServicio(int id)
        {
            var data = await LoadTipoServicioAsync(id);
            TipoServicioModel modelo = new TipoServicioModel { ID = data.ID, TipoServicio = data.TipoServicio, Descripcion = data.Descripcion };
            return View(modelo);
        }
        [HttpPost]
        public async Task<ActionResult> EliminarTipoServicio(TipoServicioModel modelo)
        {

            if (ModelState.IsValid)
            {
                try
                {
                   await EliminaTipoServicioAsync(modelo.ID);
                    return RedirectToAction("VerTiposServicios");
                }
                catch (Exception e)
                {
                    var datos = Traducir(e.Message).Result.FirstOrDefault().Translations[0];

                    ErrorModel error = new ErrorModel
                    {
                        Mensaje = datos.Text
                    };

                    return RedirectToAction("VistaError", "Errores", error);
                }

            }
            else
            {
                return View(modelo);
            }
        }
        public async Task<ActionResult> VerTiposServicios()
        {
            var data = await ListaTipoServicioAsync();
            List<TipoServicioModel> modelo = new List<TipoServicioModel>();
            foreach(var row in data)
            {
                modelo.Add(new TipoServicioModel { ID = row.ID, Descripcion = row.Descripcion, TipoServicio = row.TipoServicio });

            }
            return View(modelo);
        }
        //=================================================================================================================================
        public async Task<ActionResult> NuevoServicio(int? idcot)
        {
            if (idcot == null)
            {
                var cot = ListaCotizacionesOtrosClientesAsync();
                var emps = ListaEmpleadosAsync();
                var clients = ListaClientesAsync();
                var tipos = TiposServiciosAsync();
                //var ubis = UbicacionesListAsync();
                await Task.WhenAll(cot, emps, clients, tipos);
                var coti = await cot;
                var empls = await emps;
                var clientes = await clients;
                var tips = await tipos;
                //var locs = await ubis;
                BibliotecaDeClases.Modelos.Servicios.ServicioModel modelo = new BibliotecaDeClases.Modelos.Servicios.ServicioModel
                {
                    Cotizaciones = coti,
                    Empleados = empls,
                    Clientes = clientes,
                    TiposServicios = tips
                    //Ubicaciones = locs

                };

                return View(modelo);
            }
            else
            {
                var idservicio = IDServicioAsync();
                var cot = ListaCotizacionesAsync();
                var emps = ListaEmpleadosAsync();
                var clients = ListaClientesAraucoAsync();
                var tipos = TiposServiciosAsync();
                
                await Task.WhenAll(idservicio, cot, emps, clients, tipos);
                var idserv = await idservicio;
                var coti = await cot;
                var empls = await emps;
                var clientes = await clients;
                var tips = await tipos;
              

                BibliotecaDeClases.Modelos.Servicios.ServicioModel modelo = new BibliotecaDeClases.Modelos.Servicios.ServicioModel
                {
                    ID = idserv,
                    Cotizaciones = coti,
                    Empleados = empls,
                    Clientes = clientes,
                    IDCotizacion = (int)idcot,
                    TiposServicios = tips
                  
                };
                return View(modelo);

            }



        }
        public async Task<JsonResult> getUbicacionesAsync(int idcliente)
        {

            return Json(await UbicacionesListAsync(idcliente));
        }

        //=================================================================================================================================


        [HttpGet]
        public async Task<FileResult> GetReport(int id)
        {
            // First we construct our armClient
            var vsc = new VisualStudioCredential();
            var armClient = new ArmClient(vsc);
            VirtualMachine vm;
            // Next we get a resource group object
            // ResourceGroup is a [Resource] object from above
            ResourceGroup resourceGroup = await armClient.DefaultSubscription.GetResourceGroups().GetAsync("SANTA");

            // Next we get the container for the virtual machines
            // vmContainer is a [Resource]Container object from above
            VirtualMachineContainer vmContainer = resourceGroup.GetVirtualMachines();
            foreach (var item in vmContainer.GetAll())
            {
                if (item.Data.Name == "SantaBeatriz")
                {
                    vm = item;
                   var on = item.PowerOn();
                    if (on.HasCompleted)
                    {
                        string url = $"http://sbserviciosreportsapi.brazilsouth.cloudapp.azure.com/api/InformeServicio/{id}";

                        HttpClient client = new HttpClient();

                        using (HttpResponseMessage response = await client.GetAsync(url))
                        {
                            if (response.IsSuccessStatusCode)
                            {
                                var data = await response.Content.ReadAsStreamAsync();
                                vm.DeallocateAsync();
                                MemoryStream ms;
                                ms = new MemoryStream();
                                data.CopyTo(ms);
                                data.Close();
                                ms.Seek(0, SeekOrigin.Begin);
                                
                                return File(ms, "application/pdf", "Informe.pdf");

                            }
                            else
                            {
                                throw new Exception(response.ReasonPhrase);
                            }
                        }
                    }
                   

                }
            }
            throw new Exception("Un error ha ocurrido");
            //ReportDocument reporte = new ReportDocument();
            //reporte.Load(HttpContext.Server.MapPath("/Reportes/InformeServicio.rpt"));
            //await StartMachineApiHelper.InitializeClientAsync();



        }
    }
    public static class StartMachineApiHelper
    {
        public static HttpClient ApiClient { get; set; }

        //public static async Task InitializeClientAsync()
        //{
        //    //string subscriptionId = "b7c11e4b-b8a5-451f-b169-de1b378e04e2";
        //    //string resourceGroupName = "SANTABEATRIZ";
        //    //string vmName = "Santa";

        //    //ApiClient = new HttpClient();
        //    //HttpContent stringContent = new FormUrlEncodedContent(new[] 
        //    //{
        //    //   new KeyValuePair<string, string>("subscriptionId", "b7c11e4b-b8a5-451f-b169-de1b378e04e2"),
        //    //   new KeyValuePair<string, string>("resourceGroupName", "SANTABEATRIZ"),
        //    //   new KeyValuePair<string, string>("vmName", "Santa")
        //    //});
        //    // string url = $"https://management.azure.com/subscriptions/{subscriptionId}/resourceGroups/{resourceGroupName}/providers/Microsoft.Compute/virtualMachines/{vmName}/start?api-version=2021-07-01";
        //    //using (HttpResponseMessage response = await ApiClient.PostAsync(url,stringContent))
        //    //{
        //    //    if (response.IsSuccessStatusCode)
        //    //    {
        //    //        var data = await response.Content.ReadAsStringAsync();
                    
        //    //    }
        //    //    else
        //    //    {
        //    //        throw new Exception(response.ReasonPhrase);
        //    //    }
        //    //}
        //}
    }


}


