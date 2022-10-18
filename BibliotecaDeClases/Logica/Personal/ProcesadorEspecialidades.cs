using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BibliotecaDeClases.Modelos;
using BibliotecaDeClases.Logica;
using System.Web.Mvc;

namespace BibliotecaDeClases.Logica.Personal
{
  public  class ProcesadorEspecialidades
    {
        public static async Task<int> CreaEspecialidadAsync(EspecialidadModel especialidad)
        {
            especialidad.Especialidad = especialidad.Especialidad.ToUpperCheckForNull();
            especialidad.Descripcion = especialidad.Descripcion.ToUpperCheckForNull();
            especialidad.CreadoEn = especialidad.EditadoEn = DateTime.Now;
            string sql = @"Insert into Especialidades (Especialidad, ValorHH, Descripcion, CreadoPor, CreadoEn, EditadoPor, EditadoEn) 
            Values (@Especialidad, @ValorHH, @Descripcion, @CreadoPor, @CreadoEn, @EditadoPor, @EditadoEn)";
            
            return await AccesoBD.Comando(sql, especialidad);
            
            
        }
    
        public static async Task<int> EliminaEspecialidadAsync(int idespecialidad)
        {
            string sql = "Delete from Especialidades where ID = @ID";
            EspecialidadModel modelo = new EspecialidadModel
            {
                ID = idespecialidad
            };
            return await AccesoBD.Comando(sql, modelo);
        }
    
        public static async Task<int> ActualizaEspecialidadAsync(EspecialidadModel especialidad)
        {
            especialidad.Especialidad = especialidad.Especialidad.ToUpperCheckForNull();
            especialidad.Descripcion = especialidad.Descripcion.ToUpperCheckForNull();
            especialidad.CreadoEn = DateTime.Now;
            string sql = "Update Especialidades Set Especialidad = @ESpecialidad, ValorHH = @ValorHH, Descripcion = @Descripcion, EditadoPor = @EditadoPor, EditadoEn = @EditadoEn where ID = @ID";
            
            return await AccesoBD.Comando(sql, especialidad);
        }

        public static async Task<List<SelectListItem>> EspecialidadesAsync()

        {



            List<SelectListItem> lista = new List<SelectListItem>();
            string sql = "Select ID, Especialidad from Especialidades";

            var data = await AccesoBD.LoadDataAsync<EspecialidadModel>(sql);

            foreach (var row in data)
            {

                lista.Add(new SelectListItem() { Text = row.Especialidad, Value = row.ID.ToString() });


            }

            return lista;


        }

        public static async Task<List<EspecialidadModel>> TablaEspecialidadesAsync()
        {
            string sql = "Select * from Especialidades";

            return await AccesoBD.LoadDataAsync<EspecialidadModel>(sql);
        }

        public static async Task<EspecialidadModel> LoadEspecialidadAsync(int idespecialidad)
        {

            string sql = "Select distinct Especialidades.ID, Especialidades.Especialidad, Especialidades.ValorHH, Especialidades.Descripcion, CreadoPor.Nombre + ' ' + CreadoPor.Apellido CreadoPor, EditadoPor.Nombre + ' ' + EditadoPor.Apellido EditadoPor, Especialidades.CreadoEn, Especialidades.EditadoEn from Especialidades inner join PersonalServicios as CreadoPor on Especialidades.CreadoPor = CreadoPor.IDEmpleado inner join PersonalServicios as EditadoPor on Especialidades.EditadoPor = EditadoPor.IDEmpleado where Especialidades.ID = @ID";
            EspecialidadModel modelo = new EspecialidadModel
            {
                ID = idespecialidad
            };
            return await AccesoBD.LoadAsync(sql,modelo);
        }

    }
}
