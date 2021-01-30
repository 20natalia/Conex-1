using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Conex_1
{
    public partial class App : Application
    {
        public string FolderPath { get; private set; }

        public App()
        {
            InitializeComponent();
     
          
            MainPage = new NavigationPage(new Conex1.FramePage());

            // MainPage = new Conex1.FramePage();
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
