using AutoMapper;

using MaterialAdvisor.Application.Services.Abstraction;
using MaterialAdvisor.Data;
using MaterialAdvisor.Data.Entities;

using Microsoft.EntityFrameworkCore;

namespace MaterialAdvisor.Application.Services;

public class GeneratedFileService(MaterialAdvisorContext _dbContext, IMapper _mapper, IUserProvider _userProvider) : IGeneratedFileService
{
    public async Task<TModel> Get<TModel>(Guid id)
    {
        var entity = await _dbContext.GeneratedFiles.SingleAsync(gf => gf.Id == id);
        var model = _mapper.Map<TModel>(entity);
        return model;
    }

    public async Task<IList<TModel>> GetByKnowledgeCheckId<TModel>(Guid id)
    {
        var entities = await _dbContext.GeneratedFiles
            .Where(gf => gf.GeneratedFilesKnowldgeChecks.Any(gfkc => gfkc.KnowledgeCheckId == id))
            .ToListAsync();

        var models = _mapper.Map<IList<TModel>>(entities);
        return models;
    }

    public async Task<TModel> AddPreGeneratedFile<TModel>(Guid knowledgeCheckId)
    {
        var user = await _userProvider.GetUser();

        var created = await _dbContext.GeneratedFiles.AddAsync(new GeneratedFileEntity
        {
            OwnerId = user.Id,
            GeneratedAt = DateTime.UtcNow,
            GeneratedFilesKnowldgeChecks =
            [
                new GeneratedFilesKnowldgeChecks
                {
                    KnowledgeCheckId = knowledgeCheckId,
                }
            ]
        });
        await _dbContext.SaveChangesAsync();
        var model = _mapper.Map<TModel>(created.Entity);
        return model;
    }
}