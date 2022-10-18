using OfficeOpenXml;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.IO;
using BibliotecaDeClases.Modelos.Cotizaciones;
using static BibliotecaDeClases.Logica.ProcesadorMateriales;
using static BibliotecaDeClases.Logica.Cotizaciones.ProcesadorCotizacionesArauco;
using static BibliotecaDeClases.Logica.ProcesadorPersonal;
using static BibliotecaDeClases.Logica.Cotizaciones.ProcesadorEquiposServicios;
using System.Web;
using Spire.Xls;
using System;
using static BibliotecaDeClases.Logica.Servicios.ProcesadorTareas;
using static BibliotecaDeClases.Logica.Servicios.ProcesadorInforme;
using static BibliotecaDeClases.Logica.ProcesadorCotizaciones;
using static BibliotecaDeClases.Logica.Cotizaciones.ProcesadorSeguridad;
using BibliotecaDeClases.Modelos.Servicios;

namespace BibliotecaDeClases.Logica.Excel
{
   public class EditorExcel
    {        
        public async static Task CreaExcel(int id)
        {
            
            var modelo = CargarCotizacionAraucoSimpleAsync(id);
            var materiales = LoadMaterialesySubcotAsync(id);
            var personal = LoadCotPersonalAsync(id);
            var equipos = LoadCotEquiposAsync(id);
            await Task.WhenAll(modelo, materiales, personal, equipos);
            CotizacionAraucoFoxExcel cotizacion = await modelo;
            cotizacion.Materiales = await materiales;
            cotizacion.Personal = await personal;              
            cotizacion.Equipos = await equipos;

           await SaveExcelFileAsync(cotizacion);           

        }
        public async static Task<Stream> CreaPDFStream(int id)
        {

            var libro = await ReturnExcelFileAsync(id);
            Workbook workbook = new Workbook();
            workbook.LoadFromStream(libro);
            var stream = new MemoryStream();
            
            workbook.SaveToStream(stream, FileFormat.PDF);
            stream.Position = 0;
            return stream;
        }

        public async static Task<Stream> CreaPDFStreamOFT(int id)
        {

            var libro = await getExcelOfTecnicaAsync(id);
            Workbook workbook = new Workbook();
            workbook.LoadFromStream(libro);
            var stream = new MemoryStream();

            workbook.SaveToStream(stream, FileFormat.PDF);
            stream.Position = 0;
            return stream;
        }




