using BibliotecaDeClases.Modelos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace BibliotecaDeClases.Logica
{
   public class ProcesadorMateriales
    {
        public static async Task<int> CreaCotMaterialAsync(int idcot, string item, string unidad, decimal Cantidad, decimal punitario, int idcategoria, string creadopor)
        {
            MaterialesModel Cotizacion = new MaterialesModel
            {
                IDCot = idcot,
                Item = item,
                Unidad = unidad,
                Cantidad = Cantidad,
                PUnitario = punitario,
                CreadoPor = creadopor,
                CreadoEn = DateTime.Now,
                EditadoEn = DateTime.Now,
                EditadoPor = creadopor,
                IDCategoria = idcategoria
            };





            string sql = @"Insert into dbo.CotizacionMateriales (IDCot, Item, Unidad, Cantidad, PUnitario, IDCategoria, CreadoPor, CreadoEn, EditadoPor, EditadoEn) 
                            Values (@IDCot, @Item, @Unidad, @Cantidad, @PUnitario, @IDCategoria, @CreadoPor, @CreadoEn, @EditadoPor, @EditadoEn)";
            return await AccesoBD.Comando(sql, Cotizacion);

        }
        public static async Task<List<MaterialesModel>> LoadCotMaterialesAsync(int id)
        {
            MaterialesModel data = new MaterialesModel
            {
                IDCot = id
            };
            string sql = "Select * from dbo.CotizacionMateriales where IDCot = @IDCot;";

            return await AccesoBD.LoadDataAsync(sql, data);


        }      
        public static async Task<decimal> TotalMaterialAsync(int id)
        {
            MaterialesModel modelo = new MaterialesModel
            {
                IDCot = id
            };
            string sql = "Select sum(VTotal) Total from CotizacionMateriales where IDCot = @IDCot";
            var data = await AccesoBD.LoadAsync(sql, modelo);
            return data.Total;
        }
        public static async Task<int> BorraMaterialAsync(int id)
        {
            MaterialesModel Cotizacion = new MaterialesModel
            {
                ID = id,
              
            };


            string sql = @"Delete from dbo.CotizacionMateriales Where ID = @ID";
            return await AccesoBD.Comando(sql, Cotizacion);

        }
        public static async Task<int> BorraMaterialesAsync(int id)
        {
            MaterialesModel Cotizacion = new MaterialesModel
            {
                IDCot = id,

            };


            string sql = @"Delete from dbo.CotizacionMateriales Where IDCot = @IDCot";
            return await AccesoBD.Comando(sql, Cotizacion);

        }
        public static async Task<List<MaterialesModel>> LoadMaterialAsync(int id)
        {
            MaterialesModel data = new MaterialesModel
            {
                ID = id
            };
            string sql = "Select * from dbo.CotizacionMateriales where ID = @ID";

            return await AccesoBD.LoadDataAsync(sql, data);


        }
        public static async Task<int> EditarMaterialAsync(int id, string item, string unidad, decimal cantidad, decimal punitario, int idcategoria, string editadopor)
        {
            MaterialesModel Cotizacion = new MaterialesModel
            {
                ID = id,
                Item = item,
                Unidad = unidad,
                Cantidad = cantidad,
                PUnitario = punitario,
                EditadoEn = DateTime.Now,
                EditadoPor = editadopor,
                IDCategoria = idcategoria

            };


            string sql = @"Update dbo.CotizacionMateriales Set Item = @Item, Unidad = @Unidad, Cantidad = @Cantidad, PUnitario = @PUnitario, IDCategoria = @IDCategoria, 
                        EditadoPor = @EditadoPor, EditadoEn = @EditadoEn Where ID = @ID";
            return await AccesoBD.Comando(sql, Cotizacion);

        }

        public static Task<List<MaterialesModel>> LoadMaterialesySubcot(int id)
        {
            return Task.Run(() =>
            {
                return AccesoBD.LoadDataAsync("Select ID, Item, Unidad, PUnitario, Cantidad, Item Tipo from CotizacionMateriales where IDCot = @ID " +
               "union Select CotizacionesServicios.ID, Servicio, Unidad, dbo.CostoSubCot(CotizacionesServicios.ID) / Cantidad, Cantidad, TipoCotizacion.TipoCotizacion" +
               " from CotizacionesServicios inner join TipoCotizacion on TipoCotizacion.ID = CotizacionesServicios.IDTipoCotizacion where IDCotizacionPrincipal = @ID ", new MaterialesModel { ID = id });
            });
           
        }

        public static Task<List<MaterialForExcelModel>> LoadMaterialesySubcotAsync(int id)
        {
            return Task.Run(() =>
            {
                return AccesoBD.LoadDataAsync(@"Select ID, Item, Unidad, PUnitario, Cantidad, Item Tipo, IDCategoria from CotizacionMateriales where IDCot = @ID 
                                union Select CotizacionesServicios.ID, Servicio, Unidad, dbo.CostoSubCot(CotizacionesServicios.ID) / Cantidad, Cantidad, TipoCotizacion.TipoCotizacion, 0
                                from CotizacionesServicios inner join TipoCotizacion on TipoCotizacion.ID = CotizacionesServicios.IDTipoCotizacion where IDCotizacionPrincipal = @ID ", 
                                new MaterialForExcelModel { ID = id });
            });
            
        }

        public static  Task<List<MaterialesModel>> ListaTodosMaterialesAsync(int idcotizacion)
        {
            return Task.Run(() =>
            {
              return  AccesoBD.LoadDataAsync("Select Item, CotizacionMateriales.Unidad, Sum(CotizacionMateriales.Cantidad) Cantidad, Sum(PUnitario)/Sum(CotizacionMateriales.Cantidad) PUnitario " +
               "from CotizacionMateriales inner join CotizacionesServicios on CotizacionMateriales.IDCot = CotizacionesServicios.ID " +
               "where CotizacionesServicios.ID = 1 or CotizacionesServicios.IDCotizacionPrincipal = @IDCot " +
               "group by Item, CotizacionMateriales.Unidad", new MaterialesModel { IDCot = idcotizacion });
            });
               
        }

        public static Task<List<SelectListItem>> ListaCategorias()
        {
            
            return Task.Run(() =>
            {
                return AccesoBD.LoadDataAsync<SelectListItem>("Select ID Value, Categoria Text from CategoriaMRE");
            });
            
        }
    }
}
