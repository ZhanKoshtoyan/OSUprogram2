using Microsoft.EntityFrameworkCore;

namespace Libraries;

public static class DatabaseLoader
{
    public static async Task UploadDataToDatabase(List<FanData>? fanCollection)
    {
        await using var db = new ApplicationContext();

        // var fanCollection = new FanCollection();

        db.DataFans.AddRange(fanCollection);

        // db.SaveChanges();
        await db.SaveChangesAsync();
        Console.WriteLine("Объекты успешно сохранены");
    }

    public static async Task<List<FanData>?> DownloadDataFromDatabase()
    {
        await using var db = new ApplicationContext();

        /*
        var fanCollection = new FanCollection();

        db.DataFans.AddRange(fanCollection.Fans);

        // db.SaveChanges();
        await db.SaveChangesAsync();
        Console.WriteLine("Объекты успешно сохранены");
        */

        // получаем объекты из бд и выводим на консоль
        var fans = db.DataFans.ToList();
        return fans;
        /*Console.WriteLine("Список объектов:");
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
        }*/
    }
}

//DbContext: это класс Entity Framework определяет контекст данных, используемый для взаимодействия с базой данных
public sealed class ApplicationContext : DbContext
{
    // При каждом вызове происходит удаление и создание БД.
    public ApplicationContext()
    {
        Database.EnsureDeleted();
        Database.EnsureCreated();
    }

    // DbSet/DbSet<TEntity>: представляет набор объектов, которые хранятся в базе данных с типом данных "User"
    public DbSet<FanData> DataFans => Set<FanData>();

    protected override void OnConfiguring(
        DbContextOptionsBuilder optionsBuilder
    ) //DbContextOptionsBuilder: устанавливает параметры подключения
    {
        //UseSqlite - это наименование БД.
        //helloapp.db - это относительный путь к базе данных
        //Пример обращения к БД на сервере: (@"localhost;port=4532;database=db;username=root;password=12345")
        optionsBuilder.UseSqlite("Data Source=helloapp.db");
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<FanData>().HasKey(data => data.Id);
        modelBuilder.Entity<FanData>().HasIndex(data => data.Id);

        //Определяем зависимость между объектом FanData и зависимым типом
        modelBuilder
            .Entity<FanData>()
            .OwnsOne(f => f.TotalPressureCoefficients);
        modelBuilder.Entity<FanData>().OwnsOne(f => f.PowerCoefficients);
        modelBuilder
            .Entity<FanData>()
            .OwnsOne(f => f.OctaveNoiseCoefficients63);
        modelBuilder
            .Entity<FanData>()
            .OwnsOne(f => f.OctaveNoiseCoefficients125);
        modelBuilder
            .Entity<FanData>()
            .OwnsOne(f => f.OctaveNoiseCoefficients250);
        modelBuilder
            .Entity<FanData>()
            .OwnsOne(f => f.OctaveNoiseCoefficients500);
        modelBuilder
            .Entity<FanData>()
            .OwnsOne(f => f.OctaveNoiseCoefficients1000);
        modelBuilder
            .Entity<FanData>()
            .OwnsOne(f => f.OctaveNoiseCoefficients2000);
        modelBuilder
            .Entity<FanData>()
            .OwnsOne(f => f.OctaveNoiseCoefficients4000);
        modelBuilder
            .Entity<FanData>()
            .OwnsOne(f => f.OctaveNoiseCoefficients8000);

        //вызов метода базового класса. Это позволяет сохранить базовую функциональность при дополнительной настройке модели данных FanData (выше).
        base.OnModelCreating(modelBuilder);
    }
}
