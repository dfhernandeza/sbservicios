function rut(textbox) {
    
        var largo = (textbox).val().length
        var cadena = "";
        for (var i = 0; i < largo; i++) {
            if ($(textbox).val().substring(i, i + 1) !== '-' && $(textbox).val().substring(i, i + 1) !== '.') {
                cadena = cadena + $(textbox).val().substring(i, i + 1);
            }
        }

        if (cadena == "") {
            return;
        }

   
        if (cadena.length == 2) {
            var a = cadena.substring(0, 1);
            var b = cadena.substring(1, 2);
            var c = a + '.' + b;
            $(textbox).val(c);
            
        }

        if (cadena.length == 5) {
            var a = cadena.substring(0, 1);
            var b = cadena.substring(1, 4);
            var c = cadena.substring(4, 5);
            var d = a + '.' + b + '.' + c;
           $(textbox).val(d);
         
        }

        if (cadena.length == 8) {
            var a = cadena.substring(0, 1);
            var b = cadena.substring(1, 4);
            var c = cadena.substring(4, 7);
            var d = cadena.substring(7, 8);
            var f = a + '.' + b + '.' + c + '-' + d
            $(textbox).val(f)
            
        }

        if (cadena.length == 9) {
            var a = cadena.substring(0, 2);
            var b = cadena.substring(2, 5);
            var c = cadena.substring(5, 8);
            var d = cadena.substring(8, 9);
            var f = a + '.' + b + '.' + c + '-' + d
            $(textbox).val(f)
            
        }




  
};

function GetRut(texto) {

    if (texto.length = 12) {

        var a = texto.substring(0, 2);
        var b = texto.substring(3, 6);
        var c = texto.substring(7, 10);
        var d = texto.substring(11, 12);
        return a + b + c + d;


    }
    else if (texto.length = 11) {

        var a = texto.substring(0, 1);
        var b = texto.substring(2, 5);
        var c = texto.substring(6, 9);
        var d = texto.substring(10, 11);
        return a + b + c + d
    }
    else {
        return texto
    }

}


function formatrut(texto) {

    var largo = texto.length
    var cadena = "";
    for (var i = 0; i < largo; i++) {
        if (texto.substring(i, i + 1) !== '-' && texto.substring(i, i + 1) !== '.') {
            cadena = cadena + texto.substring(i, i + 1);
        }
    }

    if (cadena == "") {
        return;
    }


    if (cadena.length == 2) {
        var a = cadena.substring(0, 1);
        var b = cadena.substring(1, 2);
        var c = a + '.' + b;
        return c;

    }

    if (cadena.length == 5) {
        var a = cadena.substring(0, 1);
        var b = cadena.substring(1, 4);
        var c = cadena.substring(4, 5);
        var d = a + '.' + b + '.' + c;
        return d;

    }

    if (cadena.length == 8) {
        var a = cadena.substring(0, 1);
        var b = cadena.substring(1, 4);
        var c = cadena.substring(4, 7);
        var d = cadena.substring(7, 8);
        var f = a + '.' + b + '.' + c + '-' + d
        return f;

    }

    if (cadena.length == 9) {
        var a = cadena.substring(0, 2);
        var b = cadena.substring(2, 5);
        var c = cadena.substring(5, 8);
        var d = cadena.substring(8, 9);
        var f = a + '.' + b + '.' + c + '-' + d
        return f;

    }


};


function FormateaPeso(valor) {
    return Intl.NumberFormat("es-CL", { style: "currency", currency: "CLP" }).format(valor)
}

function FormatFecha(fecha) {
    var dateString = fecha.substr(6);
    var currentTime = new Date(parseInt(dateString));
    var month = currentTime.getMonth() + 1;
    if (month < 10) {
        month = '0' + month.toString();
    }

    var day = currentTime.getDate();
    if (day < 10) {
        day = '0' + day.toString();
    }
    var year = currentTime.getFullYear();
    var hours = currentTime.getHours();
    var minutes = currentTime.getMinutes();
    if (minutes < 10) {
        minutes = '0' + minutes.toString();
    }
    var date = day + "-" + month + "-" + year + ' ' + hours + ':' + minutes;
    return (date);
}
function FormatFechaST(fecha) {
    var dateString = fecha.substr(6);
    var currentTime = new Date(parseInt(dateString));
    var month = currentTime.getMonth() + 1;
    if (month < 10) {
        month = '0' + month.toString();
    }

    var day = currentTime.getDate();
    if (day < 10) {
        day = '0' + day.toString();
    }
    var year = currentTime.getFullYear();
 
  
    var date = day + "-" + month + "-" + year + ' ';
    return (date);
}
function multiplica(textbox1, textbox2, textbox3) {
    
    var q =  parseFloat($(textbox1).val().replace(',','.'));
    var p = parseFloat($(textbox2).val().replace(',', '.'));
                
        var result = "";
    
        if (q !== "" && p !== "" && $.isNumeric(q) && $.isNumeric(p)) {
            result = parseFloat(q) * parseFloat(p);
        }
        $(textbox3).val(result.toString().replace('.',','));

    
 


}


