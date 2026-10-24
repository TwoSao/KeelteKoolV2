namespace KeelteKoolV2.Models.Lecturers
{
    // Lektori andmete kuvamiseks mõeldud vaatamudel (loend ja detailid)
    public class LecturerViewModel
    {
        public Guid? Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string? Qualifications { get; set; }
        public string? UserID { get; set; }
    }
}
