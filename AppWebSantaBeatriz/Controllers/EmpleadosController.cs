using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using static BibliotecaDeClases.ProcesadorEmpleados;
using static BibliotecaDeClases.Logica.Personal.ProcesadorEspecialidades;
using System;
using static BibliotecaDeClases.Logica.Funciones;
using static BibliotecaDeClases.Logica.Traducción.TraductorProcesador;
using System.IO;
using static BibliotecaDeClases.Logica.AccesoBlobs.AccesoBlobs;
using System.Threading.Tasks;
using BibliotecaDeClases;
using BibliotecaDeClases.Modelos;
using static BibliotecaDeClases.Logica.Transacciones.ProcesadorTransacciones;
using BibliotecaDeClases.Modelos.Transacciones;
using BibliotecaDeClases.Modelos.Empleados;
using static BibliotecaDeClases.Logica.Personal.ProcesadorVacaciones;
using BibliotecaDeClases.Modelos.Traductor;
using static BibliotecaDeClases.Logica.AccesoBlobs.ProcesadorFiniquitos;
using static BibliotecaDeClases.Logica.Personal.ProcesadorFiniquitos;
namespace AplicacionWebSB.Controllers
{
    [Authorize]
    public class EmpleadosController : Controller
    {
        // GET: Empleados
        public ActionResult Index()
        {
            return View();
            
        }
        public async Task<ActionResult> Detalles(int id)
        {
            var data = await LoadEmployeeAsync(id);
            var finiquito = await obtenerFiniquitoAsync(id);
            var liquidaciones = cargaLiquidacionesEmpleado(id);
            data.Liquidaciones = liquidaciones;
            data.Finiquito = finiquito;

            return View(data);
        }
        public async Task<ActionResult> VerEmpleados()
        {

            var data = await LoadEmployeesAsync();
            data.ForEach(x => x.IDEmpleado = FormatRutView(x.IDEmpleado));
              

            return View(data);
        }
        public JsonResult ListadoEmpleados()
        {
            return Json(ListaEmpleadosAsync());
        }       
        public async Task<ActionResult> CrearEmpleado()
        {
            var esp =  EspecialidadesAsync();
            var super =  SupervisoresAsync();
            var pants =  TallaPantalonAsync();
            var zapatos =  TallaZapatosAsync();
            var top =  TallaTop();
            var paisesTask = getPaisesAsync();
            var bancosTask = getBancosAsync();
            var estadoCivilTask = getEstadosCivilAsync();
            var tipolicenciaTask = getTiposLicenciaAsync();
            var tipoCuentaBancariaTask = getTiposCuentaBancariaAsync();
            await Task.WhenAll(esp, super, pants, zapatos, top, paisesTask, bancosTask, estadoCivilTask, tipolicenciaTask, tipoCuentaBancariaTask);
            var especialidades = await esp;
            var supervisores = await super;
            var pantalones = await pants;
            var zapatoslist = await zapatos;
            var tops = await top;
            var paises = await paisesTask;
            var bancos = await bancosTask;
            var estadosCivil = await estadoCivilTask;
            var tiposliciencia = await tipolicenciaTask;
            var tiposcuentabancaria = await tipoCuentaBancariaTask;


            EmpleadoModel empleado = new EmpleadoModel
            {
                ListaEspecialidades = especialidades,
                ListaSupervisores = supervisores,
                ListaTallaPantalones = pantalones,
                ListaTallasZapatos = zapatoslist,
                ListaTallasTop = tops,
                Paises = paises,
                Bancos = bancos,
                TiposEstadoCivil = estadosCivil,
                TiposLicenciaConducir = tiposliciencia,
                TiposCuentaBancaria = tiposcuentabancaria
            };



            
            return View(empleado);
        }     
       
