using Authentication.Application.Abstracts;
using Authentication.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Authentication.Infraestructure.Repositories;

public class UserRepository(ApplicationDbContext dbContext) : IUserRepository
{
    public async Task<User?> GetUserByRefreshTokenAsync(string refreshToken)
    {
        var user = await dbContext.Users.FirstOrDefaultAsync(x=>x.RefreshToken == refreshToken);
        return user;
    }
}