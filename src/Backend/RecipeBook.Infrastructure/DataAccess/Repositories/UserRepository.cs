using Microsoft.EntityFrameworkCore;
using RecipeBook.Domain.Entities;
using RecipeBook.Domain.Repositories.User;

namespace RecipeBook.Infrastructure.DataAccess.Repositories
{
    /// <summary>
    /// Interfaces para agrupar os mesmos Endpoints. 
    ///  A       B       C       D
    /// GET     POST    PUT     DEL
    /// GET     POST    PUT     DEL
    /// Etc...
    /// </summary>
    public class UserRepository : IUserReadOnlyRepository, IUserWriteOnlyRepository
    {
        private readonly RecipeBookDbContext _dbContext;

        public UserRepository(RecipeBookDbContext dbContext) => _dbContext = dbContext;

        public async Task Add(User user) => await _dbContext.Users.AddAsync(user);

        public async Task<bool> ExistActiveUserWithEmail(string email) => await _dbContext.Users.AnyAsync(user => user.Email.Equals(email) && user.Active);

        public async Task<User?> GetByEmailAndPassword(string email, string password)
        {
            return await _dbContext
                .Users
                .AsNoTracking()
                .FirstOrDefaultAsync(user => user.Active && user.Email.Equals(email) && user.Password.Equals(password));
        }
    }
}
