using KeelteKoolV2.Core.Domain;
using KeelteKoolV2.Core.DTO;
using KeelteKoolV2.Core.ServiceInterface;
using KeelteKoolV2.Data;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Text;

namespace KeelteKoolV2.ApplicationServices.Services
{
    public class FilesServices : IFilesServices
    {
        private readonly KeelteKoolV2Context _context;
        private readonly IHostEnvironment _hostEnvironment;
        public FilesServices(KeelteKoolV2Context context, IHostEnvironment hostEnvironment)
        {
            _context = context;
            _hostEnvironment = hostEnvironment;
        }
        public Task<FileToDatabase> RemoveFileFromDatabase(FileToDatabaseDTO dto)
        {
            throw new NotImplementedException();
        }

        public Task<FileToDatabase> RemoveFilesFromDatabase(FileToDatabaseDTO[] dtos)
        {
            throw new NotImplementedException();
        }

        public void UploadFiles(LecturerDTO dto, Lecturer domain)
        {
            if (dto.Files != null && dto.Files.Count > 0)
            {
                foreach (var file in dto.Files)
                {
                    using (var target = new MemoryStream())
                    {
                        FileToDatabase files = new FileToDatabase()
                        {
                            ImageID = Guid.NewGuid(),
                            ImageTitle = file.FileName,
                            LecturerId = domain.Id
                        };
                        file.CopyTo(target);
                        files.ImageData = target.ToArray();

                        _context.Files.Add(files);
                    }
                }
            }
        }
    }
}
