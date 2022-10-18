using BibliotecaDeClases.Modelos.Proveedores;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace BibliotecaDeClases.Logica.Proveedores
{
  public  class ProcesadorProveedores
    {
        public static async Task<List<SelectListItem>> ProveedoresDropdownAsync()
        {
            string sql = "Select * from ProveedoresServicios";

            var data = await AccesoBD.LoadDataAsync<ProveedorModel>(sql);
            List<SelectListItem> Lista = new List<SelectListItem>();
            foreach(var row in data)
            {
                Lista.Add(new SelectListItem { Text = row.NombreProveedor, Value = row.IDProveedor });
            }

            return Lista;


        }

        public static async Task<List<ProveedorModel>> ListadeProveedoresAsync()
        {
            string sql = "Select * from ProveedoresServicios";
            return await AccesoBD.LoadDataAsync<ProveedorModel>(sql);
            
        }

        public static async Task<int> CreaProveedorAsync(ProveedorModel proveedor)
        {
            string sql = @"Insert into ProveedoresServicios (IDProveedor, NombreProveedor, FonoProveedor, EmailProveedor, OtrosDetalles, CreadoPor, CreadoEn, EditadoEn, EditadoPor, NombreFantasia) 
                          Output Inserted.ID
                          Values (@IDProveedor, @NombreProveedor, @FonoProveedor, @EmailProveedor, @OtrosDetalles, @CreadoPor, @CreadoEn, @EditadoEn, @EditadoPor, @NombreFantasia) ";


            var data = await AccesoBD.LoadAsync(sql, proveedor);
            return data.ID;
        }

        public static async Task<int> EditaProveedorAsync(string id, string nombre, string fono, string email, string detalles, string editadopor, string nombrefantasia)
        {
            string sql = "Update ProveedoresServicios Set (IDProveedor = @IDProveedor, NombreProveedor = @NombreProveedor, FonoProveedor = @FonoProveedor, EmailProveedor = @EmailProveedor, OtrosDetalles = @OtrosDetalles, EditadoEn = @EditadoEn, EditadoPor = @EditadoPor)";

            ProveedorModel modelo = new ProveedorModel
            {
                IDProveedor = id,
                NombreProveedor = nombre,
                FonoProveedor = fono,
                EmailProveedor = email,
                OtrosDetalles = detalles,
                EditadoEn = DateTime.Now,
                EditadoPor = editadopor,
                NombreFantasia = nombrefantasia

            };

            return await AccesoBD.Comando(sql, modelo);

        }

        public static async Task<int> EliminaProveedorAsync(string id)
        {
            string sql = "Delete from ProveedoresServicios where IDProveedor = @IDProveedor";
            ProveedorModel modelo = new ProveedorModel
            {
                IDProveedor = id
            };


            return await AccesoBD.Comando(sql, modelo);


        }

        public static async Task<ProveedorModel> CargarProveedorAsync(string id)
        {
            string sql = "Select * from ProveedoresServicios where IDProveedor = @IDProveedor";


            ProveedorModel modelo = new ProveedorModel
            {
                IDProveedor = id
            };

            return await AccesoBD.LoadAsync(sql, modelo);        
        }


        public static async Task<List<ProveedorModel>> FiltroProveedorAsync(string texto)
        {
            string sql = "Select * from ProveedoresServicios where IDProveedor like + '%' + @IDProveedor + '%' or NombreProveedor like + '%' + @IDProveedor + '%' or NombreFantasia like '%' + @IDProveedor + '%'   ";

            ProveedorModel modelo = new ProveedorModel
            {
                IDProveedor = texto
            };

            return await AccesoBD.LoadDataAsync(sql, modelo);
        }

        public static async Task<List<ProveedorModel>> Top5ProveedoresAsync()
        {
            string sql = "Select Top 5 * from ProveedoresServicios";

         
            return await AccesoBD.LoadDataAsync<ProveedorModel>(sql);
        }

    }
}
