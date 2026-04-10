using ActionApp.Commands;
using ActionApp.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Windows.Input;

namespace ActionApp.ViewModels
{
    public class MainViewModel : INotifyPropertyChanged
    {
        private object _currentViewModel = null!;
        private readonly INavigationService _navigationService;

        public object CurrentViewModel
        {
            get => _currentViewModel;
            set
            {
                _currentViewModel = value;
                OnPropertyChanged(nameof(CurrentViewModel));
            }
        }
        public MainViewModel(INavigationService navigationService)
        {
            _navigationService = navigationService;
            // Initialize via navigation service so the navigation stack is populated
            CurrentViewModel = new HomeViewModel(navigationService);
        }

        public ICommand NavToHomeCommand => new RelayCommand(_ => _navigationService.NavigateTo<HomeViewModel>());
        public ICommand NavToMoviesCommand => new RelayCommand(_ => _navigationService.NavigateTo<MoviesViewModel>());
        public ICommand NavToActorsCommand => new RelayCommand(_ => _navigationService.NavigateTo<ActorsViewModel>());
        public ICommand NavToDirectorsCommand => new RelayCommand(_ => _navigationService.NavigateTo<DirectorsViewModel>());
        public ICommand NavBackCommand => new RelayCommand(_ => _navigationService.GoBack());
        public ICommand ExitCommand => new RelayCommand(_ => System.Windows.Application.Current.Shutdown());



        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
