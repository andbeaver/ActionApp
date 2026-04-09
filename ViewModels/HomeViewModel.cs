using ActionApp.Data;
using ActionApp.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

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
            //CurrentViewModel = _navigationService.GetInitialViewModel();
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
