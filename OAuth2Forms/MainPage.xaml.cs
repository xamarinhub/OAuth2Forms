using System;
using Xamarin.Auth;
using Xamarin.Forms;

namespace OAuth2Forms
{
    public partial class MainPage : ContentPage
    {
        // docs: https://docs.microsoft.com/xamarin/xamarin-forms/data-cloud/authentication/oauth
        // sample: https://developer.xamarin.com/samples/xamarin-forms/WebServices/OAuthNativeFlow/

        const string AuthorizationEndpoint = "xxxxx";
        const string TokenEndpoint = "xxxxxx";
        const string UserInfoEndpoint = "xxxx";
        const string ClientID = "xxx";
        const string ClientSecret = "xxxx";
        const string Scopes = "openid profile offline_access";
        const string RedirectUrl = "xxxx";

        public MainPage()
        {
            InitializeComponent();

            btnLaunchDemo.Clicked += (sender, e) =>
            {

                var authenticator = new OAuth2Authenticator(
                     ClientID,
                     ClientSecret,
                     Scopes,
                     new Uri(AuthorizationEndpoint),
                     new Uri(RedirectUrl),
                     new Uri(TokenEndpoint),
                     null,
                     true)
                {
                    Title = "Login"
                };
                authenticator.Completed += Authenticator_Completed;
                authenticator.Error += Authenticator_Error;


                AuthenticationState.Authenticator = authenticator;

                var presenter = new Xamarin.Auth.Presenters.OAuthLoginPresenter();
                presenter.Login(authenticator);
            };
        }

        void Authenticator_Error(object sender, AuthenticatorErrorEventArgs e)
        {
            if (sender is OAuth2Authenticator authenticator)
            {
                authenticator.Completed -= Authenticator_Completed;
                authenticator.Error -= Authenticator_Error;
            }
        }

        async void Authenticator_Completed(object sender, AuthenticatorCompletedEventArgs ev)
        {
            if (sender is OAuth2Authenticator authenticator)
            {
                authenticator.Completed -= Authenticator_Completed;
                authenticator.Error -= Authenticator_Error;
            }

            if (ev.IsAuthenticated)
                    await DisplayAlert("OAuth2 Info", $"Authenticated:{ev.IsAuthenticated}\nAccessToken: {ev.Account.Properties["access_token"]}", "OK");
                else
                    await DisplayAlert("OAuth2 Info", $"Authenticated:{ev.IsAuthenticated}", "OK");
        }
    }
}
