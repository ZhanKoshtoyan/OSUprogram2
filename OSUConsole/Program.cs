using Libraries;

// async Task Main(string[] args)
//{

Console.WriteLine("Введите объемный расход воздуха, [м3/ч]: ");
// var inputVolumeFlow = Console.ReadLine();
var inputVolumeFlow = "4000";

Console.WriteLine("Введите полное давление воздуха, [Па]: ");
// var inputTotalPressure = Console.ReadLine();
var inputTotalPressure = "300";

Console.WriteLine(
    "Введите допустимую погрешность подбора по полному давлению воздуха, [%]: "
);
// var inputTotalPressureDeviation = Console.ReadLine();
var inputTotalPressureDeviation = "50";

Console.WriteLine("Введите температуру ежедневной эксплуатации, [°C]: ");
// var inputTemperature = Console.ReadLine();
var inputTemperature = "20";

Console.WriteLine(
    "Введите относительную влажность этой температуры (не обязательно), [%]: "
);
// var inputRelativeHumidity = Console.ReadLine();
var inputRelativeHumidity = "50";

Console.WriteLine(
    "Введите высоту над уровнем моря (не обязательно), [м]: "
);
// var inputAltitude = Console.ReadLine();
var inputAltitude = "20";

var input = new InputValidation(
    inputVolumeFlow,
    inputTotalPressure,
    inputTotalPressureDeviation,
    inputTemperature,
    inputRelativeHumidity,
    inputAltitude
);

var f = true;

if (f)
{
    await DatabaseLoader.LoadDataToDatabase();
}
// }
