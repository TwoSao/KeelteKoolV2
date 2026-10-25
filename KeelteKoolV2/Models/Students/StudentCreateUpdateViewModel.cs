using System.ComponentModel.DataAnnotations;

namespace KeelteKoolV2.Models.Students;

public class StudentCreateUpdateViewModel
{
    public Guid? Id { get; set; }

    [Required, StringLength(100)]
    public string FirstName { get; set; } = string.Empty;

    [Required, StringLength(100)]
    public string LastName { get; set; } = string.Empty;

    public string? UserID { get; set; }
}
