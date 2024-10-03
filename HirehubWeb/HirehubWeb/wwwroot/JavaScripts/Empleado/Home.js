﻿$(document).ready(function () {

    // Mostrar Listado de Campus
    var table = $('#tabla').DataTable({
        "processing": true,
        "serverSide": true,
        "ajax": {
            url: "/Empleados/Data",
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
                            $("<button>").addClass("btn btn-danger btn-eliminar btn-sm d-block d-md-inline-block") // Botón "eliminar"
                                .append($("<i>").addClass("fas fa-trash"))
                                .attr({ "data-informacion": JSON.stringify(row) }) // Atributo data-informacion para eliminar
                        )[0].outerHTML;
                },
                "sortable": false // Deshabilita ordenación en esta columna
            },
            { "name": "EmployeeID", "data": "employeeID", "targets": 0, "visible": true }, // Columna para IdTipoUsuarios
            { "name": "Identification", "data": "identification", "targets": 1 }, // Columna para Descripción
            { "name": "Name", "data": "name", "targets": 2 }, // Columna para Descripción
            {
                "name": "HireDate",
                "data": "hireDate",
                "targets": 3,
                "render": function (data) {
                    const date = new Date(data);
                    return date.toLocaleDateString('es-ES');
                }
            }, 
            { "name": "Department", "data": "department", "targets": 4 }, // Columna para Descripción
            { "name": "Position", "data": "position", "targets": 5 }, // Columna para Descripción
            {
                "name": "MonthlySalary",
                "data": "monthlySalary",
                "targets": 6,
                "render": function (data) {
                    return parseFloat(data).toLocaleString('es-ES', { minimumFractionDigits: 2, maximumFractionDigits: 2 });
                }
            }, 

            {
                "name": "Status", "data": "status", "targets": 7, // Columna para Estado
                "render": function (data) {
                    return data === "Activo" ? '<span class="badge bg-success">Activo</span>' : '<span class="badge bg-danger">No Activo</span>';
                }
            }
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





    // Abrir Formulario editar
    $(document).on('click', '.btn-editar', function (event) {
        var json = $(this).attr("data-informacion"); // Corregido a .attr() para obtener el valor
        var rowData = JSON.parse(json); // Convertimos el JSON a objeto
        $("#languageID").val(rowData.languageID); // Asigna el ID al campo oculto

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
                    url: "/Empleados/Eliminar",
                    type: "POST",
                    data: JSON.stringify(dataObj), // Enviamos el modelo completo
                    contentType: "application/json; charset=utf-8",
                    success: function (data) {
                        if (data.resultado) {
                            table.ajax.reload(); // Recarga la tabla DataTable
                            Swal.fire(
                                'Eliminado!',
                                'El usuario ha sido eliminado.',
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


// Abrir el Formulario
// Función para abrir el modal para agregar o editar un empleado
function abrirModal(id) {
    if (id) {
        // Si existe un ID, obtener los datos del empleado y llenar el formulario
        $.get('/Empleados/GetEmpleado/' + id, function (data) {
            $('#employeeID').val(data.employeeID);
            $('#identification').val(data.identification);
            $('#name').val(data.name);
            $('#department').val(data.department);
            $('#position').val(data.position);
            $('#monthlySalary').val(data.monthlySalary);
            $('#status').val(data.status);
            $('#FormModal').modal('show');
        });
    } else {
        // Si no hay ID, limpiar el formulario para agregar un nuevo empleado
        $('#formEmpleado')[0].reset();
        $('#employeeID').val('');
        $('#FormModal').modal('show');
    }
}


// Función de validación de los campos del formulario
function validarFormulario() {
    var isValid = true;

    // Validar Identificación
    if ($("#identification").val().trim() === "") {
        Swal.fire({
            icon: 'error',
            title: 'Error!',
            text: 'El campo Identificación es obligatorio.',
            confirmButtonText: 'Aceptar'
        });
        return false;
    }

    // Validar Nombre
    if ($("#name").val().trim() === "") {
        Swal.fire({
            icon: 'error',
            title: 'Error!',
            text: 'El campo Nombre es obligatorio.',
            confirmButtonText: 'Aceptar'
        });
        return false;
    }

    // Validar Departamento
    if ($("#department").val().trim() === "") {
        Swal.fire({
            icon: 'error',
            title: 'Error!',
            text: 'El campo Departamento es obligatorio.',
            confirmButtonText: 'Aceptar'
        });
        return false;
    }

    // Validar Posición
    if ($("#position").val().trim() === "") {
        Swal.fire({
            icon: 'error',
            title: 'Error!',
            text: 'El campo Posición es obligatorio.',
            confirmButtonText: 'Aceptar'
        });
        return false;
    }

    // Validar Salario Mensual
    if ($("#monthlySalary").val().trim() === "" || parseFloat($("#monthlySalary").val()) <= 0) {
        Swal.fire({
            icon: 'error',
            title: 'Error!',
            text: 'El campo Salario Mensual es obligatorio y debe ser un número mayor que 0.',
            confirmButtonText: 'Aceptar'
        });
        return false;
    }

    // Validar Estado
    if ($("#status").val().trim() === "") {
        Swal.fire({
            icon: 'error',
            title: 'Error!',
            text: 'Debe seleccionar un estado.',
            confirmButtonText: 'Aceptar'
        });
        return false;
    }

    return isValid; // Si todo es válido, devolver true
}

// Función para guardar el empleado (crear o actualizar)
function Guardar() {
    // Primero validamos los campos del formulario
    if (!validarFormulario()) {
        return; // Si la validación falla, no se ejecuta el código de guardado
    }

    var formData = $("#formEmpleado").serialize(); // Serializamos los datos del formulario

    var id = $('#employeeID').val(); // Verificamos si estamos creando o editando
    var url = id ? '/Empleados/Guardar/' + id : '/Empleados/Guardar'; // Cambia según la operación
    var tipo = id ? 'PUT' : 'POST'; // Método HTTP para actualizar o crear

    jQuery.ajax({
        url: url,
        type: tipo,
        data: formData, // Enviamos los datos serializados
        success: function (data) {
            if (data.resultado) {
                $('#FormModal').modal('hide'); // Cierra el modal
                // Recarga la tabla o actualiza la tabla de empleados
                Swal.fire({
                    icon: 'success',
                    title: 'Guardado!',
                    text: 'El empleado se ha guardado exitosamente.',
                    confirmButtonText: 'Aceptar'
                }).then(() => {
                    location.reload(); // Recarga la página o actualiza la tabla
                });
            } else {
                // Mostrar SweetAlert2 de error
                Swal.fire({
                    icon: 'error',
                    title: 'Error!',
                    text: 'No se pudo guardar el empleado.',
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
        }
    });
}