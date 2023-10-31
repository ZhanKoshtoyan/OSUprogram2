// OSUprogram2
// (c) Владимир Портянихин, 2023. Все права защищены.
// Несанкционированное копирование этого файла
// с помощью любого носителя строго запрещено.
// Конфиденциально.

namespace Libraries;

public class InputValidation
{
    private readonly double _inputAltitude;

    private readonly double _inputRelativeHumidity;

    private readonly double _inputTemperature;

    /// <summary>
    ///     Полное давление воздуха, которое ввел пользователь, [Па]
    /// </summary>
    private readonly double _inputTotalPressure;

    private readonly double _inputTotalPressureDeviation;

    /// <summary>
    ///     Объем воздуха введенный пользователем, [м3/ч]
    /// </summary>
    private readonly double _inputVolumeFlow;

    public InputValidation(
        string inputVolumeFlow,
        string inputTotalPressure,
        string inputTotalPressureDeviation,
        string inputTemperature,
        string inputRelativeHumidity,
        string inputAltitude
    )
    {
        //_______________________________________________________________________________________________________
        // не проверять не double. Parse вместо tryParse.
        var result = double.TryParse(
            inputVolumeFlow.Replace(".", ","),
            out var verifiedVolumeFlow
        );
        if (result)
        {
            if (verifiedVolumeFlow >= 0)
            {
                _inputVolumeFlow = verifiedVolumeFlow;
            }
            else
            {
                throw new ArgumentException(
                    $"Объем воздуха {verifiedVolumeFlow} [м3/ч] < 0. Вентиляторы не могут быть подобраны."
                );
            }
        }
        else
        {
            throw new ArgumentException(
                "Значение \"Объем воздуха\" не является числом. Вентиляторы не могут быть подобраны."
            );
        }
        //_______________________________________________________________________________________________________

        result = double.TryParse(
            inputTotalPressure.Replace(".", ","),
            out var verifiedTotalPressure
        );
        if (result)
        {
            if (verifiedTotalPressure >= 0)
            {
                _inputTotalPressure = verifiedTotalPressure;
            }
            else
            {
                throw new ArgumentException(
                    $"Полное давление воздуха {verifiedTotalPressure} [Па] < 0. Вентиляторы не могут быть подобраны."
                );
            }
        }
        else
        {
            throw new ArgumentException(
                "Значение \"Полное давление воздуха\" не является числом. Вентиляторы не могут быть подобраны."
            );
        }
        //_______________________________________________________________________________________________________

        result = double.TryParse(
            inputTotalPressureDeviation.Replace(".", ","),
            out var verifiedTotalPressureDeviation
        );
        if (result)
        {
            if (verifiedTotalPressureDeviation >= 0)
            {
                _inputTotalPressureDeviation = verifiedTotalPressureDeviation;
            }
            else
            {
                throw new ArgumentException(
                    $"Допустимая погрешность подбора по полному давлению воздуха {verifiedTotalPressureDeviation} [Па] < 0. Вентиляторы не могут быть подобраны."
                );
            }
        }
        else
        {
            throw new ArgumentException(
                "Значение \"Допустимая погрешность подбора\" не является числом. Вентиляторы не могут быть подобраны."
            );
        }

        //_______________________________________________________________________________________________________
        result = double.TryParse(
            inputTemperature.Replace(".", ","),
            out var verifiedTemperature
        );
        if (result)
        {
            _inputTemperature = verifiedTemperature;
        }
        else
        {
            throw new ArgumentException(
                "Значение \"Температура ежедневной эксплуатации\" не является числом. Вентиляторы не могут быть подобраны."
            );
        }

        //_______________________________________________________________________________________________________
        result = double.TryParse(
            inputRelativeHumidity.Replace(".", ","),
            out var verifiedRelativeHumidity
        );
        if (result || string.IsNullOrEmpty(inputRelativeHumidity))
        {
            _inputRelativeHumidity = verifiedRelativeHumidity;
        }
        else
        {
            throw new ArgumentException(
                "Значение \"Относительная влажность\" не является числом. Вентиляторы не могут быть подобраны."
            );
        }

        //_______________________________________________________________________________________________________
        result = double.TryParse(
            inputAltitude.Replace(".", ","),
            out var verifiedAltitude
        );
        if (result || string.IsNullOrEmpty(inputAltitude))
        {
            _inputAltitude = verifiedAltitude;
        }
        else
        {
            throw new ArgumentException(
                "Значение \"Высота над уровнем моря\" не является числом. Вентиляторы не могут быть подобраны."
            );
        }
        //_______________________________________________________________________________________________________

        var fansList = new FanCollection().Fans;

        var sortMaxMinFansList = fansList
            .Where(
                f =>
                    _inputVolumeFlow >= f.MinVolumeFlow
                    && _inputVolumeFlow <= f.MaxVolumeFlow
            )
            .ToList();

        if (sortMaxMinFansList.Count == 0)
        {
            throw new ArgumentException(
                $"Объем воздуха {inputVolumeFlow} [м3/ч] выходит за границы производительности вентиляторов. Вентиляторы не могут быть подобраны."
            );
        }

        var sortDeviationFansList = sortMaxMinFansList
            .Select(
                elementFanData =>
                    new Fan(
                        elementFanData,
                        _inputVolumeFlow,
                        _inputTotalPressure,
                        _inputTemperature,
                        _inputRelativeHumidity,
                        _inputAltitude
                    )
            )
            .Where(
                fan =>
                    Math.Abs(fan.TotalPressureDeviation)
                    <= _inputTotalPressureDeviation
            )
            .ToList();

        if (sortDeviationFansList.Count == 0)
        {
            throw new ArgumentException(
                $"Условие не удовлетворяется: Погрешность подбора по полному давлению воздуха > {inputTotalPressureDeviation}%. Вентиляторы не могут быть подобраны."
            );
        }

        foreach (var fan in sortDeviationFansList)
        {
            Console.WriteLine(
                $"\n\nТипоразмер: {fan.Data.Size}"
                + $"\nНаименование: {fan.Data.Name}"
                + $"\nОбъем воздуха, введенный пользователем: {inputVolumeFlow}"
                + " м3/ч;"
                + $"\nПолное давление воздуха, введенное пользователем: {inputTotalPressure}"
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
                + $"\nУровень звуковой мощности по октавам, [дБ]: {string.Join("; ", fan.OctaveNoise)}"
                + $"\nСуммарный уровень звуковой мощности с корректировкой фильтра А частот: 63, 125, 250, 500, 1к, 2к, 4к, 8к [Гц]: {fan.SumNoise}"
                + " [дБ(А)]"
            );
        }
    }
}
