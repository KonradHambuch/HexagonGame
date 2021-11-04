using System;

using HexagonGame.ViewModels;

using Windows.UI.Xaml.Controls;

namespace HexagonGame.Views
{
    public sealed partial class GameCreationPage : Page
    {
        private GameViewModel ViewModel => DataContext as GameViewModel;

        public GameCreationPage()
        {
            InitializeComponent();
        }
    }
}
