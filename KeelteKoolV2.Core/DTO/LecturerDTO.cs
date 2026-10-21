using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace KeelteKoolV2.Core.DTO
{
    public class LecturerDTO
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

        // Fullname TBA
        public string Qualifications { get; set; }
        public string? UserID { get; set; }
        //public ICollection<LanguageSubject> LanguageSubjects { get; set; }
        public List<IFormFile>? Files { get; set; }
        public IEnumerable<FileToDatabaseDTO>? Image { get; set; } = new List<FileToDatabaseDTO>();
        public DateTime? CreatedAt { get; set; }
        public DateTime? ModifiedAt { get; set; }
    }
}
