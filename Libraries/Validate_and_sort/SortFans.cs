using Libraries.Description_of_objects;
using Libraries.SomeFan;

namespace Libraries.Validate_and_sort;

public abstract class SortFans
{
    /// <summary>
    ///     В этом методе происходит проверка находится ли объем воздуха, который ввел пользователь, в диапазоне
    ///     производительности вентилятора. Если находится, то вентилятор добавляется в список. Следом происходит вторая
    ///     проверка: если допустимая погрешность подбора по полному давлению воздуха, которую ввел пользователь, удовлетворяет
    ///     выччисленную погрешность, то такой вентилятор и все его вычисленные свойства добавляется в список.
    /// </summary>
    /// <param name="fansList"></param>
    /// <param name="userInput"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentException"></exception>
    public static List<IFan> Sort(
        List<FanData>? fansList,
        UserInputRequired userInput
    )
    {
        var sortInputFansList = fansList!
            .Where(
                f =>
                    userInput.VolumeFlow >= f.MinVolumeFlow
                    && userInput.VolumeFlow <= f.MaxVolumeFlow
            )
            .ToList();

        if (sortInputFansList.Count == 0)
        {
            throw new ArgumentException(
                $"Объем воздуха {userInput.VolumeFlow} [м3/ч] выходит за границы производительности вентиляторов. Количество вентиляторов в списке = 0"
            );
        }

        //------------------------------------------------------------------------------------------------------------
        if (userInput.Size != 0)
        {
            sortInputFansList = sortInputFansList
                .Where(
                    f =>
                        userInput.Size
                            .GetValueOrDefault()
                            .ToString("D3") == f.Size
                )
                .ToList();
        }

        if (
            !string.IsNullOrEmpty(userInput.ImpellerRotationDirection)
            || ReferenceEquals(
                userInput.ImpellerRotationDirection,
                ImpellerRotationDirections.Values.GetValue(2)
            )
        )
        {
            sortInputFansList = sortInputFansList
                .Where(
                    f =>
                        userInput.ImpellerRotationDirection
                        == f.ImpellerRotationDirection
                )
                .ToList();
        }

        if (userInput.NominalPower != 0)
        {
            sortInputFansList = sortInputFansList
                .Where(
                    f =>
                        Math.Abs(
                            userInput.NominalPower.GetValueOrDefault()
                                - f.NominalPower
                        ) < 0.05
                )
                .ToList();
        }

        if (userInput.ImpellerRotationSpeed != 0)
        {
            sortInputFansList = sortInputFansList
                .Where(
                    f =>
                        Math.Abs(
                            userInput.ImpellerRotationSpeed.GetValueOrDefault()
                                - Math.Round(f.ImpellerRotationSpeed / 500.0)
                                    * 500
                        ) < 0.05
                )
                .ToList();
        }

        //------------------------------------------------------------------------------------------------------------

        List<IFan>? sortDeviationFansList = null;
        // Создаем словарь для соответствия значений userInput.FanVersion и кода
        Dictionary<string, Action> fanVersionCodeMap = new Dictionary<string, Action>
        {
            { FanVersion.Values[0], () =>
                {
                    sortDeviationFansList = sortInputFansList
                        .Select(
                            elementFanData => new OsuDu(elementFanData, userInput) as IFan
                        )
                        .Where(
                            fan =>
                                Math.Abs(fan.TotalPressureDeviation)
                                <= userInput.TotalPressureDeviation
                        )
                        .ToList();
                }
            },
            { FanVersion.Values[1], () =>
                {
                    sortDeviationFansList = sortInputFansList
                        .Select(
                            elementFanData => new EuFan(elementFanData, userInput) as IFan
                        )
                        .Where(
                            fan =>
                                Math.Abs(fan.TotalPressureDeviation)
                                <= userInput.TotalPressureDeviation
                        )
                        .ToList();
                }
            }
        };

        // Выполняем выбор кода на основе значения userInput.FanVersion
        if (fanVersionCodeMap.TryGetValue(userInput.FanVersion ?? throw new InvalidOperationException("Не найден элемент в массиве FanVersion.Values"), out var value))
        {
            value.Invoke();
        }


        /*if (ReferenceEquals(userInput.FanVersion, FanVersion.Values.GetValue(0)))
        {
            sortDeviationFansList = sortInputFansList
                .Select(
                    elementFanData => new OsuDu(elementFanData, userInput) as IFan
                )
                .Where(
                    fan =>
                        Math.Abs(fan.TotalPressureDeviation)
                        <= userInput.TotalPressureDeviation
                )
                .ToList();
        }

        if (ReferenceEquals(userInput.FanVersion, FanVersion.Values.GetValue(1)))
        {
            sortDeviationFansList = sortInputFansList
                .Select(
                    elementFanData => new EuFan(elementFanData, userInput) as IFan
                )
                .Where(
                    fan =>
                        Math.Abs(fan.TotalPressureDeviation)
                        <= userInput.TotalPressureDeviation
                )
                .ToList();
        }*/


        if (sortDeviationFansList is null)
        {
            throw new ArgumentException(
                $"Условие не удовлетворяется: Погрешность подбора по полному давлению воздуха > {userInput.TotalPressureDeviation}%. Вентиляторы не могут быть подобраны."
            );
        }

        var orderedNumbers = sortDeviationFansList.OrderBy(
            f => Math.Abs(f.TotalPressureDeviation)
        );

        return new List<IFan>(orderedNumbers);
    }
}
