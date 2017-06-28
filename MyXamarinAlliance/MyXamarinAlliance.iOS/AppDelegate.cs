using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.MobileServices;

using Foundation;
using UIKit;
using MyXamarinAlliance.Controllers;
using System.IO;
using Microsoft.WindowsAzure.Storage.Auth;
using Microsoft.WindowsAzure.Storage;

namespace MyXamarinAlliance.iOS
{
    // The UIApplicationDelegate for the application. This class is responsible for launching the 
    // User Interface of the application, as well as listening (and optionally responding) to 
    // application events from iOS.
    [Register("AppDelegate")]
    public partial class AppDelegate : global::Xamarin.Forms.Platform.iOS.FormsApplicationDelegate, IAuthenticate, IStreamDownloader
    {
        private MobileServiceUser user;

        //
        // This method is invoked when the application has loaded and is ready to run. In this 
        // method you should instantiate the window, load the UI into it and then make the window
        // visible.
        //
        // You have 17 seconds to return from this method, or iOS will terminate your application.
        //
        public override bool FinishedLaunching(UIApplication app, NSDictionary options)
        {
            App.Init((IAuthenticate)this);
            App.InitDownloader((IStreamDownloader)this);

            global::Xamarin.Forms.Forms.Init();
            LoadApplication(new App());

            return base.FinishedLaunching(app, options);
        }

        public async Task<bool> Authenticate()
        {
            var success = false;
            var message = string.Empty;
            try
            {
                // Sign in with Facebook login using a server-managed flow.
                if (user == null)
                {
                    user = await App.CharacterService.Client.LoginAsync(
                        UIApplication.SharedApplication.KeyWindow.RootViewController,
                        MobileServiceAuthenticationProvider.Facebook);
                    if (user != null)
                    {
                        message = string.Format("You are now signed-in as {0}.", user.UserId);
                        success = true;
                    }
                }
            }
            catch (Exception ex)
            {
                message = ex.Message;
            }

            // Display the success or failure message.
            UIAlertView avAlert = new UIAlertView("Sign-in result", message, null, "OK", null);
            avAlert.Show();

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
    }
}
