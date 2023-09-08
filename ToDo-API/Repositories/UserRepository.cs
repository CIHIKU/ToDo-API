using MongoDB.Driver;
using ToDo_API.Models;

namespace ToDo_API.Repositories;

public class UserRepository : IUserRepository
{
    private readonly IMongoCollection<User> _users;

    public UserRepository(IMongoCollection<User> user)
    {
        _users = user;
    }

    public async Task<IEnumerable<User>> GetAllUsersAsync()
    {
        try
        {
            return await _users.Find(user => true).ToListAsync();
        }
        catch (Exception e)
        {
            throw new ApplicationException("", e);
        }
    }

    public async Task<User> GetUserByIdAsync(string id)
    {
        try
        {
            return await _users.Find(user => user.Id == id).FirstOrDefaultAsync();
        }
        catch (Exception e)
        {
            throw new ApplicationException("", e);
        }
    }

    public async Task<User> GetUserByEmailAsync(string email)
    {
        try
        {
            return await _users.Find(user => user.Email == email).FirstOrDefaultAsync();
        }
        catch (Exception e)
        {
            throw new ApplicationException("A user with this email address doesn't exists.", e);
        }
    }

    public async Task CreateUserAsync(User user)
    {
        if (user == null) throw new ArgumentNullException(nameof(user));
        try
        {
            await _users.InsertOneAsync(user);
        }
        catch (Exception e)
        {
            throw new ApplicationException("", e);
        }
    }

    public async Task UpdateUserAsync(string id, User user)
    {
        if (user == null) throw new ArgumentNullException(nameof(user));
        try
        {
            var updateDefinition = new UpdateDefinitionBuilder<User>();

            var updates = new List<UpdateDefinition<User>>();
            if (updates == null) throw new ArgumentNullException(nameof(updates));

            var combinedUpdateDefinition = updateDefinition.Combine(updates);
            await _users.UpdateOneAsync(u => u.Id == id, combinedUpdateDefinition);
        }
        catch (Exception e)
        {
            throw new ApplicationException("", e);
        }
    }

    public async Task DeleteUserAsync(string id)
    {
        try
        {
            await _users.DeleteOneAsync(user => user.Id == id);
        }
        catch (Exception e)
        {
            throw new ApplicationException("", e);
        }
    }
}