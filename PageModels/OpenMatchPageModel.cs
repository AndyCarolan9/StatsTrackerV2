using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using StatsTrackerV2.Models;
using System.Collections.ObjectModel;

namespace StatsTrackerV2.PageModels
{
    public partial class OpenMatchPageModel : ObservableObject
    {
        private Match _match;

        [ObservableProperty]
        private ObservableCollection<FileEntry> _matches = [];

        [ObservableProperty]
        private FileEntry? selectedEntry;

        public OpenMatchPageModel(Match match)
        {
            _match = match;
        }

        [RelayCommand]
        private async Task Appearing()
        {
           string directory = Constants.MatchesFolderPath;
            try
            {
                foreach (string file in Directory.EnumerateFiles(directory))
                {
                    if (!file.Contains(".json"))
                    {
                        continue;
                    }

                    string[] pathComponents;

                    DevicePlatform platform = DeviceInfo.Platform;

                    if (platform == DevicePlatform.Android)
                    {
                        pathComponents = file.Split("/");
                    }
                    else
                    {
                        pathComponents = file.Split("\\");
                    }


                    if(pathComponents.Length == 0)
                    {
                        continue; 
                    }

                    string fileName = pathComponents.Last();

                    string[] fileComponents = fileName.Split(".");
                    if(fileComponents.Length != 2)
                    {
                        continue;
                    }

                    string[] matchDetails = fileComponents[0].Split("_");
                    if (matchDetails.Length != 5)
                    {
                        continue;
                    }

                    string homeTeam = System.Text.RegularExpressions.Regex.Replace(matchDetails[0], "([A-Z])(?![A-Z])", " $1");
                    string awayTeam = System.Text.RegularExpressions.Regex.Replace(matchDetails[1], "([A-Z])(?![A-Z])", " $1");

                    string displayName = $"{homeTeam} V {awayTeam} - {matchDetails[2]}/{matchDetails[3]}/{matchDetails[4]}";

                    Matches.Add(new FileEntry(displayName, file));
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        [RelayCommand]
        private async Task SelectMatch(FileEntry entry)
        {
            string filePath = Path.Combine(entry.Path);
            Match? match = JSONHelper.LoadFromJsonFile<Match>(filePath);
            if (match is not null)
            {
                _match.HydrateObject(match);
            }

            await Shell.Current.GoToAsync("..");
        }

        [RelayCommand]
        private async Task ImportMatch()
        {
            string? jsonPath = await JSONHelper.ImportMatchJson();

            if (jsonPath is not null)
            {
                FileEntry entry = new FileEntry("", jsonPath);
                await SelectMatch(entry);
            }
        }
    }
}
