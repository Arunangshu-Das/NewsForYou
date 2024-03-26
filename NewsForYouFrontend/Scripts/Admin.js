$(document).ready(function () {
    $('#navbar').load('Navbar.html');
    if (!isAdmin()) {
        window.location.href = "login.html";
    }
    $.ajax({
        url: 'https://localhost:7235/api/getcategory',
        type: 'GET',
        dataType: 'json',
        success: function (result) {
            console.log(result);
            loadAllData('feedcategory', result)
        },
        error: function () {
            alert('Error while making the AJAX call.');
        }
    });
    $.ajax({
        url: 'https://localhost:7235/api/getagency',
        type: 'GET',
        dataType: 'json',
        success: function (result) {
            console.log(result);
            loadAllData('feedagency', result)
        },
        error: function () {
            alert('Error while making the AJAX call.');
        }
    });
});


function addCategory() {
    if (document.getElementById("categoryname").value == "") {
        alert("Enter all value to procced");
        return;
    }
    var payload = {
        Title: document.getElementById("categoryname").value
    }
    $.ajax({
        url: 'https://localhost:7235/api/addcategory',
        type: 'POST',
        data: JSON.stringify(payload),
        contentType: 'application/json',
        dataType: 'json',
        success: function (result) {
            alert("Category added");
        },
        error: function () {
            alert('Error while making the AJAX call.');
        }
    });
}

function addAgency() {
    if (document.getElementById("agencyname").value == "" || document.getElementById("logopath").value == "") {
        alert("Enter all value to procced");
        return;
    }
    var payload = {
        Name: document.getElementById("agencyname").value,
        Logopath: document.getElementById("logopath").value,
    }
    console.log(payload)
    $.ajax({
        url: 'https://localhost:7235/api/addagency',
        type: 'POST',
        data: JSON.stringify(payload),
        contentType: 'application/json',
        dataType: 'json',
        success: function (result) {
            alert("Agency added");
        },
        error: function () {
            alert('Error while making the AJAX call.');
        }
    });
}

function addAgencyFeed() {
    if (document.getElementById("feedcategory").value == "") {
        alert("Enter all value to procced");
        return;
    }
    var payload = {
        AgencyId: parseInt(document.getElementById("feedagency").value),
        CategoryId: parseInt(document.getElementById("feedcategory").value),
        AgencyFeedUrl: document.getElementById("feedurl").value,
    }
    $.ajax({
        url: 'https://localhost:7235/api/addagencyfeed',
        type: 'POST',
        data: JSON.stringify(payload),
        contentType: 'application/json',
        dataType: 'json',
        success: function (result) {
            alert("Feed link added");
        },
        error: function () {
            alert('Error while making the AJAX call.');
        }
    });
}


function loadAllData(elementname, datastring) {
    dataObj = datastring;
    console.log(dataObj);
    var dropdown = document.getElementById(elementname);

    dataObj.result.forEach(function (item) {
        var option = document.createElement("option");
        option.value = item.id;
        option.textContent = item?.title || item?.name;
        dropdown.appendChild(option);
    });
}

function deleteAll() {
    var a = confirm("Are you sure want to delete all data?");
    if (a == true) {
        $.ajax({
            url: 'https://localhost:7235/api/deleteall',
            type: 'DELETE',
            success: function (result) {
                alert("Done");
            },
            error: function () {
                alert('Error while making the AJAX call');
            }
        });
    }
}

function isAdmin() {
    var cookies = document.cookie.split(';');
    var adminCookie = cookies.find(cookie => cookie.trim().startsWith('isAdmin='));
    return adminCookie && adminCookie.split('=')[1] === 'true';
}