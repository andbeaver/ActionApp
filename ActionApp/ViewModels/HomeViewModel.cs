using ActionApp.Commands;
using ActionApp.Data;
using ActionApp.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Windows.Input;

namespace ActionApp.ViewModels
{
    public class HomeViewModel : INotifyPropertyChanged
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

        public HomeViewModel(INavigationService navigationService)
        {
            _navigationService = navigationService;
            using var db = new IMDBContext();
        }

        public ICommand NavToHomeCommand => new RelayCommand(_ => _navigationService.NavigateTo<HomeViewModel>());

        public ICommand NavToAlbumsCommand => new RelayCommand(_ => _navigationService.NavigateTo<MoviesViewModel>());
        public ICommand NavToArtistsCommand => new RelayCommand(_ => _navigationService.NavigateTo<ActorsViewModel>());
        public ICommand NavToTracksCommand => new RelayCommand(_ => _navigationService.NavigateTo<DirectorsViewModel>());
        public ICommand NavBackCommand => new RelayCommand(_ => _navigationService.GoBack());
        public ICommand ExitCommand => new RelayCommand(_ => System.Windows.Application.Current.Shutdown());


        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
