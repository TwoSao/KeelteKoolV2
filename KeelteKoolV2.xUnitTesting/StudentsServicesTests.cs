using KeelteKoolV2.Core.DTO;
using KeelteKoolV2.Core.ServiceInterface;
using Xunit;

namespace KeelteKoolV2.xUnitTesting
{
    public class StudentsServicesTests : TestBase
    {
        [Fact]
        // Kontrollib esialgu ainult seda, et kehtivate andmetega õpilane luuakse.
        public async Task Should_AddNewStudent_WhenValidDataIsProvided()
        {
            var dto = new StudentDTO { FirstName = "Mari", LastName = "Tamm" };
            var result = await Svc<IStudentsServices>().Create(dto);
            Assert.NotNull(result);
        }

        [Fact]
        // Kontrollib esialgu ainult seda, et olemasoleva õpilase andmeid saab muuta.
        public async Task Should_UpdateStudent_WhenValidDataIsProvided()
        {
            var created = await Svc<IStudentsServices>().Create(new StudentDTO
                { FirstName = "Mari", LastName = "Tamm" });
            var result = await Svc<IStudentsServices>().Update(new StudentDTO
                { Id = created.Id, FirstName = "Kati", LastName = "Saar" });
            Assert.NotNull(result);
        }

        [Fact]
        // Kontrollib esialgu ainult seda, et õpilase üksikasjad leitakse tunnuse abil.
        public async Task Should_ReturnStudentDetails_WhenStudentExists()
        {
            var created = await Svc<IStudentsServices>().Create(new StudentDTO
                { FirstName = "Mari", LastName = "Tamm" });
            var result = await Svc<IStudentsServices>().DetailAsync(created.Id);
            Assert.NotNull(result);
        }

        [Fact]
        // Kontrollib esialgu ainult seda, et olemasolev õpilane kustutatakse.
        public async Task Should_DeleteStudent_WhenStudentExists()
        {
            var created = await Svc<IStudentsServices>().Create(new StudentDTO
                { FirstName = "Mari", LastName = "Tamm" });
            var result = await Svc<IStudentsServices>().Delete(created.Id);
            Assert.NotNull(result);
        }
    }
}
