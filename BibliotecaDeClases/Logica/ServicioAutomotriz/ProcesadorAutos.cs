using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using BibliotecaDeClases.Modelos.ServicioAutomotriz;
namespace BibliotecaDeClases.Logica.ServicioAutomotriz
{
    public class ProcesadorMarcaAutos
    {
        public static async Task insertarMarcaAuto(MarcasAutosModel modelo)
        {
            await Task.Run(async () => { 
            await AccesoBDAutomotriz.Comando("Insert into MarcasAutos (ID, MarcaAuto) Values (NEWID(), @MarcaAuto) ", modelo);
            
            });            
        }
        public static async Task updateMarcaAuto(MarcasAutosModel modelo)
        {
            await Task.Run(async () => {
                await AccesoBDAutomotriz.Comando("Update MarcasAutos Set MarcaAuto = @MarcaAuto where ID = @ID", modelo);

            });
        }
        public static async Task deleteMarcaAuto(string id)
        {
            await Task.Run(async () => {
                await AccesoBDAutomotriz.Comando("Delete from MarcasAutos where ID = @ID", new {ID = id });
            });

        }
        public static async Task<List<MarcasAutosModel>> selectMarcasAutosAsync()
        {
            return await Task.Run(async () =>
            {
                return await AccesoBDAutomotriz.LoadDataAsync<MarcasAutosModel>(@"Select ID, MarcaAuto from MarcasAutos Order by MarcaAuto Asc");
            });

        }
        public static async Task<List<SelectListItem>> selectMarcasAutosSelectListAsync()
        {
            return await Task.Run(async () =>
            {
                return await AccesoBDAutomotriz.LoadAsync<List<SelectListItem>>(@"Select ID Value, MarcaAuto Text from MarcasAutos");
            });

        }
        public static async Task<MarcasAutosModel> selectMarcaAutoAsync(string id)
        {
            return await Task.Run(async () =>
            {
                return await AccesoBDAutomotriz.LoadAsync(@"Select ID, MarcaAuto from MarcasAutos where ID = @ID", new MarcasAutosModel { ID = id });
            });

        }


    }
    public class ProcesadorModelosAutos
    {
        /// <summary>Inserta un nuevo Modelo de Auto</summary>
        public static async Task insertModeloAutoAsync(ModelosAutosModel modelo)
        {
            await Task.Run(async () =>
            {
                await AccesoBDAutomotriz.Comando(@"Insert into ModelosAutos (ID, ModeloAuto, IDMarcaAuto) Values (NEWID(), @ModeloAuto, @IDMarcaAuto) ", modelo);
            });
        }
        public static async Task updateModeloAutoAsync(ModelosAutosModel modelo)
        {
            await Task.Run(async () =>
            {
                await AccesoBDAutomotriz.Comando(@"Update ModelosAutos Set ModeloAuto = @ModeloAuto where ID = @ID", modelo);
            });

        }
        public static async Task deleteModeloAutoAsync(string id)
        {
            await Task.Run(async () =>
            {
                await AccesoBDAutomotriz.Comando(@"Delete from Modelos Autos where ID = @ID", new {ID = id });
            });

        }
        public static async Task<List<ModelosAutosModel>> selectModelosAutosAsync()
        {
            return await Task.Run(async () =>
            {
                return await AccesoBDAutomotriz.LoadDataAsync<ModelosAutosModel>(@"Select ID, ModeloAuto, IDMarcaAuto from ModelosAutos");
            });

        }
        public static async Task<List<ModelosAutosModel>> selectModelosAutosAsync(string idmarca)
        {
            return await Task.Run(async () =>
            {
                return await AccesoBDAutomotriz.LoadDataAsync(@"Select ID, ModeloAuto, IDMarcaAuto from ModelosAutos where IDMarcaAuto = @IDMarcaAuto order by ModeloAuto Asc", new ModelosAutosModel {IDMarcaAuto = idmarca });
            });

        }
        public static async Task<ModelosAutosModel> selectModeloAutoAsync(string id)
        {
            return await Task.Run(async () =>
            {
                return await AccesoBDAutomotriz.LoadAsync(@"Select ID, ModeloAuto, IDMarcaAuto from ModelosAutos where ID = @ID", new ModelosAutosModel { ID = id });
            });

        }
    }
    public class ProcesadorAñosAutos
    {
        public static async Task<List<AñosAutosModel>> selectListItemAñosAutosAsync()
        {
            return await Task.Run(async () =>
            {
                return await AccesoBDAutomotriz.LoadDataAsync<AñosAutosModel>(@"Select ID, AñoAutos from AñosAutos order by AñoAutos Asc");
            });
        }
    }
    public class ProcesadorModeloAñoAuto
    {
        public static async Task insertModeloAñoAutoAsync(List<ModeloAutoAñoModel> modelo)
        {
            await Task.Run(async () =>
            {
                foreach (var item in modelo)
                {
                    await AccesoBDAutomotriz.Comando(@"Insert into ModeloAñoAuto (ID, IDModeloAuto, IDAñoAuto) Values (NEWID(), @IDModelo, @IDAño)", item);
                }
                
            });


        }
        public static async Task<List<ModeloAutoAñoModel>> selectAñosModeloAsync(string idmodelo)
        {
            return await Task.Run(async () =>
            {
                return await AccesoBDAutomotriz.LoadDataAsync(@"select IDAñoAuto IDAño from ModeloAñoAuto where IDModeloAuto = @IDModelo", new ModeloAutoAñoModel { IDModelo = idmodelo });
            });

        }
    
    
    }
}
