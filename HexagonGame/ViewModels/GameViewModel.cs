using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using HexagonGame.Core.Models;
using HexagonGame.Services;
using HexagonGame.Views;
using Prism.Commands;
using Prism.Windows.Mvvm;
using Prism.Windows.Navigation;
using Windows.Foundation;
using Windows.UI.ViewManagement;

namespace HexagonGame.ViewModels
{
    public class GameViewModel : ViewModelBase
    {
        //Private fields
        private INavigationService _navigationService;
        private IDatabase Database;
        private Game game;
        //Properties
        public INavigationService NavigationService { get; set; }        
        public Game Game
        {
            get => game;
            set => SetProperty(ref game, value);
        }     
                   
        //Commands
        public DelegateCommand NewGameCommand { get; set; }
        public DelegateCommand<object> ChangeColorCommand { get; set; }
        //Ctor
        public GameViewModel(INavigationService navigationService, IDatabase dataBase)
        {  
            _navigationService = navigationService;
            Database = dataBase;
            NewGameCommand = new DelegateCommand(NewGame);
            ChangeColorCommand = new DelegateCommand<object>(ChangeColorCommandFunc);           
        }
        //Methods
        public override void OnNavigatedTo(NavigatedToEventArgs e, Dictionary<string, object> viewModelState)
        {
            base.OnNavigatedTo(e, viewModelState);
            Game = new Game((GameSettings)e.Parameter);
            Game.GameEnded += new GameEndedHandler(GameEndDetected);
        }        
        public void NewGame()
        {
            _navigationService.Navigate(PageTokens.GameCreationPage, null);
        }
        public void ChangeColorCommandFunc(object color)
        {
            Game.PlayerChooseColor((MyColor)color); 
        }
        public void NavigateToRankingsDialog()
        {  
            _navigationService.Navigate(PageTokens.RankingDialogPage, Database.GetRankings());
        }        
        public void GameEndDetected()
        {
            Database.AddWinner(Game.Winner);
            NavigateToRankingsDialog();
        }
    }
}
