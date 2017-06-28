using System;

using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using MyXamarinAlliance.Controllers;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.MobileServices;
using System.IO;
using Microsoft.WindowsAzure.Storage.Auth;
using Microsoft.WindowsAzure.Storage;

namespace MyXamarinAlliance.Droid
{
    [Activity(Label = "MyXamarinAlliance", Icon = "@drawable/icon", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity, IAuthenticate, IStreamDownloader
    {
        private MobileServiceUser user;

        public async Task<bool> Authenticate()
        {
            var success = false;
            var message = string.Empty;
            try
            {
                // Sign in with Facebook login using a server-managed flow.
                user = await App.CharacterService.Client.LoginAsync(this, MobileServiceAuthenticationProvider.Facebook);
                if (user != null)
                {
                    message = string.Format("you are now signed-in as {0}.",
                        user.UserId);
                    success = true;
                }
            }
            catch (Exception ex)
            {
                message = ex.Message;
            }

            // Display the success or failure message.
            AlertDialog.Builder builder = new AlertDialog.Builder(this);
            builder.SetMessage(message);
            builder.SetTitle("Sign-in result");
            builder.Create().Show();

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

        protected override void OnCreate(Bundle bundle)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(bundle);

            // Initialize the authenticator before loading the app.
            App.Init((IAuthenticate)this);
            App.InitDownloader((IStreamDownloader)this);

            global::Xamarin.Forms.Forms.Init(this, bundle);
            LoadApplication(new App());
        }
    }
}

