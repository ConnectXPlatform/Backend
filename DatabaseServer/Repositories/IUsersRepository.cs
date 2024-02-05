using Domain.UserDtos;

namespace DatabaseServer.Repositories;

internal interface IUsersRepository
{
    public Task<UserDto?> GetUser(string userId, CancellationToken cancellationToken);
    public Task<UserCreatedDto?> CreateUser(NewUserDto userDto, CancellationToken cancellationToken);
    public Task UpdateUser(string userId, UpdateUserDto update, CancellationToken cancellationToken);
    public Task DeleteUser(string userId, CancellationToken cancellationToken);
}