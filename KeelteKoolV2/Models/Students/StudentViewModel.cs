namespace KeelteKoolV2.Models.Students;

public class StudentViewModel
{
    public Guid Id { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string? UserID { get; set; }
}
