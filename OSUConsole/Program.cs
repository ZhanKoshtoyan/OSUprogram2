using FluentValidation;
using Libraries;
using System.Text.Json;

Console.WriteLine("Введите объемный расход воздуха, [м3/ч]: ");

// var inputVolumeFlow = Console.ReadLine();
var inputVolumeFlow = "4000";
var result = double.TryParse(
    inputVolumeFlow.Replace(".", ","),
    out var doubleVolumeFlow
);
if (!result)
{
    throw new ArgumentException(
        "Значение \"Объем воздуха\" не является числом. Вентиляторы не могут быть подобраны."
    );
}

//-----------------------------------------------------------------------------------------------------------
Console.WriteLine("Введите полное давление воздуха, [Па]: ");

// var inputTotalPressure = Console.ReadLine();
var inputTotalPressure = "350";
result = double.TryParse(
    inputTotalPressure.Replace(".", ","),
    out var doubleTotalPressure
);
if (!result)
{
    throw new ArgumentException(
        "Значение \"Полное давление воздуха\" не является числом. Вентиляторы не могут быть подобраны."
    );
}

//-----------------------------------------------------------------------------------------------------------
Console.WriteLine(
    "Введите допустимую погрешность подбора по полному давлению воздуха, [%]: "
);

// var inputTotalPressureDeviation = Console.ReadLine();
var inputTotalPressureDeviation = "30";
result = double.TryParse(
    inputTotalPressureDeviation.Replace(".", ","),
    out var doubleTotalPressureDeviation
);
if (!result)
{
    throw new ArgumentException(
        "Значение \"Допустимая погрешность подбора\" не является числом. Вентиляторы не могут быть подобраны."
    );
}

//-----------------------------------------------------------------------------------------------------------
Console.WriteLine("Введите температуру ежедневной эксплуатации, [°C]: ");

// var inputTemperature = Console.ReadLine();
var inputTemperature = "20";
result = double.TryParse(
    inputTemperature.Replace(".", ","),
    out var doubleTemperature
);
if (!result)
{
    throw new ArgumentException(
        "Значение \"Температура ежедневной эксплуатации\" не является числом. Вентиляторы не могут быть подобраны."
    );
}

//-----------------------------------------------------------------------------------------------------------
Console.WriteLine(
    "Введите относительную влажность этой температуры (не обязательно), [%]: "
);

// var inputRelativeHumidity = Console.ReadLine();
var inputRelativeHumidity = "";
result = double.TryParse(
    inputRelativeHumidity.Replace(".", ","),
    out var doubleRelativeHumidity
);
if (!result && !string.IsNullOrEmpty(inputRelativeHumidity))
{
    throw new ArgumentException(
        "Значение \"Относительная влажность\" не является числом. Вентиляторы не могут быть подобраны."
    );
}

//-----------------------------------------------------------------------------------------------------------

Console.WriteLine("Введите высоту над уровнем моря (не обязательно), [м]: ");

// var inputAltitude = Console.ReadLine();
var inputAltitude = "";
result = double.TryParse(
    inputAltitude.Replace(".", ","),
    out var doubleAltitude
);
if (!result && !string.IsNullOrEmpty(inputAltitude))
{
    throw new ArgumentException(
        "Значение \"Высота над уровнем моря\" не является числом. Вентиляторы не могут быть подобраны."
    );
}

//-----------------------------------------------------------------------------------------------------------
//Ввод адреса к файлу Json
Console.WriteLine(
    "Укажите путь файла Json (по умолчанию: \"C:\\My ProjectCSharp\\OSUprogram2\\OSUprogram2\\OSUConsole\\bin\\Debug\\net7.0\\Fans.json\"):"
);
string? pathJsonFile = null;
var input = Console.ReadLine();

if (input == "")
{
    pathJsonFile =
        "C:\\My ProjectCSharp\\OSUprogram2\\OSUprogram2\\OSUConsole\\bin\\Debug\\net7.0\\Fans.json";
}

//-----------------------------------------------------------------------------------------------------------

var userInput = new UserInput
{
    VolumeFlow = doubleVolumeFlow,
    TotalPressure = doubleTotalPressure,
    TotalPressureDeviation = doubleTotalPressureDeviation,
    Temperature = doubleTemperature,
    RelativeHumidity = doubleRelativeHumidity,
    Altitude = doubleAltitude
};

var validator = new UserInputValidator();

validator.ValidateAndThrow(userInput);

/*var resultValidation = validator.Validate(userInput);
var allMessages = resultValidation.ToString();

if (!string.IsNullOrEmpty(allMessages))
{
    throw new ArgumentException(allMessages);
}*/

var fansList = JsonLoader.Download(pathJsonFile);

var sortFans = SortFans.Sort(fansList, userInput);
ToPrint.Print(sortFans, userInput);
