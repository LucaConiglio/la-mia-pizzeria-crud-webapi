loadPizze();

function loadPizze(searchKey) {
    axios.get('/Api/PizzaApi/GetPizze', {
        params: {
            search: searchKey
        }
    })
        .then((res) => {        //se la richiesta va a buon fine
            console.log('risposta ok', res);
            if (res.data.length == 0) {     //non ci sono post da mostrare => nascondo la tabella
                document.getElementById('pizze-table').classList.add('d-none');
                document.getElementById('no-pizze').classList.remove('d-none');
            } else {                        //ci sono post da mostrare => visualizzo la tabella
                document.getElementById('pizze-table').classList.remove('d-none');
                document.getElementById('no-pizze').classList.add('d-none');

                //svuoto la tabella
                document.getElementById('pizze').innerHTML = '';
                res.data.forEach(pizza => {
                    console.log('pizza', pizza);
                    document.getElementById('pizze').innerHTML +=
                        `
                        <tr>
                            <td>
                                <a href="/Client/Detail?id=${pizza.id}">${pizza.id}</a>
                            </td>
                            <td class="image"><img src=${pizza.image}></td>
                            <td class="title">${pizza.name}</td>
                            <td class="description">${pizza.description}</td>
                            <td>
                                <a class="btn btn-primary" href="/Client/Edit?id=${pizza.id}">
                                <i class="fa-solid fa-pen-to-square"></i></a>
                            </td>
                            <td>
                                <a class="btn btn-danger" onclick="deletePizza(${pizza.id})">
                                    <i class="fa-solid fa-trash"></i>
                                </a>                                
                            </td>
                        </tr>
                        `;
                })
            }
        })
        .catch((res) => {       //se la richiesta non è andata a buon fine
            console.error('errore', res);
            alert('errore nella richiesta');
        });

}


function deletePost(pizzaId) {
    const isDelete = confirm('Sei sicuro?');
    if (isDelete) {
        axios.delete(`/api/PostsAPI/${pizzaId}`)
            .then((res) => {        //se la richiesta va a buon fine
                loadPizze();
            })
            .catch((res) => {
                console.error('errore', res);
                alert('errore nella richiesta');
            })
    }
}