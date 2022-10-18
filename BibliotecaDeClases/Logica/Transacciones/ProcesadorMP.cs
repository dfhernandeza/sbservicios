using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using BibliotecaDeClases.Modelos.Transacciones;
using static BibliotecaDeClases.AccesoBD;
namespace BibliotecaDeClases.Logica.Transacciones
{
  public class ProcesadorCheques
    {
        public static async Task nuevoCheque(ChequeModel cheque)
        {
            await Task.Run(async () =>
            {
                string sql = @"Insert into Cheques (Serie, Fecha, IDReceptor, IDTransaccion, CreadoPor, EditadoPor, 
                        CreadoEn, EditadoEn) Values (@Serie, @Fecha, @IDReceptor, @IDTransaccion, @CreadoPor, @EditadoPor, 
                        @CreadoEn, @EditadoEn)";
                await Comando(sql, cheque);
            });
            
        }
        public static async Task editarCheque(ChequeModel cheque)
        {
            await Task.Run(async() =>
            {
                string sql = @"Update Cheques set Serie = @Serie , Fecha = @Fecha, Receptor = @Receptor, 
                             EditadoPor = @EditadoPor, EditadoEn = @EditadoEn) where ID = @ID";
                await AccesoBD.Comando(sql, cheque);
            });
        }
        public static async Task eliminarCheque(int id)
        {
            await Comando("Delete from Cheques where ID = @ID", new { ID = id });
            
        }
        public static async Task<ChequeModel> cargarCheque(int id)
        {
            return await LoadAsync("Select * from Cheques where ID = @ID", new ChequeModel { ID = id });
            
        }
        public static async Task<List<ChequeModel>> cargarCheques()
        {
           return await LoadDataAsync<ChequeModel>("Select * from Cheques");
         
        }
        public static async Task<List<ChequeModel>> cargarCheques(int idreceptor)
        {
            return await LoadDataAsync("Select * from Cheques where IDReceptor = @IDReceptor", new ChequeModel {IDReceptor = idreceptor } );

        }
        public static async Task<List<ChequeModel>> cargarCheques(DateTime fecha)
        {
            return await LoadDataAsync("Select * from Cheques where Fecha = @Fecha", new ChequeModel { Fecha = fecha});

        }
        public static async Task<List<ChequeModel>> cargarChequesDoc(int iddocumento)
        {
            return await LoadDataAsync(@"select Cheques.Serie, Cheques.Fecha, Transacciones.Monto, Transacciones.IDCuentaCR from Cheques inner join Transacciones on Cheques.IDTransaccion = Transacciones.ID
                                        inner join Documentos on Transacciones.IDDocumento = Documentos.ID
                                        where documentos.id = @ID", new ChequeModel { ID = iddocumento});

        }
    }
   public class ProcesadorOTCredito
    {
        public static async Task nuevaOperacion(OpTCreditoModel operacion)
        {
            await Comando(@"Insert into OpTCredito (IDTransaccion, Cuotas, Voucher, CreadoPor, EditadoPor, CreadoEn, EditadoEn) Values
                          (@IDTransaccion, @Cuotas, @Voucher, @CreadoPor, @EditadoPor, @CreadoEn, @EditadoEn)", operacion);            
        }
        public static async Task editarOperacion(OpTCreditoModel operacion)
        {
            await Comando(@"Update OpTCredito set IDTransaccion = @IDTransaccion, Cuotas = @Cuotas, Voucher = @Voucher, EditadoPor = @EditadoPor, 
                            EditadoEn = @EditadoEn where ID = @ID", operacion);
        }
        public static async Task eliminarOperacion(int id)
        {
            await Comando("Delete from OpTCredito where ID = @ID", new { ID = id });

        }
        public static async Task<OpTCreditoModel> cargarOperacion(int id)
        {
            return await LoadAsync("Select * from OpTCredito where ID = @ID", new OpTCreditoModel { ID = id });

        }
        public static async Task<List<OpTCreditoModel>> cargarOperaciones(int iddocumento)
        {
            return await LoadDataAsync(@"Select OpTCredito.Cuotas, OpTCredito.Voucher, Transacciones.Monto, Transacciones.IDCuentaCR from OpTCredito
                                                        inner join Transacciones on Transacciones.ID = OpTCredito.IDTransaccion 
                                                        inner join Documentos on Transacciones.IDDocumento = Documentos.id 
                                                        where Documentos.ID = @ID", new OpTCreditoModel {ID = iddocumento });
        }
    }

    public class ProcesadorMP
    {
        public static async Task nuevoMedioPago(MediosPagoModel medioPago)
        {
            medioPago.CreadoEn = medioPago.EditadoEn = DateTime.Now;
            medioPago.Descripcion = medioPago.Descripcion.ToUpperCheckForNull();
            medioPago.MedioPago = medioPago.MedioPago.ToUpperCheckForNull();
            await Comando(@"Insert into MediosPago (MedioPago, IDCuenta, Descripcion, CreadoPor, EditadoPor, CreadoEn, EditadoEn) Values
                          (@MedioPago, @IDCuenta, @Descripcion, @CreadoPor, @EditadoPor, @CreadoEn, @EditadoEn)", medioPago);
        }
        public static async Task editarMedioPago(MediosPagoModel medioPago)
        {
            await Comando(@"Update MediosPago set MedioPago = @MedioPago, Descripcion = @Descripcion, EditadoPor = @EditadoPor, 
            EditadoEn = @EditadoEn where ID = @ID", medioPago);
        }
        public static async Task eliminarMedioPago(int id)
        {
            await Comando("Delete from MediosPago where ID = @ID", new { ID = id });

        }
        public static async Task<MediosPagoModel> cargarMedioPago(int id)
        {
            return await LoadAsync("Select * from OpTCredito where ID = @ID", new MediosPagoModel { ID = id });

        }
        public static async Task<List<MediosPagoModel>> cargarMediosPago()
        {
            return await LoadDataAsync<MediosPagoModel>(@"Select MediosPago.ID, MedioPago, Cuentas.Nombre CuentaContable, MediosPago.Descripcion  from MediosPago inner join Cuentas 
             on Cuentas.ID = MediosPago.IDCuenta");
        }
        public static async Task<List<SelectListItem>> cargarMediosPagoSL()
        {
            return await LoadDataAsync<SelectListItem>(@"Select IDCuenta Value, MedioPago Text from MediosPago
            UNION 
            Select   PersonalServicios.CuentaGastos  Value , Cuentas.Nombre Text from Transacciones 
            inner join Documentos on Transacciones.IDDocumento = Documentos.ID 
            inner join Cuentas on Cuentas.ID = Transacciones.IDCuentaDB
			INNer join PersonalServicios on PersonalServicios.CuentaGastos = Cuentas.ID
            where Documentos.IDTipo = 7 and [dbo].[BalanceCuenta] (Cuentas.ID)  > 0
            group by  PersonalServicios.CuentaGastos, Cuentas.Nombre");
            
        }
    }

    public class ProcesadorOTDebito
    {
        public static async Task nuevaOpDebito(OpTDebito operacion)
        {
            await Comando(@"Insert into OpTDebito (IDTransaccion, Voucher, CreadoPor, EditadoPor, CreadoEn, EditadoEn) Values
                          (@IDTransaccion, @Voucher, @CreadoPor, @EditadoPor, @CreadoEn, @EditadoEn)", operacion);
        }
        public static async Task editarOpDebito(OpTDebito operacion)
        {
            await Comando(@"Update OpTDebito set IDTransaccion = @IDTransaccion, Voucher = @Voucher, EditadoPor = @EditadoPor, 
                            EditadoEn = @EditadoEn where ID = @ID", operacion);
        }
        public static async Task eliminarOpDebito(int id)
        {
            await Comando("Delete from OpTDebito where ID = @ID", new { ID = id });

        }
        public static async Task<OpTDebito> cargarOpDebito(int id)
        {
            return await LoadAsync("Select * from OpTDebito where ID = @ID", new OpTDebito { ID = id });

        }
        public static async Task<List<OpTDebito>> cargarOpsDebito(int iddocumento)
        {
            return await LoadDataAsync(@"Select OpTDebito.Voucher, Transacciones.Monto, Transacciones.IDCuentaCR  from OpTDebito 
                                                inner join Transacciones on OpTDebito.IDTransaccion = Transacciones.ID 
                                                inner join Documentos on Transacciones.IDDocumento = Documentos.ID
                                                where Documentos.ID = @ID", new OpTDebito {ID = iddocumento });
        }
    }
    public class ProcesadorOpTElectronica
    {
        public static async Task nuevaOpTElectronica(OpTElectronica operacion)
        {
            await Comando(@"Insert into OpTElectronica (IDTransaccion, IDOperacion, CreadoPor, EditadoPor, CreadoEn, EditadoEn) Values
                          (@IDTransaccion, @IDOperacion, @CreadoPor, @EditadoPor, @CreadoEn, @EditadoEn)", operacion);
        }
        public static async Task editarOpTElectronica(OpTElectronica operacion)
        {
            await Comando(@"Update OpTElectronica set IDTransaccion = @IDTransaccion, IDOperacion = @IDOperacion, EditadoPor = @EditadoPor, 
                            EditadoEn = @EditadoEn where ID = @ID", operacion);
        }
        public static async Task eliminarOpTElectronica(int id)
        {
            await Comando("Delete from OpTElectronica where ID = @ID", new { ID = id });

        }
        public static async Task<OpTElectronica> cargarOpTElectronica(int id)
        {
            return await LoadAsync("Select * from OpTElectronica where ID = @ID", new OpTElectronica { ID = id });

        }
        public static async Task<List<OpTElectronica>> cargarOpsTelEctronica(int iddocumento)
        {
            return await LoadDataAsync(@"select OpTElectronica.IDTransaccion, Transacciones.Monto, Transacciones.IDCuentaCR from OpTElectronica 
                                                        inner join Transacciones on OpTElectronica.IDTransaccion = Transacciones.ID
                                                        inner join Documentos on Transacciones.IDDocumento = Documentos.ID
                                                        where documentos.id = @ID", new OpTElectronica {ID = iddocumento });
        }
    }
    public class ProcesadorPagoEfectivo
    {
        public static async Task<List<PagoEfectivo>> getPagosEfectivo(int iddocumento)
        {
            return await LoadDataAsync(@"select Transacciones.Monto, Cuentas.Nombre, Transacciones.IDCuentaCR from Transacciones inner join Documentos on Transacciones.IDDocumento = Documentos.ID
            inner join Cuentas on Cuentas.ID = Transacciones.IDCuentaCR  where Cuentas.Nombre = 'CAJA - EFECTIVO' and Documentos.ID = @ID", new PagoEfectivo {ID = iddocumento});
            
        }
    }
}
