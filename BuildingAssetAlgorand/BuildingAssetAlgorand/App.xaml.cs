using BuildingAssetAlgorand.Services;
using BuildingAssetAlgorand.Views;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace BuildingAssetAlgorand
{
    public partial class App : Application
    {

        public App()
        {
            InitializeComponent();

            DependencyService.Register<MockDataStore>();
            MainPage = new NavigationPage(new BuildAsset());
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
