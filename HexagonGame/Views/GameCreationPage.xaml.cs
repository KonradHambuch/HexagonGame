using System;

using HexagonGame.ViewModels;
using Windows.Foundation;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml.Controls;

namespace HexagonGame.Views
{
    public sealed partial class GameCreationPage : Page
    {
        private GameCreationViewModel ViewModel => DataContext as GameCreationViewModel;

        public GameCreationPage()
        {
            InitializeComponent();

            ApplicationView.GetForCurrentView().SetPreferredMinSize(new Size(1000, 700));
            ApplicationView.PreferredLaunchViewSize = new Size(1000, 700);
            ApplicationView.PreferredLaunchWindowingMode = ApplicationViewWindowingMode.PreferredLaunchViewSize;
        }
    }
}
