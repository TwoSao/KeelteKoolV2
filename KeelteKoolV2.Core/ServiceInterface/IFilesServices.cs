using KeelteKoolV2.Core.Domain;
using KeelteKoolV2.Core.DTO;
using System;
using System.Collections.Generic;
using System.Text;

namespace KeelteKoolV2.Core.ServiceInterface
{
    public interface IFilesServices
    {
        void UploadFiles(LecturerDTO dto, Lecturer domain);
        Task<FileToDatabase> RemoveFileFromDatabase(FileToDatabaseDTO dto);
        Task<FileToDatabase> RemoveFilesFromDatabase(FileToDatabaseDTO[] dtos);
    }
}
