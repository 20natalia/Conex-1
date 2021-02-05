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
    public partial class WelcomePage : ContentPage
    {
        public IList<Player> results { get; private set; }

        public WelcomePage()
        {
            InitializeComponent();
            results = new List<Player>();
        }

        int gameNum = 0;
        public void addResult(String name, double t)
        {
            double m = Math.Floor(t / 60);
            double s = t - 60 * m;
            gameNum++;

            results.Insert(0, new Player
            {
                Game = gameNum,
                Name = name,
                Time = m + ":" + s.ToString("00.00")
            }); ; ;
        }

        private async void PlayButton_OnClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new DrawingPage(this));
        }

        private async void ScoresButton_OnClicked(object sender, EventArgs e)
        {
            ScorePage score = new ScorePage();
            score.BindingContext = this;

            await Navigation.PushAsync(score);
        }

        private async void SettingsButton_OnClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new FramePage());
        }
    }
}