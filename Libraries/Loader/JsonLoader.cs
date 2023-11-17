using Libraries.Description_of_objects;
using System.Text.Json;

namespace Libraries.Loader;

public static class JsonLoader
{
    /*public static void Upload(List<FanData>? fanCollection)
    {
        var options = new JsonSerializerOptions()
        {
            AllowTrailingCommas = true,
            WriteIndented = true
        };
        var json = JsonSerializer.Serialize(fanCollection, options);
        var file = File.CreateText("Fans.json");
        file.WriteLine(json);
        Console.WriteLine(json);
        file.Close();
    }*/

    public static List<FanData>? Download(string? pathJsonFile)
    {
        List<FanData>? restoredFanData = null;

        if (File.Exists(pathJsonFile))
        {
            var json = File.ReadAllText(pathJsonFile);
            restoredFanData = JsonSerializer.Deserialize<List<FanData>?>(json);
            Console.WriteLine(restoredFanData);
        }
        else
        {
            Console.WriteLine("Файл *.json не найден");
        }

        return restoredFanData;
    }
}
