using KeelteKoolV2.Core.Domain;
using KeelteKoolV2.Core.DTO;
using KeelteKoolV2.Core.ServiceInterface;
using KeelteKoolV2.Data;

namespace KeelteKoolV2.ApplicationServices.Services
{
    public class LecturersServices : ILecturersServices
    {
        private readonly KeelteKoolV2Context _context;

        public LecturersServices(KeelteKoolV2Context context)
        {
            _context = context;
        }

        public Task<Lecturer> Create(LecturerDTO dto)
        {
            return null;
        }

        public Task<Lecturer> Update(LecturerDTO dto)
        {
            return null;
        }

        public Task<Lecturer> DetailAsync(Guid id)
        {
            return null;
        }

        public Task<Lecturer> Delete(Guid id)
        {
            return null;
        }
    }
}
