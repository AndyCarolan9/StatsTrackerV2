using CommunityToolkit.Mvvm.ComponentModel;
using StatsTrackerV2.Models;

namespace StatsTrackerV2.PageModels
{
    public partial class KickoutStatsPageModel : ObservableObject
    {
        private readonly Match _match;

        public StatisticDotDrawable DotDrawable { get; } = new();

        public KickoutStatsPageModel(Match match)
        {
            _match = match;
        }
    }
}
