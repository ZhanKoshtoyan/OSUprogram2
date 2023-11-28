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

    public static async Task<List<FanData>?> DownloadAsync(string pathJsonFile)
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
                await using Stream streamJson = File.OpenRead(pathJsonFile);
                restoredFanData = await JsonSerializer.DeserializeAsync<List<FanData>>(streamJson, options);
                Console.WriteLine(restoredFanData);
            }
            catch (JsonException ex)
            {
                Console.WriteLine($"Ошибка при десериализации данных: {ex.Message}");
            }
            catch (IOException ex)
            {
                Console.WriteLine($"Ошибка ввода-вывода: {ex.Message}");
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
