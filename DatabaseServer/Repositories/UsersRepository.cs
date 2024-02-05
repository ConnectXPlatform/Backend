using Firebase;
using Domain.UserDtos;

namespace DatabaseServer.Repositories;

internal sealed class UsersRepository : IUsersRepository
{
    private readonly FirebaseService firebaseService;

    public UsersRepository(FirebaseService firebaseService)
    {
        this.firebaseService = firebaseService;
    }

    public async Task<UserDto?> GetUser(string userId, CancellationToken cancellationToken)
    {
        return await firebaseService.GetUser(userId, cancellationToken);
    }

    public async Task<UserCreatedDto?> CreateUser(NewUserDto userDto, CancellationToken cancellationToken)
    {
        return await firebaseService.CreateUser(userDto, cancellationToken);
    }

    public async Task UpdateUser(string userId, UpdateUserDto update, CancellationToken cancellationToken)
    {
        await firebaseService.UpdateUser(userId, update, cancellationToken);
    }

    public async Task DeleteUser(string userId, CancellationToken cancellationToken)
    {
        await firebaseService.DeleteUser(userId, cancellationToken);
    }
}