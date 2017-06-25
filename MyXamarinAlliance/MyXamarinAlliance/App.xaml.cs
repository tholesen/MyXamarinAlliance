using MyXamarinAlliance.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Xamarin.Forms;
using XamarinAllianceApp.Controllers;
using XamarinAllianceApp.Views;

namespace MyXamarinAlliance
{
    public partial class App : Application
    {
        public static IAuthenticate Authenticator { get; private set; }
        public static CharacterService CharacterService { get; private set; }
        public App()
        {
            InitializeComponent();
            CharacterService = new CharacterService();

            //MainPage = new CharacterListPage();
            MainPage = new NavigationPage(new CharacterListPage());
        }

        public static void Init(IAuthenticate authenticator)
        {
            Authenticator = authenticator;
        }

        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}
