using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using static BibliotecaDeClases.Logica.Transacciones.ProcesadorTransacciones;
using static BibliotecaDeClases.ProcesadorEmpleados;
using static BibliotecaDeClases.Logica.Funciones;
using static BibliotecaDeClases.Logica.Traducción.TraductorProcesador;
using System.Threading.Tasks;
using BibliotecaDeClases.Modelos.Transacciones;
using static BibliotecaDeClases.Logica.Transacciones.ProcesadorMP;
using static BibliotecaDeClases.Logica.Transacciones.ProcesadorOpTElectronica;
using static BibliotecaDeClases.Logica.Transacciones.ProcesadorCheques;
using static BibliotecaDeClases.Logica.Transacciones.ProcesadorOTDebito;
using static BibliotecaDeClases.Logica.Transacciones.ProcesadorOTCredito;
using static BibliotecaDeClases.Logica.Transacciones.ProcesadorPagoEfectivo;
namespace AppWebSantaBeatriz.Controllers
{
    [Authorize]
    public class TransaccionController : Controller
    {
        //Transacciones de Gastos
        public async Task<JsonResult> obtenerItems(string term)
        {
            var data = await getItems(term);
            string[] items = data.Select(x => x.Item).ToArray();
            return Json(items, JsonRequestBehavior.AllowGet);
        }
        public async Task<JsonResult> obtenerAllItems()
        {
            var data = await getAllItems();
            string[] items = data.Select(x => x.Item).ToArray();
            return Json(items, JsonRequestBehavior.AllowGet);
        }
        public async Task<ActionResult> NuevaCompra(int? iddocrendicion)
        {
            if (iddocrendicion != null)
            {
                var cuenta = await getCuentaRendicionAsync((int)iddocrendicion);
                ViewBag.Rendicion = true;
                ViewBag.IDDocRendicion = iddocrendicion;
                ViewBag.CuentaPago = cuenta;
                ViewBag.IDReporte = cuenta.IDReporte;

            }
            else
            {
                ViewBag.Rendicion = false;
            }
            var mpago = cargarMediosPagoSL();
            var cd = ListaDeCuentasGastos();
            var lc = ListaCecosActivosAsync();
            await Task.WhenAll(mpago, cd, lc);
            ViewBag.ListaMedioPago = await mpago;
            ViewBag.ListaCuentaDestino = await cd;
            ViewBag.ListaCECOs = await lc;
            ViewBag.Tipo = 1;

            return View();

        }
        public ActionResult DetalleCompra()
        {
            return View();
        }
        public async Task<ActionResult> EditaCompra(int iddocumento)
        {

            var mpago = cargarMediosPagoSL();
            var cd = ListaDeCuentasGastos();
            var lc = ListaCecosActivosAsync();
            await Task.WhenAll(mpago, cd, lc);
            ViewBag.ListaMedioPago = await mpago;
            ViewBag.ListaCuentaDestino = await cd;
            ViewBag.ListaCECOs = await lc;
            ViewBag.Tipo = 1;
            var data = CargarDocumentoAsync(iddocumento);
            var datos = TransaccionesPorDocumentoAsync(iddocumento);
            var cheqs = cargarChequesDoc(iddocumento);
            var efe = getPagosEfectivo(iddocumento);
            var td = cargarOpsDebito(iddocumento);
            var tc = cargarOperaciones(iddocumento);
            var ote = cargarOpsTelEctronica(iddocumento);
            await Task.WhenAll(data, datos, cheqs, efe, td, tc);
            var documento = await data;
            var transacciones = await datos;
            var cheques = await cheqs;
            var efectivos = await efe;
            var tdebito = await td;
            var tcredito = await tc;
            var transferencias = await ote;

            documento.Transacciones = transacciones;
            documento.OpTElectronicas = transferencias;
            documento.Cheques = cheques;
            documento.TDebitos = tdebito;
            documento.Efectivos = efectivos;
            documento.TCreditos = tcredito;


            return View(documento);


        }
        public async Task<ActionResult> VerCompra(int iddocumento)
        {
            var data = CargarDocumentoAsync(iddocumento);
            var datos = TransaccionesPorDocumentoAsync(iddocumento);
            var cheqs = cargarChequesDoc(iddocumento);
            var efe = getPagosEfectivo(iddocumento);
            var td = cargarOpsDebito(iddocumento);
            var tc = cargarOperaciones(iddocumento);
            var ote = cargarOpsTelEctronica(iddocumento);
            await Task.WhenAll(data, datos, cheqs, efe, td, tc);
            var documento = await data;
            var transacciones = await datos;
            var cheques = await cheqs;
            var efectivos = await efe;
            var tdebito = await td;
            var tcredito = await tc;
            var transferencias = await ote;
            documento.Transacciones = transacciones;
            documento.OpTElectronicas = transferencias;
            documento.Cheques = cheques;
            documento.TDebitos = tdebito;
            documento.Efectivos = efectivos;
            documento.TCreditos = tcredito;
            return View(documento);
        }
        public async Task<JsonResult> EditarCompra(DocumentoModel documento)
        {
            documento.EditadoEn = DateTime.Now;
            documento.EditadoPor = User.Identity.Name;
            try
            {
                await LimpiaTransaccionesAsync(documento.ID);
                await EditarDocumentoAsync(documento);
                documento.Transacciones.ForEach(x =>
                {
                    //Asigna valores a propiedades
                    x.IDDocumento = documento.ID;
                    x.CreadoPor = x.EditadoPor = User.Identity.Name;
                    x.EditadoEn = x.CreadoEn = DateTime.Now;
                    x.Fecha = documento.FechaDocumento;
                    //Asigna valores a propiedades de medios de pago
                    if (x.CuentaCR != null)
                    {
                        if (x.CuentaCR == "CHEQUE")
                        {
                            x.Cheque.CreadoEn = x.Cheque.EditadoEn = DateTime.Now; x.Cheque.CreadoPor = x.Cheque.EditadoPor = User.Identity.Name;
                        }
                        else if (x.CuentaCR.Length > 17)
                        {
                            if (x.CuentaCR.Substring(0, 18) == "TARJETA DE CRÉDITO")
                            {
                                x.OperacionTC.EditadoPor = x.OperacionTC.CreadoPor = User.Identity.Name;
                                x.OperacionTC.CreadoEn = x.OperacionTC.EditadoEn = DateTime.Now;
                            }
                        }

                        else if (x.CuentaCR == "TRANSFERENCIA ELECTRÓNICA")
                        {
                            x.OperacionTE.EditadoEn = x.CreadoEn = DateTime.Now;
                            x.OperacionTE.EditadoPor = x.CreadoPor = User.Identity.Name;
                        }
                        else if (x.CuentaCR == "TARJETA DE DÉBITO")
                        {
                            x.OperacionTD.EditadoEn = x.OperacionTD.CreadoEn = DateTime.Now;
                            x.OperacionTD.EditadoPor = x.OperacionTD.CreadoPor = User.Identity.Name;
                        }
                    }

                });
                //Inserta las transacciones
                await CrearTransaccionAsync(documento.Transacciones);
                return Json("La transacción ha sido ingresada.", JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {

                return Json(FuncionError(e.Message));
            }




        }
        public async Task<JsonResult> Egreso(DocumentoModel documento)
        {
            //Cuenta crédito sería el medio de pago, cuenta corriente, efectivo, etc
            //Efectivo = caja
            //T. débito = T. débito
            //T. crédtio = Cuentasporpagar
            //Cuenta débito 
            //Materiales, Equipos etc.
            documento.CreadoEn = documento.EditadoEn = DateTime.Now;
            documento.EditadoPor = documento.CreadoPor = User.Identity.Name;
            try
            {
                //Crea documento y captura la id
                var id = await CrearDocumento(documento);
                //Itera en las transacciones
                documento.Transacciones.ForEach(x =>
                {
                    //Asigna valores a propiedades
                    x.IDDocumento = id.ID;
                    x.CreadoPor = x.EditadoPor = User.Identity.Name;
                    x.EditadoEn = x.CreadoEn = DateTime.Now;
                    x.Fecha = documento.FechaDocumento;
                    //Asigna valores a propiedades de medios de pago
                    if (x.CuentaCR != null)
                    {
                        if (x.CuentaCR == "CHEQUE")
                        {
                            x.Cheque.CreadoEn = x.Cheque.EditadoEn = DateTime.Now; x.Cheque.CreadoPor = x.Cheque.EditadoPor = User.Identity.Name;
                        }
                        else if (x.CuentaCR.Length > 17)
                        {
                            if (x.CuentaCR.Substring(0, 18) == "TARJETA DE CRÉDITO")
                            {
                                x.OperacionTC.EditadoPor = x.OperacionTC.CreadoPor = User.Identity.Name;
                                x.OperacionTC.CreadoEn = x.OperacionTC.EditadoEn = DateTime.Now;
                            }
                        }

                        else if (x.CuentaCR == "TRANSFERENCIA ELECTRÓNICA")
                        {
                            x.OperacionTE.EditadoEn = x.CreadoEn = DateTime.Now;
                            x.OperacionTE.EditadoPor = x.CreadoPor = User.Identity.Name;
                        }
                        else if (x.CuentaCR == "TARJETA DE DÉBITO")
                        {
                            x.OperacionTD.EditadoEn = x.OperacionTD.CreadoEn = DateTime.Now;
                            x.OperacionTD.EditadoPor = x.OperacionTD.CreadoPor = User.Identity.Name;
                        }

                    }

                });
                //Inserta las transacciones
                await CrearTransaccionAsync(documento.Transacciones);
                //Asocia documento a una rendición
                if (documento.Rendicion)
                {
                    await insertarDocumentoReporteGastosAsync(new DocumentoReporteGastosModel
                    {
                        IDDocumento = id.ID,
                        IDReporteGastos = documento.IDReporteRendicion,
                        CreadoPor = User.Identity.Name,
                        CreadoEn = DateTime.Now,
                        EditadoPor = User.Identity.Name,
                        EditadoEn = DateTime.Now

                    });
                }

                return Json("La transacción ha sido ingresada.", JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {

                return Json(FuncionError(e.Message));
            }


        }
        [HttpPost]
        public async Task<ActionResult> Egresos(List<TransaccionModel> modelo)
        {

            try
            {
                await CrearTransaccionAsync(modelo);

            }
            catch (Exception e)
            {
                return RedirectToAction("VistaError", "Errores", FuncionError(e.Message));
            }



            return RedirectToAction("VerCompras");

        }

        public async Task<ActionResult> EliminaCompra(int iddocumento)
        {

            var data = await CargarDocumentoAsync(iddocumento);
            var datos = await TransaccionesPorDocumentoAsync(iddocumento);

            List<Models.Costos.TransaccionModel> lista = new List<Models.Costos.TransaccionModel>();

            foreach (var row in datos)
            {
                lista.Add(new Models.Costos.TransaccionModel { ID = row.ID, IDCECO = row.IDCECO, IDCuentaCR = row.IDCuentaCR, IDCuentaDB = row.IDCuentaDB, Item = row.Item, Monto = row.Monto, Cantidad = row.Cantidad, CuentaDB = row.CuentaDB, CECO = row.CECO });
            }




            Models.Transacciones.DocumentoModel modelo = new Models.Transacciones.DocumentoModel
            {
                IDDocumento = data.IDDocumento,
                FechaDocumento = data.FechaDocumento,
                IDEmisor = data.IDEmisor,
                IDTipo = data.IDTipo,
                Descripcion = data.Descripcion,
                Comentarios = data.Comentarios,
                ListaItems = lista,
                Total = data.Total,
                Emisor = data.Emisor,
                ID = data.ID,
                MedioPago = data.MedioPago,


            };

            return View(modelo);

        }
        public async Task<JsonResult> SuprCompra(int iddocumento)
        {
            await LimpiaTransaccionesAsync(iddocumento);
            await EliminarDocumentoAsync(iddocumento);
            return Json("Eliminado");
        }
        //===========================================================================================
        //Transacciones de Ingresos
        //===========================================================================================
        public ActionResult NuevaVenta()
        {
            return View();
        }
        public async Task<JsonResult> Ingreso(List<TransaccionModel> modelo)
        {
            //Cuenta débito sería el medio de pago, cuenta corriente, efectivo, etc
            //Efectivo = caja
            //T. débito = T. débito
            //T. crédtio = Cuentasporpagar
            //Cuenta crédito 
            //Materiales, Equipos etc.
            modelo.ForEach(x => x.CreadoEn = x.EditadoEn = DateTime.Now);
            modelo.ForEach(x => x.CreadoPor = x.EditadoPor = User.Identity.Name);
            await CrearTransaccionAsync(modelo);
            return Json("La transacción ha sido ingresada.", JsonRequestBehavior.AllowGet);

        }

        //Filtros de Transacciones
        public JsonResult CostoCuenta(int idcuenta)
        {
            return Json(LoadCostoPorCuentaAsync(idcuenta));
        }

        //==================================================================================================================================================================
        //Cuentas
        //==================================================================================================================================================================
        public async Task<ActionResult> NuevaCuenta()
        {
            var tipos = ListaTiposDeCuentasAsync();
            var subtipos = ListaSubTipoCuentasAsync();
            await Task.WhenAll(tipos, subtipos);
            var tips = await tipos;
            var subtips = await subtipos;
            Models.Transacciones.CuentaModel modelo = new Models.Transacciones.CuentaModel
            {
                TiposDeCuentas = tips,
                SubTiposDeCuentas = subtips
            };

            return View(modelo);
        }
        [HttpPost]
        public async Task<ActionResult> NuevaCuenta(Models.Transacciones.CuentaModel modelo)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    await CrearCuentaAsync(modelo.Nombre, modelo.IDTipo, modelo.IDSubTipo, User.Identity.Name.ToString(), modelo.Descripcion);
                    return RedirectToAction("VerCuentas", "Transaccion");
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
        public async Task<ActionResult> VerCuentas()
        {
            var data = await TodasLasCuentasAsync();

            List<Models.Transacciones.CuentaModel> lista = new List<Models.Transacciones.CuentaModel>();

            foreach (var row in data)
            {
                Models.Transacciones.CuentaModel modelo = new Models.Transacciones.CuentaModel
                {
                    ID = row.ID,
                    Nombre = row.Nombre,
                    IDTipo = row.IDTipo,
                    Tipo = row.Tipo,
                    EditadoPor = row.EditadoPor,
                    EditadoEn = row.EditadoEn,
                    CreadoEn = row.CreadoEn,
                    CreadoPor = row.CreadoPor,
                    Descripcion = row.Descripcion


                };
                lista.Add(modelo);
            }

            return View(lista);
        }
        public async Task<ActionResult> EditarCuenta(int id)
        {
            var data = CargarCuentaAsync(id);
            var tipos = ListaTiposDeCuentasAsync();
            var subtipos = ListaSubTipoCuentasAsync();
            await Task.WhenAll(data, tipos, subtipos);
            var datos = await data;
            var tips = await tipos;
            var subtips = await subtipos;

            Models.Transacciones.CuentaModel modelo = new Models.Transacciones.CuentaModel
            {
                ID = datos.ID,
                IDTipo = datos.IDTipo,
                TiposDeCuentas = tips,
                Nombre = datos.Nombre,
                SubTiposDeCuentas = subtips,
                IDSubTipo = datos.IDSubTipo,
                Descripcion = datos.Descripcion
            };
            return View(modelo);
        }
        [HttpPost]
        public async Task<ActionResult> EditarCuenta(Models.Transacciones.CuentaModel modelo)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    await EditaCuentaAsync(modelo.ID, modelo.Nombre, modelo.IDTipo, modelo.IDSubTipo, modelo.Descripcion, User.Identity.Name);
                    return RedirectToAction("VerCuentas");
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
        public async Task<ActionResult> EliminaCuenta(int id)
        {
            var data = await CargarCuentaAsync(id);
            var tipos = await ListaTiposDeCuentasAsync();
            Models.Transacciones.CuentaModel modelo = new Models.Transacciones.CuentaModel
            {
                ID = data.ID,
                IDTipo = data.IDTipo,
                TiposDeCuentas = tipos,
                Nombre = data.Nombre,
                Descripcion = data.Descripcion,
                Tipo = data.Tipo,
                SubTipo = data.SubTipo,
                CreadoEn = data.CreadoEn,
                CreadoPor = data.CreadoPor,
                EditadoEn = data.EditadoEn,
                EditadoPor = data.EditadoPor

            };
            return View(modelo);

        }
        [HttpPost]
        public async Task<ActionResult> EliminaCuenta(Models.Transacciones.CuentaModel modelo)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    await EliminarCuentaAsync(modelo.ID);
                    return RedirectToAction("VerCuentas", "Transaccion");
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
        //==================================================================================================================================================================
        //Tipos de cuentas
        //==================================================================================================================================================================
        public async Task<ActionResult> VerTiposDeCuentas()
        {
            var data = await TiposDeCuentasAsync();
            List<Models.Transacciones.TipoCuentaModel> lista = new List<Models.Transacciones.TipoCuentaModel>();

            foreach (var row in data)
            {
                Models.Transacciones.TipoCuentaModel modelo = new Models.Transacciones.TipoCuentaModel
                {
                    Descripcion = row.Descripcion,
                    ID = row.ID,
                    Nombre = row.Nombre
                };
                lista.Add(modelo);
            }

            return View(lista);

        }
        public ActionResult CreaTipoDeCuenta()
        {
            if (User.IsInRole("Admin"))
            {
                return View();
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }
        [HttpPost]
        public async Task<ActionResult> CreaTipoDeCuenta(Models.Transacciones.TipoCuentaModel modelo)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    await CrearTipoCuentaAsync(modelo.Nombre.ToUpperCheckForNull(), modelo.Descripcion.ToUpperCheckForNull(), User.Identity.Name);
                    return RedirectToAction("VerTiposDeCuentas", "Transaccion");
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
        public async Task<ActionResult> EliminaTipoDeCuenta(int id)
        {
            var data = await TipoDeCuentaAsync(id);

            Models.Transacciones.TipoCuentaModel modelo = new Models.Transacciones.TipoCuentaModel
            {
                ID = data.FirstOrDefault().ID,
                Nombre = data.FirstOrDefault().Nombre,
                Descripcion = data.FirstOrDefault().Descripcion

            };
            return View(modelo);

        }
        [HttpPost]
        public async Task<ActionResult> EliminaTipoDeCuenta(Models.Transacciones.TipoCuentaModel modelo)
        {

            try
            {
                await EliminarTipoCuentaAsync(modelo.ID);
                return RedirectToAction("VerTiposDeCuentas", "Transaccion");
            }
            catch (Exception e)
            {
                return RedirectToAction("VistaError", "Errores", FuncionError(e.Message));
            }

        }
        public async Task<ActionResult> EditaTipoCuenta(int id)
        {
            var data = await TipoDeCuentaAsync(id);

            Models.Transacciones.TipoCuentaModel modelo = new Models.Transacciones.TipoCuentaModel
            {
                ID = data.FirstOrDefault().ID,
                Descripcion = data.FirstOrDefault().Descripcion,
                Nombre = data.FirstOrDefault().Nombre

            };
            return View(modelo);


        }
        [HttpPost]
        public async Task<ActionResult> EditaTipoCuenta(Models.Transacciones.TipoCuentaModel modelo)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    await EditarTipoCuentaAsync(modelo.ID, modelo.Nombre, modelo.Descripcion, User.Identity.Name);
                    return RedirectToAction("VerTiposDeCuentas", "Transaccion");
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
        public async Task<JsonResult> LoadTiposdeCuentas()
        {
            return Json(await TiposDeCuentasAsync());
        }
        public async Task<JsonResult> LookTipoDeCuenta(int id)
        {
            var data = await TipoDeCuentaAsync(id);


            return Json(data);
        }

        public async Task<JsonResult> LookSubTipoCuenta(int id)
        {
            var data = await CargarSubTipoCuentaAsync(id);
            return Json(data);
        }
        public async Task<ActionResult> VerTipoDeCuenta(int id)
        {
            var data = await TipoDeCuentaAsync(id);

            Models.Transacciones.TipoCuentaModel modelo = new Models.Transacciones.TipoCuentaModel
            {
                ID = data.FirstOrDefault().ID,
                Descripcion = data.FirstOrDefault().Descripcion,
                Nombre = data.FirstOrDefault().Nombre

            };
            return View(modelo);
        }
        //==================================================================================================================================================================
        //Tipos de Documentos
        //==================================================================================================================================================================
        public ActionResult NuevoTipoDocumento()
        {
            return View();
        }
        [HttpPost]
        public async Task<ActionResult> NuevoTipoDocumento(Models.Transacciones.TipoDocumentoModel modelo)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    await CrearTipoDocumentoAsync(modelo.Nombre, modelo.Descripcion, User.Identity.Name.ToString());
                    return RedirectToAction("VerTiposDocumentos", "Transaccion");
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
        public async Task<ActionResult> VerTiposDocumentos()
        {
            var data = await CargarTiposDocumentosAsync();
            List<Models.Transacciones.TipoDocumentoModel> lista = new List<Models.Transacciones.TipoDocumentoModel>();
            foreach (var row in data)
            {
                lista.Add(new Models.Transacciones.TipoDocumentoModel { ID = row.ID, Nombre = row.Nombre, Descripcion = row.Descripcion });
            }

            return View(lista);

        }
        public async Task<ActionResult> EditaTipoDocumento(int idtipo)
        {
            var data = await CargarTipoDocumentoAsync(idtipo);
            Models.Transacciones.TipoDocumentoModel modelo = new Models.Transacciones.TipoDocumentoModel
            {
                ID = data.FirstOrDefault().ID,
                Descripcion = data.FirstOrDefault().Descripcion,
                Nombre = data.FirstOrDefault().Nombre
            };

            return View(modelo);

        }
        [HttpPost]
        public async Task<ActionResult> EditaTipoDocumento(Models.Transacciones.TipoDocumentoModel modelo)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    await EditarTipoDocumentoAsync(modelo.ID, modelo.Nombre, modelo.Descripcion, User.Identity.Name.ToString());
                    return RedirectToAction("VerTiposDocumentos");
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
        public async Task<ActionResult> EliminaTipoDocumento(int idtipo)
        {
            var data = await CargarTipoDocumentoAsync(idtipo);
            Models.Transacciones.TipoDocumentoModel modelo = new Models.Transacciones.TipoDocumentoModel
            {
                ID = data.FirstOrDefault().ID,
                Descripcion = data.FirstOrDefault().Descripcion,
                Nombre = data.FirstOrDefault().Nombre,
                CreadoEn = data.FirstOrDefault().CreadoEn,
                EditadoPor = data.FirstOrDefault().CreadoPor,
                EditadoEn = data.FirstOrDefault().EditadoEn,
                CreadoPor = data.FirstOrDefault().EditadoPor
            };

            return View(modelo);

        }
        [HttpPost]
        public async Task<ActionResult> EliminaTipoDocumento(Models.Transacciones.TipoDocumentoModel modelo)
        {
            try
            {
                await EliminarTipoDocumentoAsync(modelo.ID);
                return RedirectToAction("VerTiposDocumentos");
            }
            catch (Exception e)
            {
                return RedirectToAction("VistaError", "Errores", FuncionError(e.Message));
            }

        }
        //==================================================================================================================================================================
        //Documentos
        //==================================================================================================================================================================
        public ActionResult NuevoDocumento(string idemisor, string emisor, int idtipo, string tipo)
        {
            Models.Transacciones.DocumentoModel modelo = new Models.Transacciones.DocumentoModel
            {
                IDEmisor = idemisor,
                Emisor = emisor,
                IDTipo = idtipo,
                Tipo = tipo,
                FechaDocumento = null,


            };
            return View(modelo);
        }
        public async Task<JsonResult> NuevoDocument(DocumentoModel modelo)
        {
            var id = await CrearDocumento(modelo);
            return Json(id.ID);
        }
        public async Task<ActionResult> EditaDocumento(int iddocumento)
        {
            var data = await CargarDocumentoAsync(iddocumento);
            var datos = await TransaccionesPorDocumentoAsync(iddocumento);

            List<Models.Costos.TransaccionModel> lista = new List<Models.Costos.TransaccionModel>();

            foreach (var row in datos)
            {
                lista.Add(new Models.Costos.TransaccionModel { ID = row.ID, IDCECO = row.IDCECO, IDCuentaCR = row.IDCuentaCR, IDCuentaDB = row.IDCuentaDB, Item = row.Item, Monto = row.Monto, Cantidad = row.Cantidad });
            }




            Models.Transacciones.DocumentoModel modelo = new Models.Transacciones.DocumentoModel
            {
                IDDocumento = data.IDDocumento,
                FechaDocumento = data.FechaDocumento,
                IDEmisor = data.IDEmisor,
                IDTipo = data.IDTipo,
                Descripcion = data.Descripcion,
                Comentarios = data.Comentarios,
                ListaItems = lista,

            };

            return View(modelo);
        }
        public async Task<ActionResult> VerCompras()
        {
            var data = await FiltroMensualAsync(DateTime.Today.Month, DateTime.Today.Year);
            List<Models.Transacciones.DocumentoModel> lista = new List<Models.Transacciones.DocumentoModel>();
            foreach (var row in data)
            {
                lista.Add(new Models.Transacciones.DocumentoModel { IDDocumento = row.IDDocumento, FechaDocumento = row.FechaDocumento, Descripcion = row.Descripcion, Comentarios = row.Comentarios, Emisor = row.Emisor, Total = row.Total, ID = row.ID });


            };

            return View(lista);
        }
        public async Task<JsonResult> getCompras()
        {
            var data = await FiltroMensualAsync(DateTime.Today.Month, DateTime.Today.Year);
            return Json(data);
        }
        //==================================================================================================================================================================
        //Filtros de documentos=============================================
        //==================================================================================================================================================================
        public async Task<JsonResult> FiltroDocumentoMensual(int mes, int year)
        {
            return Json(await FiltroMensualAsync(mes, year));


        }
        public async Task<JsonResult> FiltroDocumentoEmisor(string emisor)
        {
            return Json(await FiltroEmisorExternoAsync(emisor));
        }
        public async Task<JsonResult> FiltroDocuFechaEmisor(string emisor, DateTime fecha)
        {
            return Json(await FiltroEmisorFechaAsync(emisor, fecha));
        }
        public async Task<JsonResult> FiltroDocumentoIntervalo(DateTime fechainicial, DateTime fechafinal)
        {
            return Json(await FiltroIntervaloAsync(fechainicial, fechafinal));
        }
        public async Task<JsonResult> FiltroDocumentoFecha(DateTime fecha)
        {
            return Json(await FiltroFechaAsync(fecha));
        }
        //==================================================================================================================================================================
        //Centros de costos=======================================================
        //==================================================================================================================================================================
        public async Task<ActionResult> NuevoCECO()
        {
            var estados = ListaEstadosCECOsAsync();
            var encargados = ListaEmpleadosAsync();
            await Task.WhenAll(estados, encargados);

            var estds = await estados;
            var encrgds = await encargados;

            BibliotecaDeClases.Modelos.Transacciones.CECOsModel modelo = new BibliotecaDeClases.Modelos.Transacciones.CECOsModel
            {
                Estados = estds,
                Encargados = encrgds
            };
            return View(modelo);
        }
        [HttpPost]
        public async Task<ActionResult> NuevoCECO(CECOsModel modelo)
        {
            if (ModelState.IsValid)
            {
                modelo.CreadoEn = modelo.EditadoEn = DateTime.Now;
                modelo.CreadoPor = modelo.EditadoPor = User.Identity.Name;
                try
                {
                    await NuevoCecoAsync(modelo);
                    return RedirectToAction("VerCECOs");
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
        public async Task<ActionResult> VerCECOs()
        {
            var data = await CECOsPorEstadoAsync(1);

            return View(data);

        }
        public async Task<ActionResult> EliminarCECO(int idceco)
        {
            var data = await CargarCecoAsync(idceco);

            Models.Transacciones.CECOsModel modelo = new Models.Transacciones.CECOsModel
            {
                IDCECOMain = data.ID,
                Nombre = data.Nombre,
                CECOCreadoEn = data.CreadoEn,
                IDEstadoCECO = data.IDEstadoCECO,
                DescripcionCECO = data.Descripcion,
                CECOCreadoPor = data.CreadoPor,
                CECOEditadoEn = data.EditadoEn,
                CECOEditadoPor = data.EditadoPor,
                EncargadoCECO = data.Encargado
            };
            return View(modelo);
        }
        [HttpPost]
        public async Task<ActionResult> EliminarCECO(Models.Transacciones.CECOsModel modelo)
        {
            try
            {
                await EliminaCecoAsync(modelo.IDCECOMain);
                return RedirectToAction("VerCECOs");
            }
            catch (Exception e)
            {
                return RedirectToAction("VistaError", "Errores", FuncionError(e.Message));
            }

        }
        public async Task<ActionResult> EditarCECO(int idceco)
        {
            var data = CargarCecoAsync(idceco);
            var cecos = ListaEstadosCECOsAsync();
            var encargados = ListaEmpleadosAsync();
            await Task.WhenAll(data, cecos, encargados);
            var datos = await data;
            var cecoslist = await cecos;
            var encargads = await encargados;
            datos.Estados = cecoslist;
            datos.Encargados = encargads;
            return View(datos);

        }
        [HttpPost]
        public async Task<ActionResult> EditarCECO(Models.Transacciones.CECOsModel modelo)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    await EditarCecoAsync(modelo.IDCECOMain, modelo.Nombre, modelo.DescripcionCECO, User.Identity.Name.ToString(), modelo.IDEstadoCECO, modelo.IDTipoCECO);
                    return RedirectToAction("VerCECOs");
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
        public async Task<ActionResult> DetalleCECO(int idceco)
        {
            var data = await CargarDetalleCecoAsync(idceco);


            return View(data);

        }

        //==================================================================================================================================================================
        //Estado de Centros de costos
        //==================================================================================================================================================================
        public ActionResult NuevoEstadoCeco()
        {
            return View();
        }
        [HttpPost]
        public async Task<ActionResult> NuevoEstadoCeco(Models.Transacciones.EstadoCECOModel modelo)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    await NuevoEstadoCECOAsync(modelo.Estado, modelo.Descripcion, User.Identity.Name.ToString());
                    return RedirectToAction("VerEstadoCecos");
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
        public async Task<ActionResult> VerEstadoCecos()
        {
            var data = await CargarEstadosCecoAsync();

            List<Models.Transacciones.EstadoCECOModel> lista = new List<Models.Transacciones.EstadoCECOModel>();

            foreach (var row in data)
            {
                Models.Transacciones.EstadoCECOModel estado = new Models.Transacciones.EstadoCECOModel
                {
                    Estado = row.Estado,
                    Descripcion = row.Descripcion,
                    ID = row.ID
                };
                lista.Add(estado);

            }

            return View(lista);
        }
        public async Task<ActionResult> EliminarEstadoCeco(int idestadoceco)
        {
            var data = await CargarEstadoCecoAsync(idestadoceco);

            Models.Transacciones.EstadoCECOModel modelo = new Models.Transacciones.EstadoCECOModel
            {
                ID = data.ID,
                Descripcion = data.Descripcion,
                ActualizadoEn = data.ActualizadoEn,
                ActualizadoPor = data.ActualizadoPor,
                CreadoEn = data.CreadoEn,
                CreadoPor = data.CreadoPor,
                Estado = data.Estado
            };

            return View(modelo);

        }
        [HttpPost]
        public async Task<ActionResult> EliminarEstadoCeco(Models.Transacciones.EstadoCECOModel modelo)
        {
            try
            {
                await EliminaEstadoCECOAsync(modelo.ID);
                return RedirectToAction("VerEstadoCecos");
            }
            catch (Exception e)
            {
                return RedirectToAction("VistaError", "Errores", FuncionError(e.Message));
            }

        }
        public async Task<ActionResult> EditarEstadoCeco(int idestadoceco)
        {
            var data = await CargarEstadoCecoAsync(idestadoceco);

            Models.Transacciones.EstadoCECOModel modelo = new Models.Transacciones.EstadoCECOModel
            {
                ID = data.ID,
                Descripcion = data.Descripcion,
                ActualizadoEn = data.ActualizadoEn,
                ActualizadoPor = data.ActualizadoPor,
                CreadoEn = data.CreadoEn,
                CreadoPor = data.CreadoPor,
                Estado = data.Estado
            };

            return View(modelo);

        }
        [HttpPost]
        public async Task<ActionResult> EditarEstadoCeco(Models.Transacciones.EstadoCECOModel modelo)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    await EditaEstadoCECOAsync(modelo.ID, modelo.Estado, User.Identity.Name.ToString(), modelo.Descripcion);
                    return RedirectToAction("VerEstadoCecos");
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
        //==================================================================================================================================================================
        //Subtipo de Cuentas
        //==================================================================================================================================================================
        public ActionResult NuevoSubTipoCuenta()
        {
            Models.Transacciones.SubTipoCuentaModel modelo = new Models.Transacciones.SubTipoCuentaModel
            {
                SubTipo = "",
                Descripcion = ""
            };
            return View(modelo);
        }
        [HttpPost]
        public async Task<ActionResult> NuevoSubTipoCuenta(Models.Transacciones.SubTipoCuentaModel modelo)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    await CreaSubTipoCuentaAsync(modelo.SubTipo.ToUpperCheckForNull(), modelo.Descripcion.ToUpperCheckForNull(), User.Identity.Name);
                    return RedirectToAction("VerSubTipoCuentas");
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
        public async Task<ActionResult> EditarSubTipoCuenta(int idsubtipocuenta)
        {
            var data = await CargarSubTipoCuentaAsync(idsubtipocuenta);
            Models.Transacciones.SubTipoCuentaModel modelo = new Models.Transacciones.SubTipoCuentaModel
            {
                ID = data.ID,
                Descripcion = data.Descripcion,
                SubTipo = data.SubTipo
            };
            return View(modelo);
        }
        [HttpPost]
        public async Task<ActionResult> EditarSubTipoCuenta(Models.Transacciones.SubTipoCuentaModel modelo)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    await EditaSubTipoCuentaAsync(modelo.ID, modelo.SubTipo, modelo.Descripcion, User.Identity.Name);
                    return RedirectToAction("VerSubTipoCuentas");
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
        public async Task<ActionResult> EliminarSubTipoCuenta(int idsubtipocuenta)
        {
            var data = await CargarSubTipoCuentaAsync(idsubtipocuenta);
            Models.Transacciones.SubTipoCuentaModel modelo = new Models.Transacciones.SubTipoCuentaModel
            {
                ID = data.ID,
                Descripcion = data.Descripcion,
                SubTipo = data.SubTipo,
                CreadoEn = data.CreadoEn,
                CreadoPor = data.CreadoPor,
                EditadoEn = data.EditadoEn,
                EditadoPor = data.EditadoPor
            };
            return View(modelo);
        }
        [HttpPost]
        public async Task<ActionResult> EliminarSubTipoCuenta(Models.Transacciones.SubTipoCuentaModel modelo)
        {
            try
            {
                await EliminaSubTipoCuentaAsync(modelo.ID);
                return RedirectToAction("VerSubTipoCuentas");
            }
            catch (Exception e)
            {
                return RedirectToAction("VistaError", "Errores", FuncionError(e.Message));
            }

        }
        public async Task<ActionResult> VerSubTipoCuentas()
        {
            var data = await ListaSubtiposAsync();
            List<Models.Transacciones.SubTipoCuentaModel> modelo = new List<Models.Transacciones.SubTipoCuentaModel>();
            foreach (var row in data)
            {
                Models.Transacciones.SubTipoCuentaModel item = new Models.Transacciones.SubTipoCuentaModel
                {
                    ID = row.ID,
                    CreadoEn = row.CreadoEn,
                    CreadoPor = row.CreadoPor,
                    Descripcion = row.Descripcion,
                    EditadoEn = row.EditadoEn,
                    EditadoPor = row.EditadoPor,
                    SubTipo = row.SubTipo
                };
                modelo.Add(item);
            }

            return View(modelo);

        }
        public async Task<ActionResult> DetalleSubTipoCuenta(int idsubtipocuenta)
        {
            var data = await CargarSubTipoCuentaAsync(idsubtipocuenta);
            Models.Transacciones.SubTipoCuentaModel modelo = new Models.Transacciones.SubTipoCuentaModel
            {
                ID = data.ID,
                Descripcion = data.Descripcion,
                SubTipo = data.SubTipo,
                CreadoEn = data.CreadoEn,
                CreadoPor = data.CreadoPor,
                EditadoEn = data.EditadoEn,
                EditadoPor = data.EditadoPor
            };
            return View(modelo);
        }

        //===========================================================================================================
        //Resumen Costos
        //===========================================================================================================
        public async Task<JsonResult> getCostosXCECO(int idceco)
        {
            return Json(await LoadCostoPorCecoAsync(idceco));
        }
        public async Task<JsonResult> getTransaccionesServicio(int idservicio)
        {

            return Json(await loadTransaccionesPorServicio(idservicio));
        }
        //==================================================================================================================================================================
        //Medios de Pago
        //==================================================================================================================================================================
        [HttpGet]
        public async Task<ActionResult> NuevoMedioPago()
        {
            var mpago = await ListaDeCuentasMPago();
            MediosPagoModel modelo = new MediosPagoModel
            {
                Cuentas = mpago
            };
            return View(modelo);
        }
        [HttpPost]
        public async Task<ActionResult> NuevoMedioPago(MediosPagoModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    model.CreadoPor = model.EditadoPor = User.Identity.Name;
                    await BibliotecaDeClases.Logica.Transacciones.ProcesadorMP.nuevoMedioPago(model);
                    return RedirectToAction("VerMediosPago");
                }
                catch (Exception e)
                {

                    return RedirectToAction("VistaError", "Errores", FuncionError(e.Message));
                }
            }
            else
            {
                model.Cuentas = await ListaDeCuentasMPago();
                return View(model);
            }


        }

        public async Task<ActionResult> VerMediosPago()
        {
            var data = await BibliotecaDeClases.Logica.Transacciones.ProcesadorMP.cargarMediosPago();
            return View(data);

        }

        //==================================================================================================================================================================
        //Transferencias
        //==================================================================================================================================================================
        public async Task<ActionResult> nuevaTransferencia()
        {
            var cuentas = await ListaDeCuentasMPago();
            TransferenciaModel modelo = new TransferenciaModel
            { CuentasOrigen = cuentas, CuentasDestino = cuentas };
            return View(modelo);
        }
        [HttpPost]
        public async Task<ActionResult> nuevaTransferencia(TransaccionModel model)
        {
            model.CreadoEn = model.EditadoEn = DateTime.Now;
            model.CreadoPor = model.EditadoPor = User.Identity.Name;
            model.Fecha = DateTime.Now;
            var doc = await CrearDocumento(new DocumentoModel
            {
                FechaDocumento = DateTime.Now,
                Descripcion = "TRANSFERENCIA INTERNA",
                Comentarios = model.Descripcion,
                IDTipo = 7,
                IDEmisor = "1",
                CreadoEn = DateTime.Now,
                EditadoEn = DateTime.Now,
                CreadoPor = User.Identity.Name,
                EditadoPor = User.Identity.Name
            });
            model.IDDocumento = doc.ID;
            await ingresarTransaccionNoCECOAsync(model);
            return RedirectToAction("VerTransacciones", new { idtipo = 7 });
        }

        //===================================================================================================================================================
        //Colaciones
        //===================================================================================================================================================

        public async Task<ActionResult> ingresoColaciones(string idservicio)
        {
            ColacionesModel modelo = new ColacionesModel();
            var mpago = getMediosPagoAsync();
            var cecos = getCecosServicioAsync(idservicio);
            var proveedores = getProveedoresColacionesAsync();
            var tareas = new List<Task> { mpago, cecos, proveedores };
            while (tareas.Count > 0)
            {
                Task terminada = await Task.WhenAny(tareas);
                if (terminada == mpago)
                {
                    modelo.MPagos = await mpago;
                }
                else if (terminada == cecos)
                {
                    modelo.CECOs = await cecos;
                }
                else if (terminada == proveedores)
                {
                    modelo.Proveedores = await proveedores;
                }
                tareas.Remove(terminada);
            }

            return View(modelo);
        }
        [HttpPost]
        public async Task<ActionResult> ingresoColaciones(TransaccionModel modelo)
        {
            var doc = await CrearDocumento(new BibliotecaDeClases.Modelos.Transacciones.DocumentoModel
            {
                FechaDocumento = modelo.Fecha,
                Comentarios = modelo.Descripcion,
                Descripcion = "COLACIONES",
                IDTipo = 4,
                CreadoEn = DateTime.Now,
                EditadoEn = DateTime.Now,
                IDEmisor = modelo.IDProveedor.ToString(),
                CreadoPor = User.Identity.Name,
                EditadoPor = User.Identity.Name
            });
            modelo.IDDocumento = doc.ID;
            modelo.CreadoPor = modelo.EditadoPor = User.Identity.Name;
            modelo.CreadoEn = modelo.EditadoEn = DateTime.Now;
            modelo.Item = "COLACIÓN";
            await ingresarTransaccionAsync(modelo);
            return Redirect(Request.UrlReferrer.ToString());
        }
        //===================================================================================================================================================
        //Viajes
        //===================================================================================================================================================
        public async Task<ActionResult> IngresoViajes(string idservicio)
        {
            BibliotecaDeClases.Modelos.Transacciones.DocumentoModel modelo = new BibliotecaDeClases.Modelos.Transacciones.DocumentoModel();
            var mpago = getMediosPagoAsync();
            var cecos = getCecosServicioAsync(idservicio);

            var tareas = new List<Task> { mpago, cecos };
            while (tareas.Count > 0)
            {
                Task terminada = await Task.WhenAny(tareas);
                if (terminada == mpago)
                {
                    modelo.CuentasCR = await mpago;
                }
                else if (terminada == cecos)
                {
                    modelo.ListaCECOsItems = await cecos;
                }

                tareas.Remove(terminada);
            }

            return View(modelo);
        }
        [HttpPost]
        public async Task<ActionResult> IngresoViajes(List<TransaccionModel> modelo)
        {
            modelo.Remove(modelo.Where(x => x.Monto == 0).FirstOrDefault());
            var doc = await CrearDocumento(new BibliotecaDeClases.Modelos.Transacciones.DocumentoModel
            {
                FechaDocumento = modelo.FirstOrDefault().Fecha,
                Comentarios = "MOVILIZACIÓN",
                Descripcion = "MOVILIZACIÓN",
                IDTipo = 5,
                CreadoEn = DateTime.Now,
                EditadoEn = DateTime.Now,
                IDEmisor = "16",
                CreadoPor = User.Identity.Name,
                EditadoPor = User.Identity.Name
            });
            modelo.ForEach(x =>
            {
                x.IDDocumento = doc.ID;
                x.CreadoPor = x.EditadoPor = User.Identity.Name;
                x.CreadoEn = x.EditadoEn = DateTime.Now;
                x.Cantidad = 1;

            });
            foreach (var item in modelo)
            {
                await ingresarTransaccionAsync(item);
            }
            return Json("Exito");
        }

        //===================================================================================================================================================
        //Rendiciones
        //===================================================================================================================================================

        public async Task<ActionResult> VerTransacciones()
        {
            var data = await getTransaccionesRendicion();
            return View(data);
        }
        public async Task<ActionResult> ProcesarRendicion(int iddoc)
        {
            var data = await getDetalleRendirAsync(iddoc);
            var transacciones = await TransaccionesEnDocumentoAsync(iddoc);
            data.Transacciones = transacciones;
            return View(data);

        }
        [HttpPost]
        public async Task<JsonResult> ProcesarRendicionAsync(ReporteGastosModel modelo)
        {

            try
            {
                modelo.CreadoEn = modelo.EditadoEn = modelo.FechaRendicion = modelo.FechaReporte = DateTime.Now;
                modelo.CreadoPor = modelo.EditadoPor = User.Identity.Name;
                await insertarReporteGastosAsync(modelo);
                if (modelo.Devolucion != null)
                {
                    modelo.Devolucion.EditadoEn = modelo.Devolucion.CreadoEn = modelo.Devolucion.Fecha = DateTime.Now;
                    modelo.Devolucion.CreadoPor = modelo.EditadoPor = User.Identity.Name;
                    await ingresarTransaccionNoCECOAsync(modelo.Devolucion);
                }
                return Json(RedirectToAction("VerTransacciones"));
            }
            catch (Exception e)
            {

                return Json(RedirectToAction("VistaError", "Errores", FuncionError(e.Message)));
            }


        }

        public async Task<ActionResult> DetallarRendicion(int idtransaccion)
        {
            var data = await getDetalleRendirAsync(idtransaccion);
            return View(data);
        }
        [HttpPost]
        public async Task<ActionResult> DetallarRendicionAsync(ReporteGastosModel modelo)
        {
            try
            {
                await nuevoDetalleRendicionAsync(modelo);
                return RedirectToAction("VerTransacciones");
            }
            catch (Exception e)
            {

                return RedirectToAction("VistaError", "Errores", FuncionError(e.Message));
            }


        }
        [HttpGet]
        public async Task<ActionResult> AmpliarRendicion(int iddoc, int idcuenta)
        {
            var cuentas = await ListaDeCuentasMPago();
            TransaccionModel modelo = new TransaccionModel
            {
                Cuentas = cuentas,
                IDDocumento = iddoc,
                IDCuentaDB = idcuenta
            };
            return View(modelo);

        }
        [HttpPost]
        public async Task<object> AmpliarRendicion(TransaccionModel model)
        {
            model.CreadoEn = model.EditadoEn = DateTime.Now;
            model.CreadoPor = model.EditadoPor = User.Identity.Name;
            model.Fecha = DateTime.Now;
            await ingresarTransaccionNoCECOAsync(model);
            return RedirectToAction("VerTransacciones", new { idtipo = 7 });



        }

        //===================================================================================================================================================
        //Remuneraciones
        //===================================================================================================================================================

        public async Task<ActionResult> IngresoRemuneraciones()
        {
            var empleados = await ListaEmpleadosAsync();
            RemuneracionesModel modelo = new RemuneracionesModel { Empleados = empleados };
            return View(modelo);
        }
        [HttpPost]
        public async Task<JsonResult> IngresoRemuneracionesAsync(List<RemuneracionesModel> modelo)
        {
            modelo.ForEach(x => { x.CreadoPor = x.EditadoPor = User.Identity.Name; x.CreadoEn = x.EditadoEn = DateTime.Now; });
            await BibliotecaDeClases.Logica.Transacciones.ProcesadorRemuneraciones.igresarRemuneraciones(modelo);
            return Json("Exito");
        }

    }
}