using ActionApp.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace ActionApp.Services
{
    public interface INavigationService
    {
        void NavigateTo<TViewModel>() where TViewModel : class;
        void GoBack();
    }

    public class NavigationService : INavigationService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly Stack<object> _navigationStack = new Stack<object>();

        public NavigationService(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            // Do not resolve MainViewModel here to avoid circular dependencies.
        }

        public void NavigateTo<TViewModel>() where TViewModel : class
        {
            var viewModel = _serviceProvider.GetService(typeof(TViewModel)) as TViewModel;
            if (viewModel != null)
            {
                _navigationStack.Push(viewModel);

                // Resolve MainViewModel at use time and update CurrentViewModel
                var mainVm = _serviceProvider.GetService<MainViewModel>();
                if (mainVm != null)
                {
                    mainVm.CurrentViewModel = viewModel!;
                }
            }
        }

        public void GoBack()
        {
            if (_navigationStack.Count > 1)
            {
                _navigationStack.Pop();
                var viewModel = _navigationStack.Peek();

                var mainVm = _serviceProvider.GetService<MainViewModel>();
                if (mainVm != null)
                {
                    mainVm.CurrentViewModel = viewModel;
                }
            }
        }
    }
}
