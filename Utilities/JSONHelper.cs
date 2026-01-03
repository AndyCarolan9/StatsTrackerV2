using System.Text.Json;

namespace StatsTrackerV2.Utilities
{
    public static class JSONHelper
    {
        public static T? LoadFromJsonFile<T>(string filePath)
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

            return default;
        }

        public static void SaveToJsonFile<T>(string filePath, T? objectToSave)
        {
            using (StreamWriter sw = new StreamWriter(filePath))
            {
                var options = new JsonSerializerOptions()
                {
                    WriteIndented = true,
                    Converters = {
                    new ColorJsonConverter()
                }
                };

                string jsonString = JsonSerializer.Serialize(objectToSave, options);
                sw.Write(jsonString);
            }
        }

        /*public static string GetFilePath(string fileName)
        {
            return Application.StartupPath + "\\" + fileName;
        }*/
    }
}