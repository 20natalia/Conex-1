using Conex1.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Conex1.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class demo1 : ContentPage
    {
        public demo1()
        {
            InitializeComponent();
        }

        private void PickerButton_Clicked(object sender, EventArgs e)
        {
            int gameID =((GameIntense)GameType.SelectedItem).GameID;
        }
    }
}