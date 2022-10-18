
var PptoGlobal;
var IDTarea;
var Origen;
var fechai;
var fechaf;
var JsonTareas;
var OrigenAsignacion;
var fila;
var filaobjetivo;
var Detalle;
var IDEntregable;
var IDHerramienta;
var ToolEpp = 0;
var IDTransferencia;
var IDEyS;
var filaseleccionada;
$(document).ready(function () {
  
    //====================================================================================================================================================================================
    //Layout
    //====================================================================================================================================================================================
    $('[data-bs-toggle="tooltip"]').tooltip();
    $(document).on("click", "#DeslizarDerecha", function () {
      Detalle = false;
      $(".reaparece").css("display", "none");
      $(".desaparece").css("display", "table-cell");
      $("#tareadiv").removeClass("col-3");
      $("#tareadiv").addClass("col-8");
      $("#personaldiv").removeClass("col-9");
      $("#personaldiv").addClass("col-4");
      $(".aparece").css("display", "none");
      $("#TablaEmpleados").css("display", "table");
      $("#contenedordescripcion").css("display", "none");
      $(".ancho").css("width", "60%");
      $("#thveinte").css("width", "20%");
      $("#TablaEmpleados").insertAfter("#personalcontenedor");
      $("#tabs").hide();
      $("#titulopersonal").show();
      if (fila !== undefined) {
        fila.css({ "background-color": "", color: "" });
      }

      /* setTimeout(function () {document.getElementById('TituloPersonal').innerHTML = 'Personal'}, 500);*/
      LimpiaAsignaciones();
      setTimeout(function () {
        LimpiarEmpleados(), LlenarEmpleados();
      }, 801);
    });
    $(document).on("hidden.bs.modal", ".modal", function (e) {
      $(function () {
        $('[data-bs-toggle="tooltip"]').tooltip();
      });
      $(".form-check-input").prop("checked", false);
    });
    $(document).on("click", "#filamaterial", function () {
      $("#costosdiv").removeClass("col-12").addClass("col-4");

      $("#detallediv").css("display", "block");

      document.getElementById("TituloDetalle").innerText = "Detalle Materiales";

      $.ajax({
        url: "/CotizacionArauco/Data/",
        type: "POST",
        async: true,
        dataType: "text",
        data: {
          id: $("#IDCotizacion").val(),
        },
        success: function (data) {
          data = JSON.parse(data);
          $(data).each(function (i, item) {
            var row = $(
              '<tr class="resaltado"><td>' +
                item.Item +
                '</td ><td class="desaparece">' +
                FormateaPeso(item.PUnitario * item.Cantidad) +
                "</td></tr>"
            );
            $("#TablaDetallePpto").append(row);
          });
        },
      });
    });
    $(document).on("change", ".checkprincipal", function (e) {
      if ($(this).prop("checked") == false) {
        $(".deslizado").each(function (i, item) {
          $(item).prop("checked", false);
        });
      } else {
        $(".deslizado").each(function (i, item) {
          $(item).prop("checked", true);
        });
      }
    });
    $('.fecha').datepicker({ dateFormat: 'dd-mm-yy' });
    $.datepicker.setDefaults($.datepicker.regional['es']);
    $.datetimepicker.setLocale("es");
    $("input,text-area").attr("autocomplete", "off");
    $("body").tooltip({
      selector: '[rel="tooltip"]',
      trigger: "hover",
    });
    Promise.all([
      LlenarEmpleados(),
      LlenarTareas(),
      LlenarPresupuesto(),
      LlenarMateriales(),
        LlenarGastos(),
        LlenarTimeSheet()
    ]);
    $(document).on("click", ".btn-close", function (e) {
      $(".modal").modal("hide");
    });
    $(".tiempo").datetimepicker({
      format: "d-m-Y H:i",
      formatTime: "H:i",
      formatDate: "d-m-Y",
      step: 15,
      hours12: false,
      dayOfWeekStart: 1,
    });
  
       
    
   
    //====================================================================================================================================================================================
    //Tareas
    //====================================================================================================================================================================================
    $(document).on("click", "#NuevaTareaBtn", function () {
      $("#iniciotarea,#fintarea").datetimepicker({
        minDate: $("#FechaInicio").val(),
        maxDate: $("#FechaTermino").val(),
      });
      $("#nuevaTareaModal").modal("show");
      $("#nuevaTareaForm").attr("action", "/Tareas/CreaTarea/");
      document.getElementById("exampleModalLabel").innerHTML = "Nueva Tarea";
      Origen = "Nueva";
    });
    $(document).on("shown.bs.modal", "#nuevaTareaModal", function (e) {
      $("#nuevaTareaForm").validate({
        rules: {
          Nombre: "required",
          IDEncargado: "required",
          FechaInicial: "required",
          FechaFinal: "required",
          Descripcion: "required",
        },

        messages: {
          Nombre: "Debe proporcionar un nombre",
          IDEncargado: "Debe escoger un encargado",
          FechaInicial: "Debe proporcionar una fecha de inicio",
          FechaFinal: "Debe proporcionar una fecha de termino",
          Descripcion: "Debe proporcionar una descripción",
        },
        
          submitHandler: function (form) {
              var ceco = {};
              if ($('#nuevaTareaForm input[name="EsCECO"]').is(":checked")) {
                  ceco["Nombre"] = $('#NombreServicio').val() + " - " + $('#nuevaTareaForm input[name="Nombre"]').val().toUpperCase();
                  ceco["Descripcion"] = "Centro de costos correspondiente al servicio " + $('#NombreServicio').val() + ", Tarea " + $('#nuevaTareaForm input[name="Nombre"]').val().toUpperCase();
                  ceco["IDEncargado"] = $('#nuevaTareaForm select[name="IDEncargado"]').val();
              }
              else {
                  ceco = null;
              }



              var tarea = {};
              tarea["ID"] = $('#nuevaTareaForm input[name="ID"]').val();
              tarea["Nombre"] = $('#nuevaTareaForm input[name="Nombre"]').val().toUpperCase();
              tarea["FechaInicial"] = $('#nuevaTareaForm input[name="FechaInicial"]').val();
              tarea["FechaFinal"] = $('#nuevaTareaForm input[name="FechaFinal"]').val();
              tarea["Descripcion"] = $('#nuevaTareaForm textarea[name="Descripcion"]').val().toUpperCase();
              tarea["IDEncargado"] = $('#nuevaTareaForm select[name="IDEncargado"]').val();
              tarea["IDServicio"] = $('#nuevaTareaForm input[name="IDServicio"]').val();
              tarea["IDUbicacion"] = $('#nuevaTareaForm select[name="IDUbicacion"]').val();
              tarea["CECO"] = ceco;
              $.ajax({
                  url: form.action,
                  type: form.method,
                  contentType: "application/json; charset=utf-8",
                  dataType: "json",
                  data: JSON.stringify(tarea),
                  success: function (response) {
                      LimpiaTareas();
                      LlenarTareas();
                      $("#nuevaTareaModal").modal("hide");
                      LimpiarModalTarea();
                  },
              });
          },
      });
    });
    $(document).on("click", "#BtnDetalleTarea", function () {
      if (fila !== undefined) {
        fila.css({ "background-color": "", color: "" });
      }
      Detalle = true;
      $(this).closest("tr").css({ "background-color": "gold", color: "white" });
      var tds = $(this).closest("tr").find("td");
      fila = $(this).closest("tr");
      IDTarea = $(tds[0]).text();

      $("#IDTareaEntregable").val(IDTarea);
      var texto = JsonTareas.find((item) => item.ID == IDTarea);
      $("#contenedordescripcion").css("display", "block");
      $("#textodescripcion").text(texto.Descripcion);
      fechai = tds[3].innerText;
      fechaf = tds[4].innerText;
      IDTarea = tds[0].innerText;
      $("#idtareaNA").val(IDTarea);
      $("#tabs").show();
      $("#TablaEmpleados").insertAfter("#personaltab");
      $(".desaparece").css("display", "none");
      $("#TablaEmpleados").css("display", "table");
      $("#tareadiv").removeClass("col-8").addClass("col-3");
      $("#personaldiv").removeClass("col-4").addClass("col-9");
      $(".aparece").css("display", "table-cell");
      $(".ancho").css("width", "24%");
      $("#thveinte").css("width", "50%");
      $("#titulopersonal").hide();
      ///*       setTimeout(function () {document.getElementById('TituloPersonal').innerHTML = 'Personal | Tarea: ' + tds[1].innerText + ' | ' + tds[3].innerText + ' hasta ' + tds[4].innerText}, 500);*/
      LimpiaAsignaciones();
      setTimeout(function () {
        LlenarAsignaciones(), $(".reaparece").css("display", "table-cell");
      }, 801);
      LlenarEntregables();
      LlenarHerramientas();
      LlenarEpp();
      getBitacora();
      LlenarTransferencias();
      LlenarEncargadosTransferencia();
      LlenarEyS();
    });
    $(document).on("click", "#BtnEditarTarea", function () {
      Origen = "Editar";
      $("#nuevaTareaForm").attr("action", "/Tareas/EditaTarea/");
      var row = $(this).closest("tr"); // Finds the closest row <tr>
      var tds = row.find("td");
      IDTarea = $(tds[0]).text();
      $("#iDTareaNuevaTarea").val(IDTarea);
      $("#IDTareaEntregable").val(IDTarea);
      $.ajax({
        url: "/Tareas/CargarTarea/",
        type: "POST",
        async: true,
        dataType: "text",
        data: {
          idtarea: IDTarea,
        },
        success: function (data) {
          data = JSON.parse(data);
          populate($("#nuevaTareaForm"), data);
          $("#nuevaTareaModal").modal("show");
          $("#iniciotarea").val(FormatFecha(data.FechaInicial));
          $("#fintarea").val(FormatFecha(data.FechaFinal));
          document.getElementById("exampleModalLabel").innerHTML =
            "Editar Tarea";
        },
      });
    });
    $(document).on("click", "#BtnEliminarTarea", function () {
      var tds = $(this).closest("tr").find("td");
      IDTarea = tds[0].innerText;
      document.getElementById("TextoEliminar").innerHTML =
        "¿Confirma que desea eliminar " +
        tds[1].innerText +
        " de la lista de tareas?";

      $("#eliminarTareaModal").modal("show");
    });
    $(document).on("click", "#BtnConfirmarEliminarTarea", function () {
      $.ajax({
        url: "/Tareas/EliminaTarea/",
        type: "POST",
        async: true,
        dataType: "text",
        data: {
          idtarea: IDTarea,
        },
        success: function () {
          $("#eliminarTareaModal").modal("hide");
          LimpiaTareas();
          LlenarTareas();
        },
      });
    });
    $(document).on('click', '.progress', function (e) {
        var fila = $(this).closest('tr');
        IDTarea = $(fila).find('td:first').text();
        $('#tareaObjetivo').text($(fila).find('td').eq(1).text());
        $('#progresoObjetivo').text($(fila).find('.progress-bar').text())
       $('#actualizaBitacoraModal').modal('show');
       
   });

    //====================================================================================================================================================================================
    //Asignaciones
    //====================================================================================================================================================================================
    $(document).on("click", "#NuevaAsignacionBtn", function () {
      $("#inicioasignacion,#finasignacion").datetimepicker({
        minDate: fechai,
        maxDate: fechaf,
      });
      $("#nuevaAsignacionModal").modal("show");
      $("#nuevaAsignacionForm").attr("action", "/Programa/CreaAsignacion/");
      $("#asignacionModalHeader").text("Nueva Asignación");
      OrigenAsignacion = "Nueva";
    });
    $(document).on("click", "#CerrarModal", function () {
      $("#nuevaTareaModal").modal("hide");
      $("#nuevaAsignacionModal").modal("hide");
      $("#eliminarTareaModal").modal("hide");
      $("#nuevoEntregableModal").modal("hide");
      LimpiarModalTarea();
    });
    $(document).on("shown.bs.modal", "#nuevaAsignacionModal", function (e) {
      $("#nuevaAsignacionForm").validate({
        rules: {
          idempleado: "required",
          fechainicial: "required",
          fechatermino: {
            required: true,
            remote: {
              url: "/Programa/Disponibilidad",
              type: "post",
              data: {
                fechainicial: function () {
                  return $("#inicioasignacion").val();
                },
                idempleado: function () {
                  return $("#IDEmpleadoTarea").val();
                },
                idtarea: function () {
                  return $("#idtareaNA").val();
                },
                origen: function () {
                  return OrigenAsignacion;
                },
              },
            },
          },
          responsabilidades: "required",
        },
        messages: {
          idempleado: "Debe escoger un empleado",
          fechainicial: "Debe proporcionar una fecha de inicio",
          fechatermino: {
            required: "Debe proporcionar una fecha de termino",
            remote: "Intervalo no disponible",
          },
          responsabilidades:
            "Debe describir las responsabilidades de esta persona",
        },
        submitHandler: function (form) {
          $.ajax({
            url: form.action,
            type: form.method,
            data: $(form).serialize(),
            success: function (response) {
              LimpiaAsignaciones();
              LlenarAsignaciones();
              $("#nuevaAsignacionModal").modal("hide");
              LimpiarModalTarea();
            },
          });
        },
      });
    });
    $(document).on("click", "#BtnEditarAsignacion", function () {
      OrigenAsignacion = "Editar";
      $("#nuevaAsignacionForm").attr("action", "/Programa/EditaAsignacion/");
      var tds = $(this).closest("tr").find("td");
      generaJson($(tds), $("#nuevaAsignacionForm"));
      $("#nuevaAsignacionModal").modal("show");
      $("#asignacionModalHeader").text("Editar Asignación");
    });
    $(document).on("click", "#BtnEliminarAsignacion", function () {
      var tds = $(this).closest("tr").find("td");
      $("#idAsignacionEliminar").val($(tds[0]).text());
      generaJsonTextos($(tds), $("#eliminaAsignacionForm"));
      $("#eliminarAsignacionModal").modal("show");
    });
    $(document).on("click", "#NuevasAsignacionesBtn", function () {
      $("#nuevasAsignacionesForm input[name='FechaInicial']").each(function (
        i,
        item
      ) {
        $(item).val(fechai);
      });
      $("#nuevasAsignacionesForm input[name='FechaTermino']").each(function (
        i,
        item
      ) {
        $(item).val(fechaf);
      });
      $("#nuevasAsignacionesForm input[name='IDTarea']").each(function (
        i,
        item
      ) {
        $(item).val(IDTarea);
      });
      $("#nuevasAsignacionesModal").modal("show");
    });
    $(document).on("click", "#BtnIngresarAsignaciones", function (e) {
      var info = objetoLista($("#tablaNuevasAsignaciones tr:not(:first)"));
      datos = JSON.stringify(info);
      $.ajax({
        type: "POST",
        url: "/Programa/CreaAsignacionesAsync/",
        data: datos,
        async: true,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (response) {
          if (response == true) {
            LimpiaAsignaciones();
            LlenarAsignaciones();
            $("#nuevasAsignacionesModal").modal("hide");
          } else {
            $("#seccionError li").remove();
            $(response).each(function (i, item) {
              var list = "<li>" + item + "</li>";
              $("#seccionError ul").append(list);
            });

            $("#seccionError").removeClass("d-none");
          }
        },
      });
    });
    $(document).on("click", "#nuevasAsignacionesModal", function (e) {
      if ($(e.target).attr("id") != "BtnIngresarAsignaciones") {
        $("#seccionError").addClass("d-none");
      }
    });
    //====================================================================================================================================================================================
    //Entregables
    //====================================================================================================================================================================================
    $(document).on("shown.bs.modal", "#nuevoEntregableModal", function (e) {
      $("#nuevoEntregableForm").validate({
        rules: {
          Entregable: "required",
          Comentarios: "required",
          Instrucciones: "required",
          IDEncargado: "required",
          FechaEntrega: {
            required: true,
            remote: {
              url: "/Tareas/LimitesTarea",
              type: "post",
              data: {
                idtarea: function () {
                  return $("#IDTareaEntregable").val();
                },
              },
            },
          },
          FechaInicial: {
            required: true,
            remote: {
              url: "/Tareas/LimitesTarea",
              type: "post",
              data: {
                idtarea: function () {
                  return $("#IDTareaEntregable").val();
               },
               FechaEntrega:function(){
                return $("#fechainicialentregable").val();
               },
              },
            },
          },
          responsabilidades: "required",
        },
        messages: {
          Entregable: "Debe proporcionar un nombre para el entregable",
          Comentarios: "Debe proporcionar comentarios",
          Instrucciones: "Debe proporcionar instrucciones",
          IDEncargado: "Debe proporcionar un encargado",
          FechaEntrega: {
            required: "Debe proporcionar una fecha de entrega",
            remote: "Intervalo no disponible",
          },
          FechaInicial: {
            required: "Debe proporcionar una fecha de entrega",
            remote: "Intervalo no disponible",
          },
        },
        submitHandler: function (form) {
          //var datos = generaObjeto($('#nuevoEntregableForm').find('.form-control'));
          //    datos['ProductoTerminado'] = $('#ProductoTerminado').is(":checked");

          var datos = generaDataMPT($("#ContenedorMPT").find(".row"));
          $.ajax({
            url: form.action,
            type: "POST",
            async: true,
            data: "{entregable: " + JSON.stringify(datos) + "}",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            traditional: true,
            success: function (e) {
              $("#nuevoEntregableModal").modal("hide");
              $("#btnIngresarEntregable")
                .prop("disabled", false)
                .val("Ingresar");
              LlenarEntregables();
            },
          });
        },
      });
    });
    $(document).on("click", "#nuevoEntregableBtn", function (e) {
      LlenarEncargadosEntregable();
      LlenarMatPT();
      $("#tituloEntregableModal").text("Nuevo Entregable");
      $("#nuevoEntregableForm").attr("action", "/Tareas/NuevoEntregable/");
      $("#ContenedorMPTSuperior").show();
      $("#nuevoEntregableModal").modal("show");
    });
    $(document).on("click", "#nuevosEntregablesBtn", function (e) {
      $("#TablaItemsAFabricar").show();
      $("#mensajenoentregable").hide();
      LlenarItemsFabricar();
      $("#nuevosEntregableModal").modal("toggle");
    });  
    $(document).on("click", ".headerentregables", function (e) {
      $("#TablaItemsAFabricar").find("tr").not(":first").slideToggle();
    });
    $(document).on("click", "#btnIngresarEntregables", function (e) {
      $(this).prop("disabled", true).val("Espere por favor...");
      var datos = JSON.stringify(
        generaData($("#TablaItemsAFabricar").find("tr").not(":first"))
      );
      $.ajax({
        url: "/Tareas/NuevosEntregables/",
        type: "POST",
        async: true,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        data: "{entregables: " + datos + "}",
        success: function (e) {
          LlenarEntregables();
          $("#btnIngresarEntregables").prop("disabled", false).val("Ingresar");
          $("#nuevosEntregableModal").modal("hide");
        },
      });
    });
    $(document).on("click", "#BtnEditarEntregable", function (e) {
      IDEntregable = $(this).closest("tr").find("td:first").text();
      $("#nuevoEntregableForm").attr("action", "/Tareas/EditaEntregableAsync/");
      LlenarEncargadosEntregable();
      $("#tituloEntregableModal").text("Editar Entregable");
      $("#ContenedorMPTSuperior").hide();
      $.ajax({
        url: "/Tareas/CargarEntregableAsync/",
        type: "POST",
        async: true,
        dataType: "text",
        data: {
          id: IDEntregable,
        },
        success: function (data) {
          data = JSON.parse(data);

          populate("#nuevoEntregableForm", data);

          $("#nuevoEntregableModal").modal("show");
        },
      });
    });
    $(document).on("click", "#BtnVerDetalleEntregable", function (e) {
      var IDEntregable = $(this).closest("tr").find("td:first").text();
      $.ajax({
        url: "/Tareas/CargarEntregableAsync/",
        type: "POST",
        async: true,
        dataType: "text",
        data: {
          id: IDEntregable,
        },
        success: function (data) {
          data = JSON.parse(data);

          populatetextos("#infoEntregable", data);
          $("#detalleEntregableModal").modal("show");
        },
      });

      $.ajax({
        url: "/Tareas/CargaMaterialesXEntregableAsync/",
        type: "POST",
        async: true,
        dataType: "text",
        data: {
          identregable: IDEntregable,
        },
        success: function (data) {
          $(".clasedetallematerial:not(:first)").remove();
          data = JSON.parse(data);
          $.each(data, function (i, fila) {
            var row =
              '<div class="row clasedetallematerial filafondoblanco"><div class="col-6">' +
              fila.Item +
              '</div><div class="col-1">' +
              fila.Unidad +
              '</div><div class="col-1">' +
              fila.Cantidad +
              '</div><div class="col-4">' +
              fila.Estado +
              "</div></div>";
            $(".clasedetallematerial:last").after(row);
          });

          $("#detalleEntregableModal").modal("show");
        },
      });
    });
    $(document).on("click", "#BtnEliminarEntregable", function (e) {
      var tds = $(this).closest("tr").find("td");
      IDEntregable = $(tds[0]).text();
      var texto =
        "¿Confirma que desea eliminar " +
        $(tds[1]).text() +
        " de la lista de entregables?";
      $("#TextoEliminarEntregable").html(texto);
      $("#eliminarEntregableModal").modal("show");
    });
    $(document).on("click", "#BtnConfirmarEliminarEntregable", function (e) {
      $.ajax({
        type: "POST",
        url: "/Tareas/EliminaEntregable/",
        data: { id: IDEntregable },
        dataType: "json",
        success: function (response) {
          LlenarEntregables();
        },
      });
      $("#eliminarEntregableModal").modal("hide");
    });
     //====================================================================================================================================================================================
    //Bitácora
    //====================================================================================================================================================================================  
    $(document).on('hidden.bs.modal', '#actualizaBitacoraModal', function (e) {
        $(this).find('.form-control').val('');
    });
    $(document).on('shown.bs.modal', '#actualizaBitacoraModal', function (e) {
        $('#progresoform').validate({
            rules: {
                Progreso: {
                    required: true,
                    max: 100,
                    min: 0,
                    number: true
                },
                Descripcion: "required",
                Fecha: "required"
             
            },

            messages: {
                Progreso: "Debe proporcionar un número mayor que 0 y menor que 100",
                Descripcion: "Debe describir las actividades realizadas",
                Fecha : "Debe proporcionar una Fecha"
            },

            submitHandler: function (form) {
                var jsonData = { IDTarea: IDTarea, Descripcion: $('#bitacoraDescrpTxt').val().toUpperCase(), Progreso: $('#progresotxt').val(), Fecha: $('#fechaActProgTarea').val() };
                jsonData = JSON.stringify(jsonData);
                $.ajax({
                    type: "POST",
                    url: "/Tareas/ActualizarProgresoTareaAsync/",
                    contentType: "application/json; charset=utf-8",
                    data: jsonData,
                    dataType: "json",
                    success: function (response) {
                        $('#actualizaBitacoraModal').modal('hide');
                        getBitacora();
                        LimpiaTareas();
                        LlenarTareas();
                    }
                });
            }
        });



        
    });
    $(document).on('click', '#BtnEditarBitacora', function (e) {
        generaJson($(this).closest('tr').find('td'), $('#editarBitacoraForm'));
        $('#progresoTxtBx').val($('#progresoTxtBx').val().replace("%", ""));
        $('#editarBitacoraModal').modal('show');
    });   
    $(document).on('click','#btnIngresarEditarBitacora', function (e) {
        var jsonData = {
            Progreso: $('#progresoTxtBx').val(),
            Fecha: $('#fechaprogresoTxtBx').val(),
            Descripcion: $('#comentariosBitacoraModal').val().toUpperCase(),
            ID: $('#idEditarBitacora').val()
        };
        jsonData = JSON.stringify(jsonData);
        $.ajax({
            type: "POST",
            url: "/Tareas/editBitacora/",
            contentType: "application/json; charset=utf-8",
            data: jsonData,
            dataType: "json",
            success: function (response) {
                $('#editarBitacoraModal').modal('hide');
                getBitacora();
                $('[data-toggle="tooltip"]').tooltip();
            }
        });
    });
    //====================================================================================================================================================================================
    //Herramientas
    //====================================================================================================================================================================================
    $(document).on("click", "#bntNuevaFilaTools", function (e) {
      var fila =
        '<div class="row click"><div class="col-4 click"><label class="form-label" for="progresotxt">Item</label><input name="IDTarea" type="text" class="form-control d-none" id="idTareaHerramientas" /><input name="Item" type="text" class="form-control" id="itemModalHerramienta" /></div>\
        <div class="col-1 click"><label class="form-label" for="Cantidad">Cant.</label><input name="Cantidad" type="text" class="form-control" id="CantidadHerramientas" /></div>\
        <div class="col-7 click"><label class="form-label" for="progresotxt">Descripción</label><textarea rows="2" name="Descripcion" class="form-control" ></textarea></div></div>';
      $("#herramientasDiv").append(fila);
      $("#herramientasDiv .row").last().find("#itemModalHerramienta").focus();
      $("#herramientasDiv .row")
        .last()
        .find("#idTareaHerramientas")
        .val(IDTarea);
    });
    $(document).on("click", "#nuevaHerramientaBtn", function (e) {
      $("#bntNuevaFilaTools").removeClass("d-none");
      $("#herramientasForm").attr("action", "/Tareas/nuevaStHerramienta/");
      $("#newToolModal").modal("show");
      $("#idTareaHerramientas").val(IDTarea);
      $("#newToolHeader").text(
        "Requerimiento de Herramientas, equipos o vehículos"
      );
    });
    $(document).on("shown.bs.modal", "#newToolModal", function () {
      document.getElementById("itemModalHerramienta").focus();
      $("#herramientasForm").validate({
        rules: {
          Item: "required",
          Descripcion: "required",
        },

        messages: {
          Item: "Debe proporcionar el nombre del Item",
          Descripcion: "Debe describir el Item",
        },

        submitHandler: function (form) {
          var datos = listObjects($("#herramientasForm .row"));
          datos = JSON.stringify(datos);
          $.ajax({
            type: "POST",
            url: form.action,
            contentType: "application/json; charset=utf-8",
            data: datos,
            dataType: "json",
            success: function (response) {
              LlenarHerramientas();
              LlenarEpp();
              $("#newToolModal").modal("hide");
            },
          });
        },
      });
    });
    $(document).on("hidden.bs.modal", "#newToolModal", function () {
      $("#herramientasForm .form-control").val("");
      $("#herramientasDiv .row").not(":first").remove();
    });
    $(document).on("keydown", function (e) {
      if (e.keyCode == 46 && $("#newToolModal").hasClass("show")) {
        $(filaobjetivo).remove();
      } else if (
        e.keyCode == 46 &&
        $("#newTransferenciaModal").hasClass("show")
      ) {
        $(filaobjetivo).remove();
      }
    });
    $(document).on("click", "#BtnEditarHerramienta", function (e) {
      var fila = $(this).closest("tr").find("td");
      $("#herramientasForm").attr("action", "/Tareas/editaStHerramienta/");
      generaJson(fila, $("#herramientasForm"));
      $("#newToolModal").modal("show");
      $("#bntNuevaFilaTools").addClass("d-none");
      $("#newToolHeader").text(
        "Requerimiento de Herramientas, equipos o vehículos"
      );
    });
    $(document).on("click", "#BtnEliminarHerramienta", function (e) {
      var tds = $(this).closest("tr").find("td");
      IDHerramienta = $(tds[0]).text();
      var texto =
        "¿Confirma que desea eliminar " + $(tds[1]).text() + " de la lista?";
      $("#TextoEliminarHerramienta").html(texto);
      $("#eliminarHerramientaModal").modal("show");
      ToolEpp = 0;
    });
    $(document).on("click", "#BtnConfirmarEliminarHerramienta", function (e) {
      if (ToolEpp == 0) {
        $.ajax({
          type: "POST",
          url: "/Tareas/eliminaStHerramienta/",
          data: { id: IDHerramienta },
          dataType: "json",
          success: function (response) {
            LlenarHerramientas();
            $("#eliminarHerramientaModal").modal("hide");
          },
        });
      } else if (ToolEpp == 1) {
        $.ajax({
          type: "POST",
          url: "/Tareas/eliminaStEpp/",
          data: { id: IDHerramienta },
          dataType: "json",
          success: function (response) {
            LlenarEpp();
            $("#eliminarHerramientaModal").modal("hide");
          },
        });
      } else {
        $.ajax({
          type: "POST",
          url: "/Tareas/eliminaStDinero/",
          data: { id: IDTransferencia },
          dataType: "json",
          success: function (response) {
            LlenarEpp();
            $("#eliminarHerramientaModal").modal("hide");
          },
        });
      }
    });
    $(document).on("click", ".click", function (e) {
      if (e.target === this) {
        filaobjetivo = $(this).closest(".row");
        $(".click")
          .removeClass("filanewtool")
          .find("label")
          .removeClass("etiquetatools");
        filaobjetivo
          .addClass("filanewtool")
          .find("label")
          .addClass("etiquetatools");
      }
    });
    $(document).on("click", ".modal", function (e) {
      if (!$(e.target).hasClass("click")) {
        $(".click")
          .removeClass("filanewtool")
          .find("label")
          .removeClass("etiquetatools");
      }
    });
    //====================================================================================================================================================================================
    //Transferencias
    //====================================================================================================================================================================================
    $(document).on("hidden.bs.modal", "#newTransferenciaModal", function () {
      $("#transferenciasForm .form-control").val("");
      $("#transferenciasDiv .row").not(":first").remove();
    });
    $(document).on("click", "#nuevaTransferenciaBtn", function (e) {
      $("#bntNuevaFilaTransferencias").removeClass("d-none");
      $("#transferenciasForm").attr("action", "/Tareas/nuevaStDinero/");
      LlenarEncargadosTransferencia();
      $("#newTransferenciaModal").modal("show");
      $("#idTareaTransferencia").val(IDTarea);
      $("#newTransferenciaHeader").text(
        "Requerimientos de Transferencias de fondos"
      );
    });
    $(document).on("shown.bs.modal", "#newTransferenciaModal", function () {
      document.getElementById("itemModalHerramienta").focus();
      $("#transferenciasForm").validate({
        rules: {
          Monto: { required: true, number: true },
          Descripcion: "required",
        },

        messages: {
          Monto: "Debe proporcionar el monto a transferir",
          Descripcion: "Debe describir el propósito de la transferencia",
        },

        submitHandler: function (form) {
          var datos = listObjects($("#transferenciasForm .row"));
          datos = JSON.stringify(datos);
          $.ajax({
            type: "POST",
            url: form.action,
            contentType: "application/json; charset=utf-8",
            data: datos,
            dataType: "json",
            success: function (response) {
              LlenarTransferencias();
              $("#newTransferenciaModal").modal("hide");
            },
          });
        },
      });
    });
    $(document).on("click", "#BtnEditarTransferencia", function (e) {
      var fila = $(this).closest("tr").find("td");

      $("#transferenciasForm").attr("action", "/Tareas/editaStDinero/");
      generaJson($(fila), $("#transferenciasForm"));
      $("#newTransferenciaModal").modal("show");
      $("#bntNuevaFilaTransferencias").addClass("d-none");
      $("#newTransferenciaHeader").text(
        "Requerimientos de Transferencias de fondos"
      );
    });
    $(document).on("click", "#BtnEliminarTransferencia", function (e) {
      var tds = $(this).closest("tr").find("td");
      IDTransferencia = $(tds[0]).text();
      var texto = "¿Confirma que desea eliminar este elemento de la lista?";
      $("#TextoEliminarHerramienta").html(texto);
      $("#eliminarHerramientaModal").modal("show");
      ToolEpp = 2;
    });
    $(document).on("click", "#bntNuevaFilaTransferencias", function (e) {
      var fila =
        '<div class="row click"><div class="col-4 click"><label class="form-label" for="IDEmpleado">Empleado</label><select id="iDEmpleadoTransferencias" name="IDEmpleado" class="form-control"><option value="0">-Seleccionar-</option></select></div>\
        <div class="col-2 click"><label class="form-label" for="montoModalHerramienta">Monto</label><input name="IDTarea" type="text" class="form-control d-none" id="idTareaTransferencia" /><input name="Monto" type="text" class="form-control" id="montoModalHerramienta" /></div>\
        <div class="col-6 click"><label class="form-label" for="progresotxt">Descripción</label><textarea rows="2" name="Descripcion" class="form-control" ></textarea></div></div>';
      $("#transferenciasDiv").append(fila);
      $("#transferenciasDiv .row")
        .last()
        .find("#iDEmpleadoTransferencias")
        .focus();
      $("#transferenciasDiv .row")
        .last()
        .find("#idTareaTransferencia")
        .val(IDTarea);
      LlenarEncargadosTransferencia();
    });
    //====================================================================================================================================================================================
    //Epp
    //====================================================================================================================================================================================
    $(document).on("click", "#BtnEditarEpp", function (e) {
      var fila = $(this).closest("tr").find("td");
      $("#herramientasForm").attr("action", "/Tareas/editaStEpp/");
      generaJson(fila, $("#herramientasForm"));
      $("#newToolModal").modal("show");
      $("#bntNuevaFilaTools").addClass("d-none");
      $("#newToolHeader").text(
        "Requerimientos de elementos de protección personal"
      );
    });
    $(document).on("click", "#BtnEliminarEpp", function (e) {
      var tds = $(this).closest("tr").find("td");
      IDHerramienta = $(tds[0]).text();
      var texto =
        "¿Confirma que desea eliminar " + $(tds[1]).text() + " de la lista?";
      $("#TextoEliminarHerramienta").html(texto);
      $("#eliminarHerramientaModal").modal("show");
      ToolEpp = 1;
    });
    $(document).on("click", "#nuevoEppBtn", function (e) {
      $("#bntNuevaFilaTools").removeClass("d-none");
      $("#herramientasForm").attr("action", "/Tareas/nuevaStEpp/");
      $("#newToolModal").modal("show");
      $("#idTareaHerramientas").val(IDTarea);
      $("#newToolHeader").text(
        "Requerimientos de elementos de protección personal"
      );
    });
    //====================================================================================================================================================================================
    //Equipos y Servicios externos
    //====================================================================================================================================================================================  
    $(document).on('click','#btnIngresarEyS' , function (e) {
        
      var info =  objetoLista($('#tablaNuevosEyS tr:not(:first)'));
      if (info["Tipo"] == true) {
        $("#modalEySErrorMsgList li").remove();

        $(info["Errores"]).each(function (i, item) {
          var list = "<li>" + item + "</li>";
          $("#modalEySErrorMsgList ul").append(list);
        });
    
         $('#modalEySErrorMsgList').removeClass('d-none');

      }
      else{
        $.ajax({
            type: "POST",
            url: "/Tareas/nuevaStEyS/",
            data: JSON.stringify(info),
            async: true,
            contentType: "application/json; charset=utf-8",
            dataType: "json",   
            success: function (response) {
                if (response==false) {
                    $('#modalEySErrorMsg').removeClass('d-none');
                }
                else{
                    LlenarEyS();
                    $('#nuevosEySModal').modal('hide');
                }
               
                
    
            }
          });
      }
     
    });
    $(document).on('click','#nuevoEySBtn', function (e) {
        $('#nuevosEySModal').modal('show');
        $('#tablaNuevosEyS input[name="IDTarea"]').each(function(i,item){$(item).val(IDTarea)});

    });
    $(document).on("click", "#nuevosEySModal", function (e) {
        if ($(e.target).attr("id") != "btnIngresarEyS") {
          $("#modalEySErrorMsg,#modalEySErrorMsgList").addClass("d-none");
        }
      });
    $(document).on('click','#btnEditarEys', function () {
      var fila = $(this).closest("tr").find("td");
      $('#editarEySModal .modal-body').load("/Tareas/editaStEySView form", function(e){
        generaJson(fila, $("#editarEySModal form"));
        $('#editarEySModal').modal('show');
      });
      
    });  
    $(document).on('shown.bs.modal','#editarEySModal', function () {
      $(this).find('form').validate({
        rules: {
          Cantidad: { required: true, number: true }
        },

        messages: {
          Cantidad: "Debe proporcionar la cantidad"
          
        },

        submitHandler: function (form) {
          var datos = objeto($(form));
          datos = JSON.stringify(datos);
          $.ajax({
            type: "POST",
            url: form.action,
            contentType: "application/json; charset=utf-8",
            data: datos,
            dataType: "json",
            success: function (response) {
              LlenarEyS();
              $("#editarEySModal").modal("hide");
            },
          });
        },
      });
    });
    $(document).on('click','#btnEliminarEyS', function () {
     var fila = $(this).closest('tr');
      IDEyS = $(fila).find('td[name="ID"]').text();
     $('#editarEySModal .modal-body').load("/Tareas/eliminaStEyS", function(e){
      $('#textoEliminarEyS').text($(fila).find('td[name="Item"]').text());
      $('#editarEySModal').modal('show');
    });
    });
    $(document).on('click','#btnConfirmarElimnarEyS', function () {
     $.ajax({
       type: "POST",
       url: "/Tareas/eliminaStEyS/",
       data: {id:IDEyS},
       dataType: "json",
       success: function (response) {         
         LlenarEyS();
         $('#editarEySModal').modal('hide');
       }
     });
    });
    //====================================================================================================================================================================================
    //Colaciones
    //====================================================================================================================================================================================  
    $(document).on("click", "#nuevaColacionBtn", function (e) {
        e.preventDefault();
        var wWidth = $(window).width();
        var dWidth = wWidth * 0.9;
        $('#dialogodiv').load("/Transaccion/ingresoColaciones?idservicio=" + $("#ID").val() + " #contenedorform", function () {
            $('#dialogodiv').removeClass('d-none').dialog({ width: dWidth, title: "Ingresar Colaciones", height: 'auto', position: { my: 'top', at: 'top+100' },modal:true });
            $('.ui-dialog-titlebar-close').addClass('btn-close');
            $(function () {
                $(".date").datepicker({
                    dateFormat: 'dd/mm/yy'
                });
               
            });
        });
    });
     //====================================================================================================================================================================================
    //Viajes
    //====================================================================================================================================================================================  

    $(document).on("click", "#nuevoViajeBtn", function (e) {
        e.preventDefault();
        var wWidth = $(window).width();
        var dWidth = wWidth * 0.9;
        $('#dialogodiv').load("/Transaccion/IngresoViajes?idservicio=" + $("#ID").val() + " #contenedorform", function () {
            $('#dialogodiv').removeClass('d-none').dialog({ width: dWidth, title: "Ingresar Movilización", height: 'auto', position: { my: 'top', at: 'top+100' }, modal: true });
            $('.ui-dialog-titlebar-close').addClass('btn-close');
            $(function () {
                $(".date").datepicker({
                    dateFormat: 'dd/mm/yy'
                });
                
            });
            $('#formularioViaje').validate({
                rules: {
                    IDCECO: "required",
                    Fecha: "required",
                    IDCuentaCR: "required",
                    MontoPeaje: "required",
                    MontoCombustible: "required"
                },
                messages: {
                    IDCECO: "Debe seleccionar un Centro de Costos",
                    Fecha: "Debe seleccionar una fecha",
                    IDCuentaCR: "Debe seleccionar un Medio de Pago",
                    MontoPeaje: "Debe ingresar un monto (puede ser igual a cero)",
                    MontoCombustible: "Debe ingresar un monto"
                },
                submitHandler: function (form) {
                    var mpeaje = $('#MontoPeaje').val();
                    var mcombustible = $('#MontoCombustible').val();
                    var fecha = $('#Fecha').val();
                    var idcr = $('#IDCuentaCR').val();
                    var idceco = $('#IDCECO').val();
                    var modelo = [];
                    var transacciona = { Item: "PEAJE", Monto: mpeaje, Cantidad: "1", Fecha: fecha, IDCuentaCR: idcr, IDCuentaDB: "8", IDCECO: idceco };
                    var transaccionb = { Item: "COMBUSTIBLE", Monto: mcombustible, Cantidad: "1", Fecha: fecha, IDCuentaCR: idcr, IDCuentaDB: "8", IDCECO: idceco };
                    modelo.push(transacciona, transaccionb);
                    $.ajax({
                        type: "POST",
                        url: form.action,
                        contentType: "application/json; charset=utf-8",
                        data: JSON.stringify(modelo),
                        dataType: "json",
                        success: function (response) {
                            location.reload();
                        },
                    });





                }
            });
        });
    });

    //========================================================================================================
    //HH
    //========================================================================================================
    $(function () {
        $("#fechaHH").datepicker({
            showOn: 'button',
            buttonImageOnly: true,
            buttonImage: '/Content/Pictures/calendario.png',
            dateFormat: 'dd/mm/yy',            
            onSelect: function (dateText) {
                var datearray = dateText.split("/");
                var newdate = datearray[1] + '/' + datearray[0] + '/' + datearray[2];
                const myUrlWithParams = new URL(window.location.origin +"/TimeSheet/NuevoTimeSheetAsync/");
                myUrlWithParams.searchParams.append("idtarea", IDTarea);
                myUrlWithParams.searchParams.append("fecha", newdate);
                $('#dialogodiv').load(myUrlWithParams + ' #formulario', function () {
                    $('#dialogodiv').removeClass('d-none').dialog({ width: 'auto', title: "Registro de HH", height: 'auto', position: { my: 'top', at: 'top+100' }, modal: true })
                    $('.ui-dialog-titlebar-close').addClass('btn-close');
                    $(function () {
                        $(".date").datetimepicker({
                            format: "d-m-Y H:i",
                            formatTime: "H:i",
                            formatDate: "d-m-Y",
                            step: 15,
                            hours12: false,
                            dayOfWeekStart: 1


                        });
                        $.datetimepicker.setLocale("es");
                    });
                });
                    
                /*window.open(myUrlWithParams);*/
            }
        });
        $.datepicker.setDefaults($.datepicker.regional['es']);
    });
    $(function () {
        $('.ui-datepicker-trigger').data('bs-toggle', 'tooltip').data('bs-placement', 'top').attr('title', 'Registrar HH para esta tarea').tooltip()
    });
  
    $(document).on('change', "input[name='Entrada'],input[name='Salida']", function () {

        var row = $(this).closest("tr");
        var a = row.find("input[name='Entrada']").val();
        var b = row.find("input[name='Salida']").val();



        if (a !== "" && b !== "") {

            var q = row.find("input[name='Entrada']").datetimepicker('getValue');
            var p = row.find("input[name='Salida']").datetimepicker('getValue');
            var diff = (p - q) / 36e5;
            row.find("input[name='Total']").val(diff - 0.5);
        }



    });
    $(document).on('click', '#btnEnviar', function (e) {
        e.preventDefault();
        var modelo = [];
        $('#tablaHoras tr:not(:first)').each(function (i, fila) {
            var hh = {};
            $(fila).find('input').each(function (j, item) {
                var key = $(item).attr('name');
                hh[key] = $(item).val();
            });
            modelo.push(hh);
        });
        $.ajax({
            type: "POST",
            url: "/TimeSheet/NuevoTimeSheetAsync/",
            data: JSON.stringify(modelo),
            dataType: "json",
            contentType: "application/json; charset=utf-8",
            success: function (response) {
                location.reload();
            }
        });
    });    
    $(document).on('click', '.resalteHH', function (e) {
        $(this).css({ 'background-color': 'gold','color':'white'});

        $('.resalteHH').not(this).each(function (i, row) {
            var fila = $(row).closest('tr');
            fila.css({ 'background-color': '', 'color': '' });
        });
        filaseleccionada = $(this).closest('tr');
        
    });
    $(function () {
        $(document).keyup(function (e) {

            if (e.keyCode == 46 && filaseleccionada !== undefined && filaseleccionada.index()) {

                filaseleccionada.remove();
                
            }

        });
    });
    $(document).on('click', '#btnEditarTimeSheet', function () {
        var idtimesheet = $(this).data('id');
        const myUrlWithParams = new URL(window.location.origin + "/TimeSheet/EditarTimeSheetServicio/");
        myUrlWithParams.searchParams.append("id", idtimesheet);
        myUrlWithParams.searchParams.append("idservicio", $('#ID').val());
        $('#dialogodiv').load(myUrlWithParams + ' #contenedorform', function () {
            $('#dialogodiv').removeClass('d-none').dialog({ width: 'auto', title: "Editar Registro de HH", height: 'auto', position: { my: 'top', at: 'top+100' }, modal: true })
            $('.ui-dialog-titlebar-close').addClass('btn-close');
            $(function () {
                $(".date").datetimepicker({
                    format: "d-m-Y H:i",
                    formatTime: "H:i",
                    formatDate: "d-m-Y",
                    step: 15,
                    hours12: false,
                    dayOfWeekStart: 1


                });
                $.datetimepicker.setLocale("es");
            });
        })
    });
    //Filtro
    $(function () {
        $("#fechaFiltroHH").datepicker({
            showOn: 'button',
            buttonImageOnly: true,
            buttonImage: '/Content/Pictures/calendario.png',
            dateFormat: 'dd/mm/yy',
            onSelect: function (dateText) {               
                var datearray = dateText.split("/");
                var newdate = new Date(parseInt(datearray[2],10), parseInt(datearray[1] - 1,10) , parseInt(datearray[0],10));
                filtrarTimeSheet(newdate.toISOString());
             
            }
        });
        $.datepicker.setDefaults($.datepicker.regional['es']);
    });
    $(function () {
        $('.ui-datepicker-trigger:eq(1)').data('bs-toggle', 'tooltip').data('bs-placement', 'top').attr('title', 'Filtrar por día').tooltip()
    });

    $(document).on('keyup', '#filtroEmpleadoHH', function (e) {
        var texto = $(this).val();
        $.ajax({
            type: "POST",
            url: "/TimeSheet/filtrarTimeSheetByNombreAsync/",
            data: { idservicio: $('#ID').val(), texto: texto },
            dataType: "json",
            success: function (data) {
                $('#tablaGastoHH tr:not(:first)').remove();
                $(data).each(function (i, item) {
                    var row = '<tr class="tdprograma resaltado"><td>' + FormatFechaST(item.FechaDocumento) + '</td><td>' + item.Empleado + '</td><td>' + item.HorasTotales + '</td><td>' + FormateaPeso(item.ValorHH) + '</td><td><button data-bs-toggle="tooltip" data-placement="top" type="button" Title="Editar Registro" class="btn btn-success fuente"  id="btnEditarTimeSheet" data-id="' + item.ID + '"><img style="heigh:16px;width:16px" src="/Content/Pictures/editar.png" /></button></td></tr>'
                    $('#tablaGastoHH').append(row);
                });
                $('[data-bs-toggle="tooltip"]').tooltip({ trigger: "hover" });
            }
        });



    });
});




   //====================================================================================================================================================================================
    //Funciones
    //====================================================================================================================================================================================
    function LimpiaTareas() {
      $(document).ready(function (e) {
        $("#TablaTareas").find("tr:not(:first)").remove();
      });
    }
    function LimpiarEmpleados() {
      $(document).ready(function (e) {
        $("#TablaEmpleados").find("tr:not(:first)").remove();
      });
    }
    function LimpiaAsignaciones() {
      $(document).ready(function (e) {
        $("#TablaEmpleados").find("tr:not(:first)").remove();
      });
    }
    async function LlenarTareas() {
      $.ajax({
        url: "/Tareas/ListadoTareasAsync/",
        type: "POST",
        async: true,
        dataType: "text",
        data: {
          idservicio: $("#ID").val(),
        },
        success: function (data) {
          data = JSON.parse(data);
          JsonTareas = data;
          $(data).each(function (i, item) {
            if (Detalle) {
              var row = $(
                '<tr  class="resaltado tdprograma"><td  style="Display:none" class="divOne">' +
                  item.ID +
                  "</td><td>" +
                  item.Nombre +
                  '</td>\
                            <td style="display:none" class="desaparece">' +
                  item.Encargado +
                  '</td>\
                            <td style="display:none" class="desaparece">' +
                  FormatFecha(item.FechaInicial) +
                  '</td>\
                            <td style="display:none" class="desaparece">' +
                  FormatFecha(item.FechaFinal) +
                  '</td>\
                            <td style="display:none" class="desaparece">\
                                <div data-bs-toggle="tooltip" data-placement="top" Title="'+ item.Progreso +'%" class="progress"><div class="progress-bar bg-success" role = "progressbar" style = "width: ' +
                  item.Progreso +
                  '%;" aria - valuenow="' +
                  item.Progreso +
                  '" aria - valuemin="0" aria - valuemax="100" > ' +
                  item.Progreso +
                  ' %</div>\
                                </div>\
                            </td>\
                            <td>\
                                <div class="d-flex justify-content-around">\
                                 <button id="BtnEditarTarea" data-bs-toggle="tooltip" data-placement="top" type="button" Title="Editar Tarea" class="btn btn-success fuente">\
                                     <img style="heigh:16px;width:16px" src="/Content/Pictures/editar.png" /></button>\
                                 <button data-bs-toggle="tooltip" data-placement="top" id="BtnEliminarTarea" Title="Eliminar Tarea" type="button"  class="btn btn-danger fuente">\
                                      <img style="heigh:16px;width:16px" src="/Content/Pictures/delete.png" /></button>\
                                   <button data-bs-toggle="tooltip" data-placement="top" Title="Detalles" id="BtnDetalleTarea" type="button"  class="btn btn-primary fuente">\
                                      <img src="/Content/Pictures/detail.png" style="heigh:16px;width:16px" />\
                                  </button>\
                                </div>\
                            </td>\
                        </tr>'
              );
            } else {
              var row = $(
                '<tr class="resaltado tdprograma">\
                                            <td style="Display:none" class="divOne">' +
                  item.ID +
                  "</td>\
                                            <td>" +
                  item.Nombre +
                  '</td>\
                                            <td class="desaparece">' +
                  item.Encargado +
                  '</td>\
                                            <td class="desaparece">' +
                  FormatFecha(item.FechaInicial) +
                  '</td>\
                                            <td class="desaparece">' +
                  FormatFecha(item.FechaFinal) +
                  '</td>\
                                            <td class="desaparece">\
                                                <div data-bs-toggle="tooltip" data-placement="top" Title="'+ item.Progreso +'%" class="progress">\
                                                    <div class="progress-bar bg-success" role = "progressbar" style = "width: ' +
                  item.Progreso +
                  '%;" aria - valuenow="' +
                  item.Progreso +
                  '" aria - valuemin="0" aria - valuemax="100" > ' +
                  item.Progreso +
                  ' %</div>\
                                                </div >\
                                            </td>\
                                            <td>\
                                                <div class="d-flex justify-content-around">\
                                                    <button id="BtnEditarTarea" data-bs-toggle="tooltip" data-placement="top" type="button" Title="Editar Tarea" class="btn btn-success fuente" >\
                                                        <img style="heigh:16px;width:16px" src="/Content/Pictures/editar.png" />\
                                                    </button>\
                                                    <button data-bs-toggle="tooltip" data-placement="top" id="BtnEliminarTarea" Title="Eliminar Tarea" type="button"  class="btn btn-danger fuente">\
                                                        <img style="heigh:16px;width:16px" src="/Content/Pictures/delete.png" />\
                                                    </button>\
                                                    <button data-bs-toggle="tooltip" data-placement="top" Title="Detalles" id="BtnDetalleTarea" type="button"  class="btn btn-primary fuente">\
                                                        <img src="/Content/Pictures/detail.png" style="heigh:16px;width:16px" />\
                                                    </button>\
                                                </div>\
                                            </td>\
                                            </tr>'
              );
            }
            $("#TablaTareas").append(row);
            $('[data-bs-toggle="tooltip"]').tooltip({ trigger: "hover" });
          });
        },
      });
    }
    async function LlenarEmpleados() {
      $.ajax({
        url: "/Programa/EmpleadosEnServicioAsync/",
        type: "POST",
        async: true,
        dataType: "text",
        data: {
          idservicio: $("#ID").val(),
        },
        success: function (data) {
          data = JSON.parse(data);
          $(data).each(function (i, item) {
            var row = $(
              '<tr class="resaltado tdprograma">\
                                         <td style="Display:none" class="divOne">' +
                item.IDEmpleado +
                "</td>\
                                         <td>" +
                item.Empleado +
                '</td >\
                                         <td class="desaparece">' +
                item.Especialidad +
                "</td>\
                                    </tr>"
            );
            $("#TablaEmpleados").append(row);
          });
        },
      });
    }
    function LimpiarModalTarea() {
      $(".form-control").val("");
    }
    async function LlenarAsignaciones() {
      $(document).ready(function () {
        $.ajax({
          url: "/Tareas/EmpleadosPorTarea/",
          type: "POST",
          async: true,
          dataType: "text",
          data: {
            idtarea: IDTarea,
          },
          success: function (data) {
            $("#TablaEmpleados").find("tr:not(:first)").remove();
            data = JSON.parse(data);
            $(data).each(function (i, item) {
              var row = $(
                '<tr class="resaltado tdprograma">\
                        <td name="id" style="Display:none" class="divOne">' +
                  item.ID +
                  '</td>\
                        <td name="idempleado" style="Display:none">' +
                  item.IDEmpleado +
                  '</td>\
                        <td name="empleado">' +
                  item.Empleado +
                  "</td>\
                        <td>" +
                  item.Especialidad +
                  '</td>\
                        <td name="fechainicial">' +
                  FormatFecha(item.FechaInicial) +
                  '</td>\
                        <td name="fechatermino">' +
                  FormatFecha(item.FechaTermino) +
                  '</td>\
                        <td name="responsabilidades">' +
                  isNull(item.Responsabilidades) +
                  '</td>\
                        <td>\
                            <div class="d-flex justify-content-around">\
                                <button id="BtnEditarAsignacion" data-bs-toggle="tooltip" data-placement="top" type="button" Title="Editar Asignación" class="btn btn-success fuente">\
                                    <img style="heigh:16px;width:16px" src="/Content/Pictures/editar.png" />\
                                </button>\
                                <button data-bs-toggle="tooltip" data-placement="top" id="BtnEliminarAsignacion" Title="Eliminar Asignación" type="button"  class="btn btn-danger fuente">\
                                    <img style="heigh:16px;width:16px" src="/Content/Pictures/delete.png" />\
                                </button>\
                            </div>\
                        </td></tr>'
              );

              $("#TablaEmpleados").append(row);
            });

            $('[data-bs-toggle="tooltip"]').tooltip({ trigger: "hover" });
          },
        });
      });
    }
    async function LlenarPresupuesto() {
      $.ajax({
        url: "/Servicios/ObtenerPptoVsCostosAsync/",
        type: "POST",
        async: true,
        dataType: "text",
        data: {
          idcotizacion: $("#IDCotizacion").val(),
        },
        success: function (data) {
          data = JSON.parse(data);
          PptoGlobal = data;
          //var row = $('<tr id="filamaterial" class="resaltado tdprograma"><td> Materiales </td><td>' + FormateaPeso(data.PresupuestoMateriales) + '</td><td>' + FormateaPeso(data.CostoMaterial) + '</td></tr><tr id="filaequipo" class="resaltado tdprograma"><td>Equipos y Servicios</td><td>' + FormateaPeso(data.PresupuestoEquiposServicios) + '</td><td>' + FormateaPeso(data.CostoEquiposServicios) + '</td></tr><tr id="filapersonal" class="resaltado tdprograma"><td>Personal</td><td>' + FormateaPeso(data.PresupuestoPersonal) + '</td><td>' + FormateaPeso(data.CostoPersonal) + '</td></tr>');
          //   /* $('#TablaCostos').append(row);*/

          var ctx = document.getElementById("myChart").getContext("2d");
          var myChart = new Chart(ctx, {
            type: "bar",
            data: {
              labels: ["Materiales", "Personal", "Equipos y Servicios", "Gastos Generales"],
              datasets: [
                {
                  label: "Gasto",
                  backgroundColor: "rgba(8, 0, 168, 1)",
                      data: [
                          PptoGlobal.CostoMaterial,
                          PptoGlobal.CostoPersonal,
                          PptoGlobal.CostoEquiposServicios,
                          PptoGlobal.CostoGastosGenerales
                  ],
                },
                {
                  label: "Presupuesto",
                  backgroundColor: "rgba(255, 99, 132, 1)",
                  data: [
                      PptoGlobal.PresupuestoMateriales,
                      PptoGlobal.PresupuestoPersonal,
                      PptoGlobal.PresupuestoEquiposServicios,
                      PptoGlobal.GastosGenerales
                  ],
                },
              ],
            },
            options: {
              scales: {
                xAxes: [
                  {
                    stacked: true,
                  },
                ],
                yAxes: [
                  {
                    stacked: false,
                    ticks: {
                      callback: function (value, index, values) {
                        return FormateaPeso(value);
                        },
                        beginAtZero: true
                    },
                  },
                ],
              },
              tooltips: {
                callbacks: {
                  label: function (tooltipItem, data) {
                    var label = tooltipItem.value;

                    return FormateaPeso(label);
                  },
                },
              },
              responsive: true,
              maintainAspectRatio: false,
              /*events: ['click']*/
            },
          });

          $(document).on("click", "#myChart", function (evt) {
            var activePoints = myChart.getElementsAtEvent(evt);
            var chartData = activePoints[0]["_chart"].config.data;
            var idx = activePoints[0]["_index"];
            if (idx == 0) {
              LlenarMateriales();
              $("#dialogo").dialog({ height: "auto", width: "auto" });
            }

            /* do something */
          });
          $("canvas").css({ height: "100%", width: "100%" });
        },
      });
    }
    async function LlenarMateriales() {
      $.ajax({
        url: "/CotMaterial/ListadoMaterialesCompletoAsync/",
        type: "POST",
        async: true,
        dataType: "text",
        data: {
          idcot: $("#IDCotizacion").val(),
        },
        success: function (data) {
          data = JSON.parse(data);
          $(data).each(function (i, row) {
            var fila = $(
              '<tr id="filamaterial" class="resaltado tdprograma"><td>' +
                row.Item +
                "</td><td>" +
                row.Unidad +
                "</td><td>" +
                row.Cantidad +
                "</td></tr>"
            );
            $("#TablaDetalladaMateriales").append(fila);
          });
        },
      });
    }
    async function LlenarEntregables() {
      $(document).ready(function () {
        $.ajax({
          url: "/Tareas/LoadEntregablesTarea/",
          type: "POST",
          async: true,
          dataType: "text",
          data: {
            idtarea: IDTarea,
          },
          success: function (data) {
            $("#TablaEntregables").find("tr:not(:first)").remove();
            data = JSON.parse(data);
            $(data).each(function (i, item) {
              var row = $(
                '<tr class="resaltado tdprograma">\
                                        <td name="ID" style="Display:none" class="divOne">' +
                  item.ID +
                  '</td>\
                                        <td name="Entregable">' +
                  item.Entregable +
                  '</td>\
                                        <td name="FechaEntrega">' +
                  FormatFecha(item.FechaEntrega) +
                  '</td>\
                                        <td name="Unidad">' +
                  item.Unidad +
                  '</td>\
                                        <td name="Cantidad">' +
                  item.Cantidad +
                  '</td>\
                                        <td name="Estado">' +
                  item.Estado +
                  '</td>\
                                        <td>\
                                            <div class="d-flex justify-content-around">\
                                                <button id ="BtnVerDetalleEntregable" data-bs-toggle="tooltip" data-placement="top" type="button" Title="Detalles" class="btn btn-primary fuente">\
                                                    <img style="heigh:16px;width:16px" src="/Content/Pictures/list-text.png" />\
                                                </button>\
                                                <button id="BtnEditarEntregable" data-bs-toggle="tooltip" data-placement="top" type="button" Title="Editar Entregable" class="btn btn-success fuente">\
                                                    <img style="heigh:16px;width:16px" src="/Content/Pictures/editar.png"/>\
                                                </button>\
                                                <button data-bs-toggle="tooltip" data-placement="top" id="BtnEliminarEntregable" Title="Eliminar Entregable" type="button" class="btn btn-danger fuente">\
                                                    <img style="heigh:16px;width:16px" src="/Content/Pictures/delete.png" />\
                                                </button>\
                                            </div>\
                                        </td>\
                                    </tr>'
              );

              $("#TablaEntregables").append(row);
            });
          },
        });
        $('[data-bs-toggle="tooltip"]').tooltip({ trigger: "hover" });
      });
    }
    async function LlenarEncargadosEntregable() {
      $("#encargadoEntregableModal").find("option").not(":first").remove();
      $.ajax({
        url: "/Tareas/EmpleadosPorTarea/",
        type: "POST",
        async: true,
        dataType: "text",
        data: {
          idtarea: IDTarea,
        },
        success: function (data) {
          data = JSON.parse(data);
          $(data).each(function (i, item) {
            var seleccion =
              '<option value="' +
              item.IDEmpleado +
              '">' +
              item.Empleado +
              "</option>";

            $("#encargadoEntregableModal").append(seleccion);
          });
        },
      });
    }
    async function LlenarEncargadosTransferencia() {
      $.ajax({
        url: "/Tareas/EmpleadosPorTarea/",
        type: "POST",
        async: true,
        dataType: "text",
        data: {
          idtarea: IDTarea,
        },
        success: function (data) {
          data = JSON.parse(data);
          $("#transferenciasDiv .row:last")
            .find("#iDEmpleadoTransferencias")
            .find("option")
            .not(":first")
            .remove();
          $(data).each(function (i, item) {
            var seleccion =
              '<option value="' +
              item.IDEmpleado +
              '">' +
              item.Empleado +
              "</option>";
            $("#transferenciasDiv .row:last")
              .find("#iDEmpleadoTransferencias")
              .append(seleccion);
          });
        },
      });
    }
    async function LlenarMatPT() {
      $.ajax({
        url: "/Tareas/CargarMaterialesPT/",
        type: "POST",
        async: true,
        dataType: "text",
        data: {
          idcot: $("#IDCotizacion").val(),
          idservicio: $("#ID").val(),
        },
        success: function (data) {
          $("#ContenedorMPT .row:not(:first)").remove();
          data = JSON.parse(data);
          $(data).each(function (i, item) {
            var row =
              '<div class="row">\
                                    <div class="MPT" style="display:none" name="IDCotMaterial_EntregableMaterialPT">' +
              item.IDCotMaterial +
              '</div>\
                                    <div class="MPT" style="display:none" name="IDEntregablePT_EntregableMaterialPT">' +
              item.IDEntregablePT +
              '</div>\
                                    <div class="col-7">' +
              item.Item +
              '</div>\
                                    <div class="col-2">' +
              item.Unidad +
              '</div>\
                                    <div class="col-2">' +
              item.Cantidad +
              '</div>\
                                    <div class="col-1">\
                                        <div class="form-check form-switch">\
                                            <input id="checkboxbuscadoMPT" class="form-check-input deslizado" type="checkbox">\
                                        </div>\
                                    </div>\
                                </div>';
            $("#ContenedorMPT").find(".row:last").after(row);
          });
        },
      });
    }
    function Fila(selector) {
      var row = $(selector).closest("tr"); // Finds the closest row <tr>
      var tds = row.find("td");
      return tds;
    }
    async function LlenarItemsFabricar() {
      $("#TablaItemsAFabricar").find("tr:not(:first)").remove();

      $.ajax({
        url: "/Tareas/LoadItemsAFabricarAsync/",
        type: "POST",
        async: true,
        dataType: "text",
        data: {
          idcot: $("#IDCotizacion").val(),
        },
        success: function (data) {
          data = JSON.parse(data);
          if (data.length == 0) {
            $("#TablaItemsAFabricar").hide();
            $("#mensajenoentregable").show();
          } else {
            $(data).each(function (i, item) {
              var row = $(
                '<tr class="resaltado tdprograma">\
                                            <td style="Display:none" name="IDCotizacion" class="divOne">' +
                  item.IDCotizacion +
                  '</td>\
                                            <td style="Display:none" name="IDTarea">' +
                  IDTarea +
                  '</td>\
                                            <td name="Entregable">' +
                  item.Entregable +
                  '</td>\
                                            <td name="Unidad">' +
                  item.Unidad +
                  '</td>\
                                            <td name="Cantidad">' +
                  item.Cantidad +
                  '</td>\
                                            <td Name="PVenta">' +
                  FormateaPeso(item.PVenta) +
                  '</td>\
                                            <td name="PProduccion">' +
                  FormateaPeso(item.PProduccion) +
                  '</td>\
                                            <td>\
                                                <div class="form-check form-switch">\
                                                    <input id="checkboxbuscado" class="form-check-input deslizado" type="checkbox">\
                                                </div>\
                                            </td>\
                                        </tr>'
              );
              $("#TablaItemsAFabricar").append(row);
            });
          }
        },
      });
    }
    function formatdate(fecha) {
      var date = new Date(fecha);
      return (
        date.getMonth() + 1 + "/" + date.getDate() + "/" + date.getFullYear()
      );
    }
    function getBitacora() {
      $.ajax({
        type: "POST",
        url: "/Tareas/ObtenerBitacora/",
        data: {
          idtarea: IDTarea,
        },
        dataType: "json",
        success: function (data) {
          $("#tablabitacora tr:not(:first-child)").remove();
          $(data).each(function (i, row) {
            var drow =
              '<tr class="tdprograma resaltado"><td name="ID" class="d-none">' +
              row.ID +
              '</td>\
                    <td name="Fecha">' +
                FormatFechaST(row.Fecha) +
                '</td><td name="Progreso">' +
                row.Progreso + '%' +
              '</td><td name="Descripcion">' +
              row.Descripcion +
              '</td>\
                    <td><div class="d-flex justify-content-around">\
                                <button id="BtnEditarBitacora" data-bs-toggle="tooltip" data-placement="top" type="button" Title="Editar Bitacora" class="btn btn-success fuente">\
                                    <img style="heigh:16px;width:16px" src="/Content/Pictures/editar.png" />\
                                </button>\
                                <button data-bs-toggle="tooltip" data-placement="top" id="BtnEliminarBitacora" Title="Eliminar Bitacora" type="button"  class="btn btn-danger fuente">\
                                    <img style="heigh:16px;width:16px" src="/Content/Pictures/delete.png" />\
                                </button>\
                        </div>\
                    </td></tr>';
            $("#tablabitacora").append(drow);
          });
        },
      });
    }
    function LlenarHerramientas() {
      $.ajax({
        type: "POST",
        url: "/Tareas/getTools/",
        data: { idtarea: IDTarea },
        dataType: "json",
        success: function (data) {
          $("#tablaherramientas tr:not(:first-child)").remove();
          $(data).each(function (i, row) {
            var drow =
              '<tr class="tdprograma resaltado">\
                                <td name="ID" class="d-none">' +
              row.ID +
              '</td>\
                                <td name="Item">' +
              row.Item +
              '</td>\
                                <td name="Cantidad">' +
              row.Cantidad +
              '</td>\
                                <td name="Descripcion">' +
              row.Descripcion +
              '</td>\
                                <td name="Estado">' +
              row.Estado +
              '</td>\
                                <td>\
                                    <div class="d-flex justify-content-around">\
                                        <button id="BtnEditarHerramienta" data-bs-toggle="tooltip" data-placement="top" type="button" Title="Editar Requerimiento" class="btn btn-success fuente" >\
                                            <img style="heigh:16px;width:16px" src="/Content/Pictures/editar.png" />\
                                        </button> <button data-bs-toggle="tooltip" data-placement="top" id="BtnEliminarHerramienta" Title="Eliminar Requerimiento" type="button"  class="btn btn-danger fuente">\
                                            <img style="heigh:16px;width:16px" src="/Content/Pictures/delete.png" />\
                                        </button>\
                                    </div>\
                                </td>\
                            </tr>';
            $("#tablaherramientas").append(drow);
          });
        },
      });
    }
    function LlenarEpp() {
      $.ajax({
        type: "POST",
        url: "/Tareas/getEpps/",
        data: { idtarea: IDTarea },
        dataType: "json",
        success: function (data) {
          $("#tablaepp tr:not(:first-child)").remove();
          $(data).each(function (i, row) {
            var drow =
              '<tr class="tdprograma resaltado">\
                                <td name="ID" class="d-none">' +
              row.ID +
              '</td>\
                                <td name="Item">' +
              row.Item +
              '</td>\
                                <td name="Cantidad">' +
              row.Cantidad +
              '</td>\
                                <td name="Descripcion">' +
              row.Descripcion +
              '</td>\
                                <td name="Estado">' +
              row.Estado +
              '</td>\
                                <td>\
                                    <div class="d-flex justify-content-around">\
                                        <button id="BtnEditarEpp" data-bs-toggle="tooltip" data-placement="top" type="button" Title="Editar Requerimiento" class="btn btn-success fuente" >\
                                            <img style="heigh:16px;width:16px" src="/Content/Pictures/editar.png" />\
                                        </button>\
                                        <button data-bs-toggle="tooltip" data-placement="top" id="BtnEliminarEpp" Title="Eliminar Requerimiento" type="button"  class="btn btn-danger fuente">\
                                            <img style="heigh:16px;width:16px" src="/Content/Pictures/delete.png" />\
                                        </button>\
                                    </div>\
                                </td>\
                            </tr>';
            $("#tablaepp").append(drow);
          });
        },
      });
    }
    function LlenarGastos() {
      $.ajax({
        type: "POST",
          url: "/Transaccion/getTransaccionesServicio/",
        data: { idservicio: $("#ID").val() },
        dataType: "json",
        success: function (data) {
          $(data).each(function (i, item) {
            var fila =
              '<tr class="tdprograma resaltado">\
                                <td>' +
              item.Item +
              "</td>\
                                <td>" +
              item.Cantidad +
              "</td>\
                                <td>" +
              FormateaPeso(item.Monto) +
              "</td>\
                                <td>" +
              FormateaPeso(item.Cantidad * item.Monto) +
              "</td>\
                            </tr>";
            $("#tablaGastos").append(fila);
          });
        },
      });
    }
    function LlenarTransferencias() {
      $.ajax({
        type: "POST",
        url: "/Tareas/getSolDinero/",
        data: { idtarea: IDTarea },
        dataType: "json",
        success: function (data) {
          $("#tablaTransferencias tr:not(:first-child)").remove();
          $(data).each(function (i, row) {
            var drow =
              '<tr class="tdprograma resaltado">\
                            <td name="ID" class="d-none">'+ row.ID +'</td>\
                            <td name="IDEmpleado" class="d-none">' + row.IDEmpleado +'</td>\
                            <td name="Empleado">' + row.Empleado +'</td>\
                            <td name="Monto">' +  FormateaPeso(row.Monto) +'</td>\
                            <td name="Descripcion">' + row.Descripcion +'</td>\
                            <td name="Estado">' + row.Estado +'</td>\
                            <td>\
                                <div class="d-flex justify-content-around">\
                                    <button id="BtnEditarTransferencia" data-bs-toggle="tooltip" data-placement="top" type="button" Title="Editar Requerimiento" class="btn btn-success fuente" >\
                                        <img style="heigh:16px;width:16px" src="/Content/Pictures/editar.png" />\
                                    </button>\
                                    <button data-bs-toggle="tooltip" data-placement="top" id="BtnEliminarTransferencia" Title="Eliminar Requerimiento" type="button"  class="btn btn-danger fuente">\
                                        <img style="heigh:16px;width:16px" src="/Content/Pictures/delete.png" />\
                                    </button>\
                                </div>\
                            </td>\
                            </tr>';
            $("#tablaTransferencias").append(drow);
          });
        },
      });
    }
    function isNull(data) {
      if (data === null) {
        return "";
      } else {
        return data;
      }
    }
    function objetoLista(coleccion) {
      var datos = [];
      var errores = [];
      $(coleccion).each(function (i, item) {
        if ($(item).find(".deslizado").is(":checked")) {
          var jsonData = {};
          $(item)
            .find("input,textarea")
            .not(":last")
            .each(function (j, row) {
              var key = $(row).attr("name");
              if ($.trim($(this).text()).substring(0, 1) == "$") {
                jsonData[key] = $(this)
                  .val()
                  .replace(/[^0-9,-]+/g, "");
              } else {
                jsonData[key] = $(this).val();
              }
              if(key=="Cantidad" && !$(row).val()){
                    errores.push("Debe ingresar una cantidad para " + $(item).find('input[name="Item"]').val())
              }
            });
          datos.push(jsonData);
        }
      });
      if (errores.length==0) {
        return datos; 
      }
      else{
          var errorObject = {};
            errorObject["Errores"] = errores;
            errorObject["Tipo"] = true;
          return errorObject;
      }
      
    }
    function objeto(form){
        
          var jsonData = {};
          $(form).find("input,textarea").each(function (j, row) {
              var key = $(row).attr("name");
              if ($.trim($(this).text()).substring(0, 1) == "$") {
                jsonData[key] = $(this)
                  .val()
                  .replace(/[^0-9,-]+/g, "");
              } else {
                jsonData[key] = $(this).val();
              }
             
            });
         
        return jsonData;
     
    }
    function LlenarEyS(){
        $.ajax({
            type: "POST",
            url: "/Tareas/getSolEyS/",
            data: {
                idtarea: IDTarea
            },
            dataType: "json",
            success: function (data) {
                $('#tablaEyS tr:not(:first-child)').remove();
                $(data).each(function (i, row) {
                    var drow =
                      '<tr class="tdprograma resaltado">\
                                    <td name="ID" class="d-none">'+ row.ID +'</td>\
                                    <td name="Item">' + row.Item +'</td>\
                                    <td name="Unidad">' +  row.Unidad +'</td>\
                                    <td name="Cantidad">' + row.Cantidad +'</td>\
                                    <td name="Comentarios">' + isNull(row.Comentarios) +'</td>\
                                    <td name="Estado">' + row.Estado +'</td>\
                                    <td>\
                                        <div class="d-flex justify-content-around">\
                                            <button id="btnEditarEys" data-bs-toggle="tooltip" data-placement="top" type="button" Title="Editar Equipo o Servicio Externo" class="btn btn-success fuente" >\
                                                <img style="heigh:16px;width:16px" src="/Content/Pictures/editar.png" />\
                                            </button>\
                                            <button data-bs-toggle="tooltip" data-placement="top" id="btnEliminarEyS" Title="Eliminar Equipo o Servicio Externo" type="button"  class="btn btn-danger fuente">\
                                                <img style="heigh:16px;width:16px" src="/Content/Pictures/delete.png" />\
                                            </button>\
                                        </div>\
                                    </td>\
                                    </tr>';
                    $("#tablaEyS").append(drow);
                  });
            }
        });
}
function LlenarTimeSheet() {
    $.ajax({
        type: "POST",
        url: "/TimeSheet/getEmpleadosHoursValorHHAsync/",
        data: {idservicio:$('#ID').val()},
        dataType: "json",
        success: function (data) {
            $(data).each(function (i, item) {
                var row = '<tr class="tdprograma resaltado"><td>' + FormatFechaST(item.FechaDocumento) + '</td><td>' + item.Empleado + '</td><td>' + item.HorasTotales + '</td><td>' + FormateaPeso(item.ValorHH) + '</td><td><button data-bs-toggle="tooltip" data-placement="top" type="button" Title="Editar Registro" class="btn btn-success fuente"  id="btnEditarTimeSheet" data-id="' + item.ID +'"><img style="heigh:16px;width:16px" src="/Content/Pictures/editar.png" /></button></td></tr>'
                $('#tablaGastoHH').append(row);
            });
            $('[data-bs-toggle="tooltip"]').tooltip({ trigger: "hover" });
        }
    });

    
}
function filtrarTimeSheet(fechafiltro) {
    $.ajax({
        type: "POST",
        url: "/TimeSheet/filtrarTimeSheetByDayAsync",
        data: {fecha:fechafiltro},
        dataType: "json",
        success: function (data) {
            $('#tablaGastoHH tr:not(:first)').remove();
            $(data).each(function (i, item) {
                var row = '<tr class="tdprograma resaltado"><td>' + FormatFechaST(item.FechaDocumento) + '</td><td>' + item.Empleado + '</td><td>' + item.HorasTotales + '</td><td>' + FormateaPeso(item.ValorHH) + '</td><td><button data-bs-toggle="tooltip" data-placement="top" type="button" Title="Editar Registro" class="btn btn-success fuente"  id="btnEditarTimeSheet" data-id="' + item.ID + '"><img style="heigh:16px;width:16px" src="/Content/Pictures/editar.png" /></button></td></tr>'
                $('#tablaGastoHH').append(row);
            });
            $('[data-bs-toggle="tooltip"]').tooltip({ trigger: "hover" });
        }
    });
}