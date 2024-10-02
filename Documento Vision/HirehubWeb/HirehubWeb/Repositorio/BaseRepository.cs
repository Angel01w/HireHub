using HirehubWeb.Interfaces;
using HirehubWeb.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace HirehubWeb.Repositorio
{
    public class BaseRepository<T> : IBaseRepository<T> where T : class
    {
        private readonly ApplicationDbContext _applicationDBContext;
        public DbSet<T> dbSet;
        protected List<Expression<Func<T, object>>> Includes { get; } = new List<Expression<Func<T, object>>>();

        public BaseRepository(ApplicationDbContext applicationDB)
        {
            _applicationDBContext = applicationDB;
            dbSet = _applicationDBContext.Set<T>();
        }

        public async Task<Result<bool>> Add(T entity)
        {
            try
            {
                await _applicationDBContext.Set<T>().AddAsync(entity);
                await _applicationDBContext.SaveChangesAsync();
                return Result<bool>.SuccessResult(true);
            }
            catch (Exception ex)
            {
                return Result<bool>.ErrorResult("Error al Guardar: " + ex.Message);
            }
        }

        public async Task<Result<IEnumerable<T>>> GetAll()
        {
            try
            {
                var entities = await _applicationDBContext.Set<T>().ToListAsync();
                return Result<IEnumerable<T>>.SuccessResult(entities);
            }
            catch (Exception ex)
            {
                return Result<IEnumerable<T>>.ErrorResult("Error al Buscar: " + ex.Message);
            }
        }

        public virtual Result<T> GetFirst(Expression<Func<T, bool>> query)
        {
            try
            {
                IQueryable<T> currentQuery = ImplementIncludes(dbSet);
                var entity = currentQuery.FirstOrDefault(query);
                return Result<T>.SuccessResult(entity);
            }
            catch (Exception ex)
            {
                return Result<T>.ErrorResult("Error al obtener el primer registro: " + ex.Message);
            }
        }

        public async Task<Result<bool>> Delete(T entity)
        {
            try
            {
                _applicationDBContext.Set<T>().Remove(entity);
                await _applicationDBContext.SaveChangesAsync();
                return Result<bool>.SuccessResult(true);
            }
            catch (Exception ex)
            {
                return Result<bool>.ErrorResult("Error al Eliminar: " + ex.Message);
            }
        }

        public async Task<Result<bool>> Update(T entity)
        {
            try
            {
                _applicationDBContext.Set<T>().Update(entity);
                await _applicationDBContext.SaveChangesAsync();
                return Result<bool>.SuccessResult(true);
            }
            catch (Exception ex)
            {
                return Result<bool>.ErrorResult("Error al Actualizar: " + ex.Message);
            }
        }

        // Método corregido para filtrar entidades
        public async Task<Result<IEnumerable<T>>> Filter(Expression<Func<T, bool>> expression)
        {
            try
            {
                var entities = await _applicationDBContext.Set<T>().Where(expression).ToListAsync();
                return Result<IEnumerable<T>>.SuccessResult(entities);
            }
            catch (Exception ex)
            {
                return Result<IEnumerable<T>>.ErrorResult("Error al Filtrar: " + ex.Message);
            }
        }

        public IQueryable<T> ImplementIncludes(IQueryable<T> IncludedQuery)
        {
            IQueryable<T> currentQuery = IncludedQuery;
            foreach (var include in Includes)
                currentQuery = currentQuery.Include(include);

            return currentQuery;
        }


        public async Task<T> GetFirstAsync(Expression<Func<T, bool>> query)
        {
            return await dbSet.FirstOrDefaultAsync(query);
        }
    }
}
