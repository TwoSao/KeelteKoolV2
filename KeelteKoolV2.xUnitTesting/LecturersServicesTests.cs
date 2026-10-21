using KeelteKoolV2.Core.Domain;
using KeelteKoolV2.Core.DTO;
using KeelteKoolV2.Core.ServiceInterface;
using Xunit;

namespace KeelteKoolV2.xUnitTesting
{
    public class LecturersServicesTests : TestBase
    {
        // Kontrollib, et uue lektori lisamine andmebaasi töötab korrektselt
        [Fact]
        public async Task Should_AddNewLecturer_WhenResultIsReturned()
        {
            // ülesseade
            LecturerDTO dto = MockLecturerDTOData();

            // tegevus
            var result = await Svc<ILecturersServices>().Create(dto);

            // kontroll
            Assert.NotNull(result);
            Assert.Equal(dto.FirstName, result.FirstName);
            Assert.Equal(dto.LastName, result.LastName);
            Assert.Equal(dto.Qualifications, result.Qualifications);
            Assert.NotEqual(Guid.Empty, result.Id);
            Assert.True(result.CreatedAt <= DateTime.Now);
        }

        // Kontrollib, et tühjade väljadega lektori lisamine ei õnnestu
        [Fact]
        public async Task ShouldNot_AddNewLecturer_WhenRequiredFieldsAreEmpty()
        {
            // ülesseade
            LecturerDTO dto = MockLecturerDTOData();
            dto.FirstName = string.Empty;
            dto.LastName = string.Empty;

            // tegevus
            var result = await Svc<ILecturersServices>().Create(dto);

            // kontroll
            Assert.Null(result);
        }

        // Kontrollib, et lektori detailid tagastatakse õige ID järgi
        [Fact]
        public async Task Should_ReturnLecturerDetails_WhenGuidIsNotNull()
        {
            // ülesseade
            Lecturer createdLecturer = await AddLecturerToDB();

            // tegevus
            var result = await Svc<ILecturersServices>().DetailAsync(createdLecturer.Id);

            // kontroll
            Assert.NotNull(result);
            Assert.Equal(createdLecturer.Id, result.Id);
            Assert.Equal(createdLecturer.FirstName, result.FirstName);
            Assert.Equal(createdLecturer.LastName, result.LastName);
            Assert.Equal(createdLecturer.Qualifications, result.Qualifications);
            Assert.True(result.Id == createdLecturer.Id);
        }

        // Kontrollib, et lektori andmete uuendamine salvestab uued andmed õigesti
        [Fact]
        public async Task Should_UpdateLecturerWithNewData_WhenDataIsDifferentFromDB()
        {
            // ülesseade
            Lecturer createdLecturer = await AddLecturerToDB();

            LecturerDTO updatedDto = new LecturerDTO();
            updatedDto.Id = createdLecturer.Id;
            updatedDto.FirstName = "Uus Eesnimi";
            updatedDto.LastName = "Uus Perenimi";
            updatedDto.Qualifications = "Doktor";
            updatedDto.CreatedAt = createdLecturer.CreatedAt;

            // tegevus
            var result = await Svc<ILecturersServices>().Update(updatedDto);

            // kontroll
            Assert.NotNull(result);
            Assert.Equal(updatedDto.Id, result.Id);
            Assert.Equal(updatedDto.FirstName, result.FirstName);
            Assert.Equal(updatedDto.LastName, result.LastName);
            Assert.Equal(updatedDto.Qualifications, result.Qualifications);
            Assert.True(result.ModifiedAt >= createdLecturer.CreatedAt);
        }

        private async Task<Lecturer> AddLecturerToDB()
        {
            LecturerDTO dto = MockLecturerDTOData();
            var created = await Svc<ILecturersServices>().Create(dto);
            return created;
        }

        private LecturerDTO MockLecturerDTOData()
        {
            return new LecturerDTO
            {
                FirstName = "Mari",
                LastName = "Maasikas",
                Qualifications = "Magistrikraad inglise filoloogias"
            };
        }
    }
}
