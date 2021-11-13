using System;

using HexagonGame.ViewModels;
using Windows.Foundation;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml.Controls;

namespace HexagonGame.Views
{
    public sealed partial class GamePage : Page
    {
        private GameViewModel ViewModel => DataContext as GameViewModel;

        public GamePage()
        {
            InitializeComponent();
            var bounds = ApplicationView.GetForCurrentView().VisibleBounds;

            ApplicationView.PreferredLaunchViewSize = new Size(bounds.Width, bounds.Height);
            ApplicationView.PreferredLaunchWindowingMode = ApplicationViewWindowingMode.PreferredLaunchViewSize;
        }
    }
}
