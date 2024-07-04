const fnGenerarCards = function (titulo,fecha,estado,prioridad) {
    $.ajax({
        url: '/GetTask',
        method: 'GET',
        data: {
            fecha: fecha == null ? null : fecha ,
            titulo: titulo == "" ? "" : titulo,
            prioridad: prioridad == 0 ? null : prioridad,
            estado: estado === '' || estado === '0' ? '' : estado, 
            usuario: localStorage.getItem('usuario'),
            status: '*',
        },
        success: function (respuesta) {
            $('#taskContainer').empty();
            respuesta.response.forEach(function (task) {
                const priorityText = task.priority == 1 ? "Alta" : task.priority == 2 ? "Media" : "Baja";

                const taskCard = `
                <div class="col-sm-4">
                    <div class="card h-80">
                        <div class="card-body">
                            <div class="row custom">
                                <h4 class="card-title" id="titulo" style="text-align:center">${task.title}</h4>
                                <h6 class="card-title"id="fecha" style="text-align:center">Fecha Programada: ${new Date(task.datE_CONFIG).toLocaleDateString()}</h6>
                            </div>
                            <div class="row mt-3">
                                <div class="col">
                                    <p class="card-text" id="prioridad">Prioridad: ${priorityText}</p>
                                </div>
                            </div>
                            <div class="row mt-3 d-none">
                                <div class="col">
                                    <p class="card-text" id="id_tarea">id: ${task.c_ID_TASK}</p>
                                </div>
                            </div>
                            <div class="row mt-3 d-none">
                                <div class="col">
                                    <p class="card-text" id="state">estado: ${task.state}</p>
                                </div>
                            </div>
                            <div class="row mt-3 d-none">
                                <div class="col">
                                    <p class="card-text" id="id_usuario">${task.c_ID_USER}</p>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col">
                                    <a href="#" id="btn-ver" class="btn btn-primary" style="margin-right: 5px;">
                                        <i class="fa fa-eye" aria-hidden="true"></i>
                                    </a>
                                    <a href="#" id="btn-eliminar" class="btn btn-primary" style="margin-right: 5px;">
                                        <i class="fa fa-trash" aria-hidden="true"></i>
                                    </a>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            `;
                $('#taskContainer').append(taskCard);
            });

        },
        error: function (error) {
            console.error('Error al hacer la solicitud:', error);
        }
    });
}

$('.navbar-brand').on('click', (function () {
    $('.sidebar').toggleClass('show');
    if ($('.sidebar').hasClass('show')) {
        $('.sidebar').fadeIn();
    } else {
        $('.sidebar').fadeOut();
    }
}));

$('#add').on('click', function () {
    localStorage.setItem('vista', 1);
    $('#title').val(localStorage.getItem('titulo'));
    $('#date').val(localStorage.getItem('fecha'));
    $('#priority').val(localStorage.getItem('prioridad'));
    $('#state').val(localStorage.getItem('estado'));
    $('#description').val(localStorage.getItem('descripcion'));
    $('#crearTareaModal').fadeIn();
    $('.modal-backdrop').fadeIn();
});

$('.close-btn, .modal-backdrop').on('click', function () {
    $('#crearTareaModal').fadeOut();
    $('.modal-backdrop').fadeOut();
});
function guardarDatosLocalmente() {

    if (localStorage.getItem('vista') != 2) {
        var title = $('form[name=tareaModal]').find('#title').val();
        var date = $('form[name=tareaModal]').find('#date').val();
        var priority = $('form[name=tareaModal]').find('#priority').val();
        var state = $('form[name=tareaModal]').find('#state').val();
        var description = $('form[name=tareaModal]').find('#description').val();

        localStorage.setItem('titulo', title);
        localStorage.setItem('fecha', date);
        localStorage.setItem('prioridad', priority);
        localStorage.setItem('estado', state);
        localStorage.setItem('descripcion', description);
    } else {
        $('form[name=tareaModal]').find('#title').val('');
        $('form[name=tareaModal]').find('#date').val('');
        $('form[name=tareaModal]').find('#priority').val(0);
        $('form[name=tareaModal]').find('#state').val(0);
        $('form[name=tareaModal]').find('#description').val('');
    }

}

$('.close-btn').on('click', function () {
    guardarDatosLocalmente();
    $('#crearTareaModal').fadeOut();
    $('.modal-backdrop').fadeOut();
});

