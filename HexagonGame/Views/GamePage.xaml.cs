using System;

using HexagonGame.ViewModels;

using Windows.UI.Xaml.Controls;

namespace HexagonGame.Views
{
    public sealed partial class GamePage : Page
    {
        private GameViewModel ViewModel => DataContext as GameViewModel;

        public GamePage()
        {
            InitializeComponent();
        }
    }
}
