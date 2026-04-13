using ActionApp.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.ObjectModel;

namespace ActionApp_Tests
{
    [TestClass]
    public class DirectorsViewModelTests
    {
        private DirectorsViewModel _viewModel;

        [TestInitialize]
        public void Setup()
        {
            _viewModel = new DirectorsViewModel();
        }

        [TestMethod]
        public void Constructor_InitializesCollections()
        {
            var viewModel = new DirectorsViewModel();

            Assert.IsNotNull(viewModel.Directors);
            Assert.IsNotNull(viewModel.DirectorsView);
            Assert.AreEqual(0, viewModel.Directors.Count);
            Assert.IsTrue(viewModel.DirectorsView.CanFilter);
        }

        [TestMethod]
        public void SearchText_InitializesEmpty()
        {
            Assert.AreEqual(string.Empty, _viewModel.SearchText);
        }

        [TestMethod]
        public void SearchText_SetValue_UpdatesProperty()
        {
            _viewModel.SearchText = "Nolan";

            Assert.AreEqual("Nolan", _viewModel.SearchText);
        }

        [TestMethod]
        public void SearchText_SetValue_RaisesPropertyChanged()
        {
            bool raised = false;

            _viewModel.PropertyChanged += (s, e) =>
            {
                if (e.PropertyName == nameof(DirectorsViewModel.SearchText))
                    raised = true;
            };

            _viewModel.SearchText = "Spielberg";

            Assert.IsTrue(raised);
        }

        [TestMethod]
        public void FilterDirector_WithNullObject_ReturnsFalse()
        {
            Assert.IsFalse(_viewModel.FilterDirector(null));
        }

        [TestMethod]
        public void FilterDirector_WithNonDirectorItem_ReturnsFalse()
        {
            Assert.IsFalse(_viewModel.FilterDirector("not director"));
        }

        [TestMethod]
        public void FilterDirector_WithEmptySearchText_ReturnsTrue()
        {
            var dir = new DirectorItem { Name = "Christopher Nolan" };
            _viewModel.SearchText = string.Empty;

            Assert.IsTrue(_viewModel.FilterDirector(dir));
        }

        [TestMethod]
        public void FilterDirector_WithMatchingName_ReturnsTrue()
        {
            var dir = new DirectorItem { Name = "Christopher Nolan" };
            _viewModel.SearchText = "Nolan";

            Assert.IsTrue(_viewModel.FilterDirector(dir));
        }

        [TestMethod]
        public void FilterDirector_WithCaseInsensitiveMatch_ReturnsTrue()
        {
            var dir = new DirectorItem { Name = "Christopher Nolan" };
            _viewModel.SearchText = "nolan";

            Assert.IsTrue(_viewModel.FilterDirector(dir));
        }

        [TestMethod]
        public void FilterDirector_WithNonMatchingName_ReturnsFalse()
        {
            var dir = new DirectorItem { Name = "Christopher Nolan" };
            _viewModel.SearchText = "Scott";

            Assert.IsFalse(_viewModel.FilterDirector(dir));
        }

        [TestMethod]
        public void FilterDirector_WithNullNameAndSearch_ReturnsFalse()
        {
            var dir = new DirectorItem { Name = null };
            _viewModel.SearchText = "Test";

            Assert.IsFalse(_viewModel.FilterDirector(dir));
        }

        [TestMethod]
        public void DirectorsCollection_IsObservableCollection()
        {
            Assert.IsInstanceOfType(
                _viewModel.Directors,
                typeof(ObservableCollection<DirectorItem>)
            );
        }

        [TestMethod]
        public void DirectorItem_HasRequiredProperties()
        {
            var director = new DirectorItem
            {
                NameId = "nm0634240",
                Name = "Christopher Nolan",
                BirthYear = 1970,
                MovieCount = 10,
                TopMovie = "The Dark Knight"
            };

            Assert.AreEqual("nm0634240", director.NameId);
            Assert.AreEqual("Christopher Nolan", director.Name);
            Assert.AreEqual(1970, director.BirthYear);
            Assert.AreEqual(10, director.MovieCount);
            Assert.AreEqual("The Dark Knight", director.TopMovie);
        }

        [TestMethod]
        public void DirectorItem_NameIdDefaultsToEmpty()
        {
            var director = new DirectorItem();

            Assert.AreEqual(string.Empty, director.NameId);
        }
    }
}