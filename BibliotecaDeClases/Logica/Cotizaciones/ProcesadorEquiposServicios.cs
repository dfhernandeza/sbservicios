using BibliotecaDeClases.Modelos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BibliotecaDeClases.Logica.Cotizaciones
{
  public  class ProcesadorEquiposServicios
    {
        public static async Task<int> CreaCotEquiposAsync(int idcot, string item, string unidad, decimal Cantidad, decimal punitario, string creadopor)
        {
            CotizacionEquiposModel Cotizacion = new CotizacionEquiposModel
            {
                IDCot = idcot,
                Item = item,
                Unidad = unidad,
                Cantidad = Cantidad,
                PUnitario = punitario,
                CreadoEn = DateTime.Now,
                CreadoPor = creadopor,
                EditadoEn = DateTime.Now,
                EditadoPor = creadopor
               
            };





            string sql = @"Insert into dbo.CotizacionEquiposServicios (IDCot, Item, Unidad, Cantidad, PUnitario, CreadoEn, CreadoPor) Values (@IDCot, @Item, @Unidad, @Cantidad, @PUnitario, @CreadoEn, @CreadoPor)";
            return await AccesoBD.Comando(sql, Cotizacion);

        }

        //public static async Task<List<CotizacionEquiposModel>> LoadCotEquiposAsync(int id)
        //{
        //    CotizacionEquiposModel data = new CotizacionEquiposModel
        //    {
        //        IDCot = id
        //    };
        //    string sql = "Select * from dbo.CotizacionEquiposServicios where IDCot = @IDCot;";

        //    return await AccesoBD.LoadDataAsync(sql, data);


        //}

        public static Task<List<CotizacionEquiposModel>> LoadCotEquiposAsync(int id)
        {
            return Task.Run(async () => {

                CotizacionEquiposModel data = new CotizacionEquiposModel
                {
                    IDCot = id
                };
                string sql = "Select * from dbo.CotizacionEquiposServicios where IDCot = @IDCot;";

                return await AccesoBD.LoadDataAsync(sql, data);

            });
          


        }


        public static async Task<List<CotizacionEquiposModel>> LoadEquipoAsync(int id)
        {
            CotizacionEquiposModel data = new CotizacionEquiposModel
            {
                ID = id
            };
            string sql = "Select * from dbo.CotizacionEquiposServicios where ID = @ID;";

            return await AccesoBD.LoadDataAsync(sql, data);


        }

        public static async Task<int> EditaEquipoAsync(int id, string item, string unidad, decimal Cantidad, decimal punitario, string editadopor)
        {
            CotizacionEquiposModel Cotizacion = new CotizacionEquiposModel
            {
                ID = id,
                Item = item,
                Unidad = unidad,
                Cantidad = Cantidad,
                PUnitario = punitario,
                EditadoPor = editadopor,
                EditadoEn = DateTime.Now
            };

            string sql = @"Update dbo.CotizacionEquiposServicios Set Item = @Item, Unidad = @Unidad, Cantidad = @Cantidad, PUnitario = @PUnitario, EditadoPor = @EditadoPor, EditadoEn = @EditadoEn where ID = @ID";
            return await AccesoBD.Comando(sql, Cotizacion);

        }
        public static async Task<int> BorraEquipoAsync(int id)
        {
            CotizacionEquiposModel Cotizacion = new CotizacionEquiposModel
            {
                ID = id
            
            };

            string sql = @"Delete from dbo.CotizacionEquiposServicios where ID = @ID";
            return await AccesoBD.Comando(sql, Cotizacion);

        }
        public static async Task<int> BorraEquiposAsync(int id)
        {
            CotizacionEquiposModel Cotizacion = new CotizacionEquiposModel
            {
                IDCot = id

            };

            string sql = @"Delete from dbo.CotizacionEquiposServicios where IDCot = @IDCot";
            return await AccesoBD.Comando(sql, Cotizacion);

        }
        public static async Task<List<CotizacionEquiposModel>> getItemsAuto(string item)
        {
            return await AccesoBD.LoadDataAsync("Select distinct Item from CotizacionEquiposServicios where Item like '%' + @Item + '%'", new CotizacionEquiposModel { Item = item});
            
        }
        public static async Task<CotizacionEquiposModel> getPriceUnitAuto(string item)
        {
            return await AccesoBD.LoadAsync("Select Item, Unidad, PUnitario from CotizacionEquiposServicios where Item = @Item", new CotizacionEquiposModel { Item = item });
            
        }
    }
}
