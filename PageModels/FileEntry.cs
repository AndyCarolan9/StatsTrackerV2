using CommunityToolkit.Mvvm.ComponentModel;

namespace StatsTrackerV2.Models
{
    public partial class FileEntry : ObservableObject
    {
        [ObservableProperty]
        public string _displayName;

        public string FileName;

        public string Path { get; set; }

        public DateTime FileDate { get; set; }

        public FileEntry(string fileName, string path, DateTime fileDate = new DateTime(), string displayName = "")
        {
            FileName = fileName;
            Path = path;
            FileDate = fileDate;
            DisplayName = displayName;
        }
    }
}
