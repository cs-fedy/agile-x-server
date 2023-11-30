namespace AgileX.Domain.Entities;

public class User
{
    private Guid guid;
    private string v;
    private DateTime createdAt1;
    private DateTime createdAt2;

    public Guid UserId { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public string FullName { get; set; }
    public string Username { get; set; }
    public string Role { get; set; }
    public bool IsConfirmed { get; set; } = false;
    public bool IsDeleted { get; set; } = false;
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public DateTime? DeletedAt { get; set; } = null;

    public User(
        Guid userId,
        string email,
        string password,
        string fullName,
        string username,
        string role,
        DateTime createdAt,
        DateTime updatedAt,
        DateTime? deletedAt,
        bool isConfirmed = false,
        bool isDeleted = false
    )
    {
        UserId = userId;
        Email = email;
        Password = password;
        FullName = fullName;
        Username = username;
        Role = role;
        IsConfirmed = isConfirmed;
        IsDeleted = isDeleted;
        CreatedAt = createdAt;
        UpdatedAt = updatedAt;
        DeletedAt = deletedAt;
    }

    public User(Guid guid, string email, string password, string fullName, string username, string v, DateTime createdAt1, DateTime createdAt2)
    {
        this.guid = guid;
        Email = email;
        Password = password;
        FullName = fullName;
        Username = username;
        this.v = v;
        this.createdAt1 = createdAt1;
        this.createdAt2 = createdAt2;
    }
}
