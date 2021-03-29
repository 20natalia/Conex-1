using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Conex1
{
    //[Xamarin.Forms.ContentProperty("Content")]
    //[Xamarin.Forms.RenderWith(typeof(Xamarin.Forms.Platform._FrameRenderer))]

    //[XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class FramePage : ContentPage //, Xamarin.Forms.IBorderElement, Xamarin.Forms.IElementConfiguration<Xamarin.Forms.Frame>
    {
        public FramePage()
        {
            InitializeComponent();
        }

        private async void NavigateButton_OnClicked(object sender, EventArgs e)
        {
            //await Navigation.PushAsync(new DrawingPage());
        }

        private async void ThemeButton_Clicked(object sender, EventArgs e)
        {
           var currTheme = AppInfo.RequestedTheme;
           var theme = $"The Theme is {currTheme}";
           await DisplayAlert("Theme", theme, "OK");


        }
    }
    
}