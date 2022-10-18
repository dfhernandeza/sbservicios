using static BibliotecaDeClases.Logica.Traducción.TraductorProcesador;
using static BibliotecaDeClases.ProcesadorEmpleados;
using BibliotecaDeClases.Modelos.Transacciones;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AppWebSantaBeatriz.Models;
using static BibliotecaDeClases.Logica.Transacciones.ProcesadorTransacciones;
using static BibliotecaDeClases.Logica.Transacciones.ProcesadorTimeSheet;
using System.Threading.Tasks;
using static BibliotecaDeClases.Logica.Transacciones.ProcesadorHorasExtra;
namespace AppWebSantaBeatriz.Controllers
{
    public class TimeSheetController : Controller
    {
        // GET: TimeSheet
        public ActionResult Index()
        {
            return View();
        }
        public async Task<ActionResult> Nuevo()
        {
            
            var empleados = LoadEmployeesAsync();  
            var cecos = ListaCecosActivosAsync();
            var sups = SupervisoresAsync();
            await Task.WhenAll(empleados, cecos, sups);
            var employees = await empleados;
            var centros = await cecos;
            var supers = await sups;

            BibliotecaDeClases.Modelos.Transacciones.DocumentoModel modelo = new BibliotecaDeClases.Modelos.Transacciones.DocumentoModel
            {
                
                ListaEmpleados = employees,
                ListaCECOsItems = centros,
                Supervisores = supers,
                IDTipo = 2
            };

            return View(modelo);
        }
        public async Task<ActionResult> Editar(int iddoc)
        {
            var empleados = LoadEmployeesAsync();
            var cecos = ListaCecosActivosAsync();
            var sups = SupervisoresAsync();
            var doc = cargardocTimeSheetAsync(iddoc);
            var trans = getTimeSheets(iddoc);
            var dist = getDistinctTimeSheets(iddoc);

            await Task.WhenAll(empleados, cecos, sups, doc, trans,dist);
            var docu = await doc;
            var employees = await empleados;
            var centros = await cecos;
            var supers = await sups;           
            var timesheets = await trans;
            var distinct = await dist;
            docu.ListaEmpleados = employees;
            docu.ListaCECOsItems = centros;
            docu.Supervisores = supers;
            docu.IDTipo = 2;
            docu.HH = timesheets;
            docu.HHDistinct = distinct;
            return View(docu);
            
        }
        public async Task<JsonResult> NuevoTimeSheet(DocumentoModel documento)
        {
            try
            {
                documento.CreadoEn = documento.EditadoEn = DateTime.Now;
                documento.CreadoPor = documento.EditadoPor = User.Identity.Name;
                var iddoc = await nuevoDocTimeSheet(documento);
                documento.HH.ForEach(x => 
                { x.IDDocumento = iddoc; x.CreadoPor = x.EditadoPor = User.Identity.Name; x.EditadoEn = x.CreadoEn = DateTime.Now; });
                await InsertaNuevosTimeSheetAsync(documento.HH);
                return Json("Insertado");
            }
            catch (Exception e)
            {
                return Json(RedirectToAction("VistaError", "Errores", FuncionError(e.Message)));

            }
            
        }
        public async Task<JsonResult> editarTimeSheet(DocumentoModel documento)
        {
            try
            {
                documento.EditadoPor = User.Identity.Name;
                documento.EditadoEn = DateTime.Now;
                await editarDocTimeSheet(documento);
                await eliminarTimeSheets(documento.ID);                                              
                documento.HH.ForEach(x =>
                { x.IDDocumento = documento.ID; x.CreadoPor = x.EditadoPor = User.Identity.Name; x.EditadoEn = x.CreadoEn = DateTime.Now; });
                await InsertaNuevosTimeSheetAsync(documento.HH);
                return Json("Insertado");
            }
            catch (Exception e)
            {
                return Json(RedirectToAction("VistaError", "Errores", FuncionError(e.Message)));

            }

        }
        public async Task<ActionResult> VerTimeSheets()
        {

          var time = await getTimeSheetsMes();
            return View(time);
        }
        public async Task<ActionResult> NuevoTimeSheetAsync(int idtarea, DateTime fecha)
        {
            RegistroHorasModel modelo = new RegistroHorasModel ();
            var cecoTask = getCECOIDAsync(idtarea);
            var empleadosTask = BibliotecaDeClases.Logica.Servicios.ProcesadorTareas.EmpleadosTareaHH(idtarea);

            var tareas = new List<Task> { cecoTask, empleadosTask };
            while(tareas.Count > 0)
            {
                Task terminada = await Task.WhenAny(tareas);
                if(terminada == cecoTask)
                {
                    var ceco = await cecoTask;
                    modelo.IDCECO =  ceco.ID;
                    modelo.CECO = ceco.Nombre;
                }
                else if(terminada == empleadosTask)
                {
                    modelo.Empleados = await empleadosTask;
                }
                tareas.Remove(terminada);
            }
            modelo.Fecha = fecha;
            
            return View(modelo);

        }
        [HttpPost]
        public async Task<JsonResult> NuevoTimeSheetAsync(List<TimeSheetModel> modelo)
        {
            var hh = await getEmpleadosValorHHCuentaTareaAsync(modelo.FirstOrDefault().IDCECO);
            //documento
            var iddoc = await nuevoDocTimeSheet(new DocumentoModel 
            {   
             IDTipo = 2,             
             IDDocumento = Guid.NewGuid().ToString(),
             FechaDocumento = modelo.FirstOrDefault().Entrada.Date,
             Descripcion = "REGISTRO HORAS HOMBRE",
             CreadoEn = DateTime.Now,
             EditadoEn = DateTime.Now,
             CreadoPor = User.Identity.Name,
             EditadoPor = User.Identity.Name,
             IDEmisor = "1", 
             IDSupervisor = hh.FirstOrDefault().IDSupervisor

            });
           
           
            //List<TransaccionModel> transacciones = new List<TransaccionModel>();
            foreach (var item in modelo)
            {
                //Transaccion
                decimal horas = 0;

                if((item.Salida - item.Entrada).Hours >= 4)
                {
                    horas = Convert.ToDecimal((item.Salida - item.Entrada).Hours) - Convert.ToDecimal(0.5);
                }
                else
                {
                    horas = Convert.ToDecimal((item.Salida - item.Entrada).Hours);
                }

                //TimeSheet

                item.CreadoEn = item.EditadoEn = DateTime.Now;
                item.CreadoPor = item.EditadoPor = User.Identity.Name;
                item.IDDocumento = iddoc;
                item.ValorHH = hh.Where(y => y.ID == item.IDEmpleado).FirstOrDefault().ValorHH;
                item.IDCuentaEmpleado = hh.Where(y => y.ID == item.IDEmpleado).FirstOrDefault().IDCuentaEmpleado;


                var idtransaccion =  await ingresarTransaccionReturnIDAsync(new TransaccionModel
                {
                    IDDocumento = iddoc,
                    Fecha = modelo.FirstOrDefault().Entrada.Date,
                    Item = "Remuneración por pagar",
                    Cantidad = 1,
                    Monto = horas * item.ValorHH,
                    IDCuentaCR = item.IDCuentaEmpleado,
                    IDCuentaDB = 9,
                    IDCECO = item.IDCECO,
                    IDEmpleado = item.IDEmpleado.ToString(),
                    CreadoPor = User.Identity.Name,
                    EditadoPor = User.Identity.Name,
                    EditadoEn = DateTime.Now,
                    CreadoEn = DateTime.Now
                });

       
                item.IDTransaccion = idtransaccion;
                await InsertaNuevoTimeSheetAsync(item);
            }
    
            return Json("Exito",JsonRequestBehavior.AllowGet);
        }
        public async Task<JsonResult> getEmpleadosHoursValorHHAsync(int idservicio)
        {

            return Json(await getTimeSheetByServicioAsync(idservicio));
        }
        [HttpGet]
        public async Task<ActionResult> EditarTimeSheetServicio(int id, int idservicio)
        {
            var cecosTask = getCECOsIDAsync(idservicio);
            var dataTask =  getTimeSheetAsync(id);
            await Task.WhenAll(cecosTask, dataTask);
            var data = await dataTask;
           var cecos  = await cecosTask;
            cecos.Where(x => x.Value == data.IDCECO.ToString()).FirstOrDefault().Selected = true;
            data.CECOsList = cecos;
            data.IDServicio = idservicio;
            return View(data);

        }
        public async Task<ActionResult> editarTimeSheetServicioAsync(TimeSheetModel modelo)
        {
            try
            {
                modelo.EditadoEn = DateTime.Now;
                modelo.EditadoPor = User.Identity.Name;
                var valorhhTask = getEmpleadoValorHHAsync(modelo.IDEmpleado);
                var idtransTask = editarTimeSheetAsync(modelo);
                await Task.WhenAll(valorhhTask, idtransTask);
                var valorhh = await valorhhTask;
                var idtrans = await idtransTask;
                decimal horas = 0;

                if ((modelo.Salida - modelo.Entrada).Hours >= 4)
                {
                    horas = Convert.ToDecimal((modelo.Salida - modelo.Entrada).Hours) - Convert.ToDecimal(0.5);
                }
                else
                {
                    horas = Convert.ToDecimal((modelo.Salida - modelo.Entrada).Hours);
                }

                await editHHTransaccionAsync(new TransaccionModel
                {
                    Monto = valorhh * horas,
                    ID = idtrans,
                    EditadoEn = DateTime.Now,
                    EditadoPor = User.Identity.Name
                });
                return RedirectToAction("Detalles","Servicios",new {id = modelo.IDServicio});
            }
            catch (Exception e)
            {

                return RedirectToAction("VistaError", "Errores", FuncionError(e.Message));
            }
        }
        public async Task<JsonResult> filtrarTimeSheetByDayAsync(DateTime fecha)
        {

            return Json(await filtrarTimeSheetDiaAsync(fecha));
        }
        public async Task<JsonResult> filtrarTimeSheetByNombreAsync(int idservicio, string texto)
        {

            return Json(await filtrarTimeSheetEmpleadoAsync(texto, idservicio));
        }
        public async Task<ActionResult> NuevaHoraExtra()
        {
            var supervisoresTask = getSupervisoresAsync();
            var empleadosTask =  ListaEmpleadosAsync();
            var cecosTask =  ListaCecosActivosAsync();
            await Task.WhenAll(empleadosTask, cecosTask, supervisoresTask);
            var empleados = await empleadosTask;
            var cecos = await cecosTask;
            var supervisores = await supervisoresTask;
            HorasExtraModel modelo = new HorasExtraModel {EmpleadosSL = empleados, CECOs = cecos, SupervisoresSL = supervisores };
            return View(modelo);
        }
        [HttpPost]
        public async Task<JsonResult> NuevaHoraExtra(DocumentoModel modelo)
        {
            try
            {
                var costoHHExtraTask = ValorHoraExtraAsync(modelo.HorasExtra.FirstOrDefault().IDEmpleado);
                var cuentaEmpleadoTask = getCuentaPagosAsync(modelo.HorasExtra.FirstOrDefault().IDEmpleado);
                await Task.WhenAll(costoHHExtraTask, cuentaEmpleadoTask);
                var costoHHExtra = await costoHHExtraTask;
                var cuentaEmpleado = await cuentaEmpleadoTask;
                //Documento
                modelo.IDTipo = 8;
                modelo.IDDocumento = Guid.NewGuid().ToString();
                modelo.FechaDocumento = DateTime.Now.Date;
                modelo.Descripcion = "REGISTRO HORAS EXTRA";
                modelo.CreadoEn = modelo.EditadoEn = DateTime.Now;
                modelo.CreadoPor = modelo.EditadoPor = User.Identity.Name;
                modelo.IDEmisor = "1";
                var iddoc = await nuevoDocTimeSheet(modelo);
                //Transacciones          

                foreach (var item in modelo.HorasExtra)
                {

                    var idtransaccion = await ingresarTransaccionReturnIDAsync(new TransaccionModel
                    {
                        IDDocumento = iddoc,
                        Fecha = modelo.FechaDocumento.Date,
                        Item = "Hora Extra Por Pagar",
                        Cantidad = 1,
                        Monto = item.Horas * costoHHExtra * item.Factor,
                        IDCuentaCR = cuentaEmpleado.CuentaPagos,
                        IDCuentaDB = 9,
                        IDCECO = item.IDCECO,
                        IDEmpleado = item.IDEmpleado.ToString(),
                        CreadoPor = User.Identity.Name,
                        EditadoPor = User.Identity.Name,
                        EditadoEn = DateTime.Now,
                        CreadoEn = DateTime.Now
                    });
                    item.IDTransaccion = idtransaccion;
                    item.CreadoEn = item.EditadoEn = DateTime.Now;
                    item.CreadoPor = item.EditadoPor = User.Identity.Name;
                    item.IDDocumento = iddoc;
                    await insertarHorasExtraAsync(item);
                    
                }
                return Json(true);
            }
            catch (Exception e)
            {

                return Json(FuncionError(e.Message).Mensaje);
            }
           
            
        }

        public async Task<ActionResult> VerHorasExtra()
        {

            var data = await getHorasExtraAsync();
            return View(data);
        }

        public async Task<ActionResult> VerDocumentoHHExtra(int id)
        {
            var doc = await getDocHHExtraAsync(id);
            var horas = await getHHExtraDoc(id);
            doc.HorasExtra = horas;
            return View(doc);
        }

        public async Task<ActionResult> editaHoraAsync(int id)
        {
            var cecos = await ListaCecosActivosAsync();
            var data = await getHoraExtraAsync(id);
            data.CECOs = cecos;
            return View(data);
            
        }
        [HttpPost]
        public async Task<ActionResult> editaHoraAsync(HorasExtraModel modelo)
        {
            
            try
            {
                var costoHHExtra = await ValorHoraExtraAsync(modelo.IDEmpleado);
                modelo.EditadoEn = DateTime.Now;
                modelo.EditadoPor = User.Identity.Name;
                await EditarTransaccionHHExtraAsync(new TransaccionModel { 
                    Monto = costoHHExtra * modelo.Factor * modelo.Horas, 
                    IDCECO = modelo.IDCECO, 
                    EditadoEn = DateTime.Now, 
                    EditadoPor = User.Identity.Name, 
                    ID = modelo.IDTransaccion });
                await editarHoraExtraAsync(modelo);
                return RedirectToAction("VerDocumentoHHExtra", new { id = modelo.IDDocumento });
            }
            catch (Exception e)
            {

                return RedirectToAction("VistaError", "Errores", FuncionError(e.Message));
            }
        }
        public async Task<ActionResult> elimnaHoraExtraAsync(int id)
        {
            var data = await getHoraExtraAsync(id);
            return View(data);
        }
        [HttpPost]
        public async Task<ActionResult> elimnaHoraExtraAsync(HorasExtraModel modelo)
        {
            try
            {
                await EliminarTransaccionAsync(modelo.IDTransaccion);
                await eliminarHoraExtraAsync(modelo.ID);
                return RedirectToAction("VerDocumentoHHExtra", new { id = modelo.IDDocumento });
            }
            catch (Exception e)
            {

                return RedirectToAction("VistaError", "Errores", FuncionError(e.Message));
            }
           

        }
    }
}