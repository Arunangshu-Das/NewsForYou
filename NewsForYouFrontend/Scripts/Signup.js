$(document).ready(function () {
    $('#navbar').load('Navbar.html');
});

function signup() {
    if (document.getElementById("name").value == ""||document.getElementById("emailid").value == ""||document.getElementById("password").value == "") {
        alert("Enter all value to procced");
        return;
    }
    var payload = {
        Name: document.getElementById("name").value,
        Email: document.getElementById("emailid").value,
        Password: document.getElementById("password").value
    }
    $.ajax({
        url: 'https://localhost:7235/api/signup',
        type: 'POST',
        data: JSON.stringify(payload),
        contentType: 'application/json',
        dataType: 'json',
        success: function (result) {
            if(result.flag){
                alert("Signup Done");
                window.location.href = 'login.html';
            }
            else{
                alert("Error!!!!");
            }
        },
        error: function () {
            alert('Error while making the AJAX call.');
        }
    });
}

function checkEmail() {
    var emailPattern = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
    $.ajax({
        url: 'https://localhost:7235/api/checkemail',
        type: 'GET',
        data: {id:document.getElementById("emailid").value},
        contentType: 'application/json',
        dataType: 'json',
        success: function (result) {
            if(!result.flag || !emailPattern.test(document.getElementById("emailid").value)){
                alert("Email already exists or not valid");
                document.getElementById("signupform").disabled=true;
            }
            else{
                
                document.getElementById("signupform").disabled=false;
            }
        },
        error: function () {
            alert('Error while making the AJAX call.');
        }
    });
}