using System.Text;
using BCrypt.Net;
using Friday.ERP.Core.Data.Entities.AccountManagement;
using Friday.ERP.Core.Exceptions.BadRequest;
using Friday.ERP.Core.Exceptions.NotFound;
using Friday.ERP.Core.IRepositories;
using Friday.ERP.Core.IServices;
using Friday.ERP.Core.IServices.Entities;
using Friday.ERP.Shared.DataTransferObjects;
using Friday.ERP.Shared.RequestFeatures;

namespace Friday.ERP.Infrastructure.Services.Entities;

internal sealed class AccountService(IRepositoryManager repository, ILoggerManager logger) : IAccountService
{
    public async Task<UserViewDto> CreateUser(UserCreateDto userCreateDto)
    {
        var user = User.FromUserCreateDto(userCreateDto);

        var currentPassword = Encoding.UTF8.GetString(userCreateDto.Password);
        var passwordHash = BCrypt.Net.BCrypt.EnhancedHashPassword(currentPassword, HashType.SHA512);
        user.Password = passwordHash;

        _ = await ValidateUserRole(userCreateDto.RoleGuid);
        user.UserRoleGuid = userCreateDto.RoleGuid;

        var existingUser = await repository.User.GetUserByUsername(userCreateDto.Username, false);
        if (existingUser is not null)
            throw new DuplicateEntryBadRequestException(userCreateDto.Username, "Username", "User");

        var existingName = await repository.User.GetUserByName(userCreateDto.Name, false);
        if (existingName is not null)
            throw new DuplicateEntryBadRequestException(userCreateDto.Name, "Name", "User");

        if (userCreateDto.Phone != null)
        {
            var existingPhone = await repository.User.GetUserByPhone(userCreateDto.Phone!, false);
            if (existingPhone is not null)
                throw new DuplicateEntryBadRequestException(userCreateDto.Phone!, "Phone", "User");
        }

        if (userCreateDto.Email != null)
        {
            var existingEmail = await repository.User.GetUserByEmail(userCreateDto.Email!, false);
            if (existingEmail is not null)
                throw new DuplicateEntryBadRequestException(userCreateDto.Email!, "Email", "User");
        }

        repository.User.CreateUser(user);
        await repository.SaveAsync();
        return await GetUserByGuid(user.Guid);
    }

    public async Task<UserViewDto> GetUserByGuid(Guid userGuid)
    {
        var user = await ValidateUser(userGuid);
        return ToUserViewDto(user);
    }

    public async Task<UserViewDto> UpdateUserByGuid(Guid userGuid, UserUpdateDto userUpdateDto, string currentUserGuid)
    {
        var currentUser = await ValidateUser(userGuid, true);

        if (userUpdateDto.Name is not null)
        {
            var existingName = await repository.User.GetUserByUsername(userUpdateDto.Name, false);
            if (existingName is not null && existingName.Guid != userGuid)
                throw new DuplicateEntryBadRequestException(userUpdateDto.Name, "Username", "User");
            currentUser.Name = userUpdateDto.Name;
        }


        if (userUpdateDto.Phone is not null)
        {
            var existingPhone = await repository.User.GetUserByPhone(userUpdateDto.Phone!, false);
            if (existingPhone is not null && existingPhone.Guid != userGuid)
                throw new DuplicateEntryBadRequestException(userUpdateDto.Phone!, "Phone", "User");
            currentUser.Phone = userUpdateDto.Phone;
        }


        if (userUpdateDto.Email is not null)
        {
            var existingEmail = await repository.User.GetUserByEmail(userUpdateDto.Email!, false);
            if (existingEmail is not null && existingEmail.Guid != userGuid)
                throw new DuplicateEntryBadRequestException(userUpdateDto.Email!, "Email", "User");
            currentUser.Email = userUpdateDto.Email;
        }

        if (userUpdateDto.RoleGuid is not null)
        {
            _ = await ValidateUserRole(userUpdateDto.RoleGuid ?? Guid.Empty);
            currentUser.UserRoleGuid = userUpdateDto.RoleGuid!;
        }

        if (userUpdateDto is { IsForceReset: true, NewPassword: not null })
        {
            var newPassword = Encoding.UTF8.GetString(userUpdateDto.NewPassword);
            var passwordHash = BCrypt.Net.BCrypt.EnhancedHashPassword(newPassword, HashType.SHA512);
            currentUser.Password = passwordHash;
        }

        if (userUpdateDto.OldPassword is not null && userUpdateDto.NewPassword is not null)
        {
            var oldPassword = Encoding.UTF8.GetString(userUpdateDto.OldPassword);
            var isCorrect = BCrypt.Net.BCrypt.EnhancedVerify(oldPassword, currentUser.Password, HashType.SHA512);

            if (isCorrect)
            {
                var newPassword = Encoding.UTF8.GetString(userUpdateDto.NewPassword);
                var passwordHash = BCrypt.Net.BCrypt.EnhancedHashPassword(newPassword, HashType.SHA512);
                currentUser.Password = passwordHash;
            }
            else
            {
                throw new GeneralBadRequestException("Old Password and New Password didn't match");
            }
        }

        await repository.SaveAsync();

        logger.LogInfo($"User {currentUser.Guid} is Updated by UserId {currentUserGuid}");

        return await GetUserByGuid(userGuid);
    }

    public async Task<(List<UserViewDto>, MetaData metaData)> GetAllUsers(UserParameter userParameter)
    {
        var createdUsers = await repository.User.GetAllUsers(userParameter, false);
        return (createdUsers.Select(ToUserViewDto).ToList(), metaData: createdUsers.MetaData);
    }

    public async Task<List<UserRoleViewDto>> GetAllUserRoles(string? searchTerm)
    {
        var createdAllRoles = await repository.UserRole.GetAllUserRoles(searchTerm, false);
        var roleToReturn = createdAllRoles
            .Select(role => new UserRoleViewDto(Name: role.Name!, Guid: role.Guid)).ToList();
        return roleToReturn;
    }

    #region Private Methods

    private static UserViewDto ToUserViewDto(User user)
    {
        return new UserViewDto
        (
            user.Guid,
            user.Name!,
            user.Phone!,
            user.Email!,
            user.Username!,
            user.UserRole!.Guid,
            user.UserRole!.Name!
        );
    }

    private async Task<UserRole> ValidateUserRole(Guid userRoleGuid, bool trackChanges = false)
    {
        var existingRole = await repository.UserRole.GetUserRoleByGuid(userRoleGuid, trackChanges);
        if (existingRole is null)
            throw new ObjectNotFoundByFilterException("UserRoleGuid", "UserRole",
                userRoleGuid.ToString());
        return existingRole;
    }

    private async Task<User> ValidateUser(Guid userGuid, bool trackChanges = false)
    {
        var user = await repository.User.GetUserByGuid(userGuid, trackChanges);
        if (user is null)
            throw new ObjectNotFoundByFilterException("UserGuid", "User",
                userGuid.ToString());
        return user;
    }

    #endregion
}