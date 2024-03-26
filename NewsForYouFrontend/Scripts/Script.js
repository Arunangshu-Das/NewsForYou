$(document).ready(function () {
    $('#navbar').load('Navbar.html');
    if(sessionStorage.getItem("newsid")==null){
        window.location.href = 'Startup.html';
    }
    fetchAllData();
    $.ajax({
        url: 'https://localhost:7235/api/GetCategoriesFromAgencyId',
        type: 'GET',
        data: {id:parseInt(sessionStorage.getItem("newsid"))},
        contentType: 'application/json',
        dataType: 'json',
        success: function (result) {
            populateSidebar(result)
        },
        error: function () {
            alert('Error while making the AJAX call.');
        }
    });
});

function fetchAllData(){
    $.ajax({
        url: 'https://localhost:7235/api/getallnews',
        type: 'GET',
        data: {id:parseInt(sessionStorage.getItem("newsid"))},
        dataType: 'json',
        success: function (result) {
            getallnews(result['allnews'])
        },
        error: function () {
            alert('Error while making the AJAX call.');
        }
    });
}

function geturl(url){
    if(url==null) return null;
    console.log(url)
    var tempElement = document.createElement('div');
    tempElement.innerHTML = url;

    // Get the src attribute value
    var src=null;
    try{
    src = tempElement?.firstChild?.getAttribute('src');
    }
    catch(err){

    }

    return src;
}

function getallnews(data) {
    console.log(data)
    const container = document.getElementById('news');
    
    container.innerHTML = '';
  
    for (let i = 0; i < data.length; i += 3) {
        const row = document.createElement('div');
        row.classList.add('row', 'row-eq-height'); 
        
        // Loop through the current set of 3 items
        for (let j = i; j < i + 3 && j < data.length; j++) {
            const element = data[j];
            var cardDiv = document.createElement('div');
            cardDiv.classList.add('col-md-4', 'mb-3', 'd-flex'); 
            
            var cardInnerHtml = `
                <div class="card flex-fill"> <!-- Add flex-fill class for flex items -->
                    ${
                        geturl(element.newsDescription) 
                        ? `<img src="${geturl(element.newsDescription)}" class="card-img-top" alt="...">` 
                        : ''
                    }
                    <div class="card-body d-flex flex-column">
                        <h5 class="card-title">${element.newsTitle}</h5>
                        <p class="card-text">${new Date(element.newsPublishDateTime).toISOString().slice(0, 10)}</p>
                        <button onclick="readit(this,'${element.newsLink}')" id="${element.newsId}" class="btn btn-primary mt-auto">Read it</button> 
                    </div>
                </div>
            `;

            
            cardDiv.innerHTML = cardInnerHtml;
            
            row.appendChild(cardDiv);
        }
        
        container.appendChild(row);
    }
}

function populateSidebar(data) {
    var checkboxes = "";
    data.category.forEach(function(item) {
        checkboxes += '<div class="left-panel"><input type="checkbox" class="form-check-input checkbox mt-0" id="' + item.id + '" value="' + item.title + '">';
        checkboxes += '<label class="checkbox-label" for="' + item.title + '">' + item.title + '</label></div>';
    });
    $("#multiselect").html(checkboxes);
    $(".checkbox").on("click", function() {
        handleCheckboxClick();
    });
}

function handleCheckboxClick(){
    var checkboxArray = [];
    $(".checkbox").each(function() {
        var checkboxId = parseInt($(this).attr("id")); // Convert ID to integer
        var isChecked = $(this).prop("checked");
        if (isChecked) {
            checkboxArray.push(parseInt(checkboxId));
        }
    });
    if(checkboxArray.length==0){
        fetchAllData();
    }
    var payload = {
        categories: checkboxArray,
        id: sessionStorage.getItem("newsid")
    };
    $.ajax({
        url: 'https://localhost:7235/api/getnewsbycategories',
        type: 'POST',
        data: JSON.stringify(payload),
        contentType: 'application/json',
        dataType: 'json',
        success: function(result) {
            getallnews(result.getnesfromcategory)
        },
        error: function() {
            // Handle error response
            alert('Error while making the AJAX call.');
        }
    });
}



function readit(e,link){
    $.ajax({
        url: 'https://localhost:7235/api/incrementnewsclickcount',
        type: 'GET',
        data: {id:parseInt(e.id)},
        contentType: 'application/json',
        dataType: 'json',
        success: function(result) {
        },
        error: function() {
            // Handle error response
            alert('Error while making the AJAX call.');
        }
    });
    window.location.href = link;
}

// // Check for the presence of the "credential" cookie
// function checkCookie() {
//     var cookies = document.cookie.split(';');
//     var isLoggedIn = cookies.some(cookie => cookie.trim().startsWith('credential='));
//     if (isLoggedIn) {
//         document.getElementById("logoutLink").style.display = "block";
//     } else {
//         document.getElementById("loginLink").style.display = "block";
//         document.getElementById("signupLink").style.display = "block";
//     }
// }

// // Function to logout
// function logout() {
//     // Clear the credential cookie
//     document.cookie = "credential=; expires=Thu, 01 Jan 1970 00:00:00 UTC; path=/;";
//     // Redirect to login page
//     window.location.href = "login.html";
// }
