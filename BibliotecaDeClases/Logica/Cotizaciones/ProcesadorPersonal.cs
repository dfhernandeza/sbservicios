using BibliotecaDeClases.Modelos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace BibliotecaDeClases.Logica
{
   public class ProcesadorPersonal
    {
        public static async Task<double> CostoHHAsync(int id)
        {
            if(id == 0)
            {
                return 0;
            }
            else
            {
                EspecialidadModel data = new EspecialidadModel
                {
                      ID = id
                };
                string sql = "Select Especialidad, ValorHH from dbo.Especialidades where ID = @ID;";
                var datos = await AccesoBD.LoadAsync(sql, data);
                return datos.ValorHH; 
            }
           
        }
    
        public static Task<List<CotizacionPersonalModel>> LoadCotPersonalAsync(int id)
        {
            return Task.Run(() =>
            {
                CotizacionPersonalModel data = new CotizacionPersonalModel
                {
                    IDCot = id
                };
                string sql = "Select CotizacionPersonal.ID, IDCot, Especialidades.Especialidad Especialidad, CotizacionPersonal.IDEspecialidad IDEspecialidad, Cantidad, HH, CotizacionPersonal.ValorHH, Dias from CotizacionPersonal inner join Especialidades on CotizacionPersonal.IDEspecialidad = Especialidades.ID where IDCot = @IDCot;";

                return AccesoBD.LoadDataAsync(sql, data);
            });           

        }
        public static async Task<int> CreaCotPersonalAsync(CotizacionPersonalModel modelo)
        {

            string sql = @"Insert into dbo.CotizacionPersonal (IDCot, IDEspecialidad, HH, ValorHH, Cantidad, Dias, CreadoPor, CreadoEn, EditadoPor, EditadoEn) 
                        Values (@IDCot, @IDEspecialidad, @HH, @ValorHH, @Cantidad, @Dias, @CreadoPor, @CreadoEn, @EditadoPor, @EditadoEn)";
            return await AccesoBD.Comando(sql, modelo);

        }
        public static async Task<List<CotizacionPersonalModel>> LoadCotiPersonalAsync(int id)
        {
            CotizacionPersonalModel data = new CotizacionPersonalModel
            {
                ID = id
            };
            string sql = "Select * from dbo.CotizacionPersonal where ID = @ID";

            return await AccesoBD.LoadDataAsync(sql, data);


        }
        public static async Task<int> BorraPersonalAsync(int id)
        {
            CotizacionPersonalModel Cotizacion = new CotizacionPersonalModel
            {
                ID = id,

            };


            string sql = @"Delete from dbo.CotizacionPersonal Where ID = @ID";
            return await AccesoBD.Comando(sql, Cotizacion);

        }
        public static async Task<int> BorraPersonalesAsync(int id)
        {
            CotizacionPersonalModel Cotizacion = new CotizacionPersonalModel
            {
                IDCot = id,

            };


            string sql = @"Delete from dbo.CotizacionPersonal Where IDCot = @IDCot";
            return await AccesoBD.Comando(sql, Cotizacion);

        }
        public static async Task<int> EditarPersonalAsync(CotizacionPersonalModel modelo)
        {       
            string sql = @"Update dbo.CotizacionPersonal Set IDEspecialidad = @IDEspecialidad, Cantidad = @Cantidad, ValorHH = @ValorHH, HH = @HH, 
                        Dias = @Dias, EditadoPor = @EditadoPor, EditadoEn = @EditadoEn Where ID = @ID";
            return await AccesoBD.Comando(sql, modelo);

        }







    }
}
