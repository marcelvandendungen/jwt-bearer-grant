(function () {

    var authContext = new AuthenticationContext(config);

    var isCallback = authContext.isCallback(window.location.hash);
    authContext.handleWindowCallback();
    
    var error = authContext.getLoginError();
    if (error) {
        console.log(error);
    }

    if (isCallback && !authContext.getLoginError()) {
        window.location = authContext._getItem(authContext.CONSTANTS.STORAGE.LOGIN_REQUEST);
    }

    var user = authContext.getCachedUser();
    if (user) {
        document.write(user.userName + "\n");
        authContext.acquireToken(window.config.clientId, function(error, token){
            if (error) {
                document.write(error);
            }
            if (token) {
                document.write(token);
            }
        });
    } else {
        authContext.login();
    }

}());