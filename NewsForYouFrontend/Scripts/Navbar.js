function checkCookie() {
    var cookies = document.cookie.split(';');
    var isLoggedIn = cookies.some(cookie => cookie.trim().startsWith('credential='));
    var isAdmin = cookies.some(cookie => cookie.trim().startsWith('isAdmin=true'));

    if (isLoggedIn) {
        document.getElementById("logoutLink").style.display = "block";
        document.getElementById("shownews").style.display = "none";
        if (isAdmin) {
            document.getElementById("adminControlLink").style.display = "block";
            document.getElementById("adminControlExport").style.display = "block";
            document.getElementById("shownews").style.display = "block";
        }
    } else {
        document.getElementById("loginLink").style.display = "block";
        document.getElementById("signupLink").style.display = "block";
    }
}

$(document).ready(function(){
    checkCookie();
});

function logout() {
    document.cookie = "credential=; expires=Thu, 01 Jan 1970 00:00:00 UTC; path=/;";
    document.cookie = "isAdmin=; expires=Thu, 01 Jan 1970 00:00:00 UTC; path=/;";
    window.location.href = "login.html";
}
