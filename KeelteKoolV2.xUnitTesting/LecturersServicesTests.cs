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
