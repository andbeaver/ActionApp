using ActionApp.Data;
using ActionApp.Models;
using ActionApp.Models.Generated;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace ActionApp.ViewModels
{
    public class ActorsViewModel : INotifyPropertyChanged
    {
        public ObservableCollection<ActorItem> Actors { get; }

        public ICollectionView ActorsView { get; }

        private string _searchText = string.Empty;
        public string SearchText
        {
            get => _searchText;
            set
            {
                if (_searchText == value) return;
                _searchText = value;
                OnPropertyChanged();
                ActorsView.Refresh();
            }
        }

        public ActorsViewModel()
        {
            Actors = new ObservableCollection<ActorItem>();
            ActorsView = CollectionViewSource.GetDefaultView(Actors);
            ActorsView.Filter = FilterActor;

            // Start loading asynchronously — updates will be dispatched to UI thread
            _ = LoadActorsAsync();
        }

        public bool FilterActor(object obj)
        {
            if (obj is not ActorItem item) return false;
            if (string.IsNullOrWhiteSpace(SearchText)) return true;
            return item.PrimaryName?.IndexOf(SearchText, StringComparison.OrdinalIgnoreCase) >= 0;
        }

        public async Task LoadActorsAsync()
        {
            try
            {
                // Query top actors server-side to avoid loading entire tables into memory
                using var db = new IMDBContext();

                // restrict to Action genre titles
                var actionTitleIds = db.Titles
                    .Where(t => t.Genres.Any(g => g.Name == "Action"))
                    .Select(t => t.TitleId);

                // group principals by NameId (server-side) then join with Names to filter BirthYear > 1900
                var statsQuery = db.Principals
                    .Where(p => p.NameId != null && actionTitleIds.Contains(p.TitleId))
                    .GroupBy(p => p.NameId!)
                    .Select(g => new
                    {
                        NameId = g.Key,
                        MovieCount = g.Select(x => x.TitleId).Distinct().Count(),
                        FirstTitleId = g.Select(x => x.TitleId).FirstOrDefault()
                    });

                var statsWithNamesQuery = statsQuery
                    .Join(db.Names.Where(n => n.BirthYear != null && n.BirthYear > 1900),
                        s => s.NameId,
                        n => n.NameId,
                        (s, n) => new { s.NameId, s.MovieCount, s.FirstTitleId, n.PrimaryName, n.BirthYear });

                // take up to 500 actors matching the criteria
                var stats = await statsWithNamesQuery
                    .OrderByDescending(s => s.MovieCount)
                    .ThenBy(s => s.NameId)
                    .Take(500)
                    .ToListAsync();

                var titleIds = stats.Where(s => !string.IsNullOrEmpty(s.FirstTitleId)).Select(s => s.FirstTitleId!).Distinct().ToList();
                var titles = await db.Titles.Where(t => titleIds.Contains(t.TitleId)).ToListAsync();
                var titleLookup = titles.ToDictionary(t => t.TitleId, t => t.PrimaryTitle);

                var items = stats.Select(s =>
                {
                    titleLookup.TryGetValue(s.FirstTitleId ?? string.Empty, out var top);
                    return new ActorItem
                    {
                        NameId = s.NameId,
                        PrimaryName = s.PrimaryName,
                        BirthYear = s.BirthYear,
                        MovieCount = s.MovieCount,
                        TopMovie = top
                    };
                }).ToList();

                // Update collection on UI thread
                Application.Current.Dispatcher.Invoke(() =>
                {
                    Actors.Clear();
                    foreach (var it in items)
                        Actors.Add(it);
                });
            }
            catch (Exception ex)
            {
                // Log error for debugging
                System.Diagnostics.Debug.WriteLine($"ERROR loading actors: {ex.Message}");
                System.Diagnostics.Debug.WriteLine(ex.StackTrace);
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string? name = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));

    }

    public class ActorItem
    {
        public string NameId { get; set; } = string.Empty;
        public string? PrimaryName { get; set; }
        public int? BirthYear { get; set; }
        public int MovieCount { get; set; }
        public string? TopMovie { get; set; }
    }
}
