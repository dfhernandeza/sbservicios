using BibliotecaDeClases.Modelos;
using BibliotecaDeClases.Modelos.Clientes;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Mvc;
using static BibliotecaDeClases.Logica.Funciones;

namespace BibliotecaDeClases.Logica.Clientes
{
    public class ProcesadorClientes
    {

        public static async Task<List<SelectListItem>> ListaClientesAsync()
        {

            string sql = "Select ID, RazonSocial, IDCliente from ClientesServicios where Arauco = 'False'";
            List<SelectListItem> lista = new List<SelectListItem>();


            var data = await AccesoBD.LoadDataAsync<ClienteModel>(sql);

            foreach (var row in data)
            {

                lista.Add(new SelectListItem() { Text = row.RazonSocial, Value = row.ID.ToString() });


            }

            return lista;

        }

        public static async Task<List<SelectListItem>> ListaClientesAraucoAsync()
        {
            string sql = "Select ID, RazonSocial, IDCliente from ClientesServicios where Arauco = 'True' ";
            List<SelectListItem> lista = new List<SelectListItem>();


            var data = await AccesoBD.LoadDataAsync<ClienteModel>(sql);

            foreach (var row in data)
            {

                lista.Add(new SelectListItem() { Text = row.RazonSocial, Value = row.ID.ToString() });


            }

            return lista;
        }
        public static ClienteModel CrearCliente(string idcliente, string razonsocial, string alias, string giro, bool arauco, string creadopor)
        {
            ClienteModel cliente = new ClienteModel
            {
                IDCliente = idcliente,
                RazonSocial = razonsocial,
                Alias = alias,
                Giro = giro,
                Arauco = arauco,
                CreadoPor = creadopor,
                CreadoEn = DateTime.Now,
                EditadoEn = DateTime.Now,
                EditadoPor = creadopor
            };

            string sql = @"Insert into dbo.ClientesServicios (IDCliente, RazonSocial, Alias, Giro, Arauco, CreadoPor, CreadoEn, EditadoEn, EditadoPor)
                         output inserted.ID, inserted.RazonSocial
                         Values (@IDCliente, @RazonSocial, @Alias, @Giro, @Arauco, @CreadoPor, @CreadoEn, @EditadoEn, @EditadoPor)";
            return AccesoBD.ReturnID(sql, cliente);

        }

        public static async Task<List<ClienteModel>> LoadClientesAsync()
        {
            string sql = "Select * from ClientesServicios";
            return await AccesoBD.LoadDataAsync<ClienteModel>(sql);


        }

        public static async Task<ClienteModel> LoadClienteAsync(int id)
        {
            ClienteModel cliente = new ClienteModel
            {
                ID = id
            };
            string sql = "Select ID, IDCliente, Alias, RazonSocial, Giro from ClientesServicios where ID = @ID";
            return await AccesoBD.LoadAsync(sql, cliente);


        }

        public static async Task<int> EditarClienteAsync(int id, string idcliente, string razonsocial, string alias, string giro, string editadopor)
        {
            ClienteModel Cotizacion = new ClienteModel
            {
                IDCliente = idcliente,
                RazonSocial = razonsocial,
                Alias = alias,
                Giro = giro,
                EditadoPor = editadopor,
                EditadoEn = DateTime.Now,
                ID = id


            };

            string sql = @"Update ClientesServicios Set IDCliente = @IDCliente, RazonSocial = @RazonSocial, Giro = @Giro, Alias = @Alias, EditadoPor = @EditadoPor, EditadoEn = @EditadoEn where ID = @ID";
            return await AccesoBD.Comando(sql, Cotizacion);
        }

        public static async Task<int> BorrarClienteAsync(int id)

        {
            ClienteModel modelo = new ClienteModel
            {
                ID = id
            };
            string sql = "Delete from ClientesServicios where ID = @ID";
            return await AccesoBD.Comando(sql, modelo);
        }

        public static async Task<int> BorrarContactoAsync(int idcontacto)
        {
            string sql = "Delete from Contactos where ID = @ID";
            ContactoModel modelo = new ContactoModel
            {
                ID = idcontacto
            };

            return await AccesoBD.Comando(sql, modelo);
        }

        public static async Task<int> BorraUbicacionAsync(int idubicacion)
        {
            string sql = "Delete from Ubicaciones where ID = @ID";
            UbicacionesModel modelo = new UbicacionesModel
            {
                ID = idubicacion
            };

            return await AccesoBD.Comando(sql, modelo);
        }

        public static async Task<List<ClienteModel>> FiltroCLienteAsync(string id)
        {
            ClienteModel data = new ClienteModel
            {
                IDCliente = id
            };
            string sql = "Select * from ClientesServicios where IDCliente like '%'+ @IDCliente + '%' or RazonSocial like '%'+ @IDCliente + '%' or Alias like '%'+ @IDCliente + '%'  ";


            var datos = await AccesoBD.LoadDataAsync(sql, data);
            List<ClienteModel> lista = new List<ClienteModel>();
            foreach (var row in datos)
            {

                lista.Add(new ClienteModel { IDCliente = FormatRutView(row.IDCliente), RazonSocial = row.RazonSocial, Alias = row.Alias });

            }

            return lista;

        }