        private static async Task SaveExcelFileAsync (CotizacionAraucoFoxExcel modelo)
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            string book = HttpContext.Current.Server.MapPath("~/Content/Formatos/Formato.xlsx");
            var file = new FileInfo(book);
            //DeleteIfExist(file);
            using (var package = new ExcelPackage(file))
            {
                var ws = package.Workbook.Worksheets[0];
                
                //var rango = package.Workbook.Names["Print_Area"];
                
                  ws.Cells["G7"].Value = modelo.SolicitudPedido;
                  ws.Cells["G8"].Value = modelo.NLicitacion;
                  ws.Cells["G9"].Value = modelo.IngenieroContratos;
                  ws.Cells["G10"].Value = modelo.Fecha;
                  ws.Cells["G11"].Value = modelo.ValidezOferta;
                  ws.Cells["G12"].Value = modelo.IDCotizacion;
                  ws.Cells["G13"].Value = modelo.TiempoEjecucion;
                  ws.Cells["B18"].Value = modelo.Detalles;
                  ws.Cells["B22"].Value = modelo.Notas;
                  ws.Cells["E33"].Value = modelo.Utilidad/100;
                  ws.Cells["E34"].Value = modelo.GastosGenerales/100;

                for (int i = 0; i < modelo.Materiales.Count; i++)                                                                                                                                                                                                               
                {
                    ws.Cells["B"+ (i + 39).ToString()].Value = modelo.Materiales[i].Item ;
                }
                for (int i = 0; i < modelo.Materiales.Count; i++)
                {
                    ws.Cells["E" + (i + 39).ToString()].Value = modelo.Materiales[i].Unidad;
                }
                for (int i = 0; i < modelo.Materiales.Count; i++)
                {
                    ws.Cells["F" + (i + 39).ToString()].Value = modelo.Materiales[i].Cantidad;
                }
                for (int i = 0; i < modelo.Materiales.Count; i++)
                {
                    ws.Cells["G" + (i + 39).ToString()].Value = modelo.Materiales[i].PUnitario;
                }



                for (int i = 0; i < modelo.Personal.Count; i++)
                {
                    ws.Cells["B" + (i + 57).ToString()].Value = modelo.Personal[i].Especialidad;
                }
                for (int i = 0; i < modelo.Personal.Count; i++)
                {
                    ws.Cells["D" + (i + 57).ToString()].Value = modelo.Personal[i].Cantidad;
                }
                for (int i = 0; i < modelo.Personal.Count; i++)
                {
                    ws.Cells["E" + (i + 57).ToString()].Value = modelo.Personal[i].HH;
                }
               
                for (int i = 0; i < modelo.Personal.Count; i++)
                {
                    ws.Cells["G" + (i + 57).ToString()].Value = modelo.Personal[i].ValorHH;
                }


                for (int i = 0; i < modelo.Equipos.Count; i++)
                {
                    ws.Cells["B" + (i + 66).ToString()].Value = modelo.Equipos[i].Item;
                }
                for (int i = 0; i < modelo.Equipos.Count; i++)
                {
                    ws.Cells["E" + (i + 66).ToString()].Value = modelo.Equipos[i].Unidad;
                }
                for (int i = 0; i < modelo.Equipos.Count; i++)
                {
                    ws.Cells["F" + (i + 66).ToString()].Value = modelo.Equipos[i].Cantidad;
                }
                for (int i = 0; i < modelo.Equipos.Count; i++)
                {
                    ws.Cells["G" + (i + 66).ToString()].Value = modelo.Equipos[i].PUnitario;
                }

                


                string libro = HttpContext.Current.Server.MapPath("~/Content/Formatos/Formato1.xlsx");
                
                               
                var newfile = new FileInfo(libro);
                if (newfile.Exists)
                {
                    newfile.Delete();
                } 

                 await package.SaveAsAsync(newfile);
              
                
            }
        
        
        } 
        public static async Task<Stream> ReturnExcelFileAsync(int id)
        {

            var modelo = await CargarCotizacionAraucoSimpleAsync(id);
            var materiales = LoadMaterialesySubcotAsync(id);
            var personal = LoadCotPersonalAsync(id);
            var equipos = LoadCotEquiposAsync(id);
            var tareas = new List<Task> { materiales, personal, equipos };
            while(tareas.Count > 0)
            {
                Task terminada = await Task.WhenAny(tareas);
                if(terminada == materiales)
                {
                    modelo.Materiales = await materiales;
                }
                else if(terminada == personal)
                {
                    modelo.Personal = await personal;

                }
                else if(terminada == equipos)
                {
                    modelo.Equipos = await equipos;
                }
                tareas.Remove(terminada);
            }
            
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            string book = HttpContext.Current.Server.MapPath("~/Content/Formatos/Formato.xlsx");
            var file = new FileInfo(book);
            
            using (var package = new ExcelPackage(file))
            {
                var ws = package.Workbook.Worksheets[0];

                

                ws.Cells["G7"].Value = modelo.SolicitudPedido;
                ws.Cells["G8"].Value = modelo.NLicitacion;
                ws.Cells["G9"].Value = modelo.IngenieroContratos;
                ws.Cells["G10"].Value = modelo.Fecha;
                ws.Cells["G11"].Value = modelo.ValidezOferta;
                ws.Cells["G12"].Value = modelo.IDCotizacion;
                ws.Cells["G13"].Value = modelo.TiempoEjecucion;
                ws.Cells["B18"].Value = modelo.Detalles;
                //Notas
                if (modelo.Notas != null)
                {
                    var notas = newLineProcesador(modelo.Notas);
                for (int i = 0; i < notas.Length; i++)
                {
                        ws.Cells["B" + (i + 22).ToString()].Value = notas[i];
                }
                }
              
                //ws.Cells["B22"].Value = modelo.Notas;
                ws.Cells["E33"].Value = modelo.Utilidad / 100;
                ws.Cells["E34"].Value = modelo.GastosGenerales / 100;

                //Detalle presupuesto
                for (int i = 0; i < modelo.Materiales.Count; i++)
                {
                    ws.Cells["B" + (i + 39).ToString()].Value = modelo.Materiales[i].Item;
                    ws.Cells["E" + (i + 39).ToString()].Value = modelo.Materiales[i].Unidad;
                    ws.Cells["F" + (i + 39).ToString()].Value = modelo.Materiales[i].Cantidad;
                    ws.Cells["G" + (i + 39).ToString()].Value = modelo.Materiales[i].PUnitario;

                }
               
                for (int i = 0; i < modelo.Personal.Count; i++)
                {
                    ws.Cells["B" + (i + 57).ToString()].Value = modelo.Personal[i].Especialidad;
                    ws.Cells["D" + (i + 57).ToString()].Value = modelo.Personal[i].Cantidad;
                    ws.Cells["E" + (i + 57).ToString()].Value = modelo.Personal[i].HH;
                    ws.Cells["G" + (i + 57).ToString()].Value = modelo.Personal[i].ValorHH;
                   
                }
             
                for (int i = 0; i < modelo.Equipos.Count; i++)
                {
                    ws.Cells["E" + (i + 66).ToString()].Value = modelo.Equipos[i].Unidad;
                    ws.Cells["F" + (i + 66).ToString()].Value = modelo.Equipos[i].Cantidad;
                    ws.Cells["G" + (i + 66).ToString()].Value = modelo.Equipos[i].PUnitario;
                }
               

             
                var stream = new MemoryStream(await package.GetAsByteArrayAsync());

                return stream; 
             


            }


        }

