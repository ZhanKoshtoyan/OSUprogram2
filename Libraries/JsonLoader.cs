
using System.Text.Json;

namespace Libraries;

internal class Program
{
    public static void Main(string[] args)
    {
        Person tom = new Person("Tom", 37);
        string json = JsonSerializer.Serialize(tom);
        Console.WriteLine(json);
        Person? restoredPerson = JsonSerializer.Deserialize<Person>(json);
        Console.WriteLine(restoredPerson?.Name); // Tom
    }
}

class Person
{
    public string Name { get;}
    public int Age { get; set; }
    public Person(string name, int age)
    {
        Name = name;
        Age = age;
    }
}

public static class JsonLoader
{
    public static void Upload(List<FanData>? fanCollection)
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
    }

    public static List<FanData>? Download()
    {
        List<FanData>? restoredFanData = null;
        if (File.Exists("Fans.json"))
        {
            var json = File.ReadAllText("Fans.json");
            restoredFanData = JsonSerializer.Deserialize<List<FanData>?>(json);
            Console.WriteLine(restoredFanData);
        }
        else
        {
            Console.WriteLine("Файл Fans.json не найден");
        }

        return restoredFanData;
    }
}

