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
    public class DirectorItem
    {
        public string NameId { get; set; } = string.Empty;
        public string? Name { get; set; }
        public int? BirthYear { get; set; }
        public int MovieCount { get; set; }
        public string? TopMovie { get; set; }
    }
    

    public class DirectorsViewModel : INotifyPropertyChanged
    {
        public ObservableCollection<DirectorItem> Directors { get; }
        public ICollectionView DirectorsView { get; }

        private string _searchText = string.Empty;
        public string SearchText
        {
            get => _searchText;
            set
            {
                if (_searchText == value) return;
                _searchText = value;
                OnPropertyChanged();
                DirectorsView.Refresh();
            }
        }

        public DirectorsViewModel()
        {
            Directors = new ObservableCollection<DirectorItem>();
            DirectorsView = CollectionViewSource.GetDefaultView(Directors);
            DirectorsView.Filter = FilterDirector;

            _ = LoadDirectorsAsync();
        }

        private bool FilterDirector(object obj)
        {
            if (obj is not DirectorItem item) return false;
            if (string.IsNullOrWhiteSpace(SearchText)) return true;

            return item.Name?.IndexOf(SearchText, StringComparison.OrdinalIgnoreCase) >= 0;
        }

        public async Task LoadDirectorsAsync()
        {
            try
            {
                using var db = new IMDBContext();

                // Action movie IDs
                var actionTitles =
                    db.Titles
                      .Where(t => t.Genres.Any(g => g.Name == "Action")
                               && t.TitleType == "movie"
                               && t.IsAdult != true);

                // Directors grouped by NameId
                var directorStats = await db.Principals
                    .Where(p =>
                        p.JobCategory == "director" &&
                        p.NameId != null)
                    .Join(
                        actionTitles,
                        p => p.TitleId,
                        t => t.TitleId,
                        (p, t) => new { p.NameId, Title = t }
                    )
                    .GroupBy(x => x.NameId!)
                    .Select(g => new
                    {
                        NameId = g.Key,
                        MovieCount = g.Select(x => x.Title.TitleId).Distinct().Count(),
                        TopMovieTitleId =
                            g.OrderByDescending(x => x.Title.Rating!.AverageRating)
                             .Select(x => x.Title.TitleId)
                             .FirstOrDefault()
                    })
                    .Take(500)
                    .ToListAsync();

                var nameIds = directorStats.Select(d => d.NameId).ToList();
                var names = await db.Names
                    .Where(n => nameIds.Contains(n.NameId))
                    .ToDictionaryAsync(n => n.NameId);

                var topMovieIds = directorStats
                    .Where(d => d.TopMovieTitleId != null)
                    .Select(d => d.TopMovieTitleId!)
                    .Distinct()
                    .ToList();

                var movies = await db.Titles
                    .Where(t => topMovieIds.Contains(t.TitleId))
                    .ToDictionaryAsync(t => t.TitleId, t => t.PrimaryTitle);

                var items = directorStats.Select(d =>
                {
                    names.TryGetValue(d.NameId, out var name);
                    movies.TryGetValue(d.TopMovieTitleId ?? "", out var topMovie);

                    return new DirectorItem
                    {
                        NameId = d.NameId,
                        Name = name?.PrimaryName,
                        BirthYear = name?.BirthYear,
                        MovieCount = d.MovieCount,
                        TopMovie = topMovie
                    };
                }).ToList();

                Application.Current.Dispatcher.Invoke(() =>
                {
                    Directors.Clear();
                    foreach (var d in items)
                        Directors.Add(d);
                });
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"ERROR loading directors: {ex.Message}");
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string? name = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}