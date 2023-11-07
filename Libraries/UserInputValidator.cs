// OSUprogram2
// (c) Владимир Портянихин, 2023. Все права защищены.
// Несанкционированное копирование этого файла
// с помощью любого носителя строго запрещено.
// Конфиденциально.

using FluentValidation;

namespace Libraries;

public class UserInputValidator : AbstractValidator<UserInput>
{
    public UserInputValidator()
    {
        RuleFor(input => input.VolumeFlow).GreaterThanOrEqualTo(0)
            .WithMessage("Объем воздуха должен быть >= 0. Вентиляторы не могут быть подобраны.");
        RuleFor(input => input.TotalPressure).GreaterThanOrEqualTo(0)
            .WithMessage("Полное давление воздуха должно быть >= 0. Вентиляторы не могут быть подобраны.");
        RuleFor(input => input.TotalPressureDeviation).GreaterThanOrEqualTo(0).WithMessage(
            "Допустимая погрешность подбора по полному давлению воздуха должна быть >= 0. Вентиляторы не могут быть подобраны."
        );
        RuleFor(input => input.RelativeHumidity).GreaterThanOrEqualTo(0).WithMessage(
            "Значение \"Относительная влажность\" должна быть >= 0. Вентиляторы не могут быть подобраны."
        );
    }
}
