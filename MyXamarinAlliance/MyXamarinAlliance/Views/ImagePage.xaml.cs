using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MyXamarinAlliance.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ImagePage : ContentPage
    {
        public ImagePage()
        {
            InitializeComponent();
            Appearing += ImagePage_Appearing;
        }

        private async void ImagePage_Appearing(object sender, EventArgs e)
        {
            if (App.StreamDownloader != null)
            {
                var stream = await App.StreamDownloader.DownloadStream();
                stream.Position = 0;
                DownloadImage.Source = ImageSource.FromStream(() => stream);
            }
            else
            {
                await DisplayAlert("Download Error", "Image download is not supported on this platform", "OK");
            }
        }
    }
}