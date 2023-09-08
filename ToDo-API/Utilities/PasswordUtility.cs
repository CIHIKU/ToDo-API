using System.Security.Cryptography;

namespace ToDo_API.Utilities;

public class PasswordUtility : IPasswordUtility
{
    private readonly int _minLength;
    private readonly int _maxLength;
    private readonly bool _requireDigit;
    private readonly bool _requireNonAlphanumeric;
    private readonly bool _requireUppercase;
    private readonly bool _requireLowercase;
    
    public PasswordUtility(int minLength = 8, int maxLength = 100, bool requireDigit = true, bool requireNonAlphanumeric = true, bool requireUppercase = true, bool requireLowercase = true)
    {
        _minLength = minLength;
        _maxLength = maxLength;
        _requireDigit = requireDigit;
        _requireNonAlphanumeric = requireNonAlphanumeric;
        _requireUppercase = requireUppercase;
        _requireLowercase = requireLowercase;
    }
    
    public void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
    {
        if (password == null) throw new ArgumentNullException(nameof(password));

        using var hmac = new HMACSHA256();
        passwordSalt = hmac.Key;
        passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
    }

    public bool VerifyPasswordHash(string password, byte[] storedHash, byte[] storedSalt)
    {
        if (password == null) throw new ArgumentNullException(nameof(password));
        if (storedHash.Length != 32) throw new ArgumentException($"Invalid length of password hash (32 bytes expected). {storedHash.Length}", nameof(storedHash));
        if (storedSalt.Length != 64) throw new ArgumentException($"Invalid length of password salt (64 bytes expected) {storedSalt.Length}.", nameof(storedSalt));

        using var hmac = new HMACSHA256(storedSalt);
        var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
        for (int i = 0; i < computedHash.Length; i++)
            if (computedHash[i] != storedHash[i])
                return false;

        return true;
    }
    
    public (bool IsValid, string ErrorMessage) Validate(string password)
    {
        if (string.IsNullOrWhiteSpace(password))
        {
            return (false, "Password cannot be empty or whitespace.");
        }

        if (password.Length < _minLength || password.Length > _maxLength)
        {
            return (false, $"Password must be between {_minLength} and {_maxLength} characters long.");
        }

        if (_requireDigit && !password.Any(char.IsDigit))
        {
            return (false, "Password must contain at least one digit.");
        }

        if (_requireNonAlphanumeric && !password.Any(ch => !char.IsLetterOrDigit(ch)))
        {
            return (false, "Password must contain at least one non-alphanumeric character.");
        }

        if (_requireUppercase && !password.Any(char.IsUpper))
        {
            return (false, "Password must contain at least one uppercase letter.");
        }

        if (_requireLowercase && !password.Any(char.IsLower))
        {
            return (false, "Password must contain at least one lowercase letter.");
        }

        return (true, "Password is valid.");
    }
}