$(document).on('mouseup', function (e) {
    var modal = $('#crearTareaModal');
    var backdrop = $('.modal-backdrop');

    if (!modal.is(e.target) && modal.has(e.target).length === 0 &&
        !backdrop.is(e.target) && backdrop.has(e.target).length === 0) {
        guardarDatosLocalmente();
        $('#crearTareaModal').fadeOut();
        $('.modal-backdrop').fadeOut();
    }
});

$('#btnBusqueda').on('click', function () {
    const fecha = $('#date').val();
    const titulo = $('#title').val();
    const prioridad = $('#priority').val();
    const estado = $('#status').val();

    fnGenerarCards(titulo, fecha, estado, prioridad);
});

$('form.tareaModal').submit(function (event) {
    event.preventDefault();

    var form = $(this);

    var response = {
        TITLE: form.find('#title').val(),
        DATE_CONFIG: form.find('#date').val(),
        PRIORITY: form.find('#priority').val(),
        STATE: form.find('#state').val(),
        DESCRIPTION: form.find('#description').val(),
        STATUS: '*',
        C_ID_USER: localStorage.getItem('usuario')
    };

    var url = '/TaskRegister';
    var method = 'POST';

    var taskId = form.find('#task_id').val();

    if (localStorage.getItem('vista') != 1 && taskId) {
        response.C_ID_TASK = taskId;
        url = '/TaskUpdate';
        method = 'PUT';
    }

    $.ajax({
        type: method,
        url: url,
        data: response,
        success: function (response) {
            if (response.isSuccess) {
                showAlert('Éxito', response.message);
                $('#crearTareaModal').fadeOut();

                form.find('#title, #date, #priority, #state, #description').val('');

                if (localStorage.getItem('vista') == 1) {
                    localStorage.removeItem('title');
                    localStorage.removeItem('date');
                    localStorage.removeItem('priority');
                    localStorage.removeItem('state');
                    localStorage.removeItem('description');
                }

                fnGenerarCards();

            } else {
                showAlert('Error', response.message);
            }
        },
        error: function () {
            showAlert('Error', 'Ocurrió un error al ejecutar la solicitud.');
        }
    });
});

function showAlert(title, message) {
    $('#alertModalLabel').text(title);
    $('#alertMessage').text(message);
    $('#alertModal').removeClass('d-none');
    $('#alertModal').modal('show');
    setTimeout(function () {
        $('#alertModal').modal('hide');
        $('.modal-backdrop').fadeOut();
    }, 2000);
}

$(document).on('click', '#btn-eliminar', function () {
    var idTareaText = $(this).closest('.card').find('#id_tarea').text().trim();
    var idTarea = parseInt(idTareaText.split(':')[1].trim());

    $.ajax({
        url: '/TaskDelete',
        method: 'PUT',
        data: {id: idTarea,estado: '&'},
        success: function (response) {
            console.log(response);
            showAlert(response.title, response.message);
            fnGenerarCards();
        },
        error: function (error) {
            // Manejo de errores
            showAlert('Error', 'Ocurrió un error al realizar la solictud');
        }
    });
});

$(document).on('click', '#btn-ver', function () {

    var idTareaText = $(this).closest('.card').find('#id_tarea').text().trim();
    var idTarea = parseInt(idTareaText.split(':')[1].trim());
    localStorage.setItem('vista', 2);

    $.ajax({
        url: '/GetTaskId',
        method: 'GET',
        data: {
            id: idTarea
        },
        success: function (respuesta) {
            var data = respuesta.response;
            
            var fechaFormateada = data.datE_CONFIG.split('T')[0];
            $('form[name=tareaModal]').find('#title').val(data.title);
            $('form[name=tareaModal]').find('#date').val(fechaFormateada);
            $('form[name=tareaModal]').find('#priority').val(data.priority);
            $('form[name=tareaModal]').find('#state').val(data.state);
            $('form[name=tareaModal]').find('#description').val(data.description);
            $('form[name=tareaModal]').find('#task_id').val(data.c_ID_TASK);
            $('#crearTareaModal').fadeIn();
            $('.modal-backdrop').fadeIn();
        },
        error: function (error) {
            console.error('Error al hacer la solicitud:', error);
        }
    });

});
$(document).ready(function () {
    fnGenerarCards();
});