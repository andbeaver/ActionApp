using ActionApp.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.ObjectModel;

namespace ActionApp_Tests
{
    [TestClass]
    public class MoviesViewModelTests
    {
        private MoviesViewModel _viewModel;

        [TestInitialize]
        public void Setup()
        {
            _viewModel = new MoviesViewModel();
        }

        [TestMethod]
        public void Constructor_InitializesCollections()
        {
            var viewModel = new MoviesViewModel();

            Assert.IsNotNull(viewModel.Movies);
            Assert.IsNotNull(viewModel.MoviesView);
            Assert.AreEqual(0, viewModel.Movies.Count);
            Assert.IsTrue(viewModel.MoviesView.CanFilter);
        }

        [TestMethod]
        public void SearchText_InitializesEmpty()
        {
            Assert.AreEqual(string.Empty, _viewModel.SearchText);
        }

        [TestMethod]
        public void SearchText_SetValue_UpdatesProperty()
        {
            _viewModel.SearchText = "Matrix";

            Assert.AreEqual("Matrix", _viewModel.SearchText);
        }

        [TestMethod]
        public void SearchText_SetValue_RaisesPropertyChangedEvent()
        {
            bool raised = false;

            _viewModel.PropertyChanged += (s, e) =>
            {
                if (e.PropertyName == nameof(MoviesViewModel.SearchText))
                    raised = true;
            };

            _viewModel.SearchText = "Mad Max";

            Assert.IsTrue(raised);
        }

        [TestMethod]
        public void SearchText_SetSameValue_DoesNotRaiseEvent()
        {
            _viewModel.SearchText = "Initial";
            bool raised = false;

            _viewModel.PropertyChanged += (s, e) => raised = true;

            _viewModel.SearchText = "Initial";

            Assert.IsFalse(raised);
        }

        [TestMethod]
        public void FilterMovie_WithNullObject_ReturnsFalse()
        {
            Assert.IsFalse(_viewModel.FilterMovie(null));
        }

        [TestMethod]
        public void FilterMovie_WithNonMovieItem_ReturnsFalse()
        {
            Assert.IsFalse(_viewModel.FilterMovie("not a movie"));
        }

        [TestMethod]
        public void FilterMovie_WithEmptySearchText_ReturnsTrue()
        {
            var movie = new MovieItem { Title = "The Matrix" };
            _viewModel.SearchText = string.Empty;

            Assert.IsTrue(_viewModel.FilterMovie(movie));
        }

        [TestMethod]
        public void FilterMovie_WithWhitespaceSearchText_ReturnsTrue()
        {
            var movie = new MovieItem { Title = "The Matrix" };
            _viewModel.SearchText = "   ";

            Assert.IsTrue(_viewModel.FilterMovie(movie));
        }

        [TestMethod]
        public void FilterMovie_WithMatchingTitle_ReturnsTrue()
        {
            var movie = new MovieItem { Title = "The Matrix" };
            _viewModel.SearchText = "Matrix";

            Assert.IsTrue(_viewModel.FilterMovie(movie));
        }

        [TestMethod]
        public void FilterMovie_WithCaseInsensitiveMatch_ReturnsTrue()
        {
            var movie = new MovieItem { Title = "The Matrix" };
            _viewModel.SearchText = "matrix";

            Assert.IsTrue(_viewModel.FilterMovie(movie));
        }

        [TestMethod]
        public void FilterMovie_WithNonMatchingTitle_ReturnsFalse()
        {
            var movie = new MovieItem { Title = "The Matrix" };
            _viewModel.SearchText = "Avatar";

            Assert.IsFalse(_viewModel.FilterMovie(movie));
        }

        [TestMethod]
        public void FilterMovie_WithNullTitleAndEmptySearch_ReturnsTrue()
        {
            var movie = new MovieItem { Title = null };
            _viewModel.SearchText = string.Empty;

            Assert.IsTrue(_viewModel.FilterMovie(movie));
        }

        [TestMethod]
        public void FilterMovie_WithNullTitleAndSearch_ReturnsFalse()
        {
            var movie = new MovieItem { Title = null };
            _viewModel.SearchText = "Test";

            Assert.IsFalse(_viewModel.FilterMovie(movie));
        }

        [TestMethod]
        public void MoviesCollection_IsObservableCollection()
        {
            Assert.IsInstanceOfType(
                _viewModel.Movies,
                typeof(ObservableCollection<MovieItem>)
            );
        }

        [TestMethod]
        public void MovieItem_HasRequiredProperties()
        {
            var movie = new MovieItem
            {
                TitleId = "tt0133093",
                Title = "The Matrix",
                Year = 1999,
                Rating = 8.7m,
                DurationMinutes = 136
            };

            Assert.AreEqual("tt0133093", movie.TitleId);
            Assert.AreEqual("The Matrix", movie.Title);
            Assert.AreEqual(1999, movie.Year);
            Assert.AreEqual(8.7m, movie.Rating);
            Assert.AreEqual(136, movie.DurationMinutes);
        }

        [TestMethod]
        public void MovieItem_TitleIdDefaultsToEmpty()
        {
            var movie = new MovieItem();

            Assert.AreEqual(string.Empty, movie.TitleId);
        }
    }
}