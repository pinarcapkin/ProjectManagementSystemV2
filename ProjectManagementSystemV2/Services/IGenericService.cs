using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace ProjectManagementSystemV2.Services;

// Her entity (tablo) için ortak olarak kullanılacak servis kurallarını belirleyen sözleşme kağıdı
public interface IGenericService<T> where T : class
{
    // Tablodaki bütün kayıtları asenkron olarak liste şeklinde getirir
    Task<IEnumerable<T>> GetAllAsync();

    // Verilen ID numarasına ait olan tek bir kaydı getirir (Bulamazsa boş/null dönebilir)
    Task<T?> GetByIdAsync(int id);

    // Sisteme yeni bir kayıt eklemek için kullanılır
    Task AddAsync(T entity);

    // Var olan bir kaydı güncellemek için kullanılır
    Task UpdateAsync(T entity);

    // Var olan bir kaydı sistemden silmek için kullanılır
    Task RemoveAsync(T entity);

    // Belirtilen bir şartı (kriteri) sağlayan kayıt veritabanında var mı yok mu diye kontrol eder (True/False döner)
    Task<bool> AnyAsync(Expression<Func<T, bool>> predicate);
}