        public static async Task<Stream> InformeEstadoServicioAsync(int idservicio, int idcot)
        {
            var tareasTask = LoadTareasAsync(idservicio);
            var acumulado = getResumenAcumuladoServicioAsync(idservicio);
            var resumenpptoTask = getresumenPresupuestoServicio(idcot);
            await Task.WhenAll(tareasTask, acumulado, resumenpptoTask);

            var servicio = await resumenpptoTask;
            var tareas = await tareasTask;
            var tareasArray = tareas.ToArray();
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            string book = HttpContext.Current.Server.MapPath("~/Content/Formatos/FormatoInforme.xlsx");
            var file = new FileInfo(book);

            using (var package = new ExcelPackage(file))
            {



                var hoja = package.Workbook.Worksheets[0];
                var hoja2 = package.Workbook.Worksheets[1];
                //Detalles
                hoja.Cells["C3"].Value = servicio.NombreServicio;
                hoja.Cells["C4"].Value = servicio.FechaInicio;
                hoja.Cells["G4"].Value = servicio.Cliente;

                //Tabla Tareas
                for (int i = 8; i < tareasArray.Length + 8; i++)
                {
                    ExcelRange cells = hoja.Cells[8, 1, 8, 6];
                    ExcelRangeBase rangodestino = hoja.Cells[i, 1, i, 6];
                    cells.Copy(rangodestino);

                    hoja.Cells["A" + i.ToString()].Value = tareasArray[i - 8].Nombre;
                    hoja.Cells["C" + i.ToString()].Value = tareasArray[i - 8].Encargado;
                    hoja.Cells["E" + i.ToString()].Value = tareasArray[i - 8].Progreso/100;
                    hoja.Cells["F" + i.ToString()].Value = tareasArray[i - 8].Gasto;



                    //Copiar fórmulas hoja 2 
                    var fechainicial = tareasArray[i - 8].FechaInicial;
                    var fechafinal = tareasArray[i - 8].FechaFinal;
                    hoja2.Cells["F" + (i - 6).ToString()].Value = new DateTime(fechainicial.Year, fechainicial.Month, fechainicial.Day);
                    hoja2.Cells["G" + (i - 6).ToString()].Value = new DateTime(fechafinal.Year, fechainicial.Month, fechafinal.Day);
                }

                //Gráfico
                List<TablaTareas> tabla = new List<TablaTareas>();
                foreach (var item in tareas)
                {
                    tabla.Add(new TablaTareas { Nombre = item.Nombre, Inicio = item.FechaInicial, Termino = item.FechaFinal });
                }

               

                //Insertar tareas
                hoja2.Cells["A2"].LoadFromCollection(tabla);
                //Cacular máximo y mínimo
                hoja2.Cells["H2"].Formula = "MIN(F2:F"+ (tareasArray.Length + 1).ToString() +")";
                hoja2.Cells["I2"].Formula = "MAX(G2:G" + (tareasArray.Length + 1).ToString() + ")";
                hoja2.Cells["H2"].Calculate();
                hoja2.Cells["I2"].Calculate();
                var max = double.Parse(hoja2.Cells["H2"].Value.ToString());
                var min = double.Parse(hoja2.Cells["I2"].Value.ToString());



                //Crear Gráfico
                var grafico = hoja.Drawings.AddChart("Gantt", OfficeOpenXml.Drawing.Chart.eChartType.BarStacked);
                grafico.SetPosition(6, 0, 7, 0);
                grafico.SetSize(320, 299);
                grafico.RoundedCorners = false;
                string rotulos = "Hoja2!A2:A" + (tareasArray.Length + 1).ToString();
                string serie1 = "Hoja2!B2:B" + (tareasArray.Length + 1).ToString();
                string serie2 = "Hoja2!D2:D" + (tareasArray.Length + 1).ToString();

                var serieA = grafico.Series.Add(serie1,rotulos);
                var serieB = grafico.Series.Add(serie2,rotulos);
                grafico.Legend.Remove();

                grafico.YAxis.Font.Size = 7;
                //grafico.YAxis.Orientation = OfficeOpenXml.Drawing.Chart.eAxisOrientation.MinMax;
                //serieA.Fill.Style = OfficeOpenXml.Drawing.eFillStyle.NoFill;
                //grafico.XAxis.MaxValue = max;
                //grafico.XAxis.MinValue = min;
                //grafico.XAxis.MajorUnit = 5;
                grafico.XAxis.Format = "dd-mm-yyyy";
                grafico.XAxis.Font.Size = 7;
                var stream = new MemoryStream(await package.GetAsByteArrayAsync());

                return stream;
            }


        }

