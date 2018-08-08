using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace SharedElementTest
{
    public partial class FirstPage : ContentPage
    {
        public FirstPage()
        {
            InitializeComponent();
        }

        private void ImageTapped(object sender, TappedEventArgs e)
        {
            Navigation.PushAsync(new SecondPage());
        }
    }
}
