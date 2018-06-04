
export const environment = {
  production: true,
  apiUrl: 'https://localhost:44353/api/',
  openIdConnectSettings: {
    authority: 'https://localhost:44398/',
    client_id: 'tourmanagementclient',
    redirect_uri: 'https://localhost:4200/signin-oidc',
    scope: 'openid profile roles tourmanagementapi',
    response_type: 'id_token token',
    post_logout_redirect_uri: 'https://localhost:4200/',
    automaticSilentRenew: true,
    silent_redirect_uri: 'https://localhost:4200/redirect-silentrenew'
  }
};

