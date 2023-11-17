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

    public static async Task<List<FanData>?> DownloadAsync(string? pathJsonFile)
    {
        List<FanData>? restoredFanData = null;

        if (File.Exists(pathJsonFile))
        {
            await using Stream json = File.OpenRead(pathJsonFile);
            restoredFanData = await JsonSerializer.DeserializeAsync<List<FanData>?>(json);
            Console.WriteLine(restoredFanData);
        }
        else
        {
            Console.WriteLine("Файл *.json не найден");
        }

        return restoredFanData;
    }
}
