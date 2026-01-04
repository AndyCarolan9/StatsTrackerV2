using CommunityToolkit.Mvvm.ComponentModel;

namespace StatsTrackerV2.Models
{
    public partial class FileEntry : ObservableObject
    {
        [ObservableProperty]
        public string _fileName;

        public string Path { get; set; }

        public FileEntry(string fileName, string path)
        {
            FileName = fileName;
            Path = path;
        }
    }
}
