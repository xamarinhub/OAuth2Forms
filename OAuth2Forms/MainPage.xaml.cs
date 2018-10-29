using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Xml.Linq;
using Xamarin.Auth;
using Xamarin.Forms;

namespace OAuth2Forms
{
    public partial class MainPage : ContentPage
    {
        // docs: https://docs.microsoft.com/xamarin/xamarin-forms/data-cloud/authentication/oauth
        // sample: https://developer.xamarin.com/samples/xamarin-forms/WebServices/OAuthNativeFlow/

        //const string AuthorizationEndpoint = "https://idsign-sviluppo.aliaslab.net/IdSign.IdP/Jenkins/connect/authorize";
        //const string TokenEndpoint = "https://idsign-sviluppo.aliaslab.net/IdSign.IdP/Jenkins/connect/token";
        //const string UserInfoEndpoint = "https://idsign-sviluppo.aliaslab.net/IdSign.IdP/Jenkins/connect/userinfo";
        //const string ClientID = "ids-svil-code-r";
        //const string ClientSecret = "ids-svil-code";
        //const string Scopes = "openid profile offline_access";
        //const string RedirectUrl = "net.aliaslab.idsign://oauth-callback/idsign";

        //const string AuthorizationEndpoint = "https://oauthidp.polimi.it/oauthidp/oauth2/auth";
        //const string TokenEndpoint = "https://oauthidp.polimi.it/oauthidp/oauth2/token";
        //const string UserInfoEndpoint = "https://idsign-sviluppo.aliaslab.net/IdSign.IdP/Jenkins/connect/userinfo";
        //const string ClientID = "36767536";
        //const string ClientSecret = "L6rmF2FvLN";
        //const string Scopes = "openid";
        //const string RedirectUrl = "it.blube.mobile.cardholder.polimi://oauth_callback";

        const string AuthorizationEndpoint = "xxxx";
        const string TokenEndpoint = "xxxx";
        const string UserInfoEndpoint = "xx";
        const string ClientID = "xxxx";
        const string ClientSecret = "xxx";
        const string Scopes = "openid";
        const string RedirectUrl = "xxxxx";

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
                    Title = "Login",
                    ClearCookiesBeforeLogin = true
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

            if (ev.IsAuthenticated)
            {
                InternalAPI2(ev.Account);
                InternalAPI(ev.Account.Properties["access_token"]);
            }
        }

        async private void InternalAPI2(Account ac)
        {
            var request = new OAuth2Request("GET", new Uri(UserInfoEndpoint), null, ac);
            var response = await request.GetResponseAsync();
            if (response != null)
            {
                string userJson = response.GetResponseText();
                //var user = JsonConvert.DeserializeObject<User>(userJson);
            }
        }

        async private void InternalAPI(string AccessToken)
        {
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", AccessToken);

            HttpResponseMessage response = await client.GetAsync(new Uri(UserInfoEndpoint));
            if (response.IsSuccessStatusCode)
            {
                var responseString = await response.Content.ReadAsStringAsync();
                responseString = responseString.Replace(@"\", string.Empty).Trim(new char[] { '\"' });
                //var responseJson = XObject.Parse(responseString);

            }
            else
            {

            }
        }
    }
}
