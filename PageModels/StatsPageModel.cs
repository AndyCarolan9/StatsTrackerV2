using CommunityToolkit.Mvvm.ComponentModel;

namespace StatsTrackerV2.PageModels
{
    public abstract class StatsPageModel : ObservableObject
    {
        protected abstract void LoadStatsForTeam();

        protected abstract void FilterDrawnEvents();

        protected abstract void FillGraph();

        protected abstract void CalculateScoresFromEvent();
    }
}
