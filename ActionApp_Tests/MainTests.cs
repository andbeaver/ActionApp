using ActionApp.Services;
using ActionApp.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;

namespace ActionApp_Tests
{
    [TestClass]
    public class MainTests
    {
        private MainViewModel _vm;

        private class FakeNavigationService : INavigationService
        {
            public void NavigateTo<TViewModel>() where TViewModel : class { }
            public void GoBack() { }
        }

        [TestInitialize]
        public void Setup()
        {
            _vm = new MainViewModel(new FakeNavigationService());
        }

        // Constructor Tests
        [TestMethod]
        public void Constructor_Initializes_CurrentViewModel_To_HomeViewModel()
        {
            // Assert that CurrentViewModel is set to a HomeViewModel instance
            Assert.IsNotNull(_vm.CurrentViewModel);
            Assert.IsInstanceOfType(_vm.CurrentViewModel, typeof(HomeViewModel));
        }

        [TestMethod]
        public void Constructor_CurrentViewModel_IsNotNull()
        {
            // Verify CurrentViewModel is not null after construction
            Assert.IsNotNull(_vm.CurrentViewModel);
        }

        // Navigation Tests
        [TestMethod]
        public void NavToHomeCommand_Exists_And_IsNotNull()
        {
            Assert.IsNotNull(_vm.NavToHomeCommand);
        }

        [TestMethod]
        public void NavToActorsCommand_Exists_And_IsNotNull()
        {
            Assert.IsNotNull(_vm.NavToActorsCommand);
        }

        [TestMethod]
        public void NavToMoviesCommand_Exists_And_IsNotNull()
        {
            Assert.IsNotNull(_vm.NavToMoviesCommand);
        }

        [TestMethod]
        public void NavToDirectorsCommand_Exists_And_IsNotNull()
        {
            Assert.IsNotNull(_vm.NavToDirectorsCommand);
        }

        [TestMethod]
        public void NavBackCommand_Exists_And_IsNotNull()
        {
            Assert.IsNotNull(_vm.NavBackCommand);
        }

        // Command Execution Tests
        [TestMethod]
        public void AllNavCommands_CanExecute_ReturnsTrue()
        {
            // All RelayCommands with no CanExecute predicate should return true
            Assert.IsTrue(_vm.NavToHomeCommand.CanExecute(null));
            Assert.IsTrue(_vm.NavToActorsCommand.CanExecute(null));
            Assert.IsTrue(_vm.NavToMoviesCommand.CanExecute(null));
            Assert.IsTrue(_vm.NavToDirectorsCommand.CanExecute(null));

            Assert.IsTrue(_vm.NavBackCommand.CanExecute(null));
        }

        [TestMethod]
        public void AllNavCommands_Execute_DoesNotThrow()
        {
            // Verify no exceptions are thrown when executing commands
            _vm.NavToHomeCommand.Execute(null);
            _vm.NavToActorsCommand.Execute(null);
            _vm.NavToMoviesCommand.Execute(null);
            _vm.NavToDirectorsCommand.Execute(null);
            _vm.NavBackCommand.Execute(null);
            // If we got here, no exception was thrown
            Assert.IsTrue(true);
        }

        [TestMethod]
        public void CurrentViewModel_Setter_Updates_Value()
        {
            var newViewModel = new object();
            _vm.CurrentViewModel = newViewModel;

            Assert.AreEqual(newViewModel, _vm.CurrentViewModel);
        }

        [TestMethod]
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

            Assert.IsTrue(propertyChanged);
            Assert.AreEqual("CurrentViewModel", changedPropertyName);
        }

        // Exit Command
        [TestMethod]
        public void ExitCommand_Exists_But_IsNotExecuted()
        {
            Assert.IsNotNull(_vm.ExitCommand);
        }
    }
}