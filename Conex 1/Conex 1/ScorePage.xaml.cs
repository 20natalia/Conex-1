using Conex1.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Conex1
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ScorePage : ContentPage
    {
        //public IList<Player> Players { get; private set; }

        private WelcomePage main;
        public ScorePage(WelcomePage m)
        {
            main = m;
            InitializeComponent();
        }

       
        public void ClearButton_OnPressed(object sender, EventArgs e)
        {
           // main.results.Clear();
            GameType2.IsVisible = true;
        }

        public void Picker_Used2(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (GameType2.SelectedItem != null)
            {
                main.removeResult((GameIntense)GameType2.SelectedItem);
               
            } 
        }
    }
       
}