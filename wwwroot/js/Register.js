$(document).ready(function () {
    $('form.inputCredentials').submit(function (event) {
        event.preventDefault(); // Previene el envío del formulario

        let username = $('#username').val();
        let password = $('#password').val();
        let passwordConfirm = $('#password_c').val();

        if (password !== passwordConfirm) {
            showAlert('Error', 'Las contraseñas no coinciden.');
            return;
        }

        $.ajax({
            type: 'POST',
            url: '/UserRegister',
            data: { username: username, password: password },
            success: function (response) {
                if (response.isSuccess) {
                    showAlert('Éxito', response.message);
                } else {
                    showAlert('Error', response.message);
                }
            },
            error: function () {
                showAlert('Error', 'Ocurrió un error al registrar el usuario.');
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
        }, 2000);
    }
});
