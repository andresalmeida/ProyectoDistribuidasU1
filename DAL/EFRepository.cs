using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;

namespace DAL
{
    public class EFRepository : IRepository
    {
        private readonly DbContext Context;

        public EFRepository(DbContext context)
        {
            this.Context = context;
        }

        public TEntity Create<TEntity>(TEntity toCreate) where TEntity : class
        {
            TEntity result = null;
            try
            {
                Context.Set<TEntity>().Add(toCreate);
                Context.SaveChanges();
                result = toCreate;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al crear la entidad {typeof(TEntity).Name}: {ex.Message}");
                throw new Exception($"Error al crear la entidad {typeof(TEntity).Name}.", ex);
            }
            return result;
        }

        public bool Delete<TEntity>(TEntity toDelete) where TEntity : class
        {
            bool result = false;
            try
            {
                Context.Entry(toDelete).State = EntityState.Deleted;
                result = Context.SaveChanges() > 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al eliminar la entidad {typeof(TEntity).Name}: {ex.Message}");
                throw new Exception("Error al eliminar la entidad.", ex);
            }
            return result;
        }

        public bool Update<TEntity>(TEntity toUpdate) where TEntity : class
        {
            bool result = false;
            try
            {
                Context.Entry(toUpdate).State = EntityState.Modified;
                result = Context.SaveChanges() > 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al actualizar la entidad {typeof(TEntity).Name}: {ex.Message}");
                throw new Exception("Error al actualizar la entidad.", ex);
            }
            return result;
        }

        public TEntity Retrieve<TEntity>(Expression<Func<TEntity, bool>> criteria) where TEntity : class
        {
            TEntity result = null;
            try
            {
                result = Context.Set<TEntity>().FirstOrDefault(criteria);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al recuperar la entidad {typeof(TEntity).Name}: {ex.Message}");
                throw new Exception("Error al recuperar la entidad.", ex);
            }
            return result;
        }

        public List<TEntity> Filter<TEntity>(Expression<Func<TEntity, bool>> criteria) where TEntity : class
        {
            List<TEntity> result = null;
            try
            {
                result = Context.Set<TEntity>().Where(criteria).ToList();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al filtrar las entidades {typeof(TEntity).Name}: {ex.Message}");
                throw new Exception("Error al filtrar las entidades.", ex);
            }
            return result;
        }

        public List<TEntity> GetAll<TEntity>() where TEntity : class
        {
            List<TEntity> result = null;
            try
            {
                result = Context.Set<TEntity>().ToList();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al recuperar todas las entidades {typeof(TEntity).Name}: {ex.Message}");
                throw new Exception("Error al recuperar todas las entidades.", ex);
            }
            return result;
        }

        public void Dispose()
        {
            Context?.Dispose();
        }
    }
}