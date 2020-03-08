if (localStorage.getItem('theme') == null) {
    document.getElementById('bootsrapThemes').href = '/lib/bootstrap/dist/css/bootstrap.min.css';
}
else {
    document.getElementById('bootsrapThemes').href = localStorage.getItem('theme');
}

//TODO:Добавить хранение темы после закрытия браузера
var b1 = document.getElementById('darkBut')

var b2 = document.getElementById('luxBut')

var b3 = document.getElementById('grayBut')

var b4 = document.getElementById('materiaBut')



b1.addEventListener('click', function () {
    document.getElementById('bootsrapThemes').href = '/lib/bootstrap/dist/css/bootstrap.min.css';
    localStorage.setItem('theme','/lib/bootstrap/dist/css/bootstrap.min.css');
})
b2.addEventListener('click', function () {
    document.getElementById('bootsrapThemes').href = '/lib/bootstrap/dist/css/bootstrapLux.min.css';
    localStorage.setItem('theme', '/lib/bootstrap/dist/css/bootstrapLux.min.css');
})
b3.addEventListener('click', function () {
    document.getElementById('bootsrapThemes').href = '/lib/bootstrap/dist/css/bootstrapGray.min.css';
    localStorage.setItem('theme', '/lib/bootstrap/dist/css/bootstrapGray.min.css');
})
b4.addEventListener('click', function () {
    document.getElementById('bootsrapThemes').href = '/lib/bootstrap/dist/css/bootstrapMateria.min.css';
    localStorage.setItem('theme', '/lib/bootstrap/dist/css/bootstrapMateria.min.css');
})

