namespace ToDo_API.Utilities;

public interface IPasswordUtility
{
    void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt);
    bool VerifyPasswordHash(string password, byte[] storedHash, byte[] storedSalt);
    public (bool IsValid, string ErrorMessage) Validate(string password);
}