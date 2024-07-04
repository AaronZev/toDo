$('.inputCredentials').on('submit',(function (event) {
    event.preventDefault();
    const username = $('#username').val().toUpperCase();
    const password = $('#password').val();

    $.ajax({
        url: '/Login',
        method: 'POST',
        data: {
            username: username,
            password: password
        },
        success: function (response) {
            if (response.token != '') {
                localStorage.setItem('usuario', response.data.c_ID_USER);
                window.location.href = '/Principal/Task';
            } else {
                showAlert(response.title, response.message);
            }

        },
        error: function (error) {
            showAlert('Error', 'Ocurrió un error en el loggeo')
        }
    });

}));

function showAlert(title, message) {
    $('#alertModalLabel').text(title);
    $('#alertMessage').text(message);
    $('#alertModal').removeClass('d-none');
    $('#alertModal').modal('show');
    setTimeout(function () {
        $('#alertModal').modal('hide');
    }, 2000);
}