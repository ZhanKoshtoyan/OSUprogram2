namespace Libraries;

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
    public static List<Fan> Sort(List<FanData>? fansList, UserInput userInputValidated)
    {
        var sortMaxMinFansList = fansList
            .Where(
                f =>
                    userInputValidated.VolumeFlow >= f.MinVolumeFlow
                    && userInputValidated.VolumeFlow <= f.MaxVolumeFlow
            )
            .ToList();

        if (sortMaxMinFansList.Count == 0)
        {
            throw new ArgumentException(
                $"Объем воздуха {userInputValidated.VolumeFlow} [м3/ч] выходит за границы производительности вентиляторов. Вентиляторы не могут быть подобраны."
            );
        }

        var sortDeviationFansList = sortMaxMinFansList
            .Select(
                elementFanData =>
                    new Fan(
                        elementFanData,
                        userInputValidated
                    )
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

        return sortDeviationFansList;
    }
}