        public async Task<JsonResult> CreaEmpleado(EmpleadoModel model)
        {
            
                try
                {
                    //Crear Cuenta del nuevo empleado
                    var cuentagastoTask = CrearCuentaAsync(new CuentaModel
                    {
                        Nombre = model.Nombre + " " + model.Apellido,
                        IDTipo = 1,
                        IDSubTipo = 1,
                        CreadoPor = User.Identity.Name,
                        EditadoPor = User.Identity.Name,
                        Descripcion = "Cuenta que recibe transferencias efectuadas a este miembro del personal y que deben ser rendidas."
                    });
                    var cuentaPagosTask = CrearCuentaAsync(new CuentaModel
                    {
                        Nombre = model.Nombre + " " + model.Apellido,
                        IDTipo = 4,
                        IDSubTipo = 2,
                        CreadoPor = User.Identity.Name,
                        EditadoPor = User.Identity.Name,
                        Descripcion = "Cuenta que recibe transferencias efectuadas a este miembro del personal para el pago de servicios prestados, devoluciones y otros."
                    });
                    await Task.WhenAll(cuentagastoTask, cuentaPagosTask);
                   var cuentagasto = await cuentagastoTask;
                   var cuentapagos = await cuentaPagosTask;
                    //Crear nuevo empleado
                    model.CuentaGastos = cuentagasto;
                    model.CuentaPagos = cuentapagos;
                    model.IDEmpleado = GetRut(model.IDEmpleado);
                    model.CreadoPor = model.EditadoPor = User.Identity.Name;
                   var id = await CreateEmployeeAsync(model);
                model.CuentaBancaria.IDEmpleado = id;
                await nuevaCuentaBancariaAsync(model.CuentaBancaria);
                    return Json("Creado");
                }
                catch(Exception e)
                {
                    return Json("Error");
                }
            
            //else
            //{
            //    var esp = EspecialidadesAsync();
            //    var super = SupervisoresAsync();
            //    var pants = TallaPantalonAsync();
            //    var zapatos = TallaZapatosAsync();
            //    var top = TallaTop();
            //    var paisesTask = getPaisesAsync();
            //    var bancosTask = getBancosAsync();
            //    var estadoCivilTask = getEstadosCivilAsync();
            //    var tipolicenciaTask = getTiposLicenciaAsync();
            //    var tipoCuentaBancariaTask = getTiposCuentaBancariaAsync();
            //    await Task.WhenAll(esp, super, pants, zapatos, top, paisesTask, bancosTask, estadoCivilTask, tipolicenciaTask, tipoCuentaBancariaTask);
            //    var especialidades = await esp;
            //    var supervisores = await super;
            //    var pantalones = await pants;
            //    var zapatoslist = await zapatos;
            //    var tops = await top;
            //    var paises = await paisesTask;
            //    var bancos = await bancosTask;
            //    var estadosCivil = await estadoCivilTask;
            //    var tiposliciencia = await tipolicenciaTask;
            //    var tiposcuentabancaria = await tipoCuentaBancariaTask;
            //    model.ListaEspecialidades = especialidades;
            //    model.ListaSupervisores = supervisores;
            //    model.ListaTallaPantalones = pantalones;
            //    model.ListaTallasZapatos = zapatoslist;
            //    model.ListaTallasTop = tops;
            //    model.Paises = paises;
            //    model.Bancos = bancos;
            //    model.TiposEstadoCivil = estadosCivil;
            //    model.TiposLicenciaConducir = tiposliciencia;
            //    model.TiposCuentaBancaria = tiposcuentabancaria;
            //    return View(model);
            //}
                
         
        }       
        public async Task<ActionResult> Edit(int id)
        {
            
            var empleado = LoadEmployeeAsync(id);
            var esp = EspecialidadesAsync();
            var super = SupervisoresAsync();
            var pants = TallaPantalonAsync();
            var zapatos = TallaZapatosAsync();
            var top = TallaTop();
            var paisesTask = getPaisesAsync();
            var bancosTask = getBancosAsync();
            var estadoCivilTask = getEstadosCivilAsync();
            var tipolicenciaTask = getTiposLicenciaAsync();
            var tipoCuentaBancariaTask = getTiposCuentaBancariaAsync();
            var cuentaEmpleadoTask = getCuentaEmpleadoAsync(id);
            await Task.WhenAll(esp, super, pants, zapatos, top, empleado, paisesTask, bancosTask, estadoCivilTask, tipolicenciaTask, tipoCuentaBancariaTask, cuentaEmpleadoTask);
            var employee = await empleado;
            var especialidades = await esp;
            var supervisores = await super;
            var pantalones = await pants;
            var zapatoslist = await zapatos;
            var tops = await top;
            var paises = await paisesTask;
            var bancos = await bancosTask;
            var estadosCivil = await estadoCivilTask;
            var tiposliciencia = await tipolicenciaTask;
            var tiposcuentabancaria = await tipoCuentaBancariaTask;
            var cuentabancariaempleado = await cuentaEmpleadoTask;
            employee.ListaEspecialidades = especialidades;
            employee.ListaSupervisores = supervisores;
            employee.ListaSupervisores = supervisores;
            employee.ListaTallaPantalones = pantalones;
            employee.ListaTallasZapatos = zapatoslist;
            employee.ListaTallasTop = tops;
            employee.Paises = paises;
            employee.Bancos = bancos;
            employee.TiposEstadoCivil = estadosCivil;
            employee.TiposLicenciaConducir = tiposliciencia;
            employee.TiposCuentaBancaria = tiposcuentabancaria;
            employee.CuentaBancaria = cuentabancariaempleado;
            return View(employee);
        }
        [AllowAnonymous]
        public async Task<ActionResult> ActualizacionDatos(int idempleado, string cod)
        {
           
            if (cod != null)
            {
                var estado = await getEstadoActualizadoAsync(idempleado);
                if (estado.Actualizado==false)
                {
                    var codigo = await getCodEmpleadoAsync(idempleado);
                    if (codigo.Cod == cod)
                    {
                        var empleado = LoadEmployeeAsync(idempleado);
                        var esp = EspecialidadesAsync();
                        var super = SupervisoresAsync();
                        var pants = TallaPantalonAsync();
                        var zapatos = TallaZapatosAsync();
                        var top = TallaTop();
                        var paisesTask = getPaisesAsync();
                        var bancosTask = getBancosAsync();
                        var estadoCivilTask = getEstadosCivilAsync();
                        var tipolicenciaTask = getTiposLicenciaAsync();
                        var tipoCuentaBancariaTask = getTiposCuentaBancariaAsync();
                        var cuentaEmpleadoTask = getCuentaEmpleadoAsync(idempleado);
                        await Task.WhenAll(esp, super, pants, zapatos, top, empleado, paisesTask, bancosTask, estadoCivilTask, tipolicenciaTask, tipoCuentaBancariaTask, cuentaEmpleadoTask);
                        var employee = await empleado;
                        var especialidades = await esp;
                        var supervisores = await super;
                        var pantalones = await pants;
                        var zapatoslist = await zapatos;
                        var tops = await top;
                        var paises = await paisesTask;
                        var bancos = await bancosTask;
                        var estadosCivil = await estadoCivilTask;
                        var tiposliciencia = await tipolicenciaTask;
                        var tiposcuentabancaria = await tipoCuentaBancariaTask;
                        var cuentaEmpleado = await cuentaEmpleadoTask;
                        employee.ListaEspecialidades = especialidades;
                        employee.ListaSupervisores = supervisores;
                        employee.ListaSupervisores = supervisores;
                        employee.ListaTallaPantalones = pantalones;
                        employee.ListaTallasZapatos = zapatoslist;
                        employee.ListaTallasTop = tops;
                        employee.Paises = paises;
                        employee.Bancos = bancos;
                        employee.TiposEstadoCivil = estadosCivil;
                        employee.TiposLicenciaConducir = tiposliciencia;
                        employee.TiposCuentaBancaria = tiposcuentabancaria;
                        employee.CuentaBancaria = cuentaEmpleado;
                        employee.IDEmpleado = FormatRutView(employee.IDEmpleado);
                        return View(employee);
                    }
                    else
                    {
                        return RedirectToAction("VistaError", "Errores", new Error { Mensaje = "Código erroneo" });
                    }
                }
                else
                {
                    return RedirectToAction("VistaError", "Errores", new Error { Mensaje = "El enlace ya no está disponible" });
                }
            }
            else
            {
                return RedirectToAction("VistaError", "Errores", new Error { Mensaje = "No se ha propocionado el código de seguridad" });
            }
          
        }

