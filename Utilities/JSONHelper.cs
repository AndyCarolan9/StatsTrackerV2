using System.Text.Json;

namespace StatsTrackerV2.Utilities
{
    public static class JSONHelper
    {
        public static T? LoadFromJsonFile<T>(string filePath)
        {
            try
            {
                if (!File.Exists(filePath))
                {
                    return default;
                }

                using (StreamReader sr = new StreamReader(filePath))
                {
                    var options = new JsonSerializerOptions()
                    {
                        WriteIndented = true,
                        Converters = {
                    new ColorJsonConverter()
                }
                    };

                    string jsonString = sr.ReadToEnd();
                    T? json = JsonSerializer.Deserialize<T>(jsonString, options);

                    if (json != null)
                    {
                        return json;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }

            return default;
        }

        public static void SaveToJsonFile<T>(string filePath, T? objectToSave)
        {
            try
            {
                if (!File.Exists(filePath))
                {
                    string? directoryPath = Path.GetDirectoryName(filePath);
                    if(directoryPath == null)
                    {
                        return; 
                    }

                    if (!Directory.Exists(directoryPath))
                    {
                        Directory.CreateDirectory(directoryPath);
                    }

                    FileStream fs = File.Create(filePath);
                    fs.Close();
                }

                using (StreamWriter sw = new StreamWriter(filePath))
                {
                    var options = new JsonSerializerOptions()
                    {
                        WriteIndented = true,
                        Converters = 
                        {
                            new ColorJsonConverter()
                        }
                    };

                    string jsonString = JsonSerializer.Serialize(objectToSave, options);
                    sw.Write(jsonString);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        public static async Task<string?> ImportMatchJson()
        {
            var jsonFileType = new FilePickerFileType(new Dictionary<DevicePlatform, IEnumerable<string>>
                {
                    { DevicePlatform.WinUI, new[] { ".json" } },
                    { DevicePlatform.Android, new[] { "application/json" } },
                    { DevicePlatform.iOS, new[] { "public.json" } },
                    { DevicePlatform.MacCatalyst, new[] { "public.json" } },// UTType values
                });

            FileBase? result = await FilePicker.Default.PickAsync(new PickOptions
            {
                PickerTitle = "Select a Match JSON file",
                FileTypes = jsonFileType
            });

            if (result == null)
            {
                return null;
            }

            string destinationPath = Path.Combine(Constants.MatchesFolderPath, result.FileName);

            if (File.Exists(destinationPath))
            {
                await AppShell.DisplayToastAsync("File Already Exists");
                return destinationPath;
            }

            try
            {
                using var sourceStream = await result.OpenReadAsync();
                using var destinationStream = File.Create(destinationPath);
                await sourceStream.CopyToAsync(destinationStream);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return destinationPath;
        }
    }
}