$(document).ready(function () {
    // Mostrar Listado de Empleados
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
                $('#loading-spinner').removeClass('d-none');
            },
            complete: function () {
                $('#loading-spinner').addClass('d-none');
            }
        },
        "responsive": true,
        "columnDefs": [
            {
                "targets": -1,
                "data": null,
                "render": function (data, type, row, meta) {
                    return $("<div>").addClass("d-grid gap-2 d-md-flex justify-content-md-start")
                        .append(
                            $("<button>").addClass("btn btn-danger btn-eliminar btn-sm d-block d-md-inline-block")
                                .append($("<i>").addClass("fas fa-trash"))
                                .attr({ "data-informacion": JSON.stringify(row) })
                        )[0].outerHTML;
                },
                "sortable": false
            },
            { "name": "EmployeeID", "data": "employeeID", "targets": 0 },
            { "name": "Identification", "data": "identification", "targets": 1 },
            { "name": "Name", "data": "name", "targets": 2 },
            {
                "name": "HireDate",
                "data": "hireDate",
                "targets": 3,
                "render": function (data) {
                    const date = new Date(data);
                    return date.toLocaleDateString('es-ES');
                }
            },
            { "name": "Department", "data": "department", "targets": 4 },
            { "name": "Position", "data": "position", "targets": 5 },
            {
                "name": "MonthlySalary",
                "data": "monthlySalary",
                "targets": 6,
                "render": function (data) {
                    return parseFloat(data).toLocaleString('es-ES', { minimumFractionDigits: 2, maximumFractionDigits: 2 });
                }
            },
            {
                "name": "Status", "data": "status", "targets": 7,
                "render": function (data) {
                    return data === "Activo" ? '<span class="badge bg-success">Activo</span>' : '<span class="badge bg-danger">No Activo</span>';
                }
            }
        ],
        "order": [[0, "desc"]],
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
        "pagingType": "full_numbers"
    });

    // Abrir el Formulario
    $(document).on('click', '.btn-editar', function (event) {
        var json = $(this).attr("data-informacion");
        var rowData = JSON.parse(json);
        $("#languageID").val(rowData.languageID);
        abrirModal(rowData);
    });

    // Eliminar Registro
    $(document).on('click', '.btn-eliminar', function (event) {
        var json = $(this).attr("data-informacion");
        var dataObj = JSON.parse(json);

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
                    data: JSON.stringify(dataObj),
                    contentType: "application/json; charset=utf-8",
                    success: function (data) {
                        if (data.resultado) {
                            table.ajax.reload();
                            Swal.fire(
                                'Eliminado!',
                                'El empleado ha sido eliminado.',
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
function abrirModal(id) {
    if (id) {
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
        $('#formEmpleado')[0].reset();
        $('#employeeID').val('');
        $('#FormModal').modal('show');
    }
}

// Función de validación de los campos del formulario
function validarFormulario() {
    var isValid = true;
    var identificacion = $("#identification").val().trim();

    // Validar Identificación (Cédula Dominicana de 11 dígitos)
    if (identificacion === "" || !validarCedulaDominicana(identificacion)) {
        Swal.fire({
            icon: 'warning',
            title: 'Cédula Incorrecta',
            text: 'La cédula proporcionada no es válida en el formato dominicano. Asegúrese de que contenga 11 dígitos numéricos sin espacios ni caracteres especiales.',
            confirmButtonText: 'Reintentar'
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
            text: 'El campo Salario Mensual es obligatorio y debe ser mayor a 0.',
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

    return isValid;
}

// Función para validar la cédula dominicana (11 dígitos)
function validarCedulaDominicana(cedula) {
    var regexCedula = /^[0-9]{11}$/; // Expresión regular para validar 11 dígitos
    if (!regexCedula.test(cedula)) {
        return false;
    }

    // Coeficientes para la validación
    const multiplicadores = [1, 2, 1, 2, 1, 2, 1, 2, 1, 2];
    let suma = 0;

    for (let i = 0; i < 10; i++) {
        let producto = parseInt(cedula.charAt(i)) * multiplicadores[i];
        suma += producto > 9 ? Math.floor(producto / 10) + (producto % 10) : producto;
    }

    const digitoVerificador = (10 - (suma % 10)) % 10;
    return digitoVerificador === parseInt(cedula.charAt(10));
}

// Función para guardar el empleado (crear o actualizar)
function Guardar() {
    if (!validarFormulario()) {
        return;
    }

    var formData = $("#formEmpleado").serialize();
    var id = $('#employeeID').val();
    var url = id ? '/Empleados/Guardar/' + id : '/Empleados/Guardar';
    var tipo = id ? 'PUT' : 'POST';

    jQuery.ajax({
        url: url,
        type: tipo,
        data: formData,
        success: function (data) {
            if (data.resultado) {
                $('#FormModal').modal('hide');
                Swal.fire({
                    icon: 'success',
                    title: 'Guardado!',
                    text: 'El empleado se ha guardado exitosamente.',
                    confirmButtonText: 'Aceptar'
                }).then(() => {
                    location.reload();
                });
            } else {
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
            Swal.fire({
                icon: 'error',
                title: 'Error!',
                text: 'Hubo un problema al enviar la solicitud.',
                confirmButtonText: 'Aceptar'
            });
        }
    });
}
