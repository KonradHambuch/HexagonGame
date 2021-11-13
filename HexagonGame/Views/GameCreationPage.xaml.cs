using System;

using HexagonGame.ViewModels;

using Windows.UI.Xaml.Controls;

namespace HexagonGame.Views
{
    public sealed partial class GameCreationPage : Page
    {
        private GameCreationViewModel ViewModel => DataContext as GameCreationViewModel;

        public GameCreationPage()
        {
            InitializeComponent();
        }
    }
}
