$(document).ready(function () {
    $('#navbar').load('Navbar.html');
});

function login() {
    if (document.getElementById("emailid").value == "" || document.getElementById("password").value == "") {
        alert("Enter all value to proceed");
        return;
    }
    var payload = {
        Email: document.getElementById("emailid").value,
        Password: document.getElementById("password").value
    }
    $.ajax({
        url: 'https://localhost:7235/api/login',
        type: 'POST',
        data: JSON.stringify(payload),
        contentType: 'application/json',
        dataType: 'json',
        success: function (result) {
            if (result.authenticate) {
                sessionStorage.setItem("credential", result.jwtToken);
                var expirationDate = new Date();
                expirationDate.setTime(expirationDate.getTime() + (2 * 60 * 60 * 1000)); 
                if(payload.Email === 'admin@gmail.com' && payload.Password === 'admin') {
                    document.cookie = `credential=${result.jwtToken};expires=${expirationDate.toUTCString()};path=/;`;
                    document.cookie = `isAdmin=true;expires=${expirationDate.toUTCString()};path=/;`;
                } else {
                    document.cookie = `credential=${result.jwtToken};expires=${expirationDate.toUTCString()};path=/;`;
                }
                alert("Login Done");
                window.location.href = 'Startup.html';
            } else {
                alert("Invalid credential");
            }
        },
        error: function () {
            alert('Error while making the AJAX call.');
        }
    });
}

