const url = 'api/Categories';
let categories = [];

function getCategories() {
    fetch(url)
        .then(response => response.json())
        .then(data => _displayCategories(data))
        .catch(error => console.error('Unable to get categories.', error));
}

function addCategory() {
    const addNameTextbox = document.getElementById('add-name');
    const category = {
        name: addNameTextbox.value.trim(),
    };
    fetch(url, {
        method: 'POST',
        headers: {
            'Accept': 'application/json',
            'Content-Type': 'application/json'
        },
        body: JSON.stringify(category)
    })
        .then(response => response.json())
        .then(() => {
            getCategories();
            addNameTextbox.value = '';
        })
        .catch(error => console.error('Unable to add category.', error));
}
function deleteCategory(id) {
    fetch(`${url}/${id}`, {
        method: 'DELETE'
    })
        .then(() => getCategories())
        .catch(error => console.error('Unable to delete category.', error));
}
function displayEditForm(id) {
    const category = categories.find(category => category.id === id);
    document.getElementById('edit-id').value = category.id;
    document.getElementById('edit-name').value = category.name;
    document.getElementById('editForm').style.display = 'block';
}
function updateCategory() {
    const categoryId = document.getElementById('edit-id').value;
    const category = {
        id: parseInt(categoryId, 10),
        name: document.getElementById('edit-name').value.trim(),
    };
    fetch(`${url}/${categoryId}`, {
        method: 'PUT',
        headers: {
            'Accept': 'application/json',
            'Content-Type': 'application/json'
        },
        body: JSON.stringify(category)
    })
        .then(() => getCategories())
        .catch(error => console.error('Unable to update category.', error));
    closeInput();
    return false;
}
function closeInput() {
    document.getElementById('editForm').style.display = 'none';
}
function _displayCategories(data) {
    const tBody = document.getElementById('categories');

    tBody.innerHTML = '';
    const button = document.createElement('button');
    data.forEach(category => {
        let editButton = button.cloneNode(false);
        editButton.innerText = 'Edit';
        editButton.setAttribute('onclick', `displayEditForm(${category.id})`);
        let deleteButton = button.cloneNode(false);
        deleteButton.innerText = 'Delete';
        deleteButton.setAttribute('onclick', `deleteCategory(${category.id})`);
        let tr = tBody.insertRow();
        let td1 = tr.insertCell(0);
        let textNode = document.createTextNode(category.name);
        td1.appendChild(textNode);
        let td3 = tr.insertCell(1);
        td3.appendChild(editButton);
        let td4 = tr.insertCell(2);
        td4.appendChild(deleteButton);
    });
    categories = data;
}