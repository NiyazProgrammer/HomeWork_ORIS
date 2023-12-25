document.getElementById('send-form').addEventListener('submit', submitForm);

function submitForm(event) {
    event.preventDefault();
    let formData = new FormData(event.target);

    // Создаем словарь для запонения данными ключ - имя поля занчение - введенный текст
    let obj = {};
    formData.forEach((value, key) => obj[key] = value);
    //
    // Этот код создает объект Request, который будет использоваться для отправки данных на сервер. 
    let request = new Request(event.target.action, {
        method: 'POST',
        body: JSON.stringify(obj),
        headers: {
            'Content-Type': 'application/json',
        },
    });

    // функцию fetch, которая выполняет асинхронный HTTP-запрос к серверу.
    fetch(request)
    .then(
        function(response) {
            console.log(response);
        },
        function(error) {
            console.error(error);
        }
    );

    console.log('Запрос отправляется');
}