        public async Task<FileResult> ObtenerFormularioDatosEmpleado(int idempleado)
        {
            var empleado = await getEmployeeDataAsync(idempleado);
           var datos =   getFormActualizacionDatos(empleado);
            return File(datos, "application/pdf");

        }

        [AllowAnonymous]
        public async Task<JsonResult> ActualizarDatosAsync(EmpleadoModel modelo)
        {

            try
            {
                await actualizarDatosPersonalesAsync(modelo);
                var existe = await cuentaExisteAsync(modelo.ID);
                if (existe == true)
                {
                    
                    await actualizarCuentaBancariaAsync(modelo.CuentaBancaria);
                }
                else
                {
                    
                    await nuevaCuentaBancariaAsync(modelo.CuentaBancaria);
                }
                await onoffActualizadoAsync(new EmpleadoModel {Actualizado = true, ID = modelo.ID });
                return Json("Correcto");
            }
            catch (Exception e)
            {
                return Json(e.Message.ToString());
            }
        }
        [HttpPost]
        public async Task<JsonResult> Edit(EmpleadoModel model)
        {
           
                try
                {
                    model.EditadoEn = DateTime.Now;
                    model.EditadoPor = User.Identity.Name;
                     await  EditarEmpleadoAsync(model);
                var existe = await cuentaExisteAsync(model.ID);
                if (existe == false)
                {
                    await nuevaCuentaBancariaAsync(model.CuentaBancaria);
                }
                else
                {
                    await actualizarCuentaBancariaAsync(model.CuentaBancaria);
                }
                    return Json("Correcto");
                }
                catch(Exception e)
                {
                    return Json("Error");
                }
           
            
                       
       
        }
        // GET: Empleados/Delete/5
        public async System.Threading.Tasks.Task<ActionResult> Delete(int id)
        {

            var data = await LoadEmployeeAsync(id);
            var espe = await EspecialidadesAsync();
            var superv = await SupervisoresAsync();
            var pant = await TallaPantalonAsync();
            var top = await TallaTop();
            var zapatos = await TallaZapatosAsync();


        
            return View(data);
            




        }
        // POST: Empleados/Delete/5
        [HttpPost]
        public async Task<ActionResult> Delete(EmpleadoModel modelo)
        {
            try
            {
              await  BorrarEmpleadoAsync(modelo.ID);
                return RedirectToAction("VerEmpleados");
            }
            catch(Exception e)
            {
                return RedirectToAction("VistaError", "Errores", FuncionError(e.Message));
            }


        }
        public async Task<JsonResult> GetEmpleados(string id)
        {
            var valor = await FiltroEmpleadoAsync(id);
            return Json(valor);
            // return new JsonResult() { Data = valor };

        }
        //=========================================================================================================================
        //ESPECIALIDADES
        public async Task<ActionResult> VerEspecialidades()
        {
            var lista = await TablaEspecialidadesAsync();
            List<EspecialidadModel> especialidades = new List<EspecialidadModel>();
            foreach(var row in lista)
            {
                EspecialidadModel model = new EspecialidadModel
                {
                    ID = row.ID,
                    Especialidad = row.Especialidad,
                    ValorHH = row.ValorHH,
                    Descripcion = row.Descripcion,

                };
                especialidades.Add(model);
            }
          return View(especialidades);
        }
        public async Task<ActionResult> VerEspecialidad(int idespecialidad)
        {
            var modelo = await LoadEspecialidadAsync(idespecialidad);
            EspecialidadModel especialidad = new EspecialidadModel
            {
                ID = modelo.ID,
                CreadoEn = modelo.CreadoEn,
                CreadoPor = modelo.CreadoPor,
                Descripcion = modelo.Descripcion,
                EditadoEn = modelo.EditadoEn,
                EditadoPor = modelo.EditadoPor,
                Especialidad = modelo.Especialidad,
                ValorHH = modelo.ValorHH
            };
            return View(especialidad);

        }
        public ActionResult NuevaEspecialidad()
        {
            return View();
        }
        [HttpPost]
        public async Task<ActionResult> NuevaEspecialidad(EspecialidadModel modelo)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    modelo.CreadoPor = modelo.EditadoPor = User.Identity.Name;
                    await CreaEspecialidadAsync(modelo);
                    return RedirectToAction("VerEspecialidades");
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
        public async Task<ActionResult> EditarEspecialidad(int idespecialidad)
        {
            var modelo = await LoadEspecialidadAsync(idespecialidad);
            EspecialidadModel especialidad = new EspecialidadModel
            {
                ID = modelo.ID,
                Descripcion = modelo.Descripcion,
                Especialidad = modelo.Especialidad,
                ValorHH = modelo.ValorHH
            };
            return View(especialidad);

        }
        [HttpPost]
        public async Task<ActionResult> EditarEspecialidad(EspecialidadModel modelo)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    modelo.EditadoPor = User.Identity.Name;
                   await ActualizaEspecialidadAsync(modelo);
                    return RedirectToAction("VerEspecialidades");
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
        public async Task<ActionResult> EliminarEspecialidad(int idespecialidad)
        {
            var modelo = await LoadEspecialidadAsync(idespecialidad);
            EspecialidadModel especialidad = new EspecialidadModel
            {
                ID = modelo.ID,
                Descripcion = modelo.Descripcion,
                Especialidad = modelo.Especialidad,
                ValorHH = modelo.ValorHH,
                CreadoEn = modelo.CreadoEn,
                CreadoPor = modelo.CreadoPor,
                EditadoEn = modelo.EditadoEn,
                EditadoPor = modelo.EditadoPor
            };
            return View(especialidad);

        }
        [HttpPost]
        public async Task<ActionResult> EliminarEspecialidad(EspecialidadModel modelo)
        {
            try
            {
              await  EliminaEspecialidadAsync(modelo.ID);
                return RedirectToAction("VerEspecialidades");
            }

            catch(Exception e)
            {

                return RedirectToAction("VistaError", "Errores", FuncionError(e.Message));
            }


        }

