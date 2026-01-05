using CommunityToolkit.Mvvm.ComponentModel;
using StatsTrackerV2.Models;

namespace StatsTrackerV2.PageModels
{
    public partial class CreateMatchPageModel : ObservableObject
    {
        private Match _match;

        public CreateMatchPageModel(Match match)
        {
            _match = match; 
        }
    }
}
