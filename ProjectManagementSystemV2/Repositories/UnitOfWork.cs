using System;
using System.Threading.Tasks;
using ProjectManagementSystemV2.Data;

namespace ProjectManagementSystemV2.Repositories;

public class UnitOfWork : IUnitOfWork // Benim UnitOfWork adında bir sınıfım var, bu sınıf IUnitOfWork sözleşmesindeki kurallara uymak zorunda
{ 

    private readonly AppDbContext _context; // Sadece bu sınıfın içinde yaşar, dışarıdan doğrudan değiştirilemez

    public UnitOfWork(AppDbContext context) // Dışarıdan gelen veriyi aldığımız yer
    {
        _context = context; // Dışarıdan gelen misafiri, sınıfın içindeki kalıcı değişkene atıyoruz
    }

    public async Task<int> CommitAsync() // verileri kalıcı olarak kaydeden metod, int ise veritabanına kaç tane satır eklendiğini sayı olarak söyler 
    {
        return await _context.SaveChangesAsync(); // değişiklikler kalıcı olarak veritabanına yazılır.
    }

    public void Commit() // Senkron olarak değişiklikleri kaydetmek için arayüzün zorunlu kıldığı metot
    {
        _context.SaveChanges();
    }

    public void Dispose() // İşimiz bitince veritabanı bağlantısını güvenli bir şekilde kapatıp belleği temizleyen (bardağı masadan kaldıran) temizlik metodumuz
    {
        _context.Dispose();
    }   

}
