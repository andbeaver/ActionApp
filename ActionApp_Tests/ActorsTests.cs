using ActionApp.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ActionApp.Models;
using ActionApp.ViewModels;

namespace ActionApp_Tests
{

    [TestClass]
    public class ActorsViewModelTests
    {
        private ActorsViewModel _viewModel;

        [TestInitialize]
        public void Setup()
        {
            _viewModel = new ActorsViewModel();
        }

        [TestMethod]
        public void Constructor_InitializesCollections()
        {
            // Arrange & Act
            var viewModel = new ActorsViewModel();

            // Assert
            Assert.IsNotNull(viewModel.Actors);
            Assert.IsNotNull(viewModel.ActorsView);
            Assert.AreEqual(0, viewModel.Actors.Count);
            Assert.IsTrue(viewModel.ActorsView.CanFilter);
        }

        [TestMethod]
        public void SearchText_InitializesEmpty()
        {
            // Assert
            Assert.AreEqual(string.Empty, _viewModel.SearchText);
        }

        [TestMethod]
        public void SearchText_SetValue_UpdatesProperty()
        {
            // Act
            _viewModel.SearchText = "Tom";

            // Assert
            Assert.AreEqual("Tom", _viewModel.SearchText);
        }

        [TestMethod]
        public void SearchText_SetValue_RaisesPropertyChangedEvent()
        {
            // Arrange
            bool propertyChangedRaised = false;
            string changedPropertyName = null;

            _viewModel.PropertyChanged += (s, e) =>
            {
                if (e.PropertyName == nameof(ActorsViewModel.SearchText))
                {
                    propertyChangedRaised = true;
                    changedPropertyName = e.PropertyName;
                }
            };

            // Act
            _viewModel.SearchText = "Johnny";

            // Assert
            Assert.IsTrue(propertyChangedRaised);
            Assert.AreEqual(nameof(ActorsViewModel.SearchText), changedPropertyName);
        }

        [TestMethod]
        public void SearchText_SetSameValue_DoesNotRaiseEvent()
        {
            // Arrange
            _viewModel.SearchText = "Initial";
            bool eventRaised = false;

            _viewModel.PropertyChanged += (s, e) =>
            {
                if (e.PropertyName == nameof(ActorsViewModel.SearchText))
                    eventRaised = true;
            };

            // Act
            _viewModel.SearchText = "Initial";

            // Assert
            Assert.IsFalse(eventRaised);
        }

        [TestMethod]
        public void FilterActor_WithNullObject_ReturnsFalse()
        {
            // Act
            var result = _viewModel.FilterActor(null);

            // Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void FilterActor_WithNonActorItem_ReturnsFalse()
        {
            // Act
            var result = _viewModel.FilterActor("not an actor");

            // Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void FilterActor_WithEmptySearchText_ReturnsTrue()
        {
            // Arrange
            var actor = new ActionApp.ViewModels.ActorItem { PrimaryName = "Tom Hanks" };
            _viewModel.SearchText = string.Empty;

            // Act
            var result = _viewModel.FilterActor(actor);

            // Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void FilterActor_WithWhitespaceSearchText_ReturnsTrue()
        {
            // Arrange
            var actor = new ActionApp.ViewModels.ActorItem { PrimaryName = "Tom Hanks" };
            _viewModel.SearchText = "   ";

            // Act
            var result = _viewModel.FilterActor(actor);

            // Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void FilterActor_WithMatchingName_ReturnsTrue()
        {
            // Arrange
            var actor = new ActionApp.ViewModels.ActorItem { PrimaryName = "Tom Hanks" };
            _viewModel.SearchText = "Tom";

            // Act
            var result = _viewModel.FilterActor(actor);

            // Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void FilterActor_WithMatchingNameCaseInsensitive_ReturnsTrue()
        {
            // Arrange
            var actor = new ActionApp.ViewModels.ActorItem { PrimaryName = "Tom Hanks" };
            _viewModel.SearchText = "tom";

            // Act
            var result = _viewModel.FilterActor(actor);

            // Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void FilterActor_WithPartialMatchLastName_ReturnsTrue()
        {
            // Arrange
            var actor = new ActionApp.ViewModels.ActorItem { PrimaryName = "Tom Hanks" };
            _viewModel.SearchText = "nks";

            // Act
            var result = _viewModel.FilterActor(actor);

            // Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void FilterActor_WithNonMatchingName_ReturnsFalse()
        {
            // Arrange
            var actor = new ActionApp.ViewModels.ActorItem { PrimaryName = "Tom Hanks" };
            _viewModel.SearchText = "Johnny";

            // Act
            var result = _viewModel.FilterActor(actor);

            // Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void FilterActor_WithNullActorName_ReturnsTrue()
        {
            // Arrange
            var actor = new ActionApp.ViewModels.ActorItem { PrimaryName = null };
            _viewModel.SearchText = string.Empty;

            // Act
            var result = _viewModel.FilterActor(actor);

            // Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void FilterActor_WithNullActorNameAndSearchText_ReturnsFalse()
        {
            // Arrange
            var actor = new ActionApp.ViewModels.ActorItem { PrimaryName = null };
            _viewModel.SearchText = "Tom";

            // Act
            var result = _viewModel.FilterActor(actor);

            // Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void ActorsCollection_ImplementsObservableCollection()
        {
            // Assert
            Assert.IsInstanceOfType(_viewModel.Actors, typeof(ObservableCollection<ActionApp.ViewModels.ActorItem>));
        }

        [TestMethod]
        public void ActorItem_HasRequiredProperties()
        {
            // Arrange
            var actor = new ActionApp.ViewModels.ActorItem
            {
                NameId = "nm0000001",
                PrimaryName = "Tom Hanks",
                BirthYear = 1956,
                MovieCount = 100,
                TopMovie = "Forrest Gump"
            };

            // Assert
            Assert.AreEqual("nm0000001", actor.NameId);
            Assert.AreEqual("Tom Hanks", actor.PrimaryName);
            Assert.AreEqual(1956, actor.BirthYear);
            Assert.AreEqual(100, actor.MovieCount);
            Assert.AreEqual("Forrest Gump", actor.TopMovie);
        }

        [TestMethod]
        public void ActorItem_NameIdDefaultsToEmpty()
        {
            // Act
            var actor = new ActionApp.ViewModels.ActorItem();

            // Assert
            Assert.AreEqual(string.Empty, actor.NameId);
        }
    }
}

