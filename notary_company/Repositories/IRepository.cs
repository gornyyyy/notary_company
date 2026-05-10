using System.Collections.Generic;

namespace notary_company.Repositories
{
    public interface IRepository<T>
    {
        int Create(T entity);
        List<T> ReadAll();
        T ReadById(int id);
    }
}