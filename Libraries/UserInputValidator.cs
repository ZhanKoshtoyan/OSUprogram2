using FluentValidation;

namespace Libraries;

public class UserInputValidator : AbstractValidator<UserInput>
{
    public UserInputValidator()
    {
        RuleFor(input => input.VolumeFlow)
            .NotEmpty()
            .GreaterThanOrEqualTo(0)
            .WithMessage("Объем воздуха должен быть >= 0 [м3/ч].");
        RuleFor(input => input.TotalPressure)
            .NotEmpty()
            .GreaterThanOrEqualTo(0)
            .WithMessage("Полное давление воздуха должно быть >= 0 [Па].");
        RuleFor(input => input.TotalPressureDeviation)
            .NotEmpty()
            .InclusiveBetween(0, 30)
            .WithMessage(
                "Допустимая погрешность подбора по полному давлению воздуха должна быть: >= 0 и <= 30  [%]."
            );
        RuleFor(input => input.RelativeHumidity)
            .InclusiveBetween(0, 100)
            .When(input => input.RelativeHumidity != 0)
            .WithMessage(
                "Значение Относительная влажность должна быть: >= 0 и <= 100  [%]."
            );
        RuleFor(input => input.Size)
            .Must(
                input =>
                    input
                        is 40
                        or 50
                        or 56
                        or 63
                        or 71
                        or 80
                        or 90
                        or 100
                        or 112
                        or 125
            )
            .When(input => input.Size != 0)
            .WithMessage(
                "Условный размер ОВД должен быть: 40, 50, 56, 63, 71, 80, 90, 100, 112 или 125"
            );
        RuleFor(input => input.CaseLength)
            .Must(input => input is 1 or 2)
            .When(input => input.CaseLength != 0)
            .WithMessage(
                "Длина корпуса ОВД должна быть :1 - полногабаритный корпус или 2 - короткий корпус."
            );
        RuleFor(input => input.TemperatureFan)
            .Must(input => input is 300 or 400)
            .When(input => input.TemperatureFan != 0)
            .WithMessage(
                "Температура перемещаемой среды ОВД должна быть: 300 или 400  [°C]."
            );
        RuleFor(input => input.ImpellerRotationDirection)
            .Must(input => input is "RRO" or "LRO" or "REV")
            .When(
                input => !string.IsNullOrEmpty(input.ImpellerRotationDirection)
            )
            .WithMessage(
                "Направление вращения рабочего колеса ОВД должна быть: RRO - поток на мотор, LRO - поток на колесо или REV - реверс."
            );
        RuleFor(input => input.NominalPower)
            .Must(
                input =>
                    input
                        is 0.55
                        or 0.75
                        or 1.1
                        or 1.5
                        or 2.2
                        or 3
                        or 4
                        or 5.5
                        or 7.5
                        or 11
                        or 15
                        or 18.5
                        or 22
                        or 30
                        or 37
                        or 45
            )
            .When(input => input.NominalPower != 0)
            .WithMessage(
                "Номинальная мощность двигателя ОВД должна быть: 0,55; 0,75; 1,1; 1,5; 2,2; 3,0; 4,0; 5,5; 7,5; 11,0; 15,0; 18,5; 22,0; 30,0; 37,0; 45,0  [кВт]"
            );
        RuleFor(input => input.ImpellerRotationSpeed)
            .Must(input => input is 3000 or 1500 or 1000)
            .When(input => input.ImpellerRotationSpeed != 0)
            .WithMessage(
                "Условное число оборотов двигатля ОВД должно быть: 3000, 1500 или 1000 [об/мин]."
            );
        RuleFor(input => input.CaseMaterial)
            .Must(input => input is "ZN" or "NR" or "KR")
            .When(input => !string.IsNullOrEmpty(input.CaseMaterial))
            .WithMessage(
                "Материал корпуса ОВД должен быть: ZN - оцинкованная сталь, NR - нержавеющая сталь или KR - кислотостойкая нержавеющая сталь."
            );
    }
}
