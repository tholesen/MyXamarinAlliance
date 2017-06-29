using MyXamarinAlliance;
using MyXamarinAlliance.Views;
using System;
using System.Threading.Tasks;
using Xamarin.Forms;
using XamarinAllianceApp.Controllers;
using XamarinAllianceApp.Models;
using XamarinAllianceApp.ViewModels;

namespace XamarinAllianceApp.Views
{
    public partial class CharacterListPage : ContentPage
    {
        private CharacterListViewModel viewModel;
        //private CharacterService service;
        private bool authenticated;

        public CharacterListPage()
        {
            InitializeComponent();
            BindingContext = viewModel = new CharacterListViewModel();

            //service = App.CharacterService;
            ImageDownloadButton.IsVisible = false;
        }

        private async void OnItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            var character = e.SelectedItem as Models.Character;
            if (character == null)
            {
                return;
            }

            await Navigation.PushAsync(new CharacterDetailPage(character));

            // Manually deselect item
            characterList.SelectedItem = null;
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            // Set syncItems to true in order to synchronize the data on startup when running in offline mode
            if (authenticated)
            {
                //await RefreshItems(true);
                if (viewModel.Items.Count == 0)
                {
                    viewModel.LoadItemsCommand.Execute(null);
                }
                LoginButton.IsVisible = false;
                ImageDownloadButton.IsVisible = true;
            }
        }

        /*
        // http://developer.xamarin.com/guides/cross-platform/xamarin-forms/working-with/listview/#pulltorefresh
        public async void OnRefresh(object sender, EventArgs e)
        {
            var list = (ListView)sender;
            Exception error = null;
            try
            {
                await RefreshItems(false);
            }
            catch (Exception ex)
            {
                error = ex;
            }
            finally
            {
                list.EndRefresh();
            }

            if (error != null)
            {
                await DisplayAlert("Refresh Error", "Couldn't refresh data (" + error.Message + ")", "OK");
            }
        }

        public async void OnSyncItems(object sender, EventArgs e)
        {
            await RefreshItems(true);
        }

        private async Task RefreshItems(bool showActivityIndicator)
        {
            using (var scope = new ActivityIndicatorScope(syncIndicator, showActivityIndicator))
            {
                characterList.ItemsSource = await service.GetCharactersAsync();
            }
        }
        */

        private class ActivityIndicatorScope : IDisposable
        {
            private bool showIndicator;
            private ActivityIndicator indicator;
            private Task indicatorDelay;

            public ActivityIndicatorScope(ActivityIndicator indicator, bool showIndicator)
            {
                this.indicator = indicator;
                this.showIndicator = showIndicator;

                if (showIndicator)
                {
                    indicatorDelay = Task.Delay(2000);
                    SetIndicatorActivity(true);
                }
                else
                {
                    indicatorDelay = Task.FromResult(0);
                }
            }

            private void SetIndicatorActivity(bool isActive)
            {
                this.indicator.IsVisible = isActive;
                this.indicator.IsRunning = isActive;
            }

            public void Dispose()
            {
                if (showIndicator)
                {
                    indicatorDelay.ContinueWith(t => SetIndicatorActivity(false), TaskScheduler.FromCurrentSynchronizationContext());
                }
            }
        }

        private async void Login_Clicked(object sender, EventArgs e)
        {
            if (App.Authenticator != null)
            {
                authenticated = await App.Authenticator.Authenticate();
            }

            // Set syncItems to true to synchronize the data on startup when offline is enabled.
            if (authenticated == true)
            {
                //await RefreshItems(true);
                if (viewModel.Items.Count == 0)
                {
                    viewModel.LoadItemsCommand.Execute(null);
                }
                LoginButton.IsVisible = false;
                ImageDownloadButton.IsVisible = true;
            }
        }

        private async void ImageDownloadButton_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new ImagePage());
        }
    }
}

