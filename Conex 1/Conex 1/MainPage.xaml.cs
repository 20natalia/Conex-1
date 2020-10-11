using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Conex_1
{
    public partial class MainPage : ContentPage
    {
        private int count = 0;
        public MainPage()
        {
            InitializeComponent();
        }

        private void IncrementCounterClicked(object sender, EventArgs e)
        {
            count++;
            CounterLabel.Text = count.ToString();
        }
    }
}
