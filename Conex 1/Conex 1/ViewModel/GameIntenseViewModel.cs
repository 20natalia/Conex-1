using Conex1.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace Conex1.ViewModel
{
    class GameIntenseViewModel
    {

        public IList<GameIntense> GameList { get; set; }

        public GameIntenseViewModel()
        {
            try
            {
                GameList = new ObservableCollection<GameIntense>();
                GameList.Add(new GameIntense { GameID = 5, GameCNum = 5 });
                GameList.Add(new GameIntense { GameID = 10, GameCNum = 10 });
                GameList.Add(new GameIntense { GameID = 15, GameCNum = 15 });
                GameList.Add(new GameIntense { GameID = 20, GameCNum = 20 });
                GameList.Add(new GameIntense { GameID = 25, GameCNum = 25 });
            }
            catch (Exception ex)
            { }
        }


        

    }
}
