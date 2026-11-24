using System.Linq.Expressions;

namespace ProjectManagementSystemV2.Repositories
{
    public interface IGenericRepository<T> where T : class//sözleşme oluşturuyoruz.//<t>tüm entitiyler  çaılşabilir demektir
    {
        Task<IEnumerable<T>> GetAllAsync();//Tablodaki tüm kayıtları getirir

        Task<T?> GetByIdAsync(int id);//Id'si verilen kaydı getirir

        Task AddAsync(T entity);//yeni kayıt ekler

        void Update(T entity);// günceller

        void Delete(T entity);// siler

        Task<bool> AnyAsync(Expression<Func<T, bool>> predicate);// belirli bir şartı sağlayan kayıt varmı diye kontrol eder
    }
}
