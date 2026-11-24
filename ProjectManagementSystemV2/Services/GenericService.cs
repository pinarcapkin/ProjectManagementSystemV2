using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using ProjectManagementSystemV2.Repositories;

namespace ProjectManagementSystemV2.Services;

public class GenericService<T> : IGenericService<T> where T : class// 
{
    private readonly IGenericRepository<T> _repository; // Veritabanı sorgularını yapabilmek için repository nesnesi
    private readonly IUnitOfWork _unitOfWork; // Yapılan değişiklikleri veritabanına kalıcı olarak kaydetmek (commit etmek) için

    // Constructor (Yapıcı metot): Dışarıdan repository ve unitofwork bağımlılıklarını içeri alıyoruz (Dependency Injection)
    public GenericService(IGenericRepository<T> repository, IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
    }

    // Tablodaki tüm kayıtları asenkron olarak getirir
    public async Task<IEnumerable<T>> GetAllAsync()
    {
        return await _repository.GetAllAsync();
    }

    // ID'ye göre tek bir kaydı getirir
    public async Task<T?> GetByIdAsync(int id)
    {
        return await _repository.GetByIdAsync(id);
    }

    // Yeni kayıt ekler ve UnitOfWork ile veritabanına kalıcı olarak kaydeder
    public async Task AddAsync(T entity)
    {
        await _repository.AddAsync(entity);
        await _unitOfWork.CommitAsync(); // Değişikliği mühürle ve kaydet
    }

    // Kaydı günceller ve UnitOfWork ile veritabanına kalıcı olarak kaydeder
    public async Task UpdateAsync(T entity)
    {
        _repository.Update(entity);
        await _unitOfWork.CommitAsync(); // Değişikliği mühürle ve kaydet
    }

    // Kaydı siler ve UnitOfWork ile veritabanına kalıcı olarak kaydeder
    public async Task RemoveAsync(T entity)
    {
        _repository.Delete(entity);
        await _unitOfWork.CommitAsync(); // Değişikliği mühürle ve kaydet
    }

    // Şarta uyan bir kayıt olup olmadığını kontrol eder
    public async Task<bool> AnyAsync(Expression<Func<T, bool>> predicate)
    {
        return await _repository.AnyAsync(predicate);
    }
}