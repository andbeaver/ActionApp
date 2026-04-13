using ActionApp.Data;
using ActionApp.Models.Generated;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace ActionApp.ViewModels
{
    public class MovieItem
    {
        public string TitleId { get; set; } = string.Empty;
        public string? Title { get; set; }
        public int? Year { get; set; }
        public decimal? Rating { get; set; }
        public int? DurationMinutes { get; set; }
    }
    
    public class MoviesViewModel : INotifyPropertyChanged
    {
        public ObservableCollection<MovieItem> Movies { get; }
        public ICollectionView MoviesView { get; }

        private string _searchText = string.Empty;
        public string SearchText
        {
            get => _searchText;
            set
            {
                if (_searchText == value) return;
                _searchText = value;
                OnPropertyChanged();
                MoviesView.Refresh();
            }
        }

        public MoviesViewModel()
        {
            Movies = new ObservableCollection<MovieItem>();
            MoviesView = CollectionViewSource.GetDefaultView(Movies);
            MoviesView.Filter = FilterMovie;

            _ = LoadMoviesAsync();
        }

        private bool FilterMovie(object obj)
        {
            if (obj is not MovieItem item) return false;
            if (string.IsNullOrWhiteSpace(SearchText)) return true;

            return item.Title?.IndexOf(SearchText, StringComparison.OrdinalIgnoreCase) >= 0;
        }

        public async Task LoadMoviesAsync()
        {
            try
            {
                using var db = new IMDBContext();

                var movies = await db.Titles
                    .Where(t =>
                        t.Genres.Any(g => g.Name == "Action") &&
                        t.TitleType == "movie" &&
                        t.IsAdult != true
                    )
                    .Select(t => new MovieItem
                    {
                        TitleId = t.TitleId,
                        Title = t.PrimaryTitle,
                        Year = t.StartYear,
                        DurationMinutes = t.RuntimeMinutes,
                        Rating = t.Rating != null
                            ? t.Rating.AverageRating
                            : null
                    })
                    .OrderByDescending(m => m.Rating)
                    .ThenBy(m => m.Title)
                    .Take(500)
                    .ToListAsync();

                Application.Current.Dispatcher.Invoke(() =>
                {
                    Movies.Clear();
                    foreach (var m in movies)
                        Movies.Add(m);
                });
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"ERROR loading movies: {ex.Message}");
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string? name = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}