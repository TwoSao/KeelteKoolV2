namespace KeelteKoolV2.Models.Lecturers
{
    // Lektori loomiseks ja muutmiseks mõeldud vaatamudel
    public class LecturerCreateUpdateViewModel
    {
        public Guid? Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string? Qualifications { get; set; }
        public string? UserID { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? ModifiedAt { get; set; }
    }
}
