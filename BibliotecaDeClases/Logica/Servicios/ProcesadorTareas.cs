using BibliotecaDeClases.Modelos.Servicios;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BibliotecaDeClases.Modelos.Inventario;
using BibliotecaDeClases.Modelos;
using System.Dynamic;
using System.Web.Mvc;

using System.Web;

using System.IO;
using static BibliotecaDeClases.Logica.Servicios.ProcesadorServicios;

namespace BibliotecaDeClases.Logica.Servicios
{
    public class ProcesadorTareas
    {
        //==========================================================================================================
        //Tareas
        //==========================================================================================================
        public static async Task<int> CrearTareaAsync(TareaModel modelo)
        {

            var datos = await AccesoBD.LoadAsync("Insert into Tareas (Nombre, FechaInicial, FechaFinal, Descripcion, IDEncargado, IDServicio, CreadoPor, CreadoEn, EditadoPor, EditadoEn, IDUbicacion) OUTPUT INSERTED.ID " +
                     "Values (@Nombre, @FechaInicial, @FechaFinal, @Descripcion, @IDEncargado, @IDServicio, @CreadoPor, @CreadoEn, @EditadoPor, @EditadoEn, @IDUbicacion)", modelo);

            return datos.ID;

        }
        public static async Task<int> EditarTareaAsync(int ID, string Nombre, DateTime FechaInicial, DateTime FechaFinal, string Descripcion, int IDEncargado, string editadopor, int idubicacion)
        {
            TareaModel tarea = new TareaModel
            {
                ID = ID,
                Nombre = Nombre,
                FechaFinal = FechaFinal,
                FechaInicial = FechaInicial,
                Descripcion = Descripcion,
                IDEncargado = IDEncargado,
                EditadoEn = DateTime.Now,
                EditadoPor = editadopor,
                IDUbicacion = idubicacion

            };

            string sql = "Update Tareas Set Nombre = @Nombre, FechaInicial = @FechaInicial, FechaFinal = @FechaFinal, Descripcion = @Descripcion, " +
                "IDEncargado = @IDEncargado, EditadoPor = @EditadoPor, EditadoEn = @EditadoEn, IDUbicacion = @IDUbicacion where ID = @ID";

            return await AccesoBD.Comando(sql, tarea);

        }
        public static Task<List<TareaModel>> LoadTareasAsync(int idservicio)
        {
            return Task.Run(() =>
            {
                return AccesoBD.LoadDataAsync(@"Select Tareas.ID, Tareas.Nombre, FechaInicial, FechaFinal, Tareas.Descripcion, Tareas.IDEncargado, PersonalServicios.Nombre + ' ' + PersonalServicios.Apellido Encargado, 
                [dbo].[ProgresoTarea] (Tareas.ID) Progreso, dbo.GastoCECO(CECOs.ID) Gasto from Tareas 
                inner join PersonalServicios on Tareas.IDEncargado = PersonalServicios.ID
                inner join CECOs on CECOs.IDTarea = Tareas.ID where IDServicio = @IDServicio", new TareaModel { IDServicio = idservicio });

            });

        }
        public static async Task<TareaModel> LoadTareaAsync(int idtarea)
        {

            string sql = "Select * from Tareas where ID = @ID";
            TareaModel tarea = new TareaModel
            {
                ID = idtarea
            };
            var data = await AccesoBD.LoadAsync(sql, tarea);
            return data;
        }
        public static async Task BorrarTarea(int idtarea)
        {
            await Task.Run(() =>
            {
                return AccesoBD.Comando("Delete from Tareas where ID = @ID", new TareaModel { ID = idtarea });
            });

        }
        public static async Task<List<ProgramaModel>> EmpleadosTarea(int idtarea)
        {
            return await Task.Run(() =>
            {
                return AccesoBD.LoadDataAsync(@"Select  ProgramaServicios.ID, PersonalServicios.ID IDEmpleado, PersonalServicios.Nombre + ' ' + PersonalServicios.Apellido Empleado, Especialidades.Especialidad,  ProgramaServicios.FechaInicial, ProgramaServicios.FechaTermino, 
                        ProgramaServicios.Responsabilidades from ProgramaServicios inner join PersonalServicios on ProgramaServicios.IDEmpleado = PersonalServicios.ID inner join Especialidades 
                        on PersonalServicios.IDEspecialidad = Especialidades.ID where ProgramaServicios.IDTarea = @IDTarea", new ProgramaModel { IDTarea = idtarea });
            });
        }
        public static async Task<List<ProgramaModel>> EmpleadosTareaHH(int idtarea)
        {
            return await Task.Run(() =>
            {
                return AccesoBD.LoadDataAsync(@"Select distinct PersonalServicios.ID IDEmpleado, PersonalServicios.Nombre + ' ' + PersonalServicios.Apellido Empleado 
                         from ProgramaServicios inner join PersonalServicios on ProgramaServicios.IDEmpleado = PersonalServicios.ID where ProgramaServicios.IDTarea = @IDTarea", new ProgramaModel { IDTarea = idtarea });
            });
        }
        public static Task<List<SelectListItem>> UbicacionesServicio(int idservicio)
        {
            return Task.Run(() =>
            {
                return AccesoBD.LoadDataAsync(@"Select Servicios.IDUbicacion as Value, Ubicaciones.Alias as Text 
                                    from Servicios inner join Ubicaciones on Servicios.IDUbicacion = Ubicaciones.ID where Servicios.ID = @Text
                                    Union
                                    Select Ubicaciones.ID as Value, Ubicaciones.Alias as Text from Ubicaciones where Ubicaciones.IDCliente = 3 ",
                                    new SelectListItem { Text = idservicio.ToString() });

            });

        }


        //==========================================================================================================
        //Nuevos Entregables
        //==========================================================================================================
        public static async Task EntregablesIngreso(List<EntregableModel> listaentregables)
        {
            var tarea = await CreaEntregables(listaentregables);
            //await InsertaInventario(tarea);
            await InsertaSolicitudes(tarea);
        }
        public static async Task<List<EntregableModel>> CreaEntregables(List<EntregableModel> entregables)
        {

            foreach (var row in entregables)
            {
                await Task.Run(async () =>
                {

                    var dato = await AccesoBD.LoadAsync(@"Insert into Entregables (IDCotizacion, IDTarea, IDEstado, Entregable, IDCategoriaEntregable, CreadoPor, CreadoEn, EditadoPor, EditadoEn) output Inserted.ID 
                                Values (@IDCotizacion, @IDTarea, @IDEstado, @Entregable, @IDCategoriaEntregable, @CreadoPor, @CreadoEn, @EditadoPor, @EditadoEn)", new EntregableModel
                    {
                        IDTarea = row.IDTarea,
                        IDEstado = 1,
                        Entregable = row.Entregable,
                        IDCotizacion = row.IDCotizacion,
                        IDCategoriaEntregable = 1,
                        CreadoPor = row.CreadoPor,
                        CreadoEn = DateTime.Now,
                        EditadoEn = DateTime.Now,
                        EditadoPor = row.CreadoPor
                    });
                    row.ID = dato.ID;

                    await AccesoBD.Comando("Insert into InventariosServicios (Descripcion, PProduccion, PVenta, Unidad, IDCategoria, IDUbicacion, IDProveedor, IDCondicion, VidaUtil, CreadoPor, CreadoEn, EditadoPor, EditadoEn) " +
                          "Values (@Descripcion, @PProduccion, @PVenta, @Unidad, @IDCategoria, @IDUbicacion, @IDProveedor, @IDCondicion, @VidaUtil, @CreadoPor, @CreadoEn, @EditadoPor, @EditadoEn)",
                          new InventarioModel
                          {
                              Descripcion = row.Entregable,
                              PProduccion = row.PProduccion,
                              PVenta = row.PVenta,
                              Unidad = row.Unidad,
                              IDCategoria = 1,
                              IDUbicacion = 2,
                              IDProveedor = 1,
                              IDCondicion = 1,
                              VidaUtil = 0,
                              CreadoPor = row.CreadoPor,
                              EditadoPor = row.CreadoPor,
                              EditadoEn = DateTime.Now,
                              CreadoEn = DateTime.Now,
                              IDPT = dato.ID
                          });



                });

            }

            return entregables;


        }
        public static async Task InsertaSolicitudes(List<EntregableModel> lista)
        {
            await Task.Run(async () =>
            {
                foreach (var item in lista)
                {

                    await AccesoBD.Comando(@"Insert into SolicitudItem (IDEstado, IDEntregable, Tipo, CreadoPor, CreadoEn, EditadoPor, EditadoEn) 
                                          Values (1, @IDEntregable, 'MRoE', @CreadoPor, @CreadoEn, @EditadoPor, @EditadoEn)",
                                               new { IDEntregable = item.ID, CreadoPor = item.CreadoPor, CreadoEn = DateTime.Now, EditadoPor = item.CreadoPor, EditadoEn = DateTime.Now });

                }
            });

        }
        public async static Task InsertaInventario(List<EntregableModel> lista)
        {
            string sql = "";
            int i = 1;
            dynamic objeto = new ExpandoObject();
            foreach (var row in lista)
            {
                if (i == 1)
                {
                    sql += " Entregables.IDCotizacion = @IDCot" + i.ToString();
                    ((IDictionary<string, object>)objeto).Add("IDCot" + i.ToString(), row.IDCotizacion);
                }
                else
                {
                    sql += " or Entregables.IDCotizacion = @IDCot" + i.ToString();
                    ((IDictionary<string, object>)objeto).Add("IDCot" + i.ToString(), row.IDCotizacion);
                }
                i += 1;
            }
            ((IDictionary<string, object>)objeto).Add("IDEntregable", lista.FirstOrDefault().IDTarea);
            ((IDictionary<string, object>)objeto).Add("CreadoPor", lista.FirstOrDefault().CreadoPor);
            ((IDictionary<string, object>)objeto).Add("CreadoEn", DateTime.Now);
            ((IDictionary<string, object>)objeto).Add("EditadoPor", lista.FirstOrDefault().EditadoPor);
            ((IDictionary<string, object>)objeto).Add("EditadoEn", DateTime.Now);

            await Task.Run(() =>
            {
                AccesoBD.Comando(@"Insert into InventariosServicios (Descripcion, Unidad, CreadoPor, CreadoEn, EditadoPor, EditadoEn) 
                                                     Select distinct CotizacionMateriales.Item, CotizacionMateriales.Unidad, @CreadoPor, @CreadoEn, @EditadoPor, @EditadoEn from CotizacionMateriales
                                                     inner join  Entregables on CotizacionMateriales.IDCot = Entregables.IDCotizacion where" + sql, objeto);
            });






        }

        //==========================================================================================================
        //Nuevo Entregable
        //==========================================================================================================
        public async static Task InsertaInventarioMPT(List<EntregableMaterialPT> listado)
        {
            foreach (var row in listado)
            {
                if (row.IDEntregablePT == 0)
                {
                    await Task.Run(async () =>
                    {
                        await AccesoBD.Comando(@"Insert into InventariosServicios (Descripcion, Unidad, CreadoPor, CreadoEn, EditadoPor, EditadoEn) 
                                                     Select distinct CotizacionMateriales.Item, CotizacionMateriales.Unidad, @CreadoPor, @CreadoEn, @EditadoPor, @EditadoEn from CotizacionMateriales
                                                      where CotizacionMateriales.ID = @ID",
                                                     new { ID = row.IDCotMaterial, CreadoPor = row.CreadoPor, CreadoEn = DateTime.Now, EditadoPor = row.CreadoPor, EditadoEn = DateTime.Now });
                    });
                }
            }

        }
        public static async Task InsertaSolicitudesMPT(List<EntregableMaterialPT> listado, int identregable)
        {
            await Task.Run(async () =>
            {



                await AccesoBD.Comando(@"Insert into SolicitudItem (IDEstado, IDEntregable, Tipo, CreadoPor, CreadoEn, EditadoPor, EditadoEn) 
                                          Values (1, @IDEntregable, 'MRoE', @CreadoPor, @CreadoEn, @EditadoPor, @EditadoEn)",
                                           new { IDEntregable = identregable, CreadoPor = listado.FirstOrDefault().CreadoPor, CreadoEn = DateTime.Now, EditadoPor = listado.FirstOrDefault().CreadoPor, EditadoEn = DateTime.Now });


            });

        }
        public static async Task CreaEntregableAsync(EntregableModel entregable)
        {

            var data = await AccesoBD.LoadAsync(@"Insert into Entregables (IDTarea, IDEstado, Entregable, Unidad, Cantidad, FechaInicial, FechaEntrega, Comentarios, Instrucciones, CreadoPor, CreadoEn, EditadoPor, EditadoEn, IDEncargado, IDCategoriaEntregable) 
           output Inserted.ID Values (@IDTarea, @IDEstado, @Entregable, @Unidad, @Cantidad, @FechaInicial, @FechaEntrega, @Comentarios, @Instrucciones, @CreadoPor, @CreadoEn, @EditadoPor, @EditadoEn, @IDEncargado, @IDCategoriaEntregable)", new EntregableModel
            {
                IDTarea = entregable.IDTarea,
                IDEstado = 1,
                Entregable = entregable.Entregable.ToUpperCheckForNull(),
                FechaInicial = entregable.FechaInicial,
                FechaEntrega = entregable.FechaEntrega,
                Comentarios = entregable.Comentarios.ToUpperCheckForNull(),
                Instrucciones = entregable.Instrucciones.ToUpperCheckForNull(),
                CreadoPor = entregable.CreadoPor,
                CreadoEn = DateTime.Now,
                EditadoEn = DateTime.Now,
                EditadoPor = entregable.CreadoPor,
                IDEncargado = entregable.IDEncargado,
                Unidad = entregable.Unidad.ToUpperCheckForNull(),
                Cantidad = entregable.Cantidad,
                IDCategoriaEntregable = entregable.IDCategoriaEntregable
            });

            await CreaMaterialesPT(data.ID, entregable.MaterialesEntregable, entregable.CreadoPor);
            await InsertaSolicitudesMPT(entregable.MaterialesEntregable, data.ID);
            //await InsertaInventarioMPT(entregable.MaterialesEntregable);

        }
        public static async Task CreaMaterialesPT(int identregable, List<EntregableMaterialPT> listado, string creadoPor)
        {


            foreach (var row in listado)
            {
                await Task.Run(async () =>
                {
                    if (row.IDEntregablePT == 0)
                    {
                        await AccesoBD.Comando(@"Insert into EntregableMaterialPT (IDEntregable, IDCotMaterial, CreadoPor, CreadoEn, EditadoPor, EditadoEn) 
                                            Values (@IDEntregable, @IDCotMaterial, @CreadoPor, @CreadoEn, @EditadoPor, @EditadoEn)",
                                                          new EntregableMaterialPT
                                                          {
                                                              IDEntregable = identregable,
                                                              IDCotMaterial = row.IDCotMaterial,
                                                              CreadoPor = creadoPor,
                                                              CreadoEn = DateTime.Now,
                                                              EditadoEn = DateTime.Now,
                                                              EditadoPor = creadoPor
                                                          });


                    }
                    else
                    {
                        await AccesoBD.Comando(@"Insert into EntregableMaterialPT (IDEntregable, IDEntregablePT, CreadoPor, CreadoEn, EditadoPor, EditadoEn) 
                                            Values (@IDEntregable, @IDEntregablePT, @CreadoPor, @CreadoEn, @EditadoPor, @EditadoEn)",
                                                                new EntregableMaterialPT
                                                                {
                                                                    IDEntregable = identregable,
                                                                    IDEntregablePT = row.IDEntregablePT,
                                                                    CreadoPor = creadoPor,
                                                                    CreadoEn = DateTime.Now,
                                                                    EditadoEn = DateTime.Now,
                                                                    EditadoPor = creadoPor
                                                                });
                    }


                });
            }

        }

        //==========================================================================================================
        //Entregables
        //==========================================================================================================
        public static Task<EntregableModel> CargaEntregable(int id)
        {
            return Task.Run(() =>
            {
                return AccesoBD.LoadAsync(@"declare @unit nvarchar(50);
                                       declare @idencargados int;
                select @unit = Unidad from Entregables where Entregables.ID = @ID;
				select @idencargados = IDEncargado from Entregables where Entregables.ID = @ID
                IF (@unit is null and @idencargados is not null)
                BEGIN
                select Entregables.ID, Entregables.IDEncargado, PersonalServicios.Nombre + ' ' + PersonalServicios.Apellido, EstadoEntregable.Estado, Tareas.Nombre NombreTarea, 
                CategoriasEntregables.NombreCategoriaEntregable, Entregables.IDTarea, CotizacionesServicios.Unidad, CotizacionesServicios.Cantidad, 
                Entregables.Entregable, Tareas.FechaFinal FechaEntrega,Entregables.Comentarios, Entregables.Instrucciones, Entregables.IDCategoriaEntregable, Entregables.FechaInicial 
                from Entregables inner join CotizacionesServicios on Entregables.IDCotizacion = CotizacionesServicios.ID 
				inner join Tareas on Entregables.IDTarea = Tareas.id 
				inner join EstadoEntregable on EstadoEntregable.ID = Entregables.IDEstado
				inner join CategoriasEntregables on CategoriasEntregables.IDCategoriaEntregable = Entregables.IDCategoriaEntregable
				inner join PersonalServicios on PersonalServicios.ID = Entregables.IDEncargado
				where Entregables.ID = @ID
                END
                ELSE if(@unit is null and @idencargados is null)
				begin
				select Entregables.ID, Entregables.IDEncargado, EstadoEntregable.Estado, Tareas.Nombre NombreTarea, CategoriasEntregables.NombreCategoriaEntregable CategoriaEntregable, 
                Entregables.IDTarea, CotizacionesServicios.Unidad, CotizacionesServicios.Cantidad, Entregables.Entregable, Tareas.FechaFinal FechaEntrega,Entregables.Comentarios, 
                Entregables.Instrucciones, Entregables.IDCategoriaEntregable, Entregables.FechaInicial  
                from Entregables inner join CotizacionesServicios on Entregables.IDCotizacion = CotizacionesServicios.ID 
				inner join Tareas on Entregables.IDTarea = Tareas.id 
				inner join EstadoEntregable on EstadoEntregable.ID = Entregables.IDEstado
				inner join CategoriasEntregables on CategoriasEntregables.IDCategoriaEntregable = Entregables.IDCategoriaEntregable
				
				where Entregables.ID = @ID
				end
				else
                BEGIN
                Select Entregables.ID, IDTarea, Entregables.IDEncargado, PersonalServicios.Nombre + ' ' + PersonalServicios.Apellido Encargado, EstadoEntregable.Estado, 
                CategoriasEntregables.NombreCategoriaEntregable CategoriaEntregable, Tareas.Nombre NombreTarea, Unidad, Cantidad, Entregable, 
                FechaEntrega, Entregables.FechaInicial, Comentarios,Instrucciones, Entregables.IDEncargado, Entregables.IDCategoriaEntregable from Entregables
				inner join EstadoEntregable on EstadoEntregable.ID = Entregables.IDEstado
				inner join CategoriasEntregables on CategoriasEntregables.IDCategoriaEntregable = Entregables.IDCategoriaEntregable
				inner join Tareas on Entregables.IDTarea = Tareas.id 
			    inner join PersonalServicios on PersonalServicios.ID = Entregables.IDEncargado
				where Entregables.ID = @ID
                END", new EntregableModel { ID = id });
            });

        }
        public static Task<List<EntregableModel>> EntregablesXTarea(int idtarea)
        {
            return Task.Run(() =>
            {
                return AccesoBD.LoadDataAsync(@"Select Entregables.ID, Entregables.IDTarea, Entregables.Unidad, Entregables.Cantidad, Entregables.IDEstado, EstadoEntregable.Estado, Entregables.Entregable, 
                                              Entregables.FechaEntrega, Entregables.FechaInicial, Entregables.Comentarios, Entregables.Instrucciones 
                                              from Entregables inner join EstadoEntregable on Entregables.IDEstado = EstadoEntregable.ID where Entregables.IDTarea = @ID and Unidad is not null
                                              Union
                                             select Entregables.ID, Entregables.IDTarea, CotizacionesServicios.Unidad, CotizacionesServicios.Cantidad, Entregables.IDEstado, EstadoEntregable.Estado, Entregables.Entregable, Tareas.FechaFinal, Entregables.FechaInicial, Entregables.Comentarios, Entregables.Instrucciones 
                                             from Entregables inner join CotizacionesServicios on Entregables.IDCotizacion = CotizacionesServicios.ID 
                                             inner join Tareas on Entregables.IDTarea = Tareas.id 
                                             inner join EstadoEntregable on EstadoEntregable.ID = Entregables.IDEstado
                                             where Entregables.IDTarea = @ID and Entregables.Unidad is null", new EntregableModel { ID = idtarea });
            });

        }
        public static Task<List<EntregableModel>> CargaItemsAFabricarAsync(int idcot)
        {
            return Task.Run(() =>
            {
                return AccesoBD.LoadDataAsync(@"Select CotizacionesServicios.ID IDCotizacion, Servicio Entregable, Unidad, dbo.CostoSubCot(CotizacionesServicios.ID) / Cantidad PVenta, [dbo].[CostoProduccionSubCot] (CotizacionesServicios.ID)/Cantidad PProduccion, Cantidad 
                                        from CotizacionesServicios inner join TipoCotizacion on TipoCotizacion.ID = CotizacionesServicios.IDTipoCotizacion 
                                        where IDCotizacionPrincipal = @ID and CotizacionesServicios.ID NOT IN(Select IDCotizacion from Entregables where IDCotizacion is not null)", new EntregableModel { ID = idcot });
            });

        }
        public static async Task<List<EntregableMaterialPT>> ObtenerMaterialesPT(int idcot, int idservicio)
        {
            return await Task.Run(async () =>
            {
                return await AccesoBD.LoadDataAsync(@"Select CotizacionMateriales.ID IDCotMaterial, 0 IDEntregablePT,Item, CotizacionMateriales.Cantidad, CotizacionMateriales.Unidad, 0 Tipo
                                        from CotizacionMateriales
                                        inner join CotizacionesServicios on CotizacionesServicios.ID = CotizacionMateriales.IDCot
                                        where CotizacionesServicios.ID = @ID and CotizacionMateriales.ID not in (Select IDCotMaterial from EntregableMaterialPT)
                                        Union
                                        Select 0, Entregables.ID IDEntregablePT, Entregables.Entregable, CotizacionesServicios.Cantidad, CotizacionesServicios.Unidad, 1 Tipo from Entregables
                                        inner join Tareas on Entregables.IDTarea = Tareas.ID
                                        inner join CotizacionesServicios on Entregables.IDCotizacion = CotizacionesServicios.id
                                        where Tareas.IDServicio = @IDCotMaterial and Entregables.ID not in (Select IDEntregablePT from EntregableMaterialPT)", new EntregableMaterialPT { ID = idcot, IDCotMaterial = idservicio });
            });
        }
        public static async Task<List<MaterialesModel>> MaterialesXEntregable(int identregable)
        {
            return await Task.Run(() =>
            {
                return AccesoBD.LoadDataAsync(@"
                    if((select Entregables.IDCategoriaEntregable from Entregables where Entregables.ID = @ID) = 1)
                    begin
                    Select CotizacionMateriales.Item, CotizacionMateriales.Cantidad, CotizacionMateriales.Unidad, EstadoSolicitudItem.Estado 
                                                                from CotizacionMateriales 
                                                                inner join Entregables on Entregables.IDCotizacion = CotizacionMateriales.IDCot
                                                                inner join SolicitudItem on Entregables.ID = SolicitudItem.IDEntregable
                                                                inner join EstadoSolicitudItem on SolicitudItem.IDEstado= EstadoSolicitudItem.ID where Entregables.ID = @ID
                    end
                    else if((select Entregables.IDCategoriaEntregable from Entregables where Entregables.ID = @ID) = 2)
                    begin
                    Select CotizacionMateriales.Item, CotizacionMateriales.Cantidad, CotizacionMateriales.Unidad, EstadoSolicitudItem.Estado 
                    from CotizacionMateriales 
                    inner join EntregableMaterialPT on EntregableMaterialPT.IDCotMaterial = CotizacionMateriales.ID
                    inner join Entregables on Entregables.ID = EntregableMaterialPT.IDEntregable
                    inner join SolicitudItem on Entregables.ID = SolicitudItem.IDEntregable
                    inner join EstadoSolicitudItem on SolicitudItem.IDEstado= EstadoSolicitudItem.ID
                    where Entregables.ID = @ID
                    Union 
                    Select CotizacionesServicios.Servicio, CotizacionesServicios.Cantidad, CotizacionesServicios.Unidad, EstadoSolicitudItem.Estado
                    from CotizacionesServicios inner join Entregables on Entregables.IDCotizacion = CotizacionesServicios.ID 
                    inner join EntregableMaterialPT on EntregableMaterialPT.IDEntregablePT = Entregables.ID 
                    inner join SolicitudItem on Entregables.ID = SolicitudItem.IDEntregable
                    inner join EstadoSolicitudItem on SolicitudItem.IDEstado= EstadoSolicitudItem.ID
                    where EntregableMaterialPT.IDEntregable = @ID
                    end",
                                                    new MaterialesModel { ID = identregable });
            });
        }
        public static async Task<List<MaterialesModel>> MaterialesXTarea(int idtarea)
        {
            return await Task.Run(() =>
            {
                return AccesoBD.LoadDataAsync(@"Select CotizacionMateriales.Item, CotizacionMateriales.Unidad, CotizacionMateriales.Cantidad from Entregables 
                                                    inner join CotizacionMateriales on CotizacionMateriales.IDCot = Entregables.IDCotizacion where Entregables.IDTarea = @ID",
                                                    new MaterialesModel { ID = idtarea });
            });
        }
        public static Task<int> EditaEntregable(int id, string unidad, int idencargado, decimal cantidad, string entregable, DateTime fechaentrega, DateTime fechainicial, string comentarios, string instrucciones, string editadopor)
        {
            return Task.Run(() =>
            {
                return AccesoBD.Comando(@"Update Entregables Set Unidad = @Unidad, IDEncargado = @IDEncargado, Cantidad = @Cantidad, Entregable = @Entregable, FechaEntrega = @FechaEntrega, FechaInicial = @FechaInicial, 
                Comentarios = @Comentarios, Instrucciones = @Instrucciones, " +
                   "EditadoPor = @EditadoPor, EditadoEn = @EditadoEn where ID = @ID", new EntregableModel
                   {
                       ID = id,
                       IDEncargado = idencargado,
                       Unidad = unidad.ToUpperCheckForNull(),
                       Cantidad = cantidad,
                       Entregable = entregable.ToUpperCheckForNull(),
                       FechaEntrega = fechaentrega,
                       FechaInicial = fechainicial,
                       Comentarios = comentarios.ToUpperCheckForNull(),
                       Instrucciones = instrucciones.ToUpperCheckForNull(),
                       EditadoPor = editadopor,
                       EditadoEn = DateTime.Now
                   });
            });
        }
        public static async Task<int> EliminaEntregableAsync(int id)
        {
            return await AccesoBD.Comando("Delete from Entregables where ID = @ID", new { ID = id });
        }
        public static async Task NuevoEntregableMaterialPT(List<EntregableMaterialPT> modelo)
        {
            foreach (var row in modelo)
            {
                await Task.Run(() =>
                {

                    return AccesoBD.Comando(@"Insert into EntregableMaterialPT (IDEntregable, IDCotMaterial, IDEntregablePT, CreadoPor, CreadoEn, EditadoPor, EditadoEn)
                                          Values (@IDEntregable, @IDCotMaterial, @IDEntregablePT, @CreadoPor, @CreadoEn, @EditadoPor, @EditadoEn)", row);

                });
            }

        }
        public static async Task<List<EntregableMaterialPT>> CargaMaterialesYPT(int idcot, int idservicio)
        {
            return await Task.Run(() =>
            {
                return AccesoBD.LoadDataAsync(@"Select CotizacionMateriales.ID, Item, CotizacionMateriales.Cantidad, CotizacionMateriales.Unidad, 0 Tipo
                                        from CotizacionMateriales 
                                        inner join CotizacionesServicios on CotizacionesServicios.ID = CotizacionMateriales.IDCot
                                        where CotizacionesServicios.ID = @IDCotMaterial and CotizacionMateriales.ID not in (Select IDCotMaterial from EntregableMaterialPT)
                                        Union
                                        Select Entregables.ID, Entregables.Entregable, CotizacionesServicios.Cantidad, CotizacionesServicios.Unidad, 1 Tipo from Entregables
                                        inner join Tareas on Entregables.IDTarea = Tareas.ID 
                                        inner join CotizacionesServicios on Entregables.IDCotizacion = CotizacionesServicios.id
                                        where Tareas.IDServicio = @ID and Entregables.ID not in (Select IDEntregablePT from EntregableMaterialPT)",
                                        new EntregableMaterialPT { IDCotMaterial = idcot, ID = idservicio });
            });
        }
        public static async Task<List<SelectListItem>> CategoriasEntregables()
        {
            return await Task.Run(() =>
            {
                return AccesoBD.LoadDataAsync<SelectListItem>("Select IDCategoriaEntregable Value, NombreCategoriaEntregable Text from CategoriasEntregables");
            });
        }
        //========================================================================================================================
        //Tipo Tarea
        //==========================================================================================================
        public static async Task NuevoTipoTarea(TipoTareaModel modelo)
        {
            await Task.Run(async () =>
            {
                await AccesoBD.Comando(@"Insert into TipoTarea (TipoTarea, Descripcion, CreadoPor, CreadoEn, EditadoPor, EditadoEn) 
                                            Values (@TipoTarea, @Descripcion, @CreadoPor, @CreadoEn, @EditadoPor, @EditadoEn)", modelo);
            });
        }
        public static async Task EditaTipoTarea(TipoTareaModel modelo)
        {
            await Task.Run(async () =>
            {
                await AccesoBD.Comando(@"Update TipoTarea Set TipoTarea = @TipoTarea, Descripcion = @Descripcion, 
                                        EditadoPor = @EditadoPor, EditadoEn = @EditadoEn where ID = @ID", modelo);
            });
        }
        public static async Task EliminaTipoTarea(int id)
        {
            await Task.Run(async () =>
            {
                await AccesoBD.Comando(@"Delete from TipoTarea where ID = @ID", new { ID = id });
            });
        }
        public static async Task<List<TipoTareaModel>> CargarTiposTareas()
        {
            return await Task.Run(async () =>
            {
                return await AccesoBD.LoadDataAsync<TipoTareaModel>("Select * from TipoTarea");
            });

        }
        public static async Task<List<SelectListItem>> TiposTareas()
        {
            return await Task.Run(async () =>
            {
                return await AccesoBD.LoadDataAsync<SelectListItem>("Select TipoTarea Text, ID Value from TipoTarea");
            });

        }
        public static async Task<TipoTareaModel> CargarTipoTarea(int id)
        {
            return await Task.Run(async () =>
            {
                return await AccesoBD.LoadAsync("Select * from TipoTarea where ID = @ID", new TipoTareaModel { ID = id });
            });

        }
        //==========================================================================================================================
        //Reportes
        //==========================================================================================================================
  
        //==========================================================================================================================
        //Bitacora
        //==========================================================================================================================   


    

        //==========================================================================================================================
        //Herramientas
        //========================================================================================================================== 

        public static async Task insertaSolicitudHerramienta(List<SolicitudHerramientaModel> modelo)
        {
            foreach (var item in modelo)
            {
                await Task.Run(async () =>
               {

                   await AccesoBD.LoadDataAsync(@"Insert into SolicitudHerramienta (IDTarea, Item, Cantidad, Descripcion, IDEstado, CreadoPor, CreadoEn, EditadoPor, EditadoEn)
                                              Values (@IDTarea, @Item, @Cantidad, @Descripcion, @IDEstado, @CreadoPor, @CreadoEn, @EditadoPor, @EditadoEn)  ", item);

               });
            }


        }
        public static async Task updateSolicitudHerramienta(SolicitudHerramientaModel modelo)
        {

            await Task.Run(async () =>
            {

                await AccesoBD.LoadDataAsync(@"Update SolicitudHerramienta Set Item = @Item, Cantidad = @Cantidad, Descripcion = @Descripcion, EditadoPor = @EditadoPor, EditadoEn = @EditadoEn where ID = @ID", modelo);

            });



        }
        public static Task deleteHerramienta(int id)
        {
            return Task.Run(() =>
            {
                return AccesoBD.LoadDataAsync("Delete from SolicitudHerramienta where ID = @ID ", new { ID = id });

            });

        }
        public static async Task<List<SolicitudHerramientaModel>> getSolicitudesHerramienta(int idtarea)
        {
            return await Task.Run(async () =>
            {
                return await AccesoBD.LoadDataAsync(@"Select SolicitudHerramienta.Item, SolicitudHerramienta.Cantidad, SolicitudHerramienta.Descripcion, SolicitudHerramienta.ID, 
                                                    SolicitudHerramienta.IDTarea, EstadoSolicitudItem.Estado  from SolicitudHerramienta
                                                    inner join EstadoSolicitudItem on SolicitudHerramienta.IDEstado = EstadoSolicitudItem.ID where IDTarea = @IDTarea",
                                                    new SolicitudHerramientaModel { IDTarea = idtarea });

            });

        }
        //==========================================================================================================================
        //Epp
        //========================================================================================================================== 

        public static async Task insertaSolicitudEpp(List<SolicitudEppModel> modelo)
        {
            foreach (var item in modelo)
            {
                await Task.Run(async () =>
                {

                    await AccesoBD.LoadDataAsync(@"Insert into SolicitudEpp (IDTarea, Item, Cantidad, Descripcion, IDEstado, CreadoPor, CreadoEn, EditadoPor, EditadoEn)
                                              Values (@IDTarea, @Item, @Cantidad, @Descripcion, @IDEstado, @CreadoPor, @CreadoEn, @EditadoPor, @EditadoEn)  ", item);

                });
            }


        }
        public static async Task updateSolicitudEpp(SolicitudEppModel modelo)
        {

            await Task.Run(async () =>
            {

                await AccesoBD.LoadDataAsync(@"Update SolicitudEpp Set Item = @Item, Cantidad = @Cantidad, Descripcion = @Descripcion, EditadoPor = @EditadoPor, EditadoEn = @EditadoEn where ID = @ID", modelo);

            });



        }
        public static Task deleteEpp(int id)
        {
            return Task.Run(() =>
            {
                return AccesoBD.LoadDataAsync("Delete from SolicitudEpp where ID = @ID ", new { ID = id });

            });

        }
        public static async Task<List<SolicitudEppModel>> getSolicitudesEpp(int idtarea)
        {
            return await Task.Run(async () =>
            {
                return await AccesoBD.LoadDataAsync(@"Select SolicitudEpp.Item, SolicitudEpp.Cantidad, SolicitudEpp.Descripcion, SolicitudEpp.ID, SolicitudEpp.IDTarea, 
                                                    EstadoSolicitudItem.Estado  from SolicitudEpp
                                                    inner join EstadoSolicitudItem on SolicitudEpp.IDEstado = EstadoSolicitudItem.ID where IDTarea = @IDTarea",
                                                    new SolicitudEppModel { IDTarea = idtarea });

            });

        }
        //==========================================================================================================================
        //Dinero
        //========================================================================================================================== 

        public static async Task insertaSolicitudDinero(List<SolicitudDinero> modelo)
        {
            foreach (var item in modelo)
            {
                await Task.Run(async () =>
                {

                    await AccesoBD.LoadDataAsync(@"Insert into SolicitudDinero (IDTarea, Monto, Descripcion, IDEstado, IDEmpleado, CreadoPor, CreadoEn, EditadoPor, EditadoEn)
                                              Values (@IDTarea, @Monto, @Descripcion, @IDEstado, @IDEmpleado, @CreadoPor, @CreadoEn, @EditadoPor, @EditadoEn)  ", item);

                });
            }


        }
        public static async Task updateSolicitudDinero(SolicitudDinero modelo)
        {

            await Task.Run(async () =>
            {

                await AccesoBD.LoadDataAsync(@"Update SolicitudDinero Set Monto = @Monto, Descripcion = @Descripcion, IDEmpleado = @IDEmpleado, EditadoPor = @EditadoPor, EditadoEn = @EditadoEn where ID = @ID", modelo);

            });



        }
        public static Task deleteSolicitudDinero(int id)
        {
            return Task.Run(() =>
            {
                return AccesoBD.LoadDataAsync("Delete from SolicitudDinero where ID = @ID ", new { ID = id });

            });

        }
        public static async Task<List<SolicitudDinero>> getSolicitudesDinero(int idtarea)
        {
            return await Task.Run(async () =>
            {
                return await AccesoBD.LoadDataAsync(@"Select SolicitudDinero.Monto, SolicitudDinero.Descripcion, SolicitudDinero.ID, SolicitudDinero.IDTarea, SolicitudDinero.IDEmpleado, PersonalServicios.Nombre + ' ' + PersonalServicios.Apellido Empleado, 
                                                    EstadoSolicitudItem.Estado from SolicitudDinero
                                                    inner join EstadoSolicitudItem on SolicitudDinero.IDEstado = EstadoSolicitudItem.ID 
                                                    inner join PersonalServicios on PersonalServicios.ID = SolicitudDinero.IDEmpleado
                                                    where IDTarea = @IDTarea",

                                                    new SolicitudDinero { IDTarea = idtarea });

            });

        }

        //==========================================================================================================================
        //Equipos de apoyo y Servicios Profesionales Externos
        //==========================================================================================================================

        public static async Task insertaSolicitudEyS(List<TareaEyS> modelo)
        {
            foreach (var item in modelo)
            {
                await Task.Run(async () =>
                {

                    await AccesoBD.LoadDataAsync(@"Insert into TareaEyS (IDEstado, IDTarea, IDCotEyS, Cantidad, Comentarios, CreadoPor, CreadoEn, EditadoPor, EditadoEn)
                                              Values (@IDEstado, @IDTarea, @IDCotEyS, @Cantidad, @Comentarios, @CreadoPor, @CreadoEn, @EditadoPor, @EditadoEn) ", item);

                });
            }


        }
        public static async Task updateSolicitudEyS(TareaEyS modelo)
        {

            await Task.Run(async () =>
            {

                await AccesoBD.LoadDataAsync(@"Update TareaEyS Set Cantidad = @Cantidad, Comentarios = @Comentarios, EditadoPor = @EditadoPor, EditadoEn = @EditadoEn where ID = @ID", modelo);

            });



        }
        public static Task deleteSolicitudEyS(int id)
        {
            return Task.Run(() =>
            {
                return AccesoBD.LoadDataAsync("Delete from TareaEyS where ID = @ID ", new { ID = id });

            });

        }
        public static async Task<List<TareaEyS>> getSolicitudesEyS(int idtarea)

        {
            return await Task.Run(async () =>
            {
                return await AccesoBD.LoadDataAsync(@"Select CotizacionEquiposServicios.Item, CotizacionEquiposServicios.Unidad, TareaEyS.Cantidad, TareaEyS.Comentarios, TareaEyS.ID, TareaEyS.IDTarea, 
                                                    EstadoSolicitudItem.Estado from TareaEyS
                                                    inner join EstadoSolicitudItem on TareaEyS.IDEstado = EstadoSolicitudItem.ID 
                                                    inner join CotizacionEquiposServicios on TareaEyS.IDCotEyS = CotizacionEquiposServicios.ID 
                                                    where IDTarea = @IDTarea",

                                                    new TareaEyS { IDTarea = idtarea });

            });

        }
        public static async Task<List<CotizacionEquiposModel>> getItemEyS(int idcot)
        {
            return await Task.Run(async () =>
            {
                return await AccesoBD.LoadDataAsync("Select Item, ID, Unidad from CotizacionEquiposServicios where IDCot = @IDCot", new CotizacionEquiposModel { IDCot = idcot });
            });
        }
        //==========================================================================================================================
        //Actualizar Progreso Tarea
        //==========================================================================================================================

        public static async Task actualizarProgresoTareaAsync(ActualizacionTareasModel modelo)
        {
            await Task.Run(async () => {
                await AccesoBD.Comando(@"Insert into ActualizacionTareas (IDTarea, Fecha, Progreso, Descripcion, CreadoPor, EditadoPor, CreadoEn, EditadoEn)
                                  Values (@IDTarea, @Fecha, @Progreso, @Descripcion, @CreadoPor, @EditadoPor, @CreadoEn, @EditadoEn)", modelo);
            });
        }

     
        public static async Task editaProgreso(ActualizacionTareasModel modelo)
        {
            await Task.Run(async () =>
            {
                return await AccesoBD.Comando(@"Update Actualizaciontareas Set Progreso = @Progreso where IDTarea = @IDTarea", 
                        new ActualizacionTareasModel { IDTarea = modelo.IDTarea });

            });

        }
        public static async Task<List<ActualizacionTareasModel>> getBitacora(int idtarea)
        {
            return await Task.Run(async () =>
            {
                return await AccesoBD.LoadDataAsync(@"Select ID, Fecha, Progreso, Descripcion from ActualizacionTareas where IDTarea = @IDTarea",
                  new ActualizacionTareasModel { IDTarea = idtarea });
            });

        }

        public static async Task editaBitacora(ActualizacionTareasModel modelo)
        {
            await Task.Run(async () =>
            {
                return await AccesoBD.Comando(@"Update ActualizacionTareas Set Fecha = @Fecha, Progreso = @Progreso, Descripcion = @Descripcion, 
                EditadoPor = @EditadoPor, EditadoEn = @EditadoEn  where ID = @ID", modelo);

            });

        }
    }
}
