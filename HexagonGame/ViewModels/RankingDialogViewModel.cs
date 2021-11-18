using HexagonGame.Core.Models;
using Prism.Commands;
using Prism.Windows.Mvvm;
using Prism.Windows.Navigation;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;

namespace HexagonGame.ViewModels
{
    class RankingDialogViewModel : ViewModelBase
    {
        private INavigationService _navigationService;
        public ObservableCollection<Ranking> Rankings { get; set; } = new ObservableCollection<Ranking>();
        public DelegateCommand NewGameCommand { get; set; }
        public DelegateCommand ExitCommand { get; set; }
        public INavigationService NavigationService { get; set; }
        public RankingDialogViewModel(INavigationService navigationService)
        {
            _navigationService = navigationService;
            NewGameCommand = new DelegateCommand(NewGame);
            ExitCommand = new DelegateCommand(Exit);
        }
        public override void OnNavigatedTo(NavigatedToEventArgs e, Dictionary<string, object> viewModelState)
        {
            base.OnNavigatedTo(e, viewModelState);
            var rankings = (Dictionary<string, Ranking>) e.Parameter;
            foreach (var ranking in rankings.Values.ToList())
            {
                Rankings.Add(ranking);
            }            
        }
        public void NewGame()
        {
            _navigationService.Navigate(PageTokens.GameCreationPage, null);
        }
        public void Exit()
        {
            Application.Current.Exit();
        }
    }
}
