using Microsoft.WindowsAzure.MobileServices;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Auth;
using MyXamarinAlliance.Controllers;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

namespace MyXamarinAlliance.UWP
{
    public sealed partial class MainPage : IAuthenticate, IStreamDownloader
    {
        private MobileServiceUser user;

        public MainPage()
        {
            this.InitializeComponent();
            // Initialize the authenticator before loading the app.
            MyXamarinAlliance.App.Init(this);
            MyXamarinAlliance.App.InitDownloader(this);
            LoadApplication(new MyXamarinAlliance.App());
        }

        public async Task<bool> Authenticate()
        {
            string message = string.Empty;
            var success = false;

            try
            {
                // Sign in with Facebook login using a server-managed flow.
                if (user == null)
                {
                    user = await MyXamarinAlliance.App.CharacterService.Client.LoginAsync(MobileServiceAuthenticationProvider.Facebook);
                    if (user != null)
                    {
                        success = true;
                        message = string.Format("You are now signed-in as {0}.", user.UserId);

                        //var guid = await GetDiplomaGuid();
                    }
                }

            }
            catch (Exception ex)
            {
                message = string.Format("Authentication Failed: {0}", ex.Message);
            }

            // Display the success or failure message.
            await new MessageDialog(message, "Sign-in result").ShowAsync();

            return success;
        }

        public async Task<Stream> DownloadStream()
        {
            // Get token
            var token = await MyXamarinAlliance.App.CharacterService.Client.InvokeApiAsync("/api/StorageToken/CreateToken");

            // Get blob client
            string storageAccountName = "xamarinalliance";
            StorageCredentials credentials = new StorageCredentials(token.ToString());
            CloudStorageAccount account = new CloudStorageAccount(credentials, storageAccountName, null, true);
            var blobClient = account.CreateCloudBlobClient();

            // Download stream
            var container = blobClient.GetContainerReference("images");
            var blob = container.GetBlobReference("XAMARIN-Alliance-logo.png");

            MemoryStream stream = new MemoryStream();
            await blob.DownloadToStreamAsync(stream);

            return stream;
        }

        private async Task<string> GetDiplomaGuid()
        {
            var guid = await MyXamarinAlliance.App.CharacterService.Client.InvokeApiAsync("/api/XamarinAlliance/ReceiveCredit");
            return guid.ToString();
        }
    }
}
