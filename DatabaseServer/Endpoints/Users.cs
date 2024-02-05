using DatabaseServer.Repositories;
using Domain.UserDtos;

namespace DatabaseServer.Endpoints;

internal static class Users
{
    public static T MapUsersEndpoints<T>(this T endpointRouteBuilder) where T : IEndpointRouteBuilder
    {
        endpointRouteBuilder.MapGet("/{userId}", GetUser)
            .Produces<UserDto>()
            .WithName(nameof(GetUser));
        endpointRouteBuilder.MapPost("/", CreateUser)
            .Produces<UserCreatedDto>(StatusCodes.Status201Created);
        endpointRouteBuilder.MapPut("/{userId}", UpdateUser);
        endpointRouteBuilder.MapDelete("/{userId}", DeleteUser);

        return endpointRouteBuilder;
    }

    private static async Task<IResult> GetUser(IUsersRepository usersRepository, string userId,
        CancellationToken cancellationToken)
    {
        UserDto? user = await usersRepository.GetUser(userId, cancellationToken);
        if (user is null)
            return Results.NotFound();
        return Results.Ok(user);
    }

    private static async Task<IResult> CreateUser(IUsersRepository usersRepository, NewUserDto userDto,
        HttpContext httpContext, CancellationToken cancellationToken)
    {
        IResult user = await GetUser(usersRepository, userDto.UserId, cancellationToken);
        if (user is not null)
        {
            return Results.Conflict();
        }
        UserCreatedDto? createdDto = await usersRepository.CreateUser(userDto, cancellationToken);
        if (createdDto is null)
            return Results.BadRequest();

        return Results.CreatedAtRoute(nameof(GetUser), new { userId = createdDto.User.Id }, createdDto);
    }

    private static async Task<IResult> UpdateUser(IUsersRepository usersRepository, string userId, UpdateUserDto update,
        CancellationToken cancellationToken)
    {
        await usersRepository.UpdateUser(userId, update, cancellationToken);
        return Results.NoContent();
    }

    private static async Task<IResult> DeleteUser(IUsersRepository usersRepository, string userId,
        CancellationToken cancellationToken)
    {
        await usersRepository.DeleteUser(userId, cancellationToken);
        return Results.NoContent();
    }
}