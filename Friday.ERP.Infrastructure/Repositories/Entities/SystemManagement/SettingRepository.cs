using Friday.ERP.Core.Data.Entities.SystemManagement;
using Friday.ERP.Core.IRepositories.Entities.SystemManagement;
using Microsoft.EntityFrameworkCore;

namespace Friday.ERP.Infrastructure.Repositories.Entities.SystemManagement;

internal sealed class SettingRepository(RepositoryContext repositoryContext)
    : RepositoryBase<Setting>(repositoryContext), ISettingRepository
{
    public async Task<Setting?> GetSetting(bool trackChanges)
    {
        return await FindAll(trackChanges).SingleOrDefaultAsync();
    }
}