        public static async Task<List<SelectListItem>> ListaContactosAsync(int idcliente)
        {
            return await Task.Run(async () =>
            {
                return await AccesoBD.LoadDataAsync(@"Select Nombre + ' ' + Apellido Text, ID Value from Contactos where 
               IDCliente = @Value ", new SelectListItem { Value = idcliente.ToString() });
            });


        }

        public static async Task<List<UbicacionesModel>> ListaUbicacionesAsync(int idcliente)
        {
            string sql = "Select Ubicaciones.ID, Ubicaciones.Nombre, Ubicaciones.Ciudad, Ubicaciones.Direccion, Ubicaciones.Alias, ClientesServicios.RazonSocial Cliente, Ubicaciones.IDCliente from Ubicaciones inner Join ClientesServicios on Ubicaciones.IDCliente = ClientesServicios.ID where Ubicaciones.IDCliente = @IDCliente ";
            UbicacionesModel modelo = new UbicacionesModel
            {
                IDCliente = idcliente
            };
            return await AccesoBD.LoadDataAsync(sql, modelo);

        }

        public static async Task<List<SelectListItem>> UbicacionesListAsync(int idcliente)
        {
            string sql = "Select ID,  Alias from Ubicaciones where Ubicaciones.IDCliente = @IDCliente ";
            UbicacionesModel modelo = new UbicacionesModel
            {
                IDCliente = idcliente
            };

            List<SelectListItem> lista = new List<SelectListItem>();
            var data = await AccesoBD.LoadDataAsync(sql, modelo);
            foreach (var row in data)
            {

                lista.Add(new SelectListItem() { Text = row.Alias, Value = row.ID.ToString() });


            }

            return lista;


        }
        public static async Task<List<ContactoModel>> ListaDeContactosAsync(int idcliente)
        {
            ContactoModel modelo = new ContactoModel
            {
                IDCliente = idcliente
            };
            string sql = "Select * from Contactos where IDCliente = @IDCliente";
            return await AccesoBD.LoadDataAsync(sql, modelo);


        }

        public static async Task<ContactoModel> CreaContacto(string idcontacto, string alias, string cargo, string nombre, string apellido, string email, string fono, string notas, int idcliente, string area, string creadopor)
        {
            return await Task.Run(() =>
             {
                 string sql = "Insert into Contactos (IDContacto, Alias, Cargo, Nombre, Apellido, Email, Ocupacion, Fono, Notas, IDCliente, Area)  " +
                             "output inserted.ID, inserted.Nombre, inserted.Apellido " +
                             "Values (@IDContacto, @Alias, @Cargo, @Nombre, @Apellido, @Email, @Ocupacion, @Fono, @Notas, @IDCliente, @Area)";
                 ContactoModel modelo = new ContactoModel
                 {
                     IDContacto = idcontacto,
                     Alias = alias,
                     Cargo = cargo,
                     Nombre = nombre,
                     Apellido = apellido,
                     Email = email,
                     Fono = fono,
                     Notas = notas,
                     IDCliente = idcliente,
                     Area = area,
                     CreadoPor = creadopor,
                     CreadoEn = DateTime.Now,
                     EditadoEn = DateTime.Now,
                     EditadoPor = creadopor
                 };

                 return AccesoBD.ReturnID(sql, modelo);
             });

        }

        public static async Task<int> CreaUbicacionAsync(string nombreubicacion, int idcliente, string direccion, string ciudad, string alias, string creadopor)
        {
            UbicacionesModel model = new UbicacionesModel
            {
                IDCliente = idcliente,
                Ciudad = ciudad,
                Direccion = direccion,
                Nombre = nombreubicacion,
                Alias = alias,
                CreadoPor = creadopor,
                CreadoEn = DateTime.Now,
                EditadoEn = DateTime.Now,
                EditadoPor = creadopor


            };
            string sql = "Insert into Ubicaciones (Nombre, IDCliente, Direccion, Ciudad, Alias, CreadoPor, CreadoEn, EditadoPor, EditadoEn) Values (@Nombre, @IDCliente, @Direccion, @Ciudad, @Alias, @CreadoPor, @CreadoEn, @EditadoPor, @EditadoEn) ";

            return await AccesoBD.Comando(sql, model);
        }

        public static async Task<int> EditaUbicacionAsync(int id, string nombreubicacion, string direccion, string ciudad, string alias, string editadopor)
        {
            UbicacionesModel model = new UbicacionesModel
            {

                Ciudad = ciudad,
                Direccion = direccion,
                Nombre = nombreubicacion,
                EditadoEn = DateTime.Now,
                EditadoPor = editadopor,
                ID = id,
                Alias = alias,



            };

            string sql = "Update Ubicaciones set Nombre = @Nombre, Direccion = @Direccion, Ciudad = @Ciudad, Alias = @Alias, EditadoPor = @EditadoPor, EditadoEn = @EditadoEn Where ID = @ID ";

            return await AccesoBD.Comando(sql, model);

        }
        public static async Task<ContactoModel> VerContactoAsync(int idcontacto)
        {
            string sql = "Select * from Contactos where ID = @ID";

            ContactoModel modelo = new ContactoModel
            {
                ID = idcontacto
            };

            return await AccesoBD.LoadAsync(sql, modelo);


        }
        public static async Task<int> EditaContactoAsync(int id, string idcontacto, string alias, string cargo, string nombre, string apellido, string email, string fono, string notas, string area, string editadopor)
        {
            string sql = "Update Contactos Set IDContacto = @IDContacto, Alias = @Alias, Cargo = @Cargo, Nombre = @Nombre, Apellido = @Apellido, Email = @Email, Ocupacion = @Ocupacion, Fono = @Fono, Notas = @Notas, EditadoPor = @EditadoPor, @EditadoEn = @EditadoEn, Area = @Area where ID = @ID";
            ContactoModel modelo = new ContactoModel
            {
                IDContacto = idcontacto,
                Alias = alias,
                Cargo = cargo,
                Nombre = nombre,
                Apellido = apellido,
                Email = email,
                Fono = fono,
                Notas = notas,
                Area = area,
                EditadoPor = editadopor,
                EditadoEn = DateTime.Now,
                ID = id


            };

            return await AccesoBD.Comando(sql, modelo);
        }

    }
}
