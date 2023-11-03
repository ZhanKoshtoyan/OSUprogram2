using Microsoft.EntityFrameworkCore;

namespace Libraries;

public static class DatabaseLoader
{
    public static async Task LoadDataToDatabase()
    {
        await using var db = new ApplicationContext();

        var fanCollection = new FanCollection();
        /*foreach (var fanData in fanCollection.Fans)
        {
            foreach (var fanDataProperty in typeof(FanData).GetProperties())
            {
                if (fanDataProperty.PropertyType == typeof(double[]))
                {
                    var fanDataPropertyValue = (double[]) fanDataProperty.GetValue(fanData)!;

                    // Преобразование значений типа double[] в строку
                    var fanDataPropertyStringValue = string.Join(", ", fanDataPropertyValue);

                    // Установка преобразованного значения обратно в свойство
                    fanDataProperty.SetValue(fanData, fanDataPropertyStringValue);
                }
            }
        }*/

        db.FanDatas.AddRange(fanCollection.Fans);

        // db.SaveChanges();
        await db.SaveChangesAsync();
        Console.WriteLine("Объекты успешно сохранены");

        // получаем объекты из бд и выводим на консоль
        var fans = db.FanDatas.ToList();
        Console.WriteLine("Список объектов:");
        foreach (var f in fans)
        {
            Console.WriteLine(
                $"\n\nТипоразмер: {f.Size}"
                    + $"\nНаименование: {f.Name}"
                    + $"\nСкорость вращения крыльчатки: {f.ImpellerRotationSpeed}"
                    + $"\nМинимальный объем воздуха: {f.MinVolumeFlow}"
                    + " м3/ч;"
                    + $"\nМаксимальный объем воздуха: {f.MaxVolumeFlow}"
                    + " м3/ч;"
                    + $"\nКоэффициенты полинома 6-й степени Pv(Q) - полного давления от объемного воздуха: {f.TotalPressureCoefficients}"
                    + ""
                    + $"\nКоэффициенты полинома 6-й степени Pv(N) - мощности вентилятора в рабочей точке от объемного воздуха: {f.PowerCoefficients}"
                    + ""
                    + $"\nПлощадь сечения на входе: {f.InletCrossSection}"
                    + " [м2]"
                    + $"\nНоминальная мощность: {f.NominalPower}"
                    + " [кВт]"
            );
        }
    }
}

public sealed class ApplicationContext : DbContext //DbContext: это класс Entity Framework определяет контекст данных, используемый для взаимодействия с базой данных
{
    // Если базы данных нет, то происходит создание БД.
    public ApplicationContext() => Database.EnsureCreated();

    // DbSet/DbSet<TEntity>: представляет набор объектов, которые хранятся в базе данных с типом данных "User"
    public DbSet<FanData> FanDatas => Set<FanData>();
    public DbSet<PolynomialType> PolynomialValues => Set<PolynomialType>();

    protected override void OnConfiguring(
        DbContextOptionsBuilder optionsBuilder
    ) //DbContextOptionsBuilder: устанавливает параметры подключения
    {
        //UseSqlite - это наименование БД.
        //helloapp.db - это относительный путь к базе данных
        //Пример обращения к БД на сервере: (@"localhost;port=4532;database=db;username=root;password=12345")
        optionsBuilder.UseSqlite("Data Source=helloapp.db");

        /*protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Ignore<>();
        }*/
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<FanData>().HasKey(data => data.Id);
        modelBuilder.Entity<FanData>().HasIndex(data => data.Id);
        modelBuilder
            .Entity<FanData>()
            .HasOne(f => f.TotalPressureCoefficients)
            .WithOne(p => p.FanData)
            .HasForeignKey<PolynomialType>(fp => fp.Id);
        // .Ignore(data => data.OctaveNoiseCoefficients63);
        modelBuilder.Entity<FanData>().ToTable("FanDatas");
        modelBuilder.Entity<PolynomialType>().ToTable("FanDatas");
        base.OnModelCreating(modelBuilder);
    }
}
