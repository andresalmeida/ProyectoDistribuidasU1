using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace DAL
{
    public interface IRepository : IDisposable
    {
        // Agrega una nueva entidad a la base de datos
        TEntity Create<TEntity>(TEntity toCreate) where TEntity : class;

        // Elimina una entidad de la base de datos
        bool Delete<TEntity>(TEntity toDelete) where TEntity : class;

        // Actualiza una entidad existente en la base de datos
        bool Update<TEntity>(TEntity toUpdate) where TEntity : class;

        // Recupera una entidad según un criterio específico
        TEntity Retrieve<TEntity>(Expression<Func<TEntity, bool>> criteria) where TEntity : class;

        // Recupera un conjunto de entidades que cumplen con un criterio
        List<TEntity> Filter<TEntity>(Expression<Func<TEntity, bool>> criteria) where TEntity : class;

        // Recupera todos los registros de una entidad
        List<TEntity> GetAll<TEntity>() where TEntity : class;
    }
}
