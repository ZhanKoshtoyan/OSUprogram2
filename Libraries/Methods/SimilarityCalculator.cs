namespace Libraries.Methods;

public static class SimilarityCalculator
{
    public static double SimilarVolumeFlow(
        double oldVolumeFlow,
        int oldImpellerRotationSpeed,
        int oldSize,
        int newImpellerRotationSpeed,
        int newSize
    ) =>
        oldVolumeFlow *
        Math.Pow((double) newImpellerRotationSpeed / oldImpellerRotationSpeed, 1) *
        Math.Pow((double) newSize / oldSize, 3);

    public static double SimilarPower(
        double oldPower,
        int oldImpellerRotationSpeed,
        int oldSize,
        double oldAirDensity,
        int newImpellerRotationSpeed,
        int newSize,
        double newAirDensity
    ) =>
        oldPower *
        Math.Pow((double) newImpellerRotationSpeed / oldImpellerRotationSpeed, 3) *
        Math.Pow((double) newSize / oldSize, 5) *
        Math.Pow(newAirDensity / oldAirDensity, 1);

    public static double SimilarPressure(
        double oldPressure,
        int oldImpellerRotationSpeed,
        int oldSize,
        double oldAirDensity,
        int newImpellerRotationSpeed,
        int newSize,
        double newAirDensity
    ) =>
        oldPressure *
        Math.Pow((double) newImpellerRotationSpeed / oldImpellerRotationSpeed, 2) *
        Math.Pow((double) newSize / oldSize, 2) *
        Math.Pow(newAirDensity / oldAirDensity, 1);
}
