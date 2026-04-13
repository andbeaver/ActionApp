using ActionApp.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using Xunit;
using ActionApp.Models;

namespace ActionApp_Tests
{
    public class ActorsViewModelTests
    {
        private ActorsViewModel _viewModel;

        public ActorsViewModelTests()
        {
            _viewModel = new ActorsViewModel();
        }

        [Fact]
        public void Constructor_InitializesCollections()
        {
            // Arrange & Act
            var viewModel = new ActorsViewModel();

            // Assert
            Assert.NotNull(viewModel.Actors);
            Assert.NotNull(viewModel.ActorsView);
            Assert.Empty(viewModel.Actors);
            Assert.True(viewModel.ActorsView.CanFilter);
        }

        [Fact]
        public void SearchText_InitializesEmpty()
        {
            // Assert
            Assert.Equal(string.Empty, _viewModel.SearchText);
        }

        [Fact]
        public void SearchText_SetValue_UpdatesProperty()
        {
            // Act
            _viewModel.SearchText = "Tom";

            // Assert
            Assert.Equal("Tom", _viewModel.SearchText);
        }

        [Fact]
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
            Assert.True(propertyChangedRaised);
            Assert.Equal(nameof(ActorsViewModel.SearchText), changedPropertyName);
        }

        [Fact]
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
            Assert.False(eventRaised);
        }

        [Fact]
        public void FilterActor_WithNullObject_ReturnsFalse()
        {
            // Act
            var result = _viewModel.FilterActor(null);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void FilterActor_WithNonActorItem_ReturnsFalse()
        {
            // Act
            var result = _viewModel.FilterActor("not an actor");

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void FilterActor_WithEmptySearchText_ReturnsTrue()
        {
            // Arrange
            var actor = new ActionApp.ViewModels.ActorItem { PrimaryName = "Tom Hanks" };
            _viewModel.SearchText = string.Empty;

            // Act
            var result = _viewModel.FilterActor(actor);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void FilterActor_WithWhitespaceSearchText_ReturnsTrue()
        {
            // Arrange
            var actor = new ActionApp.ViewModels.ActorItem { PrimaryName = "Tom Hanks" };
            _viewModel.SearchText = "   ";

            // Act
            var result = _viewModel.FilterActor(actor);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void FilterActor_WithMatchingName_ReturnsTrue()
        {
            // Arrange
            var actor = new ActionApp.ViewModels.ActorItem { PrimaryName = "Tom Hanks" };
            _viewModel.SearchText = "Tom";

            // Act
            var result = _viewModel.FilterActor(actor);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void FilterActor_WithMatchingNameCaseInsensitive_ReturnsTrue()
        {
            // Arrange
            var actor = new ActionApp.ViewModels.ActorItem { PrimaryName = "Tom Hanks" };
            _viewModel.SearchText = "tom";

            // Act
            var result = _viewModel.FilterActor(actor);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void FilterActor_WithPartialMatchLastName_ReturnsTrue()
        {
            // Arrange
            var actor = new ActionApp.ViewModels.ActorItem { PrimaryName = "Tom Hanks" };
            _viewModel.SearchText = "nks";

            // Act
            var result = _viewModel.FilterActor(actor);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void FilterActor_WithNonMatchingName_ReturnsFalse()
        {
            // Arrange
            var actor = new ActionApp.ViewModels.ActorItem { PrimaryName = "Tom Hanks" };
            _viewModel.SearchText = "Johnny";

            // Act
            var result = _viewModel.FilterActor(actor);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void FilterActor_WithNullActorName_ReturnsTrue()
        {
            // Arrange
            var actor = new ActionApp.ViewModels.ActorItem { PrimaryName = null };
            _viewModel.SearchText = string.Empty;

            // Act
            var result = _viewModel.FilterActor(actor);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void FilterActor_WithNullActorNameAndSearchText_ReturnsFalse()
        {
            // Arrange
            var actor = new ActionApp.ViewModels.ActorItem { PrimaryName = null };
            _viewModel.SearchText = "Tom";

            // Act
            var result = _viewModel.FilterActor(actor);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void ActorsCollection_ImplementsObservableCollection()
        {
            // Assert
            Assert.IsType<ObservableCollection<ActionApp.ViewModels.ActorItem>>(_viewModel.Actors);
        }

        [Fact]
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
            Assert.Equal("nm0000001", actor.NameId);
            Assert.Equal("Tom Hanks", actor.PrimaryName);
            Assert.Equal(1956, actor.BirthYear);
            Assert.Equal(100, actor.MovieCount);
            Assert.Equal("Forrest Gump", actor.TopMovie);
        }

        [Fact]
        public void ActorItem_NameIdDefaultsToEmpty()
        {
            // Act
            var actor = new ActionApp.ViewModels.ActorItem();

            // Assert
            Assert.Equal(string.Empty, actor.NameId);
        }
    }
}