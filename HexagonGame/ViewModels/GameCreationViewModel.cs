using System;
using System.Collections.Generic;
using HexagonGame.Core.Models;
using Prism.Commands;
using Prism.Windows.Mvvm;
using Prism.Windows.Navigation;

namespace HexagonGame.ViewModels
{
    public class GameCreationViewModel : ViewModelBase
    {
        private INavigationService _navigationService;
        private bool canAddMorePlayers = true;
        public bool CanAddMorePlayers
        {
            get => canAddMorePlayers;
            set => SetProperty(ref canAddMorePlayers, value);
        }
        public DelegateCommand StartGameCommand { get; set; }
        public DelegateCommand NewPlayerCommand { get; set; }
        public DelegateCommand NewRobotCommand { get; set; }

        private GameSettings gameSettings;
        public GameSettings GameSettings
        {
            get => gameSettings;
            set => SetProperty(ref gameSettings, value);
        }
        public GameCreationViewModel(INavigationService navigationService)
        {
            _navigationService = navigationService;
            StartGameCommand = new DelegateCommand(StartGame);
            NewPlayerCommand = new DelegateCommand(NewPlayer);
            NewRobotCommand = new DelegateCommand(NewRobotPlayer);

            GameSettings = new GameSettings();
        }            
        public void StartGame()
        {
            _navigationService.Navigate(PageTokens.GamePage, GameSettings);                
        }
        public void NewPlayer()
        {
            CanAddMorePlayers = GameSettings.NewPlayer();            
        }
        public void NewRobotPlayer()
        {
            CanAddMorePlayers = GameSettings.NewRobotPlayer();
        }
    }
}
