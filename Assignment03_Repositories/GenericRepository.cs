using Assignment03_BussinessObject.Entities;
using Assignment03_Repositories.IRepositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assignment03_Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        protected readonly eStore_AspNetContext eBookStoreDBContext;
        protected readonly DbSet<T> dbSet;

        public GenericRepository(eStore_AspNetContext eBookStoreDBContext)

        {
            if (this.eBookStoreDBContext == null)
            {
                this.eBookStoreDBContext = eBookStoreDBContext;
            }
            dbSet = this.eBookStoreDBContext.Set<T>();
        }
        public async Task<bool> Add(T item)
        {
            try
            {
                dbSet.Add(item);
                await eBookStoreDBContext.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<bool> Delete(object id)
        {
            try
            {
                var item = dbSet.Find(id);
                if (item != null)
                {
                    dbSet.Remove(item);
                    await eBookStoreDBContext.SaveChangesAsync();
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<T> Get(object id)
        {
            try
            {
                return await dbSet.FindAsync(id);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public IEnumerable<T> GetAll()
        {
            try
            {
                IQueryable<T> query = dbSet;
                var result = query.ToList();
                return result;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<bool> Update(object id, T item)
        {
            try
            {
                var check = dbSet.Find(id);
                if (check != null)
                {
                    eBookStoreDBContext.Entry(check).State = EntityState.Detached;
                    dbSet.Update(item);
                    await eBookStoreDBContext.SaveChangesAsync();
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public virtual async Task<IEnumerable<T>> GetAllProduct()
        {
            try
            {
                return await dbSet.ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception($"Error when get {nameof(T)}: {ex.Message}");
            }
        }

        public IEnumerable<T> GetAllOrder(Func<T, bool> func)
        {
            try
            {
                return dbSet.Where(func);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<bool> UpdateOrder(object id1, object id2, T item)
        {
            try
            {
                if (id2 == null)
                {
                    var itemDb = dbSet.Find(id1);
                    if (itemDb != null)
                    {
                        eBookStoreDBContext.Attach(itemDb).State = EntityState.Detached;
                        dbSet.Update(item);
                        await eBookStoreDBContext.SaveChangesAsync();
                        return true;
                    }
                    else
                    {
                        throw new Exception($"Not Found {nameof(T)}");
                    }
                }
                else
                {
                    var itemDb = dbSet.Find(id1, id2);
                    if (itemDb != null)
                    {
                        eBookStoreDBContext.Attach(itemDb).State = EntityState.Detached;
                        dbSet.Update(item);
                        await eBookStoreDBContext.SaveChangesAsync();
                        return true;
                    }
                    else
                    {
                        throw new Exception($"Not Found {nameof(T)}");
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public async Task<bool> UpdateProducts(List<Product> products)
        {
            try
            {
                foreach (var product in products)
                {
                    var itemDb = eBookStoreDBContext.Products.Find(product.ProductId);
                    if (itemDb != null)
                    {
                        eBookStoreDBContext.Products.Attach(itemDb).State = EntityState.Detached;
                        eBookStoreDBContext.Products.Update(product);
                    }
                    else
                    {
                        throw new Exception($"Not Found {nameof(T)}");
                    }
                }
                await eBookStoreDBContext.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public async Task<bool> DeleteAll(List<object> id1s, object id2)
        {
            try
            {
                foreach (var id1 in id1s)
                {
                    if (id2 == null)
                    {
                        var item = dbSet.Find(id1);
                        if (item != null)
                        {
                            dbSet.Remove(item);
                        }
                        else
                        {
                            throw new Exception($"Not Found {nameof(T)}");
                        }
                    }
                    else
                    {
                        var item = dbSet.Find(id2, id1);
                        if (item != null)
                        {
                            dbSet.Remove(item);
                        }
                        else
                        {
                            throw new Exception($"Not Found {nameof(T)}");
                        }
                    }
                }
                await eBookStoreDBContext.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public async Task<bool> DeleteOrder(object id1, object id2)
        {
            try
            {
                if (id2 == null)
                {
                    var item = dbSet.Find(id1);
                    if (item != null)
                    {
                        dbSet.Remove(item);
                        await eBookStoreDBContext.SaveChangesAsync();
                        return true;
                    }
                    else
                    {
                        throw new Exception($"Not Found {nameof(T)}");
                    }
                }
                else
                {
                    var item = dbSet.Find(id1, id2);
                    if (item != null)
                    {
                        dbSet.Remove(item);
                        await eBookStoreDBContext.SaveChangesAsync();
                        return true;
                    }
                    else
                    {
                        throw new Exception($"Not Found {nameof(T)}");
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

    }
}
