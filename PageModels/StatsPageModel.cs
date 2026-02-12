using CommunityToolkit.Mvvm.ComponentModel;
using StatsTrackerV2.Models;

namespace StatsTrackerV2.PageModels
{
    public abstract class StatsPageModel : ObservableObject
    {
        protected abstract void LoadStatsForTeam();

        protected abstract void FilterDrawnEvents();

        protected abstract void FillGraph();

        protected abstract void CalculateScoresFromEvent();

        protected abstract bool CanShowEvent(MatchEvent matchEvent);
    }
}
