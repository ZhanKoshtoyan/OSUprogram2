using FluentValidation;
using Libraries.Description_of_objects;
using Libraries.Description_of_objects.Parameters;
using Libraries.Description_of_objects.UserInput;

namespace Libraries.Validate_and_sort;

public class UserInputValidator : AbstractValidator<UserInput>
{
    public UserInputValidator()
    {
        RuleFor(input => input.UserInputWorkPoint.VolumeFlow)
            .GreaterThanOrEqualTo(0)
            .WithMessage("Объем воздуха должен быть >= 0 [м3/ч].");
        RuleFor(input => input.UserInputWorkPoint.TotalPressure)
            .GreaterThanOrEqualTo(0)
            .WithMessage("Полное давление воздуха должно быть >= 0 [Па].");
        RuleFor(input => input.UserInputWorkPoint.TotalPressureDeviation)
            .NotEmpty()
            .InclusiveBetween(0, 30)
            .WithMessage(
                "Допустимая погрешность подбора по полному давлению воздуха должна быть: >= 0 и <= 30  [%]."
            );
        RuleFor(input => input.UserInputFan.FanVersion)
            .Must(input => FanVersion.Values.Contains(input))
            .WithMessage(
                $"Исполнение вентилятора должно быть: {string.Join(", ", FanVersion.Names)}."
            );
        RuleFor(input => input.UserInputAir.RelativeHumidity)
            .InclusiveBetween(0, 100)
            .When(input => input is not null)
            .WithMessage(
                "Значение Относительная влажность должна быть: >= 0 и <= 100  [%]."
            );
        RuleFor(input => input.UserInputFan.Size.GetValueOrDefault())
            .Must(input => Sizes.Values.Contains(input))
            .When(input => input.UserInputFan.Size != 0)
            .WithMessage(
                $"Условный размер ОВД должен быть: {string.Join(", ", Sizes.Names)}"
            );
        RuleFor(input => input.UserInputFan.FanBodyLength.GetValueOrDefault())
            .Must(input => FanBodyLengths.Values.Contains(input))
            .When(input => input.UserInputFan.FanBodyLength != 0)
            .WithMessage(
                $"Длина корпуса ОВД должна быть : {string.Join(", ", FanBodyLengths.Names)}."
            );
        RuleFor(input => input.UserInputAir.FanOperatingMaxTemperature.GetValueOrDefault())
            .Must(input => FanOperatingMaxTemperatures.Values.Contains(input))
            .When(input => input.UserInputAir.FanOperatingMaxTemperature != 0)
            .WithMessage(
                $"Температура перемещаемой среды ОВД должна быть: {string.Join(", ", FanOperatingMaxTemperatures.Names)} [°C]."
            );
        RuleFor(input => input.UserInputFan.ImpellerRotationDirection)
            //TODO:Проверить string?
            .Must(input => ImpellerRotationDirections.Values.Contains(input))
            .When(
                input => !string.IsNullOrEmpty(input.UserInputFan.ImpellerRotationDirection)
            )
            .WithMessage(
                $"Направление вращения рабочего колеса ОВД должна быть: {string.Join(", ", ImpellerRotationDirections.Names)}."
            );
        RuleFor(input => input.UserInputFan.NominalPower.GetValueOrDefault())
            .Must(input => NominalPowers.Values.Contains(input))
            .When(input => input.UserInputFan.NominalPower != 0)
            .WithMessage(
                $"Номинальная мощность двигателя ОВД должна быть: {string.Join("; ", NominalPowers.Names)}  [кВт]"
            );
        RuleFor(input => input.UserInputFan.ImpellerRotationSpeed.GetValueOrDefault())
            .Must(input => ImpellerRotationSpeeds.Values.Contains(input))
            .When(input => input.UserInputFan.ImpellerRotationSpeed != 0)
            .WithMessage(
                $"Условное число оборотов двигателя ОВД должно быть: {string.Join(", ", ImpellerRotationSpeeds.Names)} [об/мин]."
            );
        RuleFor(input => input.UserInputFan.CaseExecutionMaterial)
            .Must(input => CaseExecutionMaterials.Values.Contains(input))
            .When(input => !string.IsNullOrEmpty(input.UserInputFan.CaseExecutionMaterial))
            .WithMessage(
                $"Материал корпуса ОВД должен быть: {string.Join(", ", CaseExecutionMaterials.Names)}."
            );
    }
}
