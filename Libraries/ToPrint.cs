namespace Libraries;

public abstract class ToPrint
{
    public static void Print(List<Fan> sortFans, UserInput userInput)
    {
        foreach (var fan in sortFans)
        {
            Console.WriteLine(
                $"\n\nТипоразмер: {fan.Data.Size}"
                + $"\nНаименование: {fan.Data.Name}"
                + $"\nОбъем воздуха, введенный пользователем: {userInput.VolumeFlow}"
                + " м3/ч;"
                + $"\nПолное давление воздуха, введенное пользователем: {userInput.TotalPressure}"
                + " Па"
                + $"\nРасчетное полное давление воздуха: {fan.TotalPressure}"
                + " Па"
                + $"\nПогрешность подбора по полному давлению воздуха: {fan.TotalPressureDeviation:+0.0;-0.0;0}"
                + " %"
                + $"\nРасчетное статическое давление воздуха: {fan.StaticPressure}"
                + " Па"
                + $"\nСкорость вращения крыльчатки: {fan.Data.ImpellerRotationSpeed}"
                + " об/мин"
                + $"\nРасчетная мощность в рабочей точке: {fan.Power}"
                + " кВт"
                + $"\nРасчетный полный КПД вентилятора: {fan.Efficiency}"
                + " %"
                + $"\nСкорость воздуха: {fan.AirVelocity}"
                + " м/с"
                + $"\nНоминальная мощность: {fan.Data.NominalPower}"
                + " кВт"
                + $"\nУровень звуковой мощности по октавам, [дБ]: {fan.OctaveNoise63}, {fan.OctaveNoise125}, {fan.OctaveNoise250}, {fan.OctaveNoise500}, {fan.OctaveNoise1000}, {fan.OctaveNoise2000}, {fan.OctaveNoise4000}, {fan.OctaveNoise8000}"
                + $"\nСуммарный уровень звуковой мощности с корректировкой фильтра А частот: 63, 125, 250, 500, 1к, 2к, 4к, 8к [Гц]: {fan.SumNoiseA}"
                + " [дБ(А)]"
            );
        }
    }
}