        //=======================================================================================================================
        //Liquidaciones
        //=======================================================================================================================
        public async Task<JsonResult> getEmpleadosAsync()
        {
            return Json(await ListaEmpleadosAsync());
            
        }
        public async Task<ActionResult> FileUpload()
        {
            var emps = await ListaEmpleadosAsync();
            var meses = await getMeses();
            AppWebSantaBeatriz.Models.Empleados.EmpModel modelo = new AppWebSantaBeatriz.Models.Empleados.EmpModel
            {
                Empleados = emps,
                Meses = meses
                
            };
            return View(modelo);
        }
        [HttpPost]
        public async Task<ActionResult> FileUploadAsync(AppWebSantaBeatriz.Models.Empleados.EmpModel files)
        {

            String FileExt = Path.GetExtension(files.Documento.FileName).ToUpper();

            if (FileExt == ".PDF")
            {
                BibliotecaDeClases.Modelos.Empleados.EmpModel modelo = new BibliotecaDeClases.Modelos.Empleados.EmpModel
                {
                    IDEmpleado = files.IDEmpleado,
                    Ano = files.Ano,
                    Mes = files.Mes,
                    Documento = files.Documento,
                    CreadoPor = User.Identity.Name,
                    EditadoPor = User.Identity.Name
                };
                if (files.Multi == "Multi")
                {
                    await subirLiquidaciones(modelo);
                }
                else
                {
                    await subirLiquidacion(modelo);
                }
              //await SubirArchivo("liquidaciones", modelo);
               
                return RedirectToAction("VerLiquidaciones");
            }
            else
            {

                ViewBag.FileStatus = "Invalid file format.";

                return View();

            }

        }
        [HttpGet]
        public async Task<FileResult> DownLoadFile(string iddoc)
        {
            var data = await CargarLiquidacionAsync(iddoc, "liquidaciones");


            return File(data, "application/pdf");

        }
        public async Task<ActionResult> VerLiquidaciones()
        {
            
         
            var data = await getLiquidacionesAsync();
            var mesesT =  getMesesAsync();
            var mesesselectT = getDistinctMesLiquidaciones();
            var añoselectT = getDistinctAñoLiquidaciones();
            await Task.WhenAll(mesesT, mesesselectT, añoselectT);
            var meses = await mesesT;
            var mesesselect = await mesesselectT;
            var añosselect = await añoselectT;


            data.ForEach( x => { x.Mes = meses.Where(y => y.Numero == x.Mes).FirstOrDefault().Nombre; });
            mesesselect.ForEach(x => { x.Text = meses.Where(y => y.Numero == x.Value).FirstOrDefault().Nombre; });
            data.FirstOrDefault().MesesSelect = mesesselect;
            data.FirstOrDefault().Años = añosselect;
            return View(data);
        }
        [HttpGet]
        public async System.Threading.Tasks.Task<ActionResult> DeleteLiquidacionAsync(string iddoc)
        {
            await EliminarArchivo(iddoc, "liquidaciones");
            await deleteLiquidacionAsync(iddoc);
            return RedirectToAction("VerLiquidaciones");
        }
        public async Task<JsonResult> firmaLiquidacionAsync(string iddoc)
        {
            await firmarLiquidacion(iddoc);
           return Json("");
        } 
        [HttpGet]
        public async Task<FileResult> DownloadFirmadas(string mes, string año)
        {
            var data = await getLiquidacionesMergedMes(mes, año);

            return File(data, "application/pdf");

        }
        //=======================================================================================================================
        //Vacaciones
        //=======================================================================================================================
        public async  Task<ActionResult> RegistrarVacaciones(int idempleado)
        {
            var modelo = await getDiasDisponiblesAsync(idempleado);
            var registro = await getRegistroVacacionesEmpleadoAsync(idempleado);
            var periodos = await getPeriodosAsync(idempleado);
            modelo.Registro = registro;
            modelo.Periodos = periodos;
            return View(modelo);
        }
        [HttpPost]
        public async Task<ActionResult> RegistrarVacaciones(RegistroVacacionesModel modelo)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    modelo.CreadoEn = modelo.EditadoEn = DateTime.Now;
                    modelo.CreadoPor = modelo.EditadoPor = User.Identity.Name;
                    await insertarRegistroVacacionesAsync(modelo);
                    return RedirectToAction("VerEmpleados");
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

        public async Task<ActionResult> VerRegistroVacaciones()
        {
            var data = await getDiasDisponiblesAsync();
            return View(data);
        }

        public async Task<ActionResult> EditarRegistroVacaciones(int id)
        {
          
            var modelo = await getRegistroVacacionesAsync(id);
            var dias = await getDiasDisponiblesAsync(modelo.IDEmpleado);
            var registro = await getRegistroVacacionesEmpleadoAsync(modelo.IDEmpleado);
            modelo.Registro = registro;
            modelo.DiasDisponibles = dias.DiasDisponibles;
            return View(modelo);
        }

        [HttpPost]
        public async Task<ActionResult> EditarRegistroVacaciones(RegistroVacacionesModel modelo)
        {
            if (ModelState.IsValid)
            {
                try
                {

                    modelo.CreadoEn = modelo.EditadoEn = DateTime.Now;
                    modelo.CreadoPor = modelo.EditadoPor = User.Identity.Name;
                    await editarRegistroVacacionesAsync(modelo);
                    return RedirectToAction("VerRegistroVacaciones");
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
        [HttpGet]
        public async Task<FileResult> DescargarComprobante(int id)
        {
            var data = await getRegistroVacacionesAsync(id);
            return File(getComprobanteAsync(data), "application/pdf");
            
           
        }
        [AllowAnonymous]
        public ActionResult PaginaExito()
        {
            return View();
        }
        //=======================================================================================================================
        //Finiquitos
        //=======================================================================================================================

        public async Task<ActionResult> NuevoFiniquito(int id)
        {
            var employee = await LoadEmployeeAsync(id);
            var modelo = new FiniquitoModel {IDEmpleado = employee.ID, Empleado = employee.Nombre + employee.Apellido};
            return View(modelo);
        }
        [HttpPost]
        public async Task<ActionResult> NuevoFiniquito(FiniquitoModel modelo)
        {
            await nuevoFiniquitoAsync(modelo);
            return RedirectToAction("Detalles", new { id = modelo.IDEmpleado });
        }
        public async Task<FileResult> DownloadFiniquito(string id)
        {
            var data = await descargarFiniquito(id);
            return File(data, "application/pdf");
        }
    }
}
