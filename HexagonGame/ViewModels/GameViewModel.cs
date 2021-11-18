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
        private Database Database = new Database();
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
        public GameViewModel(INavigationService navigationService)
        {  
            _navigationService = navigationService;            
            
            NewGameCommand = new DelegateCommand(NewGame);
            ChangeColorCommand = new DelegateCommand<object>(ChangeColorCommandFunc);           
        }
        //Methods
        public override void OnNavigatedTo(NavigatedToEventArgs e, Dictionary<string, object> viewModelState)
        {
            base.OnNavigatedTo(e, viewModelState);
            Game = new Game((GameSettings)e.Parameter); 
        }        
        public void NewGame()
        {
            _navigationService.Navigate(PageTokens.GameCreationPage, null);
        }
        public async void ChangeColorCommandFunc(object color)
        {            
            Game.ActivePlayer.ChangeColor((MyColor)color, Game.Fields);            
            Game.ActivateNextPlayer();
            Game.FindFreeColors();
            if (TryDetectEndOfGame()) NavigateToRankingsDialog();
            if(Game.ActivePlayer is RobotPlayer)
            {
                Game.RealPlayerTakingTurn = false;
                await Game.TakeRobotTurns();
            }

        }
        public void NavigateToRankingsDialog()
        {  
            _navigationService.Navigate(PageTokens.RankingDialogPage, Database.Rankings);
        }
        public bool TryDetectEndOfGame()
        {
            if (Game.Fields.All(f => Game.Players.Any(p => p.OwnFields.Contains(f))))
            {
                Player WINNER = Game.Players.Aggregate((mostFields, next) => next.OwnFields.Count > mostFields.OwnFields.Count ? next : mostFields);
                Database.AddWinner(WINNER);
                return true;
            }
            return false;
        }
    }
}
