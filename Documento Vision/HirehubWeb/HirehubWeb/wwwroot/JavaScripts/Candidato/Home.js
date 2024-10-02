﻿$(document).ready(function () {

    // Mostrar Listado de Campus
    var table = $('#tabla').DataTable({
        "processing": true,
        "serverSide": true,
        "ajax": {
            url: "/Cadidatos/Data",
            type: 'POST',
            headers: {
                'RequestVerificationToken': $('input[name="__RequestVerificationToken"]').val()
            },
            data: function (d) {
                // Agrega parámetros personalizados si es necesario
            },
            beforeSend: function () {
                // Mostrar el spinner antes de cargar los datos
                $('#loading-spinner').removeClass('d-none');
            },
            complete: function () {
                // Ocultar el spinner cuando los datos estén listos
                $('#loading-spinner').addClass('d-none');
            }
        },
        "responsive": true, // Hace la tabla responsive
        "columnDefs": [
            {
                "targets": -1, // Ultima columna para botones
                "data": null,
                "render": function (data, type, row, meta) {
                    return $("<div>").addClass("d-grid gap-2 d-md-flex justify-content-md-start") // Botones se acomodan en pantallas móviles
                        .append(
                            $("<button>").addClass("btn btn-primary btn-editar btn-sm me-md-2 d-block d-md-inline-block") // Botón "editar"
                                .append($("<i>").addClass("fas fa-pen"))
                                .attr({ "data-informacion": JSON.stringify(row) }) // Atributo data-informacion para editar
                        )
                        .append(
                            $("<button>").addClass("btn btn-danger btn-eliminar btn-sm d-block d-md-inline-block") // Botón "eliminar"
                                .append($("<i>").addClass("fas fa-trash"))
                                .attr({ "data-informacion": JSON.stringify(row) }) // Atributo data-informacion para eliminar
                        )[0].outerHTML;
                },
                "sortable": false // Deshabilita ordenación en esta columna
            },
            { "name": "TrainingID", "data": "trainingID", "targets": 0, "visible": true }, // Columna para IdTipoUsuarios
            { "name": "Description", "data": "description", "targets": 1, "visible": true }, // Columna para IdTipoUsuarios
            { "name": "Level", "data": "level", "targets": 2 }, // Columna para Descripción
            {
                "name": "StartDate",
                "data": "startDate",
                "targets": 3,
                "render": function (data) {
                    const date = new Date(data);
                    return date.toLocaleDateString('es-ES');
                }
            }, 
            {
                "name": "EndDate",
                "data": "endDate",
                "targets": 4,
                "render": function (data) {
                    const date = new Date(data);
                    return date.toLocaleDateString('es-ES');
                }
            }, 
            { "name": "Institution", "data": "institution", "targets": 5 }, // Columna para Intistucion
        ],
        "order": [[0, "desc"]], // Ordenar por la primera columna (IdTipoUsuarios)
        "language": {
            "processing": "Procesando...",
            "lengthMenu": "Mostrar _MENU_ registros",
            "zeroRecords": "No se encontraron resultados",
            "emptyTable": "Ningún dato disponible en esta tabla",
            "info": "Mostrando registros del _START_ al _END_ de un total de _TOTAL_ registros",
            "infoEmpty": "Mostrando registros del 0 al 0 de un total de 0 registros",
            "infoFiltered": "(filtrado de un total de _MAX_ registros)",
            "search": "Buscar:",
            "paginate": {
                "first": "Primero",
                "last": "Último",
                "next": "Siguiente",
                "previous": "Anterior"
            },
            "loadingRecords": "Cargando...",
            "aria": {
                "sortAscending": ": Activar para ordenar la columna de manera ascendente",
                "sortDescending": ": Activar para ordenar la columna de manera descendente"
            }
        },
        "dom": '<"row"<"col-sm-12 col-md-6"l><"col-sm-12 col-md-6"f>>' +
            '<"row"<"col-sm-12"tr>>' +
            '<"row"<"col-sm-12 col-md-5"i><"col-sm-12 col-md-7"p>>',
        "pagingType": "full_numbers" // Estilo de paginación
    });

    // Abrir el Formulario
    window.abrirModal = function (json) {

        // Crear todas las opciones como un bloque de HTML
        var opciones = `
        <option value="Grado">Grado</option>
        <option value="Post-grado">Post-grado</option>
        <option value="Maestría">Maestría</option>
        <option value="Doctorado">Doctorado</option>
        <option value="Técnico">Técnico</option>
        <option value="Gestión">Gestión</option>
    `;

        // Insertar todas las opciones de una vez
        $("#level").html(opciones);
        $("#trainingID").val(0);
        $("#description").val("");
        $("#startDate").val("");
        $("#endDate").val("");
        $("#institution").val("");

        if (json != null) {
            $("#trainingID").val(json.trainingID); // Asegúrate de usar los nombres correctos
            $("#description").val(json.description);
            $("#level").val(json.level);
            $("#startDate").val(json.startDate);
            $("#endDate").val(json.endDate);
            $("#institution").val(json.institution);
            $("#status").val(json.estado == "A" ? 1 : 0);
        }

        $('#FormModal').modal('show');
    }


    // Función de validación de los campos del formulario
    function validarFormulario() {
        var isValid = true;
        var descripcion = $("#description").val().trim();
        // Validar que la descripción no esté vacía
        if (descripcion === "") {
            Swal.fire({
                icon: 'error',
                title: 'Error!',
                text: 'El campo Descripción es obligatorio.',
                confirmButtonText: 'Aceptar'
            });
            return false;
        }

        if ($("#status").length && $("#status").val().trim() === "") {
            Swal.fire({
                icon: 'error',
                title: 'Error!',
                text: 'Debe seleccionar un estado.',
                confirmButtonText: 'Aceptar'
            });
            isValid = false;
            return isValid;
        }


        return isValid; // Si todo es válido, devolver true
    }

    // Guardar Registro usando serialize
    window.Guardar = function () {
        // Primero validamos los campos del formulario
        if (!validarFormulario()) {
            return; // Si la validación falla, no se ejecuta el código de guardado
        }

        var formData = $("#formNivel").serialize(); // Serializamos los datos del formulario

        jQuery.ajax({
            url: "/Capacitaciones/Guardar",
            type: "POST",
            data: formData, // Enviamos los datos serializados
            success: function (data) {
                if (data.resultado) {
                    table.ajax.reload(); // Recarga la tabla DataTable
                    $('#FormModal').modal('hide'); // Cierra el modal
                    // Mostrar SweetAlert2 de éxito
                    Swal.fire({
                        icon: 'success',
                        title: 'Guardado!',
                        text: 'Cambios se guardaron exitosamente.',
                        confirmButtonText: 'Aceptar'
                    });
                } else {
                    // Mostrar SweetAlert2 de error
                    Swal.fire({
                        icon: 'error',
                        title: 'Error!',
                        text: 'No se pudo guardar los cambios.',
                        confirmButtonText: 'Aceptar'
                    });
                }
            },
            error: function (error) {
                console.log(error);
                // Mostrar SweetAlert2 en caso de error en la solicitud
                Swal.fire({
                    icon: 'error',
                    title: 'Error!',
                    text: 'Hubo un problema al enviar la solicitud.',
                    confirmButtonText: 'Aceptar'
                });
            },
            beforeSend: function () {
                // Mostrar el spinner si es necesario
            }
        });
    }


    // Abrir Formulario editar
    $(document).on('click', '.btn-editar', function (event) {
        var json = $(this).attr("data-informacion"); // Corregido a .attr() para obtener el valor
        var rowData = JSON.parse(json); // Convertimos el JSON a objeto
        $("#trainingID").val(rowData.trainingID); // Asigna el ID al campo oculto

        abrirModal(rowData); // Llama a la función para abrir el modal con los datos
    });

    // Eliminar Registro

    $(document).on('click', '.btn-eliminar', function (event) {
        var json = $(this).attr("data-informacion"); // Obtiene la información como string JSON
        var dataObj = JSON.parse(json); // Convierte el string JSON en un objeto JavaScript

        Swal.fire({
            title: '¿Estás seguro?',
            text: "¡No podrás revertir esto!",
            icon: 'warning',
            showCancelButton: true,
            confirmButtonColor: '#3085d6',
            cancelButtonColor: '#d33',
            confirmButtonText: 'Sí, eliminarlo!'
        }).then((result) => {
            if (result.isConfirmed) {
                jQuery.ajax({
                    url: "/Capacitaciones/Eliminar",
                    type: "POST",
                    data: JSON.stringify(dataObj), // Enviamos el modelo completo
                    contentType: "application/json; charset=utf-8",
                    success: function (data) {
                        if (data.resultado) {
                            table.ajax.reload(); // Recarga la tabla DataTable
                            Swal.fire(
                                'Eliminado!',
                                'La Capacitacion ha sido eliminado.',
                                'success'
                            );
                        } else {
                            Swal.fire({
                                icon: 'error',
                                title: 'Error!',
                                text: data.mensaje,
                                confirmButtonText: 'Aceptar'
                            });
                        }
                    },
                    error: function (error) {
                        console.log(error);
                        Swal.fire({
                            icon: 'error',
                            title: 'Error!',
                            text: 'Hubo un problema al enviar la solicitud.',
                            confirmButtonText: 'Aceptar'
                        });
                    }
                });
            }
        });
    });

});
