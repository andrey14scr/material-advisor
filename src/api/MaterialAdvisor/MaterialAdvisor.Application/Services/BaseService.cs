using MaterialAdvisor.Data;
using MaterialAdvisor.Data.Entities;

using Microsoft.EntityFrameworkCore;

namespace MaterialAdvisor.Application.Services;

public abstract class BaseService<TEntity> : IBaseService where TEntity : class, IEntity
{
    public async Task<TModel> Create<TModel>(TModel model)
    {
        var entityToCreate = await MapToEntity(model);
        var createdEntity = await CreateAndSave(entityToCreate);
        var createdModel = await MapToModel<TModel>(createdEntity);
        return createdModel;
    }

    public async Task<bool> Delete(Guid id)
    {
        var entityToDelete = await GetFullEntityForDelete().SingleAsync(t => t.Id == id);
        var deleted = await DeleteAndSave(entityToDelete);
        return deleted != 0;
    }

    public async Task<TModel> Get<TModel>(Guid id)
    {
        var entity = await GetFullEntity().AsNoTracking().SingleAsync(t => t.Id == id);
        var model = await MapToModel<TModel>(entity);
        return model;
    }

    public async Task<TModel> HardUpdate<TModel>(TModel model)
    {
        var entityToUpdate = await MapToEntity(model);
        var existingEntity = await GetFullEntityForDelete().SingleAsync(t => t.Id == entityToUpdate.Id);

        using var transaction = await GetDbContext().Database.BeginTransactionAsync();
        try
        {
            await DeleteAndSave(existingEntity);
            var createdEntity = await CreateAndSave(entityToUpdate);
            return await MapToModel<TModel>(createdEntity);
        }
        catch
        {
            await transaction.RollbackAsync();
            throw;
        }
    }

    public async Task<TModel> SoftUpdate<TModel>(TModel model)
    {
        var entityToUpdate = await MapToEntity(model);
        var updatedEntity = await UdpateAndSave(entityToUpdate);
        var updatedModel = await MapToModel<TModel>(updatedEntity);
        return updatedModel;
    }

    protected abstract IQueryable<TEntity> GetFullEntity();

    protected virtual IQueryable<TEntity> GetFullEntityForDelete() => GetFullEntity();

    protected abstract Task<TEntity> MapToEntity<TModel>(TModel model);

    protected abstract Task<TModel> MapToModel<TModel>(TEntity entity);

    protected abstract DbSet<TEntity> GetDbSet();

    protected abstract MaterialAdvisorContext GetDbContext();

    protected async Task<int> DeleteAndSave(TEntity entity)
    {
        GetDbSet().Remove(entity);
        GetDbContext().RemoveUnusedLanguageTexts();
        var deleted = await GetDbContext().SaveChangesAsync();
        return deleted;
    }

    private async Task<TEntity> CreateAndSave(TEntity entity)
    {
        var createdTopic = await GetDbSet().AddAsync(entity);
        await GetDbContext().SaveChangesAsync();
        return createdTopic.Entity;
    }

    private async Task<TEntity> UdpateAndSave(TEntity entity)
    {
        var updatedEntity = GetDbSet().Update(entity);
        await GetDbContext().SaveChangesAsync();
        return updatedEntity.Entity;
    }
}
