using System.Collections;

namespace Libraries;

public abstract class SortFans : IEnumerable
{
    public static List<Fan> Sort(IEnumerable<FanData> fansList, UserInput userInputValidated)
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

    public IEnumerator GetEnumerator() =>
        throw new NotImplementedException();
}
