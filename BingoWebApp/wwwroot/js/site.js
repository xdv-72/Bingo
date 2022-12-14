// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.
// Write your JavaScript code.

function jsGetModelItems(gamePadUID) {
    var urlGet = '/Home/GetGamePadItems?gamePadUID=' + gamePadUID;

    return fetch(urlGet)
    .then((response) => {
        return response.json();
    })
    .then((data) => {
        return data;
    })
    .catch((error) => {
        console.log('Failed ');
        $('form').submit();
    });
}

function jsBingoNextStep(gamePadUID)
{
    var urlPost = '/Home/NextStep/' + gamePadUID;

    fetch(urlPost, {
        method: 'POST',
        headers: {
            'Content-type': 'application/json',
        }
    })
    .then((response) => {
        if (response.status == 200 || response.status == 205)
            return response.json();
        else if (response.status == 404) {
            $('form').submit();
        }
    })
    .then((value) => {
        if (value.status == 200)
        {
            var beansCellOwner = $('#beans-cell-owner');
            if (beansCellOwner !== undefined) {
                var szItemHtml = '<span class="beans-cell">' + value.value + '</span>';
                var item = $.parseHTML(szItemHtml);
                beansCellOwner.append(item);
            }
            return jsGetModelItems(gamePadUID);            
        }
        else if (value.status == 205)
            $('form').submit();
    })
    .then((data) => {
        for (var i = 0; i < data.length; i++) {
            var btn = $('#' + data[i].row + '-' + data[i].col + '-cell')[0];
            if (btn !== undefined && data[i].isActive) {
                btn.parentElement.style.backgroundColor = '#dda660b3';
            }
        }
    })
    .catch((error) => {
        console.log('Failed ');
        $('form').submit();
    });       
}

// method for user manual selecting item...
function jsOnBingoCellClick( cellData ) 
{
    var urlPost = '/Home/OnCellClick?row=' + cellData.row + '&col=' + cellData.col + '&gameuid=' + cellData.gameuid; 

    fetch(urlPost, {
        method: 'POST',
        headers: {
            'Content-type': 'application/json',
        }
    }).then(function (response) {
        if (response.status == 200) {
            $('form').submit();
        }
        else if (response.status == 202) {
            var btn = $('#' + cellData.row + '-' + cellData.col + '-cell')[0];
            if (btn !== undefined) {
                btn.parentElement.style.backgroundColor = '#dda660b3';
            }
        }
        else if (response.status !== 204) {
            return Promise.reject(response);
        }
    }).catch(function (error) {
        console.log('Failed ');
        $('form').submit();
    });
}