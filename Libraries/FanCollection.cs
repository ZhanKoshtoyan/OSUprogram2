﻿namespace Libraries;

/// <summary>
///     Коллекция вентиляторов
/// </summary>
public class FanCollection
{
    public List<FanData> Fans { get; } =
        new()
        {
            new FanData
            {
                Id = Guid.NewGuid(),
                Size = "040",
                Name = "ОСУ-ДУ",
                ImpellerRotationSpeed = 2820,
                MinVolumeFlow = 68,
                MaxVolumeFlow = 6790,
                TotalPressureCoefficients = new PolynomialType
                {
                    SixthCoefficient = -1.78089715845477E-19,
                    FifthCoefficient = 3.94906840722657E-15,
                    FourthCoefficient = -3.30765912375102E-11,
                    ThirdCoefficient = 1.26312196079582E-07,
                    SecondCoefficient = -0.000199808911402692,
                    FirstCoefficient = -0.0154525722368974,
                    ZeroCoefficient = 673.944825212064
                },
                PowerCoefficients = new PolynomialType
                {
                    SixthCoefficient = -2.53773204407154E-22,
                    FifthCoefficient = 5.4193319577496E-18,
                    FourthCoefficient = -4.33922909953863E-14,
                    ThirdCoefficient = 1.54271497327987E-10,
                    SecondCoefficient = -2.0481666039729E-07,
                    FirstCoefficient = -0.000029046058709531,
                    ZeroCoefficient = 0.811874973631031
                },
                InletCrossSection = 0.1256,
                NominalPower = 0.75,
                OctaveNoiseCoefficients63 = new PolynomialType
                {
                    SixthCoefficient = -6.14671213297709E-21,
                    FifthCoefficient = 1.31079844703572E-16,
                    FourthCoefficient = -1.04888807752614E-12,
                    ThirdCoefficient = 3.89982507266809E-09,
                    SecondCoefficient = -6.80710188447047E-06,
                    FirstCoefficient = 0.00462753224177455,
                    ZeroCoefficient = 84.9159954779106
                },
                OctaveNoiseCoefficients125 = new PolynomialType
                {
                    SixthCoefficient = -6.51454264058702E-21,
                    FifthCoefficient = 1.32710060105779E-16,
                    FourthCoefficient = -1.00768520298903E-12,
                    ThirdCoefficient = 3.53133195915813E-09,
                    SecondCoefficient = -5.77127123957964E-06,
                    FirstCoefficient = 0.0036640965527022,
                    ZeroCoefficient = 80.7774784631891
                },
                OctaveNoiseCoefficients250 = new PolynomialType
                {
                    SixthCoefficient = -6.3280380665082E-21,
                    FifthCoefficient = 1.33585775972799E-16,
                    FourthCoefficient = -1.0547802400351E-12,
                    ThirdCoefficient = 3.85311433506571E-09,
                    SecondCoefficient = -6.58558260855349E-06,
                    FirstCoefficient = 0.00437936399349243,
                    ZeroCoefficient = 80.1321037992639
                },
                OctaveNoiseCoefficients500 = new PolynomialType
                {
                    SixthCoefficient = -9.26207513005705E-21,
                    FifthCoefficient = 2.01106944887471E-16,
                    FourthCoefficient = -1.645992156887E-12,
                    ThirdCoefficient = 6.29288811892878E-09,
                    SecondCoefficient = -0.000011326274187587,
                    FirstCoefficient = 0.00793143362351284,
                    ZeroCoefficient = 79.9113271768486
                },
                OctaveNoiseCoefficients1000 = new PolynomialType
                {
                    SixthCoefficient = -9.22086650949535E-21,
                    FifthCoefficient = 1.95357629510521E-16,
                    FourthCoefficient = -1.54850515374065E-12,
                    ThirdCoefficient = 5.67845220868845E-09,
                    SecondCoefficient = -9.74084682425671E-06,
                    FirstCoefficient = 0.00649851312394153,
                    ZeroCoefficient = 81.2023515031343
                },
                OctaveNoiseCoefficients2000 = new PolynomialType
                {
                    SixthCoefficient = -6.3280380665082E-21,
                    FifthCoefficient = 1.33585775972799E-16,
                    FourthCoefficient = -1.0547802400351E-12,
                    ThirdCoefficient = 3.85311433506571E-09,
                    SecondCoefficient = -6.58558260855349E-06,
                    FirstCoefficient = 0.00437936399349243,
                    ZeroCoefficient = 80.1321037992639
                },
                OctaveNoiseCoefficients4000 = new PolynomialType
                {
                    SixthCoefficient = -4.18111766792606E-21,
                    FifthCoefficient = 8.47123052004552E-17,
                    FourthCoefficient = -6.33367477938545E-13,
                    ThirdCoefficient = 2.14815146615349E-09,
                    SecondCoefficient = -3.33695497084726E-06,
                    FirstCoefficient = 0.00198400211970034,
                    ZeroCoefficient = 80.7807772176639
                },
                OctaveNoiseCoefficients8000 = new PolynomialType
                {
                    SixthCoefficient = -6.91846255419214E-21,
                    FifthCoefficient = 1.46984399433531E-16,
                    FourthCoefficient = -1.17197702914312E-12,
                    ThirdCoefficient = 4.34312093881319E-09,
                    SecondCoefficient = -7.55422636808502E-06,
                    FirstCoefficient = 0.00511581608781055,
                    ZeroCoefficient = 76.9863162523303
                }
            },
            new FanData
            {
                Id = Guid.NewGuid(),
                Size = "040",
                Name = "ОСУ-ДУ",
                ImpellerRotationSpeed = 2820,
                MinVolumeFlow = 84,
                MaxVolumeFlow = 8440,
                TotalPressureCoefficients = new PolynomialType
                {
                    SixthCoefficient = -1.40391882190154E-19,
                    FifthCoefficient = 4.07973805765995E-15,
                    FourthCoefficient = -4.55468137920892E-11,
                    ThirdCoefficient = 2.40844217732171E-07,
                    SecondCoefficient = -0.000588002525185077,
                    FirstCoefficient = 0.445721096324641,
                    ZeroCoefficient = 671.557184046322
                },
                PowerCoefficients = new PolynomialType
                {
                    SixthCoefficient = -2.55628003250976E-22,
                    FifthCoefficient = 7.55442353790038E-18,
                    FourthCoefficient = -8.6314158661347E-14,
                    ThirdCoefficient = 4.70385579823238E-10,
                    SecondCoefficient = -1.18621012838402E-06,
                    FirstCoefficient = 0.000971975662647876,
                    ZeroCoefficient = 1.43633158103282
                },
                InletCrossSection = 0.1256,
                NominalPower = 1.1,
                OctaveNoiseCoefficients63 = new PolynomialType
                {
                    SixthCoefficient = -1.20646766123701E-21,
                    FifthCoefficient = 2.78905204053343E-17,
                    FourthCoefficient = -2.24733987669612E-13,
                    ThirdCoefficient = 7.22419964177808E-10,
                    SecondCoefficient = -7.66932868978317E-07,
                    FirstCoefficient = -4.40653694017652E-05,
                    ZeroCoefficient = 86.4112422795434
                },
                OctaveNoiseCoefficients125 = new PolynomialType
                {
                    SixthCoefficient = -5.59562233156906E-22,
                    FifthCoefficient = 9.07475778997895E-18,
                    FourthCoefficient = -1.68800884881408E-14,
                    ThirdCoefficient = -3.5759706640285E-10,
                    SecondCoefficient = 1.8692154248822E-06,
                    FirstCoefficient = -0.00251845052821754,
                    ZeroCoefficient = 82.9026970648196
                },
                OctaveNoiseCoefficients250 = new PolynomialType
                {
                    SixthCoefficient = -1.20646766123701E-21,
                    FifthCoefficient = 2.78905204053343E-17,
                    FourthCoefficient = -2.24733987669612E-13,
                    ThirdCoefficient = 7.22419964177808E-10,
                    SecondCoefficient = -7.66932868978317E-07,
                    FirstCoefficient = -4.40653694017652E-05,
                    ZeroCoefficient = 81.4112422795434
                },
                OctaveNoiseCoefficients500 = new PolynomialType
                {
                    SixthCoefficient = -6.51650001216414E-22,
                    FifthCoefficient = 1.197308258368E-17,
                    FourthCoefficient = -4.73435519621482E-14,
                    ThirdCoefficient = -2.32033540697713E-10,
                    SecondCoefficient = 1.6697870401924E-06,
                    FirstCoefficient = -0.00242908906164528,
                    ZeroCoefficient = 81.5969420585426
                },
                OctaveNoiseCoefficients1000 = new PolynomialType
                {
                    SixthCoefficient = 2.05852500322287E-22,
                    FifthCoefficient = -9.24312176209533E-18,
                    FourthCoefficient = 1.47569198121579E-13,
                    ThirdCoefficient = -1.05599960331183E-09,
                    SecondCoefficient = 3.28893930602657E-06,
                    FirstCoefficient = -0.00366252380824962,
                    ZeroCoefficient = 81.6895881870266
                },
                OctaveNoiseCoefficients2000 = new PolynomialType
                {
                    SixthCoefficient = -5.0030758045737E-22,
                    FifthCoefficient = 9.32369932161965E-18,
                    FourthCoefficient = -3.85823947740183E-14,
                    ThirdCoefficient = -1.66789819567E-10,
                    SecondCoefficient = 1.26100321852411E-06,
                    FirstCoefficient = -0.00185329458882568,
                    ZeroCoefficient = 81.550415233285
                },
                OctaveNoiseCoefficients4000 = new PolynomialType
                {
                    SixthCoefficient = 1.34433213151603E-22,
                    FifthCoefficient = -8.88537801000161E-18,
                    FourthCoefficient = 1.59813849764913E-13,
                    ThirdCoefficient = -1.18360593925454E-09,
                    SecondCoefficient = 3.71310752796305E-06,
                    FirstCoefficient = -0.00413334407614593,
                    ZeroCoefficient = 81.7266371627445
                },
                OctaveNoiseCoefficients8000 = new PolynomialType
                {
                    SixthCoefficient = -1.28639079482538E-21,
                    FifthCoefficient = 3.01821599153012E-17,
                    FourthCoefficient = -2.45739796501079E-13,
                    ThirdCoefficient = 7.8478257898983E-10,
                    SecondCoefficient = -7.8231726924654E-07,
                    FirstCoefficient = -0.000149039574325027,
                    ZeroCoefficient = 78.4207201290831
                }
            }
        };
}
