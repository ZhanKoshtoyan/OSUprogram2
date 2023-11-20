using FluentValidation;
using Libraries.Description_of_objects;

namespace Libraries.Validate_and_sort;

public class UserInputValidator : AbstractValidator<UserInputRequired>
{
    public UserInputValidator()
    {
        RuleFor(input => input.VolumeFlow)
            .GreaterThanOrEqualTo(0)
            .WithMessage("Объем воздуха должен быть >= 0 [м3/ч].");
        RuleFor(input => input.TotalPressure)
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
            .When(input => input is not null)
            .WithMessage(
                "Значение Относительная влажность должна быть: >= 0 и <= 100  [%]."
            );
        RuleFor(input => input.Size)
            .Must(input => Sizes.Values.Contains(input))
            .When(input => input.Size != 0)
            .WithMessage(
                $"Условный размер ОВД должен быть: {string.Join(", ", Sizes.Names)}"
            );
        RuleFor(input => input.FanBodyLength)
            .Must(input => FanBodyLengths.Values.Contains(input))
            .When(input => input.FanBodyLength != 0)
            .WithMessage(
                $"Длина корпуса ОВД должна быть : {string.Join(", ", FanBodyLengths.Names)}."
            );
        RuleFor(input => input.FanOperatingMaxTemperature)
            .Must(input => FanOperatingMaxTemperatures.Values.Contains(input))
            .When(input => input.FanOperatingMaxTemperature != 0)
            .WithMessage(
                $"Температура перемещаемой среды ОВД должна быть: {string.Join(", ", FanOperatingMaxTemperatures.Names)} [°C]."
            );
        RuleFor(input => input.ImpellerRotationDirection)
            .Must(input => ImpellerRotationDirections.Values.Contains(input))
            .When(
                input => !string.IsNullOrEmpty(input.ImpellerRotationDirection)
            )
            .WithMessage(
                $"Направление вращения рабочего колеса ОВД должна быть: {string.Join(", ", ImpellerRotationDirections.Names)}."
            );
        RuleFor(input => input.NominalPower)
            .Must(input => NominalPowers.Values.Contains(input))
            .When(input => input.NominalPower != 0)
            .WithMessage(
                $"Номинальная мощность двигателя ОВД должна быть: {string.Join("; ", NominalPowers.Names)}  [кВт]"
            );
        RuleFor(input => input.ImpellerRotationSpeed)
            .Must(input => ImpellerRotationSpeeds.Values.Contains(input))
            .When(input => input.ImpellerRotationSpeed != 0)
            .WithMessage(
                $"Условное число оборотов двигателя ОВД должно быть: {string.Join(", ", ImpellerRotationSpeeds.Names)} [об/мин]."
            );
        RuleFor(input => input.CaseExecutionMaterial)
            .Must(input => CaseExecutionMaterials.Values.Contains(input))
            .When(input => !string.IsNullOrEmpty(input.CaseExecutionMaterial))
            .WithMessage(
                $"Материал корпуса ОВД должен быть: {string.Join(", ", CaseExecutionMaterials.Names)}."
            );
    }
}
