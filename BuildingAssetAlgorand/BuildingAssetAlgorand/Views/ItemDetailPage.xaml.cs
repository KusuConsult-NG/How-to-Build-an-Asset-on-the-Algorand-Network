using BuildingAssetAlgorand.ViewModels;
using System.ComponentModel;
using Xamarin.Forms;

namespace BuildingAssetAlgorand.Views
{
    public partial class ItemDetailPage : ContentPage
    {
        public ItemDetailPage()
        {
            InitializeComponent();
            BindingContext = new ItemDetailViewModel();
        }
    }
}