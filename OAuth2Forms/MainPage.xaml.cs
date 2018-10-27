using System;
using Xamarin.Auth;
using Xamarin.Forms;

namespace OAuth2Forms
{
    public partial class MainPage : ContentPage
    {
        // docs: https://docs.microsoft.com/xamarin/xamarin-forms/data-cloud/authentication/oauth
        // sample: https://developer.xamarin.com/samples/xamarin-forms/WebServices/OAuthNativeFlow/

        const string AuthorizationEndpoint = "https://idsign-sviluppo.aliaslab.net/IdSign.IdP/Jenkins/connect/authorize";
        const string TokenEndpoint = "https://idsign-sviluppo.aliaslab.net/IdSign.IdP/Jenkins/connect/token";
        const string UserInfoEndpoint = "https://idsign-sviluppo.aliaslab.net/IdSign.IdP/Jenkins/connect/userinfo";
        const string ClientID = "ids-svil-code-r";
        const string ClientSecret = "ids-svil-code";
        const string Scopes = "openid profile offline_access";
        const string RedirectUrl = "net.aliaslab.idsign://oauth-callback/idsign";

        public MainPage()
        {
            InitializeComponent();

            btnLaunchDemo.Clicked += (sender, e) => {

               var authenticator = new OAuth2Authenticator(
                    ClientID,
                    ClientSecret,
                    Scopes,
                    new Uri(AuthorizationEndpoint),
                    new Uri(RedirectUrl),
                    new Uri(TokenEndpoint),
                    null,
                    true);

                authenticator.Completed += async (object s, AuthenticatorCompletedEventArgs ev) => {
                
                    if (ev.IsAuthenticated)
                        await DisplayAlert("OAuth2 Info", $"Authenticated:{ev.IsAuthenticated}\nAccessToken: {ev.Account.Properties["access_token"]}","OK");
                    else
                        await DisplayAlert("OAuth2 Info", $"Authenticated:{ev.IsAuthenticated}", "OK");
                };

                AuthenticationState.Authenticator = authenticator;

                var presenter = new Xamarin.Auth.Presenters.OAuthLoginPresenter();
                presenter.Login(authenticator);
            };
        }
    }
}
