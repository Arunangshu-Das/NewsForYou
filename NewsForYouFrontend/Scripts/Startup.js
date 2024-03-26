$(document).ready(function () {
    $('#navbar').load('Navbar.html');
    $.ajax({
        url: 'https://localhost:7235/api/getagency',
        type: 'GET',
        dataType: 'json',
        success: function (result) {
            console.log(result)
            populatepaper(result.result)
        },
        error: function () {
            alert('Error while making the AJAX call.');
        }
    });
});

function populatepaper(data) {
    const container = document.getElementById('allpaper');
    
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
                <div class="card flex-fill" width="30%">
                    <img src="${element.logopath}" class="card-img-top" alt="...">
                    <div class="card-body d-flex flex-column">
                        <button id="${element.id}" onclick="setid(this)" class="btn btn-primary mt-auto">${element.name}</button> <!-- Add mt-auto for bottom alignment -->
                    </div>
                </div>
            `;
            
            cardDiv.innerHTML = cardInnerHtml;
            
            row.appendChild(cardDiv);
        }
        
        container.appendChild(row);
    }
}

function setid(e){
    sessionStorage.setItem("newsid", parseInt(e.id));
    window.location.pathname = '/Index.html';
}
