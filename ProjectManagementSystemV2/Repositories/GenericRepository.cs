using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ProjectManagementSystemV2.Data;

namespace ProjectManagementSystemV2.Repositories;

public class GenericRepository<T> : IGenericRepository<T> where T : class
{
    protected readonly AppDbContext _context;// _ context veri tabanına  açılan ana kapı sQl ile kouşmamızı sağlar
    private readonly DbSet<T> _dbSet;// o ana hangi talo ile çalışıyorsak onu temsil eden dinamik yapımızdır
    
    public GenericRepository(AppDbContext context)
    {
        _context = context;//bağlantıyı sınıfın içine kaydederiz
        _dbSet = _context.Set<T>();//(örneğin User), tablosunu hemen hafızaya al
    }

    public async Task<IEnumerable<T>> GetAllAsync()//İlgili tablodaki bütün satırları asenkron (arkada uygulamayı dondurmadan) bir liste halinde çeker ve getirir.
    {
        return await _dbSet.ToListAsync();
    }

    public async Task<T?> GetByIdAsync(int id)// ID kayıt getir
    {
        return await _dbSet.FindAsync(id);
    }

    public async Task AddAsync(T entity)// yeni kayıt ekel
    {
        await _dbSet.AddAsync(entity);
    }

    public void Update(T entity)// kayıt güncelle
    {
        _dbSet.Update(entity);
    }

    public void Delete(T entity)// kayıt sil
    {
        _dbSet.Remove(entity);
    }

    public async Task<bool> AnyAsync(Expression<Func<T, bool>> predicate)// şartlı kontrol et
    {
        // Belirtilen şartı sağlayan kayıt var mı diye veritabanında arar (True veya False döner)
        return await _dbSet.AnyAsync(predicate);
    }
}