
$(document).ready(function () {

    $('.inputCredentials').submit(function (event) {
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
                showAlert(response.title,response.message)
            },
            error: function (error) {
                showAlert('Error', 'Ocurrió un error en el loggeo')
            }
        });

    });

    function showAlert(title, message) {
        $('#alertModalText').text(title);
        $('#alertMessage').text(message);
        $('#alertModal').removeClass('d-none');
        $('#alertModal').modal('show');
        setTimeout(function () {
            $('#alertModal').modal('hide');
        }, 2000);
    }

});
