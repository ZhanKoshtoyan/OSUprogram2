using Libraries.Description_of_objects;
using System.Text.Json;

namespace Libraries.Loader;

public static class JsonLoader
{
    /*public static async void Upload(List<FanData>? fanCollection, string? pathJsonFile)
    {
        if (fanCollection == null)
        {
            throw new ArgumentNullException(nameof(fanCollection));
        }

        if (pathJsonFile == null)
        {
            throw new ArgumentNullException(nameof(pathJsonFile));
        }

        var options = new JsonSerializerOptions
        {
            AllowTrailingCommas = true,
            WriteIndented = true
        };
        var json = JsonSerializer.Serialize(fanCollection, options);
        var file = File.CreateText(pathJsonFile);
        await file.WriteLineAsync(json);
        Console.WriteLine(json);
        file.Close();
    }*/

    /// <summary>
    /// Загрузка данных по вентиляторам из файла *.json. Ассинхронный метод.
    /// </summary>
    /// <param name="pathJsonFile"></param>
    /// <returns>Список объектов FanData</returns>
    public static async Task<List<FanData>?> DownloadAsync(string? pathJsonFile)
    {
        List<FanData>? restoredFanData = null;

        if (File.Exists(pathJsonFile))
        {
            await using Stream json = File.OpenRead(pathJsonFile);
            restoredFanData =
                await JsonSerializer.DeserializeAsync<List<FanData>?>(json);
            Console.WriteLine(restoredFanData);
        }
        else
        {
            Console.WriteLine("Файл *.json не найден");
        }

        return restoredFanData;
    }
}
