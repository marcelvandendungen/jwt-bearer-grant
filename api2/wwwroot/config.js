window.config = {
    instance: 'https://login.microsoftonline.com/',
    tenant: 'common', // multi-tenant
    clientId: '<INSERT CLIENTID FOR API2 HERE>',   // client id
    postLogoutRedirectUri: window.location.origin,
    cacheLocation: 'localStorage'
};
