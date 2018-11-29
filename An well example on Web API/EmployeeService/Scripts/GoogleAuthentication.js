/// <reference path="jquery-3.3.1.min.js" />

function getAccesstoken() {
    if (location.hash) {// check if the url got a #
        if (location.hash.split('access_token=')) {
            var accessToken = location.hash.split('access_token=')[1].split('&')[0];
            if (accessToken) {
                isUserRegistered(accessToken);
            }
        }
    }
}

function isUserRegistered(accessToken) {
    $.ajax({
        url: '/api/Account/UserInfo',
        method: 'GET',
        headers: {
            'Content-type': 'application/JSON',
            'Authorization': 'Bearer ' + accessToken
        },
        success: function(response) {
            if (response.HasRegistered) {
                localStorage.setItem('accessToken', accessToken);
                localStorage.setItem('userName', response.Email);
                window.location.href = 'Data.html';
            }
            else {
                signupExternalUser(accessToken, response.LoginProvider);
            }
        }
    });
}

function signupExternalUser(accessToken, provider) {
    $.ajax({
        url: '/api/Account/RegisterExternal',
        method: 'POST',
        headers: {
            'Content-type': 'application/JSON',
            'Authorization': 'Bearer ' + accessToken
        },
        success: function () {
            window.location.href = "/api/Account/ExternalLogin?provider=" + provider + "&response_type=token&client_id=self&redirect_uri=http%3A%2F%2Flocalhost%3A30380%2FLogin.html&state=mF3uGzUBkM7sgV4sE_GjFnhZfDMPVrA1-G5p_Ku0TSk1";
        }
    });
}