        public class TablaTareas
        {
            public string Nombre { get; set; }
            public DateTime Inicio { get; set; }
            public DateTime Termino { get; set; }
        }

        public static string[] newLineProcesador(string text)
        {
            string[] lines = text.Split('\n');
            return lines;
            //List<string> lista = new List<string>();
            //string stopAt = "\r\n";

            //while (text.Length > 0)
            //{
            //    int charLocation = text.IndexOf(stopAt, StringComparison.Ordinal);
            //    if (charLocation > 0)
            //    {
            //        lista.Add(text.Substring(0, charLocation));

            //        text = text.Substring(charLocation);
            //    }
            //    else if (text.Length > 0)
            //    {
            //        lista.Add(text);
            //    }
            //}

            //return lista;

        }


        public static async Task<Stream> getExcelOfTecnicaAsync(int id)
        {

            var modelo = await CargarCotizacionAraucoSimpleAsync(id);
            var materiales = LoadMaterialesySubcotAsync(id);
            var personal = LoadCotPersonalAsync(id);
            var equipos = LoadCotEquiposAsync(id);
            var tareas = new List<Task> { materiales, personal, equipos };
            while (tareas.Count > 0)
            {
                Task terminada = await Task.WhenAny(tareas);
                if (terminada == materiales)
                {
                    modelo.Materiales = await materiales;
                }
                else if (terminada == personal)
                {
                    modelo.Personal = await personal;

                }
                else if (terminada == equipos)
                {
                    modelo.Equipos = await equipos;
                }
                tareas.Remove(terminada);
            }

            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            string book = HttpContext.Current.Server.MapPath("~/Content/Formatos/FormatoTecnica.xlsx");
            var file = new FileInfo(book);
            using (var package = new ExcelPackage(file))
            {
                var ws = package.Workbook.Worksheets[0];

                ws.Cells["G7"].Value = modelo.SolicitudPedido;
                ws.Cells["G8"].Value = modelo.NRFPAriba;
                ws.Cells["G9"].Value = modelo.IngenieroContratos;
                ws.Cells["G10"].Value = modelo.Fecha.ToString("dd-MM-yyyy");
                ws.Cells["G11"].Value = modelo.ValidezOferta.ToString("dd-MM-yyyy");
                ws.Cells["G12"].Value = modelo.IDCotizacion;
                ws.Cells["B18"].Value = "Oferta Técnica " + modelo.Detalles;
                //Notas
                if (modelo.DATOE != null)
                {
                    var DATOE = newLineProcesador(modelo.DATOE);
                    for (int i = 0; i < DATOE.Length; i++)
                    {
                        ws.Cells["B" + (i + 22).ToString()].Value = DATOE[i];
                    }
                }



                //Detalle presupuesto

                //Materiales
                var x = modelo.Materiales.Count - 10;
                if (x>0)
                {
                    ws.InsertRow(44, x);
                }
                for (int i = 0; i < modelo.Materiales.Count; i++)
                {
                    ws.Cells["B" + (i + 43).ToString()].Value = modelo.Materiales[i].Item;
                    ws.Cells["F" + (i + 43).ToString()].Value = modelo.Materiales[i].Unidad;
                    ws.Cells["G" + (i + 43).ToString()].Value = modelo.Materiales[i].Cantidad;
                }
               

                for (int i = 0; i < modelo.Personal.Count; i++)
                {
                    ws.Cells["B" + (i + 55+x).ToString()].Value = modelo.Personal[i].Especialidad;
                    ws.Cells["D" + (i + 55+x).ToString()].Value = modelo.Personal[i].Cantidad;
                    ws.Cells["E" + (i + 55+x).ToString()].Value = modelo.Personal[i].Especialidad;
                    ws.Cells["F" + (i + 55+x).ToString()].Value = modelo.Personal[i].Dias;
                    ws.Cells["E" + (i + 55+x).ToString()].Value = modelo.Personal[i].Cantidad * modelo.Personal[i].HH;
                }
                
                for (int i = 0; i < modelo.Equipos.Count; i++)
                {
                    ws.Cells["B" + (i + 68+x).ToString()].Value = modelo.Equipos[i].Item;
                    ws.Cells["F" + (i + 68+x).ToString()].Value = modelo.Equipos[i].Unidad;
                    ws.Cells["G" + (i + 68+x).ToString()].Value = modelo.Equipos[i].Cantidad;
                }
               


                var stream = new MemoryStream(await package.GetAsByteArrayAsync());

                return stream;



            }
        }


        public static async Task<Stream> ReturnExcelSBFileAsync(int id)
        {

            var modelo = await LoadCotizacionAsync(id);
            var materiales = LoadCotMaterialesAsync(id);
            var personal = LoadCotPersonalAsync(id);
            var equipos = LoadCotEquiposAsync(id);
            var seguridad = LoadCotSeguridadAsync(id);
            var tareas = new List<Task> { materiales, personal, equipos, seguridad };
            while (tareas.Count > 0)
            {
                Task terminada = await Task.WhenAny(tareas);
                if (terminada == materiales)
                {
                    modelo.Materiales = await materiales;
                }
                else if (terminada == personal)
                {
                    modelo.Personal = await personal;

                }
                else if (terminada == equipos)
                {
                    modelo.Equipos = await equipos;
                }
                else if (terminada == seguridad)
                {
                    modelo.Seguridad = await seguridad;
                }
                tareas.Remove(terminada);
            }

            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            string book = HttpContext.Current.Server.MapPath("~/Content/Formatos/FormatoSB.xlsx");
            var file = new FileInfo(book);

            using (var package = new ExcelPackage(file))
            {
                var ws = package.Workbook.Worksheets[0];


                ws.Cells["J3"].Value = modelo.IDCotizacion?.ToString() ?? "";
                ws.Cells["F5"].Value = "Solicitud de Pedido (SP) " + modelo.SolicitudPedido?.ToString() ?? "";
                ws.Cells["F6"].Value = "Supervisor EE.SS.: " + modelo.Supervisor?.ToString() ?? "";
                ws.Cells["I9"].Value = "Fecha oferta: " + modelo.Fecha.ToString();
                ws.Cells["B12"].Value = modelo.Detalles?.ToString() ?? "";
                

                //Detalle presupuesto
                for (int i = 0; i < modelo.Materiales.Count; i++)
                {
                    ws.Cells["B" + (i + 25).ToString()].Value = modelo.Materiales[i].Item;
                    ws.Cells["F" + (i + 25).ToString()].Value = modelo.Materiales[i].Unidad;
                    ws.Cells["G" + (i + 25).ToString()].Value = modelo.Materiales[i].Cantidad;
                    ws.Cells["I" + (i + 25).ToString()].Value = modelo.Materiales[i].PUnitario;

                }

                for (int i = 0; i < modelo.Personal.Count; i++)
                {
                    ws.Cells["B" + (i + 43).ToString()].Value = modelo.Personal[i].Especialidad;
                    ws.Cells["F" + (i + 43).ToString()].Value = modelo.Personal[i].Cantidad;
                    ws.Cells["G" + (i + 43).ToString()].Value = modelo.Personal[i].HH;
                    ws.Cells["I" + (i + 43).ToString()].Value = modelo.Personal[i].ValorHH;

                }

                for (int i = 0; i < modelo.Seguridad.Count; i++)
                {
                    ws.Cells["B" + (i + 61).ToString()].Value = modelo.Seguridad[i].Item;
                    ws.Cells["F" + (i + 61).ToString()].Value = modelo.Seguridad[i].Cantidad;
                    ws.Cells["I" + (i + 61).ToString()].Value = modelo.Seguridad[i].PUnitario;
                   
                }


                for (int i = 0; i < modelo.Equipos.Count; i++)
                {
                    ws.Cells["B" + (i + 61).ToString()].Value = modelo.Equipos[i].Item;
                    ws.Cells["F" + (i + 61).ToString()].Value = modelo.Equipos[i].Unidad;
                    ws.Cells["G" + (i + 61).ToString()].Value = modelo.Equipos[i].Cantidad;
                    ws.Cells["I" + (i + 61).ToString()].Value = modelo.Equipos[i].PUnitario;
                }



                var stream = new MemoryStream(await package.GetAsByteArrayAsync());

                return stream;



            }


        }


    }









}
