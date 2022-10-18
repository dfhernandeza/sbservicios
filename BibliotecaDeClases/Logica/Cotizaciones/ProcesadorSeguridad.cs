using BibliotecaDeClases.Modelos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BibliotecaDeClases.Logica.Cotizaciones
{
   public class ProcesadorSeguridad
    {
        public static async Task<int> CreaCotSeguridadAsync(int idcot, string item, double cantidad, double punitario)
        {
            CotizacionSeguridadModel Cotizacion = new CotizacionSeguridadModel
            {
                IDCot = idcot,
                Item = item,
               Cantidad = cantidad,
                PUnitario = punitario
            };





            string sql = @"Insert into dbo.CotizacionSeguridad (IDCot, Item, Cantidad, PUnitario) Values (@IDCot, @Item, @Cantidad, @PUnitario)";
            return await AccesoBD.Comando(sql, Cotizacion);

        }

        public static async Task<List<CotizacionSeguridadModel>> LoadCotSeguridadAsync(int id)
        {
            CotizacionSeguridadModel data = new CotizacionSeguridadModel
            {
                IDCot = id
            };
            string sql = "Select * from dbo.CotizacionSeguridad where IDCot = @IDCot;";

            return await AccesoBD.LoadDataAsync(sql, data);


        }

        public static async Task<List<CotizacionSeguridadModel>> LoadSeguridadAsync(int id)
        {
            CotizacionSeguridadModel data = new CotizacionSeguridadModel
            {
                ID = id
            };
            string sql = "Select * from dbo.CotizacionSeguridad where ID = @ID;";

            return await AccesoBD.LoadDataAsync(sql, data);


        }

        public static async Task<int> BorraSeguridadAsync(int id)
        {
            CotizacionSeguridadModel Cotizacion = new CotizacionSeguridadModel
            {
                ID = id,

            };


            string sql = @"Delete from dbo.CotizacionSeguridad Where ID = @ID";
            return await AccesoBD.Comando(sql, Cotizacion);

        }


        public static async Task<int> BorraSeguridadesAsync(int id)
        {
            CotizacionSeguridadModel Cotizacion = new CotizacionSeguridadModel
            {
                IDCot = id,

            };


            string sql = @"Delete from dbo.CotizacionSeguridad Where IDCot = @IDCot";
            return await AccesoBD.Comando(sql, Cotizacion);

        }




        public static async Task<int> EditarSeguridadAsync(int id, string item, double cantidad, double punitario)
        {
            CotizacionSeguridadModel Cotizacion = new CotizacionSeguridadModel
            {
                ID = id,
                Item = item,
                Cantidad = cantidad,
                PUnitario = punitario

            };


            string sql = @"Update dbo.CotizacionSeguridad Set Item = @Item, Cantidad = @Cantidad, PUnitario = @PUnitario Where ID = @ID";
            return await AccesoBD.Comando(sql, Cotizacion);

        }

    }
}










