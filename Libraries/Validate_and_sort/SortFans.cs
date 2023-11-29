using Libraries.Description_of_objects;

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
    /// <param name="userInputValidated"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentException"></exception>
    public static List<Fan> Sort(
        List<FanData>? fansList,
        UserInputRequired userInputValidated
    )
    {
        var sortInputFansList = fansList!
            .Where(
                f =>
                    userInputValidated.VolumeFlow >= f.MinVolumeFlow
                    && userInputValidated.VolumeFlow <= f.MaxVolumeFlow
            )
            .ToList();

        if (sortInputFansList.Count == 0)
        {
            throw new ArgumentException(
                $"Объем воздуха {userInputValidated.VolumeFlow} [м3/ч] выходит за границы производительности вентиляторов. Количество вентиляторов в списке = 0"
            );
        }

        //------------------------------------------------------------------------------------------------------------
        if (userInputValidated.Size != 0)
        {
            sortInputFansList = sortInputFansList
                .Where(
                    f =>
                        userInputValidated.Size
                            .GetValueOrDefault()
                            .ToString("D3") == f.Size
                )
                .ToList();
        }

        if (!string.IsNullOrEmpty(userInputValidated.ImpellerRotationDirection) || ReferenceEquals(userInputValidated
                .ImpellerRotationDirection, ImpellerRotationDirections.Values.GetValue(2)))
        {
            sortInputFansList = sortInputFansList
                .Where(
                    f =>
                        userInputValidated.ImpellerRotationDirection
                        == f.ImpellerRotationDirection
                )
                .ToList();
        }

        if (userInputValidated.NominalPower != 0)
        {
            sortInputFansList = sortInputFansList
                .Where(
                    f =>
                        Math.Abs(
                            userInputValidated.NominalPower.GetValueOrDefault() - f.NominalPower
                        ) < 0.05
                )
                .ToList();
        }

        if (userInputValidated.ImpellerRotationSpeed != 0)
        {
            sortInputFansList = sortInputFansList
                .Where(
                    f =>
                        Math.Abs(
                            userInputValidated.ImpellerRotationSpeed.GetValueOrDefault()
                            - Math.Round(f.ImpellerRotationSpeed / 500.0)
                            * 500
                        ) < 0.05
                )
                .ToList();
        }

        //------------------------------------------------------------------------------------------------------------

        var sortDeviationFansList = sortInputFansList
            .Select(
                elementFanData => new Fan(elementFanData, userInputValidated)
            )
            .Where(
                fan =>
                    Math.Abs(fan.TotalPressureDeviation)
                    <= userInputValidated.TotalPressureDeviation
            )
            .ToList();

        if (sortDeviationFansList.Count == 0)
        {
            throw new ArgumentException(
                $"Условие не удовлетворяется: Погрешность подбора по полному давлению воздуха > {userInputValidated.TotalPressureDeviation}%. Вентиляторы не могут быть подобраны."
            );
        }

        var orderedNumbers = sortDeviationFansList.OrderBy(
            f => Math.Abs(f.TotalPressureDeviation)
        );

        return new List<Fan>(orderedNumbers);
    }
}
