document.getElementById('loginForm').addEventListener('submit', async function(event) {
    event.preventDefault();
    
    // Obtener los datos del formulario
    const username = document.getElementById('username').value;
    const password = document.getElementById('password').value;

    try {
        // Enviar la solicitud POST al servidor
        const response = await fetch('http://localhost:3000/login', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify({ username, password })
        });

        const data = await response.json();

        // Mostrar el mensaje según la respuesta
        const messageElement = document.getElementById('responseMessage');
        if (response.ok) {
            messageElement.style.color = 'green';
            messageElement.textContent = 'Login exitoso!';
        } else {
            messageElement.style.color = 'red';
            messageElement.textContent = data.message || 'Error de login';
        }
    } catch (error) {
        console.error('Error al realizar el login:', error);
    }
});