function dynamicSort(property) {
    var sortOrder = 1;

    if (property[0] === "-") {
        sortOrder = -1;
        property = property.substr(1);
    }

    return function (a, b) {
        if (sortOrder == -1) {
            return b[property].localeCompare(a[property]);
        } else {
            return a[property].localeCompare(b[property]);
        }
    }
}

function GetSortOrder(prop, orden) {
    return function (a, b) {
        if (orden == 'asc') {
            if (a[prop] > b[prop]) {
                return 1;
            } else if (a[prop] < b[prop]) {
                return -1;
            }
            return 0;
        }
        else {
            if (a[prop] < b[prop]) {
                return 1;
            } else if (a[prop] > b[prop]) {
                return -1;
            }
            return 0;
        }
      
    }
} 

function populate(frm, data) {
    $.each(data, function (key, value) {
        if (key == 'FechaEntrega' || key == 'FechaInicial' ) {
            if (value == "/Date(-62135586000000)/") {
                $('[name=' + key + ']', frm).val("");
            }
            else{
                $('[name=' + key + ']', frm).val(FormatFecha(value));
            }
            
        }
        else {
            $('[name=' + key + ']', frm).val(value);
        }
        
    });
}

function generaJson(coleccion,form) {
    var jsonData = {};
    $(coleccion).each(function (i) {
        var key = $(this).attr('name');
        if ($.trim($(this).text()).substring(0, 1) == '$') {
            jsonData[key] = $(this).text().replace(/[^0-9,-]+/g, "")
        }
        else {
            jsonData[key] = $(this).text();
        }
        
    });
   /* return jsonData;*/
    return(populate(form,jsonData))
}

function generaJsonTextos(coleccion, form) {
    var jsonData = {};
    $(coleccion).each(function (i) {
        var key = $(this).attr('name');
        if ($.trim($(this).text()).substring(0, 1) == '$') {
            jsonData[key] = $(this).text().replace(/[^0-9,-]+/g, "")
        }
        else {
            jsonData[key] = $(this).text();
        }

    });
    /* return jsonData;*/
    return (populatetextos(form, jsonData))
}
function populatetextos(frm, data) {
    $.each(data, function (key, value) {
        if (key == 'FechaEntrega') {
            $('[name=' + key + ']', frm).text(FormatFecha(value));
        }
        else {
            $('[name=' + key + ']', frm).text(value);
        }
    });
}
function objeto(coleccion) {
    var jsonData = {};
    coleccion.each(function (i, row) {  
         $(row).children().each(function (i) {
             var key = $(this).attr('name');
             if ($.trim($(this).text()).substring(0, 1) == '$') {
                 jsonData[key] = $(this).val().replace(/[^0-9,-]+/g, "")
             }
             else {
                 jsonData[key] = $(this).val();
             }

         });

    });
    
    return (jsonData)
}

function generaObjeto(coleccion) {
    var jsonData = {};
    coleccion.each(function (i, item) {
        
            var key = $(item).attr('name');
            if ($.trim($(this).text()).substring(0, 1) == '$') {
                jsonData[key] = $(this).val().replace(/[^0-9,-]+/g, "")
            }
            else {
                jsonData[key] = $(this).val();
            }

    });

    return (jsonData)
}

function generaData(coleccion) {
    var datos = [];
    
    coleccion.each(function (i, item) {
        if ($(item).find('#checkboxbuscado').is(':checked')) {
            var jsonData = {};
            $(item).find('td').not(':last').each(function (j, row) {

                var key = $(row).attr('name');
                if ($.trim($(this).text()).substring(0, 1) == '$') {
                    jsonData[key] = $(this).text().replace(/[^0-9,-]+/g, "")
                }
                else {
                    jsonData[key] = $(this).text();
                }

               
            });
            datos.push(jsonData);
        }
      

    });

    
    return (datos);
}

function generaDataMPT(coleccion) {
    var datos = [];
    var dates = generaObjeto($('#nuevoEntregableForm').find('.form-control'));
    dates['ProductoTerminado'] = $('#ProductoTerminado').is(":checked");
    coleccion.each(function (i, item) {
        if ($(item).find('#checkboxbuscadoMPT').is(':checked')) {
            var jsonData = {};
            $(item).find('.MPT').each(function (j, row) {

                var key = $(row).attr('name');
                if ($.trim($(this).text()).substring(0, 1) == '$') {
                    jsonData[key] = $(this).text().replace(/[^0-9,-]+/g, "")
                }
                else {
                    jsonData[key] = $(this).text();
                }


            });
            datos.push(jsonData);
        }
    });


    

    dates['MaterialesEntregable'] = datos;

    return (dates);
}

function listObjects(coleccion) {
    var list = [];
    coleccion.each(function (i, row) {
        var jsonData = {};
        $(row).find('input, textarea, select').each(function (j, item) {     
            var key = $(item).attr('name');
            jsonData[key] = $(this).val().toUpperCase();           

        });
        list.push(jsonData);
    });
    return list;
}

