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

        public ScorePage()
        {
            InitializeComponent();
        }

        private void ClearButton_OnPressed(object sender, EventArgs e)
        {
           
        }
    }
       
}