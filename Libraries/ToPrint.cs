using Libraries.Description_of_objects.UserInput;
using Libraries.SomeFan;

namespace Libraries;

public static class ToPrint
{
    public static void Print(List<IFan> sortFans, UserInput userInput)
    {
        string? endingOfTheWord1;
        string? endingOfTheWord2;
        switch (sortFans.Count)
        {
            case 1:
                endingOfTheWord1 = "подобран";
                endingOfTheWord2 = "вентилятор";
                break;
            case 2:
            case 3:
            case 4:
                endingOfTheWord1 = "подобрано";
                endingOfTheWord2 = "вентилятора";
                break;
            default:
                endingOfTheWord1 = "подобраны";
                endingOfTheWord2 = "вентиляторов";
                break;
        }

        Console.WriteLine($"\nВсего {endingOfTheWord1} {sortFans.Count} {endingOfTheWord2}.");

        foreach (var fan in sortFans)
        {
            Console.WriteLine(
                $"\n\nТипоразмер: {fan.Data.Size}"
                + $"\nНаименование: {fan.ProjectId}"
                + $"\nОбъем воздуха, введенный пользователем: {userInput.UserInputWorkPoint.VolumeFlow}"
                + " м3/ч;"
                + $"\nПолное давление воздуха, введенное пользователем: {userInput.UserInputWorkPoint.TotalPressure}"
                + " Па;"
                + $"\nРасчетное полное давление воздуха: {fan.TotalPressure}"
                + " Па;"
                + $"\nПогрешность подбора по полному давлению воздуха: {fan.TotalPressureDeviation:+0.0;-0.0;0}"
                + " %;"
                + $"\nРасчетное статическое давление воздуха: {fan.StaticPressure}"
                + " Па;"
                + $"\nСкорость вращения крыльчатки: {fan.Data.ImpellerRotationSpeed}"
                + " об/мин;"
                + $"\nРасчетная мощность в рабочей точке: {fan.Power}"
                + " кВт;"
                + $"\nРасчетный полный КПД вентилятора: {fan.Efficiency}"
                + " %;"
                + $"\nСкорость воздуха: {fan.AirVelocity}"
                + " м/с;"
                + $"\nНоминальная мощность: {fan.Data.NominalPower}"
                + " кВт;"
                + $"\nУровень звуковой мощности по октавам: {fan.OctaveNoise63}; {fan.OctaveNoise125}; {fan.OctaveNoise250}; {fan.OctaveNoise500}; {fan.OctaveNoise1000}; {fan.OctaveNoise2000}; {fan.OctaveNoise4000}; {fan.OctaveNoise8000} [дБ];"
                + $"\nСуммарный уровень звуковой мощности с корректировкой фильтра А частот: 63; 125; 250; 500; 1к; 2к; 4к; 8к [Гц]: {fan.SumNoiseA}"
                + "дБ(А)"
            );
        }
    }
}
