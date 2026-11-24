using System;
using System.Threading.Tasks;

namespace ProjectManagementSystemV2.Repositories;

public interface IUnitOfWork : IDisposable// arayüz:ne yapılacağını söyleyen sözleşme kağıdıdır.
{
   Task<int> CommitAsync();//VERİ ATBAANINDA YAPILAN DEĞİŞİKLİKLERİ İNTTE KAÇ SATIR DDEĞİŞİKLİK YAPILDIĞINI GÖSTERİR
   void Commit();
}