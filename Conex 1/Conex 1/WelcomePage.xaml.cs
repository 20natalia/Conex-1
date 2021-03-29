using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Conex1.Models;

namespace Conex1
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class WelcomePage : ContentPage
    {
        public ObservableCollection<Player> results { get; private set; }

        public WelcomePage()
        {
            InitializeComponent();
            results = new ObservableCollection<Player>();
            gameName = new ObservableCollection<GameIntense>();
            gameName.Add(new GameIntense { GameID = 5, GameCNum = 5 });
            gameName.Add(new GameIntense { GameID = 10, GameCNum = 10 });
            gameName.Add(new GameIntense { GameID = 15, GameCNum = 15 });
            gameName.Add(new GameIntense { GameID = 20, GameCNum = 20 });
            gameName.Add(new GameIntense { GameID = 25, GameCNum = 25 });
            BindingContext = this;
        }

        public int gID = 0;
        public string pName = "Anonymous";
        public ObservableCollection<GameIntense> gameName { get; private set; }



        int gameNum = 0;
        public void addResult(double t, int numCircles)
        {
            double m = Math.Floor(t / 60);
            double s = t - 60 * m;
            gameNum++;

            results.Insert(0, new Player
            {
                Game = gameNum,
                Name = pName,
                Time = m + ":" + s.ToString("00.00"),
                nCircles = numCircles
            }); ; ; ;
        }

        public void removeResult(int numCircles)
        {
                int i = 0;
                while (i < results.Count)
                {
                    if (results.ElementAt(i).nCircles == numCircles)
                    {
                        results.RemoveAt(i);
                    }
                    else
                    {
                        i++;
                    }
                }
            
           
        }
                  
        private async void PlayButton_OnClicked(object sender, EventArgs e)
        {
            DrawingPage draw = new DrawingPage(this);
            draw.BindingContext = this;
            await Navigation.PushAsync(draw);
        }

        private async void ScoresButton_OnClicked(object sender, EventArgs e)
        {

            ScorePage score = new ScorePage(this);
            score.BindingContext = this;

            await Navigation.PushAsync(score);
        }

        private async void SettingsButton_OnClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new FramePage());
        }

        private async void PickerButton_Clicked(object sender, EventArgs e)
        {
            gID = ((GameIntense)GameType.SelectedItem).GameID;
                       
            DrawingPage draw = new DrawingPage(this);
            draw.BindingContext = this;
            await Navigation.PushAsync(draw);

        }
        
        public int getGameID()
        {
            return gID;
        }

        public void Picker_Used(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (GameType.SelectedItem == null)
            {
                GameStart.IsEnabled = false;
            }
            else
            {
                GameStart.IsEnabled = true;
            }
        }

        private void NameEntry_TextChanged(object sender, TextChangedEventArgs e)
        {
            pName = e.NewTextValue;
        }

        private void NameEntry_Completed(object sender, EventArgs e)
        {
            var text = ((Entry)sender).Text;
        }
    }

}