$(document).ready(function () {
    $('#navbar').load('Navbar.html');
});

function login() {
    if (document.getElementById("emailid").value == "" || document.getElementById("password").value == "") {
        alert("Enter all value to proceed");
        return;
    }
    var payload = {
        email: document.getElementById("emailid").value,
        password: document.getElementById("password").value
    }
    makePostRequest('login',payload,success,error,false);

    function success(result){
        if (result.authenticate) {
            sessionStorage.setItem("credential", result.jwtToken);
            var expirationDate = new Date();
            expirationDate.setTime(expirationDate.getTime() + (2 * 60 * 60 * 1000)); 
            if(payload.email === 'admin@gmail.com' && payload.password === 'admin') {
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
    }
    function error(error){
        alert('Error while making the AJAX call.');
    }
}

