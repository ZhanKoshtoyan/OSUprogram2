using Libraries.Description_of_objects;
using System.Text.Json;

namespace Libraries.Loader;

public static class JsonLoader
{
    public static async Task UploadAsync(List<FanData>? fanCollection, string? pathJsonFile)
    {
        if (fanCollection == null)
        {
            throw new ArgumentNullException(nameof(fanCollection));
        }

        if (pathJsonFile == null)
        {
            throw new ArgumentNullException(nameof(pathJsonFile));
        }

        if (File.Exists(pathJsonFile))
        {
            Console.WriteLine("Файл уже существует. Хотите перезаписать его? (Y/N)");
            var response = Console.ReadLine();
            if (response?.ToUpper() != "Y")
            {
                return;
            }
        }

        var options = new JsonSerializerOptions
        {
            AllowTrailingCommas = true,
            WriteIndented = true
        };
        var json = JsonSerializer.Serialize(fanCollection);
        var file = File.CreateText(pathJsonFile);
        await file.WriteLineAsync(json);
        Console.WriteLine(json);
        file.Close();
    }

    public static List<FanData>? Download(string pathJsonFile)
    {
        var options = new JsonSerializerOptions
        {
            AllowTrailingCommas = true,
            WriteIndented = true
        };

        List<FanData>? restoredFanData = null;
        if (File.Exists(pathJsonFile))
        {
            try
            {
                using var streamJson = File.OpenRead(pathJsonFile);
                {
                    restoredFanData = JsonSerializer.Deserialize<List<FanData>>(streamJson, options);
                    Console.WriteLine(restoredFanData);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Произошла ошибка: {ex.Message}");
            }
        }
        else
        {
            Console.WriteLine("Файл *.json не найден");
        }

        return restoredFanData;
    }
}
