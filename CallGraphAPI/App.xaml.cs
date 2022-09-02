using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using Microsoft.Identity.Client;

namespace CallGraphAPI
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    /// 
    public partial class App : Application
    {
        static App()
        {
            // AzureCloudInstance.AzurePublic resolves to "https://login.microsoftonline.com/"
            // For a desktop app using .NET Framework, the redirect should be https://login.microsoftonline.com/common/oauth2/nativeclient
            //      The application will open a popup login screen from Azure AD.
            // For a UWP it should be value of WebAuthenticationBroker.GetCurrentApplicationCallbackUri()
            // For a .NET Core app it should be http://localhost so the user goes through a browser for interactive authentication since .NET Core doesn't have a UI for the embedded web view at the moment.
            //      For browsers in desktop scenarios the redirect uri is intercepted by MSAL to detect that a response is returned from the identity provider.
            // This uri can therefore be used in any cloud without seeing an actual redirect to that uri.  This means you can and should use https://login.microsoftonline.com/common/oauth2/nativeclient in any cloud.
            // If you prefer you can also translate this to another uri as long as you configure the redirect uri correctly with MSAL.
            // Specifying the above in the application registration means there is the least amount of setup in MSAL.
            _clientApp = PublicClientApplicationBuilder.Create(ClientId)
                .WithAuthority(AzureCloudInstance.AzurePublic, Tenant)
                .WithRedirectUri("http://localhost")
                //.WithDefaultRedirectUri()
                .Build();
        }

        //   - For Work or School account in your org, use your tenant ID, or domain
        //   - for any Work or School accounts, use `organizations`
        //   - for any Work or School accounts, or Microsoft personal account, use `common`
        //   - for Microsoft Personal account, use 'consumers'
        private static string Tenant = "organizations";

        private static IPublicClientApplication _clientApp;
        public static IPublicClientApplication PublicClientApp { get { return _clientApp; } }
        private static string ClientId = "60252733-a707-4e1b-aaf3-87275633656b";
    }
}
