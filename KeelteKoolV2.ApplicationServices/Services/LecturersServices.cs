using KeelteKoolV2.Core.Domain;
using KeelteKoolV2.Core.DTO;
using KeelteKoolV2.Core.ServiceInterface;
using KeelteKoolV2.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace KeelteKoolV2.ApplicationServices.Services
{
    public class LecturersServices : ILecturersServices
    {
        private readonly KeelteKoolV2Context _context;
        public LecturersServices(KeelteKoolV2Context context)
        {
            _context = context;
        }
        public async Task<Lecturer> Create(LecturerDTO dto)
        {
            //check if not null
            if (dto == null)
            {
                return null;
            }
            //convert to domain-type
            Lecturer domain = new Lecturer();
            domain.Id = Guid.NewGuid();
            if(dto.FirstName.Length < 1)
            {
                dto.FirstName = "Jane";
            }
            if(dto.LastName.Length < 1)
            {
                dto.LastName = "Doe";
            }
            domain.FirstName = dto.FirstName;
            domain.LastName = dto.LastName;
            domain.Qualifications = dto.Qualifications;
            domain.UserID = dto.UserID;

            domain.CreatedAt = DateTime.Now;
            domain.ModifiedAt = DateTime.Now;

            await _context.Lecturers.AddAsync(domain);
            await _context.SaveChangesAsync();

            return domain;
        }

        public Task<Lecturer> Delete(Guid id)
        {
            return null;
        }

        public Task<Lecturer> DetailAsync(Guid id)
        {
            return null;
        }

        public Task<Lecturer> Update(LecturerDTO dto)
        {
            return null;
        }
    }
}
