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
            throw new ApplicationException("An error occurred while retrieving all ToDo items.", e);
        }
    }

    public async Task<User> GetUserByIdAsync(string id)
    {
        
        try
        {
            return await _users.Find<User>(user => user.Id == id).FirstOrDefaultAsync();
        }
        catch (Exception e)
        {
            throw new ApplicationException("An error occurred while retrieving all ToDo items.", e);
        }
    }

    public async Task<User> GetUserByEmailAsync(string email)
    {
        try
        {
            return await _users.Find<User>(user => user.Email == email).FirstOrDefaultAsync();
        }
        catch (Exception e)
        {
            throw new ApplicationException("An error occurred while retrieving all ToDo items.", e);
        }
    }

    public async Task CreateUserAsync(User user)
    {
        try
        {
            await _users.InsertOneAsync(user);
        }
        catch (Exception e)
        {
            throw new ApplicationException("An error occurred while retrieving all ToDo items.", e);
        }
    }

    public async Task UpdateUserAsync(User user)
    {
        try
        {
            await _users.ReplaceOneAsync(u => u.Id == user.Id, user);
        }
        catch (Exception e)
        {
            throw new ApplicationException("An error occurred while retrieving all ToDo items.", e);
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
            throw new ApplicationException("An error occurred while retrieving all ToDo items.", e);
        }
    }
}