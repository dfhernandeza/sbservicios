using BibliotecaDeClases.Modelos.Transacciones;
using Microsoft.SqlServer.Server;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using static BibliotecaDeClases.Logica.Transacciones.ProcesadorOTCredito;
using static BibliotecaDeClases.Logica.Transacciones.ProcesadorCheques;
using static BibliotecaDeClases.Logica.Transacciones.ProcesadorOTDebito;
using static BibliotecaDeClases.Logica.Transacciones.ProcesadorOpTElectronica;
namespace BibliotecaDeClases.Logica.Transacciones
{
    public class ProcesadorTransacciones
    {
        /// <summary>Ingresa una lista de Transacciones y especifica el medio de pago.</summary>
        public static async Task CrearTransaccionAsync(List<TransaccionModel> model)
        {
            await Task.Run(() => {
                model.ForEach(async x =>
                {
                    string sql;
                    if(x.IDCuentaDB != 0)
                    {
                       sql = @"Insert into Transacciones (Item, Monto, Cantidad, IDCECO, Fecha, IDCuentaDB, IDDocumento, IDEmpleado, CreadoEn, CreadoPor, EditadoEn, EditadoPor) 
                       Output Inserted.ID          
                       Values (@Item, @Monto, @Cantidad, @IDCECO, @Fecha, @IDCuentaDB, @IDDocumento, @IDEmpleado, @CreadoEn, @CreadoPor, @EditadoEn, @EditadoPor)";

                    }
                    else
                    {
                        sql = @"Insert into Transacciones (Item, Monto, Cantidad, Fecha, IDCuentaCR, IDDocumento, IDEmpleado, CreadoEn, CreadoPor, EditadoEn, EditadoPor) 
                       Output Inserted.ID          
                       Values (@Item, @Monto, @Cantidad, @Fecha, @IDCuentaCR, @IDDocumento, @IDEmpleado, @CreadoEn, @CreadoPor, @EditadoEn, @EditadoPor)";
                    }
                    x.Item = x.Item.ToUpperCheckForNull();
                    var transaccion = await AccesoBD.LoadAsync(sql, x);
                if (x.CuentaCR != null)
                    {
                        if (x.CuentaCR == "CHEQUE")
                        {
                            x.Cheque.IDTransaccion = transaccion.ID;
                            await nuevoCheque(x.Cheque);
                        }
                        else if (x.CuentaCR.Length > 17)
                        {
                              if (x.CuentaCR.Substring(0, 18) == "TARJETA DE CRÉDITO")
                            {
                                x.OperacionTC.IDTransaccion = transaccion.ID;
                                await nuevaOperacion(x.OperacionTC);
                            }
                        }
                       
                        else if (x.CuentaCR == "TARJETA DE DÉBITO")
                        {
                            x.OperacionTD.IDTransaccion = transaccion.ID;
                            await nuevaOpDebito(x.OperacionTD);
                        }
                        else if (x.CuentaCR == "TRANSFERENCIA ELECTRÓNICA")
                        {
                            x.OperacionTE.IDTransaccion = transaccion.ID;
                            await nuevaOpTElectronica(x.OperacionTE);
                        }
                       
                      
                    }
                
                 
                });
               
            });         

        }
        /// <summary>Ingresa una Transacción.</summary>
        public static async Task ingresarTransaccionAsync(TransaccionModel model)
        {
            await Task.Run(async () =>
            {
                string sql = @"Insert into Transacciones (IDDocumento, Item, Monto, Cantidad, Fecha, IDCuentaCR, IDCuentaDB, IDCECO, CreadoEn, CreadoPor, EditadoEn, EditadoPor) 
                       Output Inserted.ID          
                       Values (@IDDocumento, @Item, @Monto, @Cantidad, @Fecha, @IDCuentaCR, @IDCuentaDB, @IDCECO, @CreadoEn, @CreadoPor, @EditadoEn, @EditadoPor)";
                model.Descripcion = model.Descripcion.ToUpperCheckForNull();
                model.Item = model.Item.ToUpperCheckForNull();
                await AccesoBD.LoadAsync(sql, model);
            });
        }
        /// <summary>Ingresa una Transacción sin especificar el centro de costos.</summary>
        public static async Task ingresarTransaccionNoCECOAsync(TransaccionModel model)
        {
            await Task.Run(async () =>
            {
                string sql = @"Insert into Transacciones (IDDocumento, Item, Monto, Cantidad, Fecha, IDCuentaCR, IDCuentaDB, Descripcion, CreadoEn, CreadoPor, EditadoEn, EditadoPor) 
                       Output Inserted.ID          
                       Values (@IDDocumento, @Item, @Monto, @Cantidad, @Fecha, @IDCuentaCR, @IDCuentaDB, @Descripcion, @CreadoEn, @CreadoPor, @EditadoEn, @EditadoPor)";
                model.Descripcion = model.Descripcion.ToUpperCheckForNull();
                model.Item = model.Item.ToUpperCheckForNull();
                await AccesoBD.LoadAsync(sql, model);
            });
        }
        /// <summary>Ingresa una Transacción y devuelve la ID de la fila insertada.</summary>
        public static async Task<int> ingresarTransaccionReturnIDAsync(TransaccionModel model)
        {
          return await Task.Run(async () =>
            {
                string sql = @"Insert into Transacciones (IDDocumento, Item, Monto, Cantidad, Fecha, IDCuentaCR, IDCuentaDB, IDCECO, CreadoEn, CreadoPor, EditadoEn, EditadoPor) 
                       Output Inserted.ID          
                       Values (@IDDocumento, @Item, @Monto, @Cantidad, @Fecha, @IDCuentaCR, @IDCuentaDB, @IDCECO, @CreadoEn, @CreadoPor, @EditadoEn, @EditadoPor)";
                model.Descripcion = model.Descripcion.ToUpperCheckForNull();
                model.Item = model.Item.ToUpperCheckForNull();
               var transa = await AccesoBD.LoadAsync(sql, model);
                return transa.ID;
            });
        }
        /// <summary>Devuelve detalle de Transaccion.</summary>
        public static async Task<List<TransaccionModel>> CargarTransaccionAsync(int id)
        {
            string sql = "Select * from Transacciones where ID = @ID";
            TransaccionModel modelo = new TransaccionModel
            {
                ID = id
            };
            return await AccesoBD.LoadDataAsync(sql, modelo);
        }
        public static async Task EditarTransaccionHHExtraAsync(TransaccionModel modelo)
        {
            await Task.Run(async() => {
                await AccesoBD.Comando("Update Transacciones Set Monto = @Monto, IDCECO = @IDCECO, EditadoPor = @EditadoPor, EditadoEn = @EditadoEn where ID = @ID", modelo);
            });

           

        }
        public static async Task<int> EliminarTransaccionAsync(int id)
        {
            string sql = "Delete from Transacciones where ID = @ID";
            TransaccionModel modelo = new TransaccionModel
            {
                ID = id
            };

            return await AccesoBD.Comando(sql, modelo);

        }
        public static async Task<List<TransaccionModel>> TransaccionesPorDocumentoAsync(int iddocumento)
        {
            string sql = @"Select Transacciones.ID, Transacciones.IDDocumento, Transacciones.Fecha, Transacciones.Item, 
            Transacciones.Cantidad, Transacciones.Monto, Transacciones.IDCuentaCR, Transacciones.IDCECO, Transacciones.IDEmpleado, 
            Transacciones.Descripcion, Transacciones.IDCuentaDB, Transacciones.CreadoPor, Transacciones.CreadoEn, 
            Transacciones.EditadoPor, Transacciones.EditadoEn, CECOs.Nombre CECO, Cuentas.Nombre CuentaDB 
            from Transacciones inner join Cuentas on Transacciones.IDCuentaDB = Cuentas.ID 
            inner join CECOs on Transacciones.IDCECO = CECOs.ID where IDDocumento = @IDDocumento";
            TransaccionModel modelo = new TransaccionModel
            {
                IDDocumento = iddocumento
            };
            return await AccesoBD.LoadDataAsync(sql, modelo);
        }
        public static async Task<List<TransaccionModel>> TransaccionesEnDocumentoAsync(int iddoc)
        {
            string sql = @"Select * from Transacciones where IDDocumento = @IDDocumento";
            TransaccionModel modelo = new TransaccionModel
            {
                IDDocumento = iddoc
            };
            return await AccesoBD.LoadDataAsync(sql, modelo);
        }
        public static async Task<List<AutoCompleteModel>> getItems(string item)
        {

            return await AccesoBD.LoadDataAsync("Select distinct item from Transacciones where IDCuentaDB is not null and item like '%'+ @Item +'%'", new AutoCompleteModel {Item = item});
        }
        public static async Task<List<AutoCompleteModel>> getAllItems()
        {

            return await AccesoBD.LoadDataAsync<AutoCompleteModel>("Select distinct item from Transacciones where IDCuentaDB is not null");
        }
        public static async Task<List<AutoCompleteModel>> getItemsCot(string item)
        {

            return await AccesoBD.LoadDataAsync(@"Select distinct item from CotizacionMateriales where Item like '%'+ @Item +'%' 
            union 
            select distinct item from Transacciones where IDCuentaDB is not null and Item like '%'+ @Item +'%'", new AutoCompleteModel { Item = item });
        }
        public static async Task<decimal> getPriceCot(string item)
        {

            var precio = await AccesoBD.LoadAsync(@"select monto Precio from Transacciones where Item = @Item
                        union select PUnitario Precio from CotizacionMateriales where Item = @Item", new AutoCompleteModel { Item = item });
            return precio.Precio;
        }
        public static async Task<List<SelectListItem>> getMediosPagoAsync()
        {

           return await Task.Run( async () =>
            {
                return await AccesoBD.LoadDataAsync<SelectListItem>(@"Select Cuentas.ID Value, Cuentas.Nombre Text from Cuentas inner join SubTipoCuentas 
                                                    on Cuentas.IDSubTipo = SubTipoCuentas.ID where Cuentas.IDSubtipo = 1 ");
            });
        }
        public static async Task<List<TransaccionModel>> getTransacciones()
        {

            return await Task.Run(async () => {return await AccesoBD.LoadDataAsync<TransaccionModel>(@"

                Select Documentos.IDDocumento,Transacciones.Fecha, NULL CECO, Nombre CuentaCR, 'PAGO' as Item, Transacciones.Cantidad,   Transacciones.Monto 
                from Transacciones inner join Cuentas on Transacciones.IDCuentaCR = Cuentas.ID 
                inner join Documentos on Transacciones.IDDocumento = Documentos.ID
                Union
                
                Select Documentos.IDDocumento, Transacciones.Fecha, CECOs.Nombre CECO, Cuentas.Nombre CuentaCR, Transacciones.Item, Transacciones.Cantidad, Transacciones.Monto 
                from Transacciones inner join Cuentas  on Transacciones.IDCuentaDB = Cuentas.ID
                inner join Documentos on Transacciones.IDDocumento = Documentos.ID
                inner join CECOs on Transacciones.IDCECO = CECOs.ID
                
                
                order by IDDocumento"); });

        }
        public static async Task editHHTransaccionAsync(TransaccionModel modelo)
        {
            await Task.Run(async () => {
                await AccesoBD.Comando("Update Transacciones Set Monto = @Monto, EditadoEn = @EditadoEn, EditadoPor = @EditadoPor where ID = @ID", modelo);
            });
            
        }
        //Todos los gastos
        public static async Task<List<TransaccionModel>> LoadTransaccionesAsync()
        {
            string sql = "Select * from Transacciones";
            return await AccesoBD.LoadDataAsync<TransaccionModel>(sql);
        }
        //Todos los gastos en una cuenta
        public static async Task<List<TransaccionModel>> LoadCostoPorCuentaAsync(int idcuenta)
        {
            TransaccionModel modelo = new TransaccionModel
            {
                IDCuentaCR = idcuenta,
                IDCuentaDB = idcuenta

            };

            string sql = "Select * from Transacciones where IDCuentaCR = @IDCuenta or IDCuentaDB = @IDCuenta";
            return await AccesoBD.LoadDataAsync(sql, modelo);


        }
        public static async Task<List<TransaccionModel>> LoadCostoPorCuentaFechaAsync(int idcuenta, DateTime fecha)
        {
            TransaccionModel modelo = new TransaccionModel
            {
                IDCuentaCR = idcuenta,
                IDCuentaDB = idcuenta,
                Fecha = fecha
            };

            string sql = "Select * from Transacciones where (IDCuentaCR = @IDCuenta or IDCuentaDB = @IDCuenta) and Fecha = @Fecha";
            return await AccesoBD.LoadDataAsync(sql, modelo);


        }
        public static async Task<List<TransaccionModel>> LoadCostoPorCuentaFechaAsync(int idcuenta, int mes, int año)
        {
            TransaccionModel modelo = new TransaccionModel
            {
                IDCuentaCR = idcuenta,
                IDCuentaDB = idcuenta,
                Mes = mes,
                Año = año
            };

            string sql = "Select * from Transacciones where (IDCuentaCR = @IDCuenta or IDCuentaDB = @IDCuenta) and  DatePart(MM,Fecha) = @Mes and DatePart(YEAR,Fecha) = @Año";
            return await AccesoBD.LoadDataAsync(sql, modelo);


        }
        public static async Task<int> LimpiaTransaccionesAsync(int id)
        {
            return await AccesoBD.Comando("Delete from Transacciones where IDDocumento = @IDDocumento", new TransaccionModel { IDDocumento = id });
        }
        //===========================================================================================
        //Costos=====================================================================================
        //===========================================================================================
        //Lista de gastos totales en un ceco 
        public static async Task<List<TransaccionModel>> LoadCostoPorCecoAsync(int idceco)
        {
            TransaccionModel modelo = new TransaccionModel
            {
                IDCECO = idceco
            };
            string sql = "Select * from Transacciones where IDCECO = @IDCECO";
            return await AccesoBD.LoadDataAsync(sql, modelo);


        }
        public static async Task<List<TransaccionModel>> loadTransaccionesPorServicio(int idservicio)
        {
            TransaccionModel modelo = new TransaccionModel
            {
                ID = idservicio
            };
            string sql = @"Select Item, Cantidad, Monto from Transacciones inner join CECOs on 
            Transacciones.IDCECO = CECOs.ID inner join Tareas on CECOs.IDTarea = Tareas.ID where 
            IDServicio = @ID";
            return await AccesoBD.LoadDataAsync(sql, modelo);


        }

        //Suma gasto total del servicio
        public static async Task<List<TransaccionModel>> LoadCostoPorServicioAsync(int idceco)
        {
            string sql = "Select Sum(Transacciones.Cantidad * Transacciones.Monto) Total from Transacciones inner join CECOs on Transacciones.IDCECO = CECOs.IDCECO where CECOs.IDCECO = @IDCECO";
            TransaccionModel modelo = new TransaccionModel
            {
                IDCECO = idceco
            };
            return await AccesoBD.LoadDataAsync(sql, modelo);

        }
        //Suma gasto total de una cuenta dentro de un servicio
        public static async Task<List<TransaccionModel>> LoadCostoPorServicioCuentaAsync(int idcuenta, int idceco)
        {
            TransaccionModel modelo = new TransaccionModel
            {
                IDCuentaCR = idcuenta,
                IDCECO = idceco
            };
            string sql = "Select Sum(Transacciones.Cantidad * Transacciones.Monto) Total from Transacciones inner join CECOs on Transacciones.IDCECO = CECOs.IDCECO where CECOs.IDCECO = @IDCECO and IDCuentaCR = @IDCuentaCR";
            return await AccesoBD.LoadDataAsync(sql, modelo);


        }
        //Lista de gastos en un servicio
        public static async Task<List<TransaccionModel>> GastosPorServicioAsync(int idceco)
        {
            string sql = "Select Fecha, Item, Cantidad, Monto, Cantidad * Monto Total from Transacciones inner join CECOs on CECOs.IDCECO = Transacciones.IDCECO inner join Servicios on CECOs.IDCECO = Servicios.IDCECO where IDCECO = @IDCECO";
            TransaccionModel modelo = new TransaccionModel
            {
                IDCECO = idceco
            };
            return await AccesoBD.LoadDataAsync(sql, modelo);


        }
        //===========================================================================================
        //Documentos ================================================================================
        //===========================================================================================
        public static async Task<DocumentoModel> CrearDocumento(DocumentoModel modelo)
        {
          return await Task.Run(async () =>
            {
                modelo.Comentarios = modelo.Comentarios.ToUpperCheckForNull();
                modelo.Descripcion = modelo.Descripcion.ToUpperCheckForNull();
                string sql = @"Insert into Documentos (IDDocumento, FechaDocumento, Descripcion, Comentarios, IDTipo, IDEmisor, CreadoPor, 
             CreadoEn, EditadoPor, EditadoEn, IDEmpleadoEmisor) output inserted.ID Values (@IDDocumento, @FechaDocumento, @Descripcion, 
            @Comentarios, @IDTipo, @IDEmisor, @CreadoPor, @CreadoEn, @EditadoPor, @EditadoEn, @IDEmpleadoEmisor)";
                return await  AccesoBD.LoadAsync(sql, modelo);
            });
       
  

                     
            


        }
        public static async Task<int> EditarDocumentoAsync(DocumentoModel documento)
        {
            documento.Descripcion = documento.Descripcion.ToUpperCheckForNull();
            documento.Comentarios = documento.Comentarios.ToUpperCheckForNull();
            string sql = @"Update Documentos Set IDDocumento = @IDDocumento, FechaDocumento = @FechaDocumento, Descripcion = @Descripcion, 
                        Comentarios = @Comentarios, IDEmisor = @IDEmisor, EditadoEn = @EditadoEn,  EditadoPor = @EditadoPor where ID= @ID";
     
            return await AccesoBD.Comando(sql, documento);
        }
        public static async Task<int> EliminarDocumentoAsync(int iddocumento)
        {

            //DocumentoModel modelo = new DocumentoModel
            //{
            //    ID = iddocumento
            //};
            return await AccesoBD.Comando("Delete from Documentos where ID = @ID", new {ID = iddocumento });
        }
        public async Task<List<DocumentoModel>> DocumentosPorCECOAsync(int idceco)

        {
            DocumentoModel modelo = new DocumentoModel
            {
                IDCECO = idceco
            };

            string sql = "Select Documentos.IDDocumento, Documentos.FechaDocumento, Documentos.Tipo, IDEmisor, sum(Transacciones.Cantidad*Transacciones.Monto) Total from Documentos inner join Transacciones on Documentos.IDDocumento = Transacciones.IDDocumento where Transacciones.IDCECO = @IDCECO group by Documentos.IDDocumento, Documentos.FechaDocumento, Documentos.Tipo, Documentos.IDEmisor";
            return await AccesoBD.LoadDataAsync(sql, modelo);
        }
        public static async Task<DocumentoModel> CargarDocumentoAsync(int iddocumento)
        {
            string sql = @"Select Documentos.ID, Documentos.IDDocumento, FechaDocumento, Documentos.Descripcion, 
            Comentarios, dbo.TotalDocumento(Documentos.ID) Total, IDEmisor, Documentos.IDTipo,  ProveedoresServicios.NombreFantasia Emisor
            from Documentos inner join ProveedoresServicios on 
            Documentos.IDEmisor = ProveedoresServicios.ID 		
            where Documentos.ID = @ID";
            DocumentoModel modelo = new DocumentoModel
            {
                ID = iddocumento
            };

            return await AccesoBD.LoadAsync(sql, modelo);
        }
        public static async Task<List<DocumentoModel>> FiltroIntervaloAsync(DateTime fechainicial, DateTime fechafinal)
        {
            string sql = "Select * from Documentos where FechaDocumento > = @FechaFiltroInicial and FechaDocumento < = @FechaFiltroFinal";

            DocumentoModel modelo = new DocumentoModel
            {
                FechaFiltroInicial = fechainicial,
                FechaFiltroFinal = fechafinal
            };
            return await AccesoBD.LoadDataAsync(sql, modelo);
        }
        public static async Task<List<DocumentoModel>> FiltroEmisorInternoAsync(string emisor)
        {
            string sql = "Select * from Documentos inner join Personal on Documentos.IDEmisor = Personal.IDEmpleado where Personal.Nombre like '%' + @Emisor + '%' or Personal.Apellido like '%' + @Emisor + '%'";

            DocumentoModel modelo = new DocumentoModel
            {
                Emisor = emisor
            };
            return await AccesoBD.LoadDataAsync(sql, modelo);
        }
        public static async Task<List<DocumentoModel>> FiltroEmisorExternoAsync(string emisor)
        {
            string sql = @"Select documentos.ID, IDDocumento, FechaDocumento, Descripcion, Comentarios, ProveedoresServicios.NombreFantasia Emisor, 
                        ([dbo].[TotalDocumento](Documentos.ID)) Total from Documentos 
                        inner join ProveedoresServicios on Documentos.IDEmisor = ProveedoresServicios.ID 
                        where ProveedoresServicios.IDProveedor like '%' + @Emisor + '%' or ProveedoresServicios.NombreFantasia like '%' + @Emisor + '%' 
                        or ProveedoresServicios.NombreProveedor like '%' + @Emisor + '%' or Documentos.IDDocumento like '%' + @Emisor + '%'";

            DocumentoModel modelo = new DocumentoModel
            {
                Emisor = emisor
            };
            return await AccesoBD.LoadDataAsync(sql, modelo);
        }
        public static async Task<List<DocumentoModel>> FiltroEmisorFechaAsync(string emisor, DateTime fecha)
        {
            string sql = "Select IDDocumento, FechaDocumento, Descripcion, Comentarios, ProveedoresServicios.NombreFantasia Emisor from Documentos inner join ProveedoresServicios on Documentos.IDEmisor = ProveedoresServicios.IDProveedor where ProveedoresServicios.IDProveedor like '%' + @Emisor + '%' or ProveedoresServicios.NombreFantasia like '%' + @Emisor + '%' or ProveedoresServicios.NombreProveedor like '%' + @Emisor + '%' and FechaDocumento = @FechaFiltro";

            DocumentoModel modelo = new DocumentoModel
            {
                Emisor = emisor,
                FechaFiltro = fecha
            };
            return await AccesoBD.LoadDataAsync(sql, modelo);
        }
        public static async Task<List<DocumentoModel>> FiltroMensualAsync(int mes, int year)
        {
            DocumentoModel modelo = new DocumentoModel
            {
                Mes = mes,
                Year = year
            };
            string sql = @"Select IDDocumento , FechaDocumento ,Descripcion , Comentarios , ProveedoresServicios.NombreFantasia Emisor , ([dbo].[TotalDocumento](Documentos.ID)) Total, Documentos.ID from Documentos
                           inner join ProveedoresServicios on Documentos.IDEmisor = ProveedoresServicios.ID where DatePart(Month,FechaDocumento) = @Mes and DatePart(YEAR,FechaDocumento)= @Year and Documentos.IDTipo = 1 ";

            return await AccesoBD.LoadDataAsync(sql, modelo);
        }
        public static async Task<List<DocumentoModel>> FiltroFechaAsync(DateTime fecha)
        {
            string sql = @"Select Documentos.ID, IDDocumento , FechaDocumento ,Descripcion , Comentarios , ProveedoresServicios.NombreFantasia Emisor ,  ([dbo].[TotalDocumento](Documentos.ID)) Total 
                         from Documentos inner join ProveedoresServicios on Documentos.IDEmisor = ProveedoresServicios.ID   
                        where DATEPART(Y, FechaDocumento) = DATEPART(Y, @FechaFiltro) and DATEPART(YEAR, FechaDocumento) = DATEPART(YEAR, @FechaFiltro)";
            DocumentoModel modelo = new DocumentoModel
            {
                FechaFiltro = fecha
            };

            return await AccesoBD.LoadDataAsync(sql, modelo);
        }
        //===========================================================================================
        //Tipos de Documentos ========================================================================
        //===========================================================================================
        public static async Task<int> CrearTipoDocumentoAsync(string nombre, string descripcion, string creadopor)
        {

            string sql = "Insert into TipoDocumento (Nombre, Descripcion, CreadoPor, CreadoEn, EditadoPor, EditadoEn) Values (@Nombre, @Descripcion, @CreadoPor, @CreadoEn, @EditadoPor, @EditadoEn)";
            TipoDocumentoModel modelo = new TipoDocumentoModel
            {
                Nombre = nombre,
                Descripcion = descripcion,
                CreadoPor = creadopor,
                EditadoEn = DateTime.Now,
                CreadoEn = DateTime.Now,
                EditadoPor = creadopor

            };
            return await AccesoBD.Comando(sql, modelo);
        }
        public static async Task<List<SelectListItem>> ListaTipoDocumentoAsync()
        {
            string sql = "Select * from TipoDocumento";
            var data = await AccesoBD.LoadDataAsync<TipoDocumentoModel>(sql);
            List<SelectListItem> lista = new List<SelectListItem>();
            foreach (var row in data)
            {
                lista.Add(new SelectListItem { Text = row.Nombre, Value = row.ID.ToString() });
            }

            return lista;
        }
        public static async Task<List<TipoDocumentoModel>> CargarTiposDocumentosAsync()

        {
            string sql = "Select * from TipoDocumento";
            return await AccesoBD.LoadDataAsync<TipoDocumentoModel>(sql);
        }
        public static async Task<int> EliminarTipoDocumentoAsync(int idtipo)
        {
            string sql = "Delete from TipoDocumento where ID = @ID";
            TipoDocumentoModel modelo = new TipoDocumentoModel
            {
                ID = idtipo
            };

            return await AccesoBD.Comando(sql, modelo);

        }
        public static async Task<List<TipoDocumentoModel>> CargarTipoDocumentoAsync(int idtipo)
        {
            string sql = "Select * from TipoDocumento where ID = @ID";
            TipoDocumentoModel modelo = new TipoDocumentoModel
            {
                ID = idtipo
            };

            return await AccesoBD.LoadDataAsync(sql, modelo);

        }
        public static async Task<int> EditarTipoDocumentoAsync(int idtipo, string nombre, string descripcion, string actualizadopor)
        {
            string sql = "Update TipoDocumento set Nombre = @Nombre, Descripcion = @Descripcion, EditadoPor = @EditadoPor, EditadoEn = @EditadoEn where ID = @ID";
            TipoDocumentoModel modelo = new TipoDocumentoModel
            {
                ID = idtipo,
                Nombre = nombre,
                Descripcion = descripcion,
                EditadoPor = actualizadopor,
                EditadoEn = DateTime.Now
            };

            return await AccesoBD.Comando(sql, modelo);
        }
        //===========================================================================================
        //Cuentas  ==================================================================================
        //===========================================================================================
        public static async Task<List<SelectListItem>> ListaDeCuentasAsync(int idtipo)
        {
            string sql = "Select * from Cuentas where IDTipo = @IDTipo";
            CuentaModel modelo = new CuentaModel
            {
                IDTipo = idtipo
            };
            List<SelectListItem> lista = new List<SelectListItem>();


            var data = await AccesoBD.LoadDataAsync(sql, modelo);

            foreach (var row in data)
            {

                lista.Add(new SelectListItem() { Text = row.Nombre, Value = row.ID.ToString() });


            }

            return lista;
        }
        public static async Task<List<SelectListItem>> ListaDeCuentasMPago()
        {
            string sql = "Select Nombre Text, ID Value from Cuentas where IDSubTipo = 1";    
            var data = await AccesoBD.LoadDataAsync<SelectListItem>(sql);
            return data;
        }
        public static async Task<List<SelectListItem>> ListaDeCuentasGastos()
        {
            string sql = "Select Nombre Text, ID Value from Cuentas where IDSubTipo = 2";
            var data = await AccesoBD.LoadDataAsync<SelectListItem>(sql);
            return data;
        }
        public static async Task<int> CrearCuentaAsync(string nombre, int idtipo, int idsubtipo, string insertadapor, string descripcion)
        {
            CuentaModel modelo = new CuentaModel
            {
                Nombre = nombre.ToUpper(),
                IDTipo = idtipo,
                IDSubTipo = idsubtipo,
                CreadoPor = insertadapor,
                EditadoPor = insertadapor,
                CreadoEn = DateTime.Now,
                EditadoEn = DateTime.Now,
                Descripcion = descripcion.ToUpper()
            };

            string sql = "Insert into Cuentas (Nombre, IDTipo, IDSubTipo, Creadopor, EditadoPor, CreadoEn, EditadoEn, Descripcion) Values (@Nombre, @IDTipo, @IDSubTipo, @Creadopor, @EditadoPor, @CreadoEn, @EditadoEn, @Descripcion)";

            return await AccesoBD.Comando(sql, modelo);


        }
        public static async Task<int> CrearCuentaAsync(CuentaModel modelo)
        {
            modelo.Nombre = modelo.Nombre.ToUpperCheckForNull();
            modelo.Descripcion = modelo.Descripcion.ToUpper();
            modelo.CreadoEn = modelo.EditadoEn = DateTime.Now;

        
            string sql = @"Insert into Cuentas (Nombre, IDTipo, IDSubTipo, Creadopor, EditadoPor, CreadoEn, EditadoEn, Descripcion) 
                        output inserted.ID
                            Values (@Nombre, @IDTipo, @IDSubTipo, @Creadopor, @EditadoPor, @CreadoEn, @EditadoEn, @Descripcion)";

            var cuenta = await AccesoBD.LoadAsync(sql, modelo);
            return cuenta.ID;

        }

        public static async Task<List<CuentaModel>> TodasLasCuentasAsync()
        {
            string sql = "Select Cuentas.ID, Cuentas.Nombre, TipoCuenta.Nombre Tipo, Cuentas.Descripcion from Cuentas inner join TipoCuenta on Cuentas.IDTipo = TipoCuenta.ID";
            return await AccesoBD.LoadDataAsync<CuentaModel>(sql);


        }
        public static async Task<int> EliminarCuentaAsync(int idcuenta)
        {
            string sql = "Delete from Cuentas where ID = @ID";
            CuentaModel modelo = new CuentaModel
            {
                ID = idcuenta
            };

            return await AccesoBD.Comando(sql, modelo);
        }
        public static async Task<int> EditaCuentaAsync(int idcuenta, string nombre, int idtipo, int idsubtipo, string descripcion, string actualizadapor)
        {
            CuentaModel modelo = new CuentaModel
            {
                ID = idcuenta,
                Nombre = nombre.ToUpper(),
                IDTipo = idtipo,
                EditadoPor = actualizadapor,
                EditadoEn = DateTime.Now,
                Descripcion = descripcion.ToUpper(),
                IDSubTipo = idsubtipo
            };
            string sql = "Update Cuentas Set Nombre = @Nombre, IDTipo = @IDTipo, IDSubTipo = @IDSubTipo, Descripcion = @Descripcion, EditadoPor = @EditadoPor, EditadoEn = @EditadoEn where ID = @ID";
            return await AccesoBD.Comando(sql, modelo);
        }
        public static async Task<CuentaModel> CargarCuentaAsync(int idcuenta)
        {
            string sql = "Select Cuentas.Nombre, Cuentas.ID, Cuentas.IDSubTipo, Cuentas.IDTipo, Cuentas.Descripcion, Cuentas.CreadoEn, Cuentas.CreadoPor, Cuentas.EditadoEn, Cuentas.EditadoPor, TipoCuenta.Nombre Tipo, SubTipoCuentas.SubTipo SubTipo" +
                " from cuentas inner join TipoCuenta on Cuentas.IDTipo = TipoCuenta.ID " +
                "inner join SubTipoCuentas on Cuentas.IDSubTipo = SubTipoCuentas.ID " +
                "where Cuentas.ID = @ID";
            CuentaModel modelo = new CuentaModel
            {
                ID = idcuenta
            };

            return await AccesoBD.LoadAsync(sql, modelo);
        }
        //===========================================================================================
        //Tipos de cuentas  ========================================================================
        //===========================================================================================
        public static async Task<List<SelectListItem>> ListaTiposDeCuentasAsync()
        {
            string sql = "Select * from TipoCuenta";

            List<SelectListItem> lista = new List<SelectListItem>();


            var data = await AccesoBD.LoadDataAsync<TipoCuentaModel>(sql);

            foreach (var row in data)
            {

                lista.Add(new SelectListItem() { Text = row.Nombre, Value = row.ID.ToString() });


            }

            return lista;
        }
        public static async Task<List<TipoCuentaModel>> TiposDeCuentasAsync()
        {
            string sql = "Select * from TipoCuenta";
            return await  AccesoBD.LoadDataAsync<TipoCuentaModel>(sql);

        }
        public static async Task<int> CrearTipoCuentaAsync(string nombre, string descripcion, string creadopor)
        {
            TipoCuentaModel modelo = new TipoCuentaModel
            {
                Nombre = nombre.ToUpper(),
                Descripcion = descripcion.ToUpper(),
                CreadoPor = creadopor,
                CreadoEn = DateTime.Now,
                EditadoEn = DateTime.Now,
                EditadoPor = creadopor
            };
            string sql = "Insert into TipoCuenta (Nombre, Descripcion, CreadoPor, CreadoEn, EditadoPor, EditadoEn) Values (@Nombre, @Descripcion, @CreadoPor, @CreadoEn, @EditadoPor, @EditadoEn)";

            return await AccesoBD.Comando(sql, modelo);
        }
        public static async Task<int> EliminarTipoCuentaAsync(int tipocuentacod)
        {
            string sql = "Delete from TipoCuenta where ID = @ID";
            TipoCuentaModel modelo = new TipoCuentaModel
            {
                ID = tipocuentacod
            };
            return await AccesoBD.Comando(sql, modelo);

        }
        public static async Task<int> EditarTipoCuentaAsync(int tipocuentacod,string nombre, string descripcion, string editadopor)
        {
            TipoCuentaModel modelo = new TipoCuentaModel
            {
                Nombre = nombre.ToUpper(),
                Descripcion = descripcion.ToUpper(),
                ID = tipocuentacod,
                EditadoPor = editadopor,
                EditadoEn = DateTime.Now
            };
            string sql = "Update TipoCuenta Set Nombre = @Nombre, Descripcion = @Descripcion, EditadoPor = @EditadoPor, EditadoEn = @EditadoEn where ID = @ID";
            return await AccesoBD.Comando(sql, modelo);
        }
        public static async Task<List<TipoCuentaModel>> TipoDeCuentaAsync(int tipocuentacod)
        {
            string sql = "Select * from TipoCuenta where ID = @ID";
            TipoCuentaModel modelo = new TipoCuentaModel
            {
                ID = tipocuentacod
            };
            return await AccesoBD.LoadDataAsync(sql, modelo);
        }
        //===========================================================================================
        //Centros de Costos  ========================================================================
        //===========================================================================================
        public static async Task<List<SelectListItem>> ListaCecosActivosAsync()
        {
            string sql = "Select CECOs.ID, Nombre, Gasto, CECOs.Descripcion, CECOs.CreadoPor, CECOs.CreadoEn, CECOs.EditadoPor, CECOs.EditadoEn from CECOs inner join EstadoCECO on CECOs.IDEstadoCECO = EstadoCECO.ID where EstadoCECO.Estado = 'Activo'";

            var data = await AccesoBD.LoadDataAsync<CECOsModel>(sql);
            List<SelectListItem> lista = new List<SelectListItem>();
            foreach (var row in data)
            {
                SelectListItem item = new SelectListItem { Text = row.Nombre, Value = row.ID.ToString() };
                lista.Add(item);
            }
            return lista;

        }
        public static async Task<int> NuevoCecoAsync(CECOsModel modelo)
        {
            string sql = @"Insert into CECOs (Nombre, CreadoEn, Descripcion, CreadoPor, EditadoPor, EditadoEn, IDEstadoCECO, IDEncargado, IDTipoCECO, IDTarea) 
            output inserted.ID Values (@Nombre, @CreadoEn, @Descripcion, @CreadoPor, @EditadoPor, @EditadoEn, 1, @IDEncargado, 1, @IDTarea)";
                       
            var datos = await AccesoBD.LoadAsync(sql, modelo);
            return datos.ID;
        }
        public static async Task<int> EliminaCecoAsync(int idceco)
        {
            string sql = "Delete from CECOs where ID = @ID";
            CECOsModel model = new CECOsModel
            {
                ID = idceco
            };

            return await AccesoBD.Comando(sql, model);
        }
        public static async Task<int> EditarCecoAsync(int idceco, string nombre, string descripcion, string actualizadopor, int idestadoceco, int idtipoceco)
        {
            string sql = "Update CECOs Set Nombre = @Nombre, Descripcion = @Descripcion, EditadoPor = @EditadoPor, EditadoEn = @EditadoEn, IDTipoCECO = @IDTipoCECO where ID = @ID";
            CECOsModel model = new CECOsModel
            {
                ID = idceco,
                Nombre = nombre.ToUpper(),
                Descripcion = descripcion.ToUpper(),
                EditadoEn = DateTime.Now,
                EditadoPor = actualizadopor,
                IDEstadoCECO = idestadoceco,
                IDTipoCECO = idtipoceco
            };

            return await AccesoBD.Comando(sql, model);
        }
        public static async Task<CECOsModel> CargarCecoAsync(int idceco)
        {
            string sql = "Select CECOs.Nombre Nombre, CECOs.ID, CECOs.Gasto, CECOs.CreadoEn, CECOs.Descripcion,CECOs.CreadoPor, CECOs.EditadoPor,CECOs.EditadoEn, CECOs.IDEstadoCECO, CECOs.IDEncargado, EstadoCECO.Estado, PersonalServicios.Nombre + ' ' + PersonalServicios.Apellido Encargado from CECOs inner join EstadoCECO on CECOs.IDEstadoCECO = EstadoCECO.ID inner join PersonalServicios on CECOs.IDEncargado = PersonalServicios.ID where CECOs.ID = @ID";
            CECOsModel model = new CECOsModel
            {
                ID = idceco
            };

           return await AccesoBD.LoadAsync(sql, model);
        }
        public static async Task<CECOsModel> CargarDetalleCecoAsync(int idceco)
        {
            string sql = @"Select CECOs.Nombre Nombre, CECOs.ID, [dbo].[GastoCECO] (@ID) Gasto,   CECOs.Gasto, CECOs.CreadoEn, CECOs.CreadoPor, CECOs.EditadoPor, CECOs.EditadoEn ,CECOs.Descripcion,                        
                        CECOs.IDEstadoCECO, EstadoCECO.Estado, PersonalServicios.Nombre + ' ' + PersonalServicios.Apellido Encargado from 
                        CECOs inner join EstadoCECO on CECOs.IDEstadoCECO = EstadoCECO.ID inner join PersonalServicios on CECOs.IDEncargado = PersonalServicios.ID 
                         where CECOs.ID = @ID";
            CECOsModel model = new CECOsModel
            {
                ID = idceco
            };

            return await AccesoBD.LoadAsync(sql, model);
        }
        public static async Task<List<CECOsModel>> CECOsPorEstadoAsync(int estado)

        {
            CECOsModel modelo = new CECOsModel
            {
                IDEstadoCECO = estado
            };
            string sql = @"Select CECOs.ID, Nombre, [dbo].[GastoCECO] (CECOs.ID)   Gasto, CECOs.CreadoEn, CECOs.Descripcion, EstadoCECO.Estado from CECOs inner join 
                         EstadoCECO on CECOs.IDEstadoCECO = EstadoCECO.ID where CECOs.IDEstadoCECO = @IDEstadoCECO";

            return await AccesoBD.LoadDataAsync(sql, modelo);

        }
        public static async Task<CECOsModel> getCECOIDAsync(int idtarea)
        {
            return await Task.Run(async () =>
            {
               return await AccesoBD.LoadAsync("Select ID, Nombre  from CECOs where IDTarea = @IDTarea ", new CECOsModel { IDTarea = idtarea });
               
            });
            
        }
        public static async Task<List<SelectListItem>> getCECOsIDAsync(int idservicio)
        {
            return await Task.Run(async () =>
            {
                return await AccesoBD.LoadDataAsync("Select Cecos.ID Value, Cecos.Nombre Text from CECOs inner join Tareas on CECOs.IDTarea = Tareas.ID where Tareas.IDServicio = @Value ", new SelectListItem { Value = idservicio.ToString() });

            });

        }

        //===========================================================================================
        //Estado de Centros de Costos
        //===========================================================================================
        public static async Task<int> NuevoEstadoCECOAsync(string estado, string descripcion, string creadopor)
        {
            string sql = "Insert into EstadoCECO (Estado, Descripcion, CreadoPor, CreadoEn, ActualizadoEn, ActualizadoPor) Values(@Estado, @Descripcion, @CreadoPor, @CreadoEn, @ActualizadoEn, @ActualizadoPor)";
            EstadoCECOModel modelo = new EstadoCECOModel
            {
                Estado = estado.ToUpper(),
                Descripcion = descripcion.ToUpper(),
                CreadoPor = creadopor,
                CreadoEn = DateTime.Now,
                ActualizadoEn = DateTime.Now,
                ActualizadoPor = creadopor

            };
            return await AccesoBD.Comando(sql, modelo);
        }
        public static async Task<int> EliminaEstadoCECOAsync(int ideestadoceco)
        {
            string sql = "Delete from EstadoCECO where ID= @ID";
            EstadoCECOModel modelo = new EstadoCECOModel
            {
                ID = ideestadoceco
            };

          return await AccesoBD.Comando(sql, modelo);
        }
        public static async Task<EstadoCECOModel> CargarEstadoCecoAsync(int ideestadoceco)
        {
            string sql = "Select * from EstadoCECO where ID = @ID";
            EstadoCECOModel modelo = new EstadoCECOModel
            {
                ID = ideestadoceco
            };
            return await AccesoBD.LoadAsync(sql,modelo);
        }
        public static async Task<List<EstadoCECOModel>> CargarEstadosCecoAsync()
        {
            string sql = "Select * from EstadoCECO";

            return await AccesoBD.LoadDataAsync<EstadoCECOModel>(sql);
        }
        public static async Task<int> EditaEstadoCECOAsync(int ideestadoceco, string estado, string actualizadopor, string descripcion)
        {
            EstadoCECOModel modelo = new EstadoCECOModel
            {
                ID = ideestadoceco,
                Estado = estado.ToUpper(),
                ActualizadoPor = actualizadopor,
                ActualizadoEn = DateTime.Now,
                Descripcion = descripcion
            };
            string sql = "Update EstadoCECO Set Estado = @Estado, Descripcion = @Descripcion, ActualizadoPor = @ActualizadoPor, ActualizadoEn = @ActualizadoEn where ID = @ID";
           return await AccesoBD.Comando(sql, modelo);

        }
        public static async Task<List<SelectListItem>> ListaEstadosCECOsAsync()
        {
            string sql = "Select * from EstadoCECO";
            var data = await AccesoBD.LoadDataAsync<EstadoCECOModel>(sql);
            List<SelectListItem> lista = new List<SelectListItem>();

            foreach(var row in data)
            {
                SelectListItem item = new SelectListItem { Text = row.Estado, Value = row.ID.ToString() };
                lista.Add(item);
            }

            return lista;

        }
        //===========================================================================================
        //SubTipo de Cuentas
        //===========================================================================================
        public static async Task<int> CreaSubTipoCuentaAsync(string subtipocuenta, string descripcion, string creadopor)
        {
            string sql = "Insert into SubTipoCuentas (SubTipo, Descripcion, CreadoPor, CreadoEn, EditadoPor, EditadoEn) Values (@SubTipo, @Descripcion, @CreadoPor, @CreadoEn, @EditadoPor, @EditadoEn)";
            SubTipoCuentaModel modelo = new SubTipoCuentaModel
            {
                SubTipo = subtipocuenta.ToUpper(),
                Descripcion = descripcion.ToUpper(),
                CreadoEn = DateTime.Now,
                CreadoPor = creadopor,
                EditadoEn = DateTime.Now,
                EditadoPor = creadopor
            };
            return await AccesoBD.Comando(sql, modelo);
        }
        public static async Task<int> EditaSubTipoCuentaAsync(int idsubtipo, string subtipocuenta, string descripcion, string editadopor)
        {
            string sql = "Update SubTipoCuentas Set SubTipo = @Subtipo, Descripcion = @Descripcion, EditadoPor = @EditadoPor, EditadoEn = @EditadoEn where ID = @ID";

            SubTipoCuentaModel model = new SubTipoCuentaModel
            {
                ID = idsubtipo,
                SubTipo = subtipocuenta.ToUpper(),
                Descripcion = descripcion.ToUpper(),
                EditadoEn = DateTime.Now,
                EditadoPor = editadopor

            };

            return await AccesoBD.Comando(sql, model);
        }
        public static async Task<int> EliminaSubTipoCuentaAsync(int idsubtipo)
        {
            SubTipoCuentaModel modelo = new SubTipoCuentaModel
            {
                ID = idsubtipo
            };
            string sql = "Delete from SubTipoCuentas where ID = @ID ";
           return await AccesoBD.Comando(sql, modelo);

        }
        public static async Task<List<SelectListItem>> ListaSubTipoCuentasAsync()
        {
            string sql = "Select * from SubTipoCuentas";
            var data = await AccesoBD.LoadDataAsync<SubTipoCuentaModel>(sql);
            List<SelectListItem> lista = new List<SelectListItem>();
            foreach(var row in data)
            {
                SelectListItem subtipo = new SelectListItem { Text = row.SubTipo, Value = row.ID.ToString() };
                lista.Add(subtipo);
            }
            return lista;
        }
        public static async Task<List<SubTipoCuentaModel>> ListaSubtiposAsync()
        {
            string sql = "Select * from SubTipoCuentas";
            return await AccesoBD.LoadDataAsync<SubTipoCuentaModel>(sql);

           
        }
        public static async Task<SubTipoCuentaModel> CargarSubTipoCuentaAsync(int idsubtipo)
        {
            string sql = "Select * from SubTipoCuentas where ID = @ID";
            SubTipoCuentaModel modelo = new SubTipoCuentaModel
            {
                ID = idsubtipo
            };
            return await AccesoBD.LoadAsync(sql,modelo);
        }
        //===========================================================================================
        //Colaciones
        //===========================================================================================
        public static async Task<List<SelectListItem>> getProveedoresColacionesAsync()
        {

            return await Task.Run(async () =>
            {

                return await AccesoBD.LoadDataAsync<SelectListItem>("Select ID Value, NombreFantasia Text from ProveedoresServicios where IDTipo = 2");
            });
        }
        public static async Task<List<SelectListItem>> getCecosServicioAsync(string idservicio)
        {

            return await Task.Run(async () =>
            {

                return await AccesoBD.LoadDataAsync(@"Select CECOs.ID Value, CECOs.Nombre Text from CECOs 
                                    inner join Tareas on CECOs.IDTarea = Tareas.ID where Tareas.IDServicio = @Value", new SelectListItem { Value = idservicio });
            });
        }

        //===========================================================================================
        //Rendiciones
        //===========================================================================================

        /// <summary>Obtiene lista de transferencias a empleados abiertas.</summary>
        public static async Task<List<RendicionModel>> getTransaccionesRendicion()
        {

            return await Task.Run(async () => { return await AccesoBD.LoadDataAsync<RendicionModel>(@"
        Select Documentos.ID, Documentos.FechaDocumento Fecha, PersonalServicios.Nombre + ' ' + PersonalServicios.Apellido Empleado , PersonalServicios.CuentaGastos IDCuentaEmpleado , [dbo].[UltimaActualizacionRendicion] (Documentos.ID) EditadoEn,  [dbo].[CuentaTransaccionesDocumento] (Documentos.ID) CuentaTransacciones,   
			sum(Transacciones.Cantidad * Transacciones.Monto)  Total,   [dbo].[MontoRendicion] (Documentos.ID) MontoRendido ,  [dbo].[FechaRendicion] (Documentos.ID) FechaRendicion   from Transacciones 
            inner join Documentos on Transacciones.IDDocumento = Documentos.ID 
            inner join Cuentas on Cuentas.ID = Transacciones.IDCuentaDB
			INNer join PersonalServicios on PersonalServicios.CuentaGastos = Cuentas.ID
            where Documentos.IDTipo = 7 and [dbo].[BalanceCuenta] (Cuentas.ID)  > 0
            group by  Documentos.ID, Documentos.FechaDocumento, PersonalServicios.Nombre + ' ' + PersonalServicios.Apellido, PersonalServicios.CuentaGastos"); });

        }
        /// <summary>Obtiene Detalle a rendir.</summary>
        public static async Task<RendicionModel> getDetalleRendirAsync(int iddoc)
        {

            return await Task.Run(async () => { return await AccesoBD.LoadAsync(@"Select Documentos.ID IDDocumento, Documentos.FechaDocumento Fecha, PersonalServicios.Nombre + ' ' + PersonalServicios.Apellido Empleado , PersonalServicios.CuentaGastos IDCuentaEmpleado ,
			sum(Transacciones.Cantidad * Transacciones.Monto)  Total  from Transacciones 
            inner join Documentos on Transacciones.IDDocumento = Documentos.ID 
            inner join Cuentas on Cuentas.ID = Transacciones.IDCuentaDB
			
			inner join PersonalServicios on PersonalServicios.CuentaGastos = Cuentas.ID
            where Documentos.ID = @ID
            group by  Documentos.ID, Documentos.FechaDocumento, PersonalServicios.Nombre + ' ' + PersonalServicios.Apellido, PersonalServicios.CuentaGastos", new RendicionModel { ID = iddoc }); });
        }
        /// <summary>Inserta Nuevo reporte de gastos sin detalle.</summary>
        public static async Task insertarReporteGastosAsync(ReporteGastosModel modelo)
        {
            await Task.Run(async () => { await AccesoBD.Comando(@"Insert into ReporteGastos (IDDocumento, FechaRendicion, MontoRendido, 
            CreadoPor, EditadoPor, CreadoEn, EditadoEn) Values (@IDDocumento, @FechaRendicion, @MontoRendido, 
            @CreadoPor, @EditadoPor, @CreadoEn, @EditadoEn)", modelo); });
            
        }
        /// <summary>Actualiza el reporte de gastos detallando.</summary>
        public static async Task actualizarReporteGastosAsync(ReporteGastosModel modelo)
        {
            await Task.Run(async () => { await AccesoBD.Comando(@"Update ReporteGastos Set FechaReporte = @FechaReporte, 
            @EditadoPor = @EditadoPor, @EditadoEn = @EditadoEn where ID = @ID)", modelo); });

        }
        /// <summary>Ingresa Transacciones, documentos y relaciona la rendicion.</summary>
        public static async Task nuevoDetalleRendicionAsync(ReporteGastosModel modelo)
        {
            await Task.Run(async () => {

                foreach (var item in modelo.Documentos)
                {
                  
                    var doc = await CrearDocumento(item);
                    foreach (var transaccion in item.Transacciones)
                    {
                        transaccion.IDDocumento = doc.ID;
                        await ingresarTransaccionAsync(transaccion);
                    }
                   await insertarDocumentoReporteGastosAsync(new DocumentoReporteGastosModel { IDDocumento = doc.ID, IDReporteGastos = modelo.ID });

                }
               await actualizarReporteGastosAsync(modelo);
            });
            
        }
        /// <summary>Relaciona documentos con rendiciones.</summary>
        public static async Task insertarDocumentoReporteGastosAsync(DocumentoReporteGastosModel modelo)
        {
            await Task.Run(async () =>
            {
                await AccesoBD.Comando(@"Insert into DocumentoReporteGastos (IDDocumento, IDReporteGastos, CreadoPor, CreadoEn, EditadoPor, EditadoEn)
                                        Values (@IDDocumento, @IDReporteGastos, @CreadoPor, @CreadoEn, @EditadoPor, @EditadoEn);
                                         Update ReporteGastos Set FechaReporte = @EditadoEn where ID = @IDReporteGastos   ", modelo);
            });
           
        }

        public static async Task<int> getIDDocForReporteGastosAsync(int iddoc)
        {
            return await Task.Run(async () => {
                var doc = await AccesoBD.LoadAsync(@"Select distinct Documentos.ID from Documentos 
                inner join Transacciones on Transacciones.IDDocumento = Documentos.ID
                inner join Cuentas on Cuentas.ID = Transacciones.IDCuentaDB 
                where Documentos.IDTipo = 7 and [dbo].[BalanceCuenta] (Cuentas.ID)  > 0 and Transacciones.IDCuentaDB = @ID", new DocumentoModel { ID = iddoc });
                return doc.ID;
            });
        }

        public static async Task<CuentaModel> getCuentaRendicionAsync(int iddoc)
        {
            return await Task.Run(async () => {
                return await AccesoBD.LoadAsync(@"	Select distinct Cuentas.ID, Cuentas.Nombre, ReporteGastos.ID IDReporte from Cuentas 
                inner join Transacciones on Transacciones.IDCuentaDB = Cuentas.ID
                inner join Documentos on Transacciones.IDDocumento = Documentos.ID
				inner join ReporteGastos on ReporteGastos.IDDocumento = Documentos.ID
                where Documentos.ID = @ID", new CuentaModel { ID = iddoc });
                
            });
        }


    }
}
