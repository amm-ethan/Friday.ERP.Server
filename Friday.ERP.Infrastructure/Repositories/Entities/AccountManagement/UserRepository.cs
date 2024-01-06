using Friday.ERP.Core.Data.Entities.AccountManagement;
using Friday.ERP.Core.IRepositories.Entities.AccountManagement;
using Friday.ERP.Infrastructure.Utilities.Entities;
using Friday.ERP.Shared.DataTransferObjects;
using Friday.ERP.Shared.RequestFeatures;
using Microsoft.EntityFrameworkCore;

namespace Friday.ERP.Infrastructure.Repositories.Entities.AccountManagement;

internal sealed class UserRepository(RepositoryContext repositoryContext)
    : RepositoryBase<User>(repositoryContext), IUserRepository
{
    public void CreateUser(User user)
    {
        Create(user);
    }

    public async Task<User?> GetUserByGuid(Guid guid, bool trackChanges)
    {
        return await FindByCondition(c => c.Guid.Equals(guid), trackChanges)
            .Include(c => c.UserRole)
            .SingleOrDefaultAsync();
    }

    public async Task<User?> GetUserByUsername(string username, bool trackChanges)
    {
        return await FindByCondition(c => c.Username == username, trackChanges)
            .Include(c => c.UserRole)
            .SingleOrDefaultAsync();
    }

    public async Task<User?> GetUserByEmail(string email, bool trackChanges)
    {
        return await FindByCondition(c => c.Email == email, trackChanges)
            .SingleOrDefaultAsync();
    }

    public async Task<User?> GetUserByName(string name, bool trackChanges)
    {
        return await FindByCondition(c => c.Name == name, trackChanges)
            .SingleOrDefaultAsync();
    }

    public async Task<User?> GetUserByPhone(string phone, bool trackChanges)
    {
        return await FindByCondition(c => c.Phone == phone, trackChanges)
            .SingleOrDefaultAsync();
    }

    public async Task<PagedList<User>> GetAllUsers(UserParameter userParameter, bool trackChanges)
    {
        var users = await FindAll(trackChanges).Include(c => c.UserRole)
            .Search(userParameter.SearchTerm)
            .Sort(userParameter.OrderBy!)
            .Skip((userParameter.PageNumber - 1) * userParameter.PageSize)
            .Take(userParameter.PageSize)
            .ToListAsync();

        var count = await FindAll(trackChanges)
            .Search(userParameter.SearchTerm)
            .Sort(userParameter.OrderBy!).CountAsync();

        return PagedList<User>.ToPagedList(users, count, userParameter.PageNumber,
            userParameter.PageSize);
    }

    public async Task<IEnumerable<Guid>> GetAllUserGuids(bool trackChanges)
    {
        return await FindAll(trackChanges)
            .Select(c => c.Guid)
            .ToListAsync();
    }
}