document.addEventListener('DOMContentLoaded', () => {
    loadUsers();
    document.getElementById('user-form').addEventListener('submit', saveUser);
    document.getElementById('search').addEventListener('input', filterUsers);
});

const apiUrl = 'http://localhost:3000';

function loadUsers() {
    fetch(`${apiUrl}/users`)
        .then(response => response.json())
        .then(data => {
            const userList = document.getElementById('user-list');
            userList.innerHTML = '';
            data.forEach(user => {
                const li = document.createElement('li');
                li.classList.add('list-group-item');
                li.innerHTML = `
                    <span>${user.Name} - ${user.Email}</span>
                    <span>
                        <button class="btn btn-edit btn-sm" onclick="editUser(${user.Id})"><i class="fas fa-edit"></i> Editar</button>
                        <button class="btn btn-delete btn-sm" onclick="deleteUser(${user.Id})"><i class="fas fa-trash-alt"></i> Eliminar</button>
                    </span>
                `;
                userList.appendChild(li);
            });
        });
}

function saveUser(e) {
    e.preventDefault();

    const id = document.getElementById('user-id').value;
    const name = document.getElementById('user-name').value;
    const email = document.getElementById('user-email').value;
    const phone = document.getElementById('user-phone').value;

    const method = id ? 'PUT' : 'POST';
    const url = id ? `${apiUrl}/users/${id}` : `${apiUrl}/users`;

    fetch(url, {
        method: method,
        headers: {
            'Content-Type': 'application/json'
        },
        body: JSON.stringify({ name, email, phone })
    }).then(response => response.json())
      .then(() => {
          loadUsers();
          document.getElementById('user-form').reset();
          document.getElementById('user-id').value = '';
      });
}

function editUser(id) {
    fetch(`${apiUrl}/users/${id}`)
        .then(response => response.json())
        .then(user => {
            document.getElementById('user-id').value = user.Id;
            document.getElementById('user-name').value = user.Name;
            document.getElementById('user-email').value = user.Email;
            document.getElementById('user-phone').value = user.Phone;
        });
}

function deleteUser(id) {
    fetch(`${apiUrl}/users/${id}`, {
        method: 'DELETE'
    }).then(() => {
        loadUsers();
    });
}

function filterUsers() {
    const search = document.getElementById('search').value.toLowerCase();
    const userList = document.getElementById('user-list');
    const users = userList.getElementsByTagName('li');

    Array.from(users).forEach(user => {
        const userName = user.textContent.toLowerCase();
        if (userName.includes(search)) {
            user.style.display = '';
        } else {
            user.style.display = 'none';
        }
    });
}
