using Friday.ERP.Shared.DataTransferObjects;
using Friday.ERP.Shared.RequestFeatures;

namespace Friday.ERP.Core.IServices.Entities;

public interface IAccountService
{
    Task<UserViewDto> CreateUser(UserCreateDto userCreateDto);

    Task<UserViewDto> GetUserByGuid(Guid userGuid);

    Task<UserViewDto> UpdateUserByGuid(Guid userGuid, UserUpdateDto userUpdateDto, string currentUserGuid);

    Task<(List<UserViewDto>, MetaData metaData)> GetAllUsers(UserParameter userParameter);

    Task<List<UserRoleViewDto>> GetAllUserRoles(string? searchTerm);
}