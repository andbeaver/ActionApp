using ActionApp.Services;
using ActionApp.ViewModels;
using Xunit;
using System;
using System.Collections.Generic;
using System.Text;

namespace ActionApp_Tests
{
    public class MainTests
    {
        private MainViewModel _vm;

        private class FakeNavigationService : INavigationService
        {
            public void NavigateTo<TViewModel>() where TViewModel : class { }
            public void GoBack() { }
        }

        public MainTests()
        {
            _vm = new MainViewModel(new FakeNavigationService());
        }

        // Constructor Tests
        [Fact]
        public void Constructor_Initializes_CurrentViewModel_To_HomeViewModel()
        {
            // Assert that CurrentViewModel is set to a HomeViewModel instance
            Assert.NotNull(_vm.CurrentViewModel);
            Assert.IsType<HomeViewModel>(_vm.CurrentViewModel);
        }

        [Fact]
        public void Constructor_CurrentViewModel_IsNotNull()
        {
            // Verify CurrentViewModel is not null after construction
            Assert.NotNull(_vm.CurrentViewModel);
        }

        // Navigation Tests
        [Fact]
        public void NavToHomeCommand_Exists_And_IsNotNull()
        {
            Assert.NotNull(_vm.NavToHomeCommand);
        }

        [Fact]
        public void NavToActorsCommand_Exists_And_IsNotNull()
        {
            Assert.NotNull(_vm.NavToActorsCommand);
        }

        [Fact]
        public void NavToMoviesCommand_Exists_And_IsNotNull()
        {
            Assert.NotNull(_vm.NavToMoviesCommand);
        }

        [Fact]
        public void NavToDirectorsCommand_Exists_And_IsNotNull()
        {
            Assert.NotNull(_vm.NavToDirectorsCommand);
        }

        [Fact]
        public void NavBackCommand_Exists_And_IsNotNull()
        {
            Assert.NotNull(_vm.NavBackCommand);
        }

        // Command Execution Tests
        [Fact]
        public void AllNavCommands_CanExecute_ReturnsTrue()
        {
            // All RelayCommands with no CanExecute predicate should return true
            Assert.True(_vm.NavToHomeCommand.CanExecute(null));
            Assert.True(_vm.NavToActorsCommand.CanExecute(null));
            Assert.True(_vm.NavToMoviesCommand.CanExecute(null));
            Assert.True(_vm.NavToDirectorsCommand.CanExecute(null));

            Assert.True(_vm.NavBackCommand.CanExecute(null));
        }

        [Fact]
        public void AllNavCommands_Execute_DoesNotThrow()
        {
            // Verify no exceptions are thrown when executing commands
            _vm.NavToHomeCommand.Execute(null);
            _vm.NavToActorsCommand.Execute(null);
            _vm.NavToMoviesCommand.Execute(null);
            _vm.NavToDirectorsCommand.Execute(null);
            _vm.NavBackCommand.Execute(null);
            // If we got here, no exception was thrown
        }

        [Fact]
        public void CurrentViewModel_Setter_Updates_Value()
        {
            var newViewModel = new object();
            _vm.CurrentViewModel = newViewModel;

            Assert.Same(newViewModel, _vm.CurrentViewModel);
        }

        [Fact]
        public void CurrentViewModel_Setter_Raises_PropertyChanged()
        {

            var propertyChanged = false;
            var changedPropertyName = string.Empty;

            _vm.PropertyChanged += (s, e) =>
            {
                propertyChanged = true;
                changedPropertyName = e.PropertyName;
            };

            var newViewModel = new object();
            _vm.CurrentViewModel = newViewModel;

            Assert.True(propertyChanged);
            Assert.Equal("CurrentViewModel", changedPropertyName);
        }

        // Exit Command
        [Fact]
        public void ExitCommand_Exists_But_IsNotExecuted()
        {
            Assert.NotNull(_vm.ExitCommand);
        }
    }
}
