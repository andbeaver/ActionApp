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
            var viewModel = new ActorsViewModel();

            Assert.IsNotNull(viewModel.Actors);
            Assert.IsNotNull(viewModel.ActorsView);
            Assert.AreEqual(0, viewModel.Actors.Count);
            Assert.IsTrue(viewModel.ActorsView.CanFilter);
        }

        [TestMethod]
        public void SearchText_InitializesEmpty()
        {
            Assert.AreEqual(string.Empty, _viewModel.SearchText);
        }

        [TestMethod]
        public void SearchText_SetValue_UpdatesProperty()
        {
            _viewModel.SearchText = "Tom";

            Assert.AreEqual("Tom", _viewModel.SearchText);
        }

        [TestMethod]
        public void SearchText_SetValue_RaisesPropertyChangedEvent()
        {
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

            _viewModel.SearchText = "Johnny";

            Assert.IsTrue(propertyChangedRaised);
            Assert.AreEqual(nameof(ActorsViewModel.SearchText), changedPropertyName);
        }

        [TestMethod]
        public void SearchText_SetSameValue_DoesNotRaiseEvent()
        {
            _viewModel.SearchText = "Initial";
            bool eventRaised = false;

            _viewModel.PropertyChanged += (s, e) =>
            {
                if (e.PropertyName == nameof(ActorsViewModel.SearchText))
                    eventRaised = true;
            };

            _viewModel.SearchText = "Initial";

            Assert.IsFalse(eventRaised);
        }

        [TestMethod]
        public void FilterActor_WithNullObject_ReturnsFalse()
        {
            var result = _viewModel.FilterActor(null);

            Assert.IsFalse(result);
        }

        [TestMethod]
        public void FilterActor_WithNonActorItem_ReturnsFalse()
        {
            var result = _viewModel.FilterActor("not an actor");

            Assert.IsFalse(result);
        }

        [TestMethod]
        public void FilterActor_WithEmptySearchText_ReturnsTrue()
        {
            var actor = new ActionApp.ViewModels.ActorItem { PrimaryName = "Tom Hanks" };
            _viewModel.SearchText = string.Empty;

            var result = _viewModel.FilterActor(actor);

            Assert.IsTrue(result);
        }

        [TestMethod]
        public void FilterActor_WithWhitespaceSearchText_ReturnsTrue()
        {
            var actor = new ActionApp.ViewModels.ActorItem { PrimaryName = "Tom Hanks" };
            _viewModel.SearchText = "   ";

            var result = _viewModel.FilterActor(actor);

            Assert.IsTrue(result);
        }

        [TestMethod]
        public void FilterActor_WithMatchingName_ReturnsTrue()
        {
            var actor = new ActionApp.ViewModels.ActorItem { PrimaryName = "Tom Hanks" };
            _viewModel.SearchText = "Tom";

            var result = _viewModel.FilterActor(actor);

            Assert.IsTrue(result);
        }

        [TestMethod]
        public void FilterActor_WithMatchingNameCaseInsensitive_ReturnsTrue()
        {
            var actor = new ActionApp.ViewModels.ActorItem { PrimaryName = "Tom Hanks" };
            _viewModel.SearchText = "tom";

            var result = _viewModel.FilterActor(actor);

            Assert.IsTrue(result);
        }

        [TestMethod]
        public void FilterActor_WithPartialMatchLastName_ReturnsTrue()
        {
            var actor = new ActionApp.ViewModels.ActorItem { PrimaryName = "Tom Hanks" };
            _viewModel.SearchText = "nks";

            var result = _viewModel.FilterActor(actor);

            Assert.IsTrue(result);
        }

        [TestMethod]
        public void FilterActor_WithNonMatchingName_ReturnsFalse()
        {
            var actor = new ActionApp.ViewModels.ActorItem { PrimaryName = "Tom Hanks" };
            _viewModel.SearchText = "Johnny";

            var result = _viewModel.FilterActor(actor);

            Assert.IsFalse(result);
        }

        [TestMethod]
        public void FilterActor_WithNullActorName_ReturnsTrue()
        {
            var actor = new ActionApp.ViewModels.ActorItem { PrimaryName = null };
            _viewModel.SearchText = string.Empty;

            var result = _viewModel.FilterActor(actor);

            Assert.IsTrue(result);
        }

        [TestMethod]
        public void FilterActor_WithNullActorNameAndSearchText_ReturnsFalse()
        {
            var actor = new ActionApp.ViewModels.ActorItem { PrimaryName = null };
            _viewModel.SearchText = "Tom";

            var result = _viewModel.FilterActor(actor);

            Assert.IsFalse(result);
        }

        [TestMethod]
        public void ActorsCollection_ImplementsObservableCollection()
        {
            Assert.IsInstanceOfType(_viewModel.Actors, typeof(ObservableCollection<ActionApp.ViewModels.ActorItem>));
        }

        [TestMethod]
        public void ActorItem_HasRequiredProperties()
        {
            var actor = new ActionApp.ViewModels.ActorItem
            {
                NameId = "nm0000001",
                PrimaryName = "Tom Hanks",
                BirthYear = 1956,
                MovieCount = 100,
                TopMovie = "Forrest Gump"
            };

            Assert.AreEqual("nm0000001", actor.NameId);
            Assert.AreEqual("Tom Hanks", actor.PrimaryName);
            Assert.AreEqual(1956, actor.BirthYear);
            Assert.AreEqual(100, actor.MovieCount);
            Assert.AreEqual("Forrest Gump", actor.TopMovie);
        }

        [TestMethod]
        public void ActorItem_NameIdDefaultsToEmpty()
        {
            var actor = new ActionApp.ViewModels.ActorItem();

            Assert.AreEqual(string.Empty, actor.NameId);
        }
    }
}

