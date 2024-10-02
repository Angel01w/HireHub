function Guardar() {
    // Prevenir el comportamiento por defecto del formulario
    event.preventDefault();

    // Obtener los valores del formulario
    var username = document.getElementById("username").value;
    var email = document.getElementById("email").value;
    var password = document.getElementById("password").value;
    var confirmPassword = document.getElementById("confirmPassword").value;

    // Validar que las contraseñas coincidan
    if (password !== confirmPassword) {
        Swal.fire({
            icon: 'error',
            title: 'Error',
            text: 'Las contraseñas no coinciden',
            confirmButtonText: 'Aceptar'
        });
        return;
    }

    // Enviar los datos mediante AJAX
    $.ajax({
        url: '/Login/Registrar',  // URL del controlador y acción de registro
        type: 'POST',
        data: {
            Nombre: username,
            Correo: email,
            Contrasena: password,
            ConfirmarContrasena: confirmPassword
        },
        success: function (response) {
            if (response.resultado) {
                Swal.fire({
                    icon: 'success',
                    title: 'Usuario registrado',
                    text: 'El usuario ha sido registrado exitosamente',
                    confirmButtonText: 'Aceptar'
                }).then(function () {
                    // Limpiar el formulario
                    document.getElementById("registerUserForm").reset();
                });
                setTimeout(function () {
                    window.location.href = '/Login/Index';  // Cambia la URL por la de tu vista de Login
                }, 1000);  // 6000 milisegundos = 6 segundos
            } else {
                Swal.fire({
                    icon: 'error',
                    title: 'Error',
                    text: response.message,
                    confirmButtonText: 'Aceptar'
                });
            }
        },
        error: function (error) {
            Swal.fire({
                icon: 'error',
                title: 'Error',
                text: 'Hubo un problema al enviar la solicitud.',
                confirmButtonText: 'Aceptar'
            });
        }
    });
}
