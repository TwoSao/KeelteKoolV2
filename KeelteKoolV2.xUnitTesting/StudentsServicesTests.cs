using KeelteKoolV2.Core.Domain;
using KeelteKoolV2.Core.DTO;
using KeelteKoolV2.Core.ServiceInterface;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace KeelteKoolV2.xUnitTesting;

public class StudentsServicesTests : TestBase
{
    [Fact]
    public async Task Should_AddNewStudent_WhenResultIsReturned()
    {
        var dto = ValidStudent();
        var result = await Svc<IStudentsServices>().Create(dto);

        Assert.NotNull(result);
        Assert.NotEqual(Guid.Empty, result.Id);
        Assert.Equal(dto.FirstName, result.FirstName);
        Assert.Equal(dto.LastName, result.LastName);
        Assert.Equal(dto.UserID, result.UserID);
        Assert.NotEqual(default, result.CreatedAt);
        Assert.Equal(result.CreatedAt, result.ModifiedAt);
    }

    [Fact]
    public async Task ShouldNot_AddNewStudent_WhenRequiredFieldsAreEmpty()
    {
        var dto = ValidStudent();
        dto.FirstName = string.Empty;
        dto.LastName = string.Empty;
        var result = await Svc<IStudentsServices>().Create(dto);

        Assert.Null(result);
        Assert.Empty(await Svc<KeelteKoolV2.Data.KeelteKoolV2Context>().Students.ToListAsync());
        Assert.Equal(string.Empty, dto.FirstName);
        Assert.Equal(string.Empty, dto.LastName);
        Assert.NotNull(dto.UserID);
    }

    [Fact]
    public async Task Should_ReturnStudentDetails_WhenIdIsValid()
    {
        var created = await AddStudentToDatabase();
        var result = await Svc<IStudentsServices>().DetailAsync(created.Id);

        Assert.NotNull(result);
        Assert.Equal(created.Id, result.Id);
        Assert.Equal(created.FirstName, result.FirstName);
        Assert.Equal(created.LastName, result.LastName);
        Assert.Equal(created.UserID, result.UserID);
        Assert.Equal(created.CreatedAt, result.CreatedAt);
    }

    [Fact]
    public async Task Should_UpdateStudent_WhenNewDataIsProvided()
    {
        var created = await AddStudentToDatabase();
        var dto = ValidStudent("Kati", "Saar");
        dto.Id = created.Id;
        dto.UserID = "student-user-updated";

        var result = await Svc<IStudentsServices>().Update(dto);

        Assert.NotNull(result);
        Assert.Equal(created.Id, result.Id);
        Assert.Equal(dto.FirstName, result.FirstName);
        Assert.Equal(dto.LastName, result.LastName);
        Assert.Equal(dto.UserID, result.UserID);
        Assert.Equal(created.CreatedAt, result.CreatedAt);
        Assert.True(result.ModifiedAt >= created.ModifiedAt);
    }

    [Fact]
    public async Task Should_DeleteStudent_WhenValidIdIsGiven()
    {
        var created = await AddStudentToDatabase();
        var deleted = await Svc<IStudentsServices>().Delete(created.Id);
        var result = await Svc<IStudentsServices>().DetailAsync(created.Id);

        Assert.NotNull(deleted);
        Assert.Equal(created.Id, deleted.Id);
        Assert.Equal(created.FirstName, deleted.FirstName);
        Assert.Equal(created.LastName, deleted.LastName);
        Assert.Equal(created.UserID, deleted.UserID);
        Assert.Null(result);
    }

    [Fact]
    public async Task Should_ReturnNull_WhenStudentDoesNotExist()
    {
        var id = Guid.NewGuid();
        var result = await Svc<IStudentsServices>().DetailAsync(id);
        var deleted = await Svc<IStudentsServices>().Delete(id);

        Assert.Null(result);
        Assert.Null(deleted);
        Assert.NotEqual(Guid.Empty, id);
        Assert.Empty(await Svc<IStudentsServices>().GetAll());
        Assert.Empty(await Svc<KeelteKoolV2.Data.KeelteKoolV2Context>().Students.ToListAsync());
    }

    [Fact]
    public async Task ShouldNot_UpdateStudent_WhenRequiredFieldsAreEmpty()
    {
        var created = await AddStudentToDatabase();
        var dto = ValidStudent(string.Empty, " ");
        dto.Id = created.Id;
        var result = await Svc<IStudentsServices>().Update(dto);
        var unchanged = await Svc<IStudentsServices>().DetailAsync(created.Id);

        Assert.Null(result);
        Assert.NotNull(unchanged);
        Assert.Equal(created.Id, unchanged.Id);
        Assert.Equal(created.FirstName, unchanged.FirstName);
        Assert.Equal(created.LastName, unchanged.LastName);
        Assert.Equal(created.ModifiedAt, unchanged.ModifiedAt);
    }

    [Fact]
    public async Task ShouldNot_UpdateStudent_WhenStudentDoesNotExist()
    {
        var dto = ValidStudent();
        dto.Id = Guid.NewGuid();
        var result = await Svc<IStudentsServices>().Update(dto);

        Assert.Null(result);
        Assert.NotEqual(Guid.Empty, dto.Id);
        Assert.Empty(await Svc<IStudentsServices>().GetAll());
        Assert.Empty(await Svc<KeelteKoolV2.Data.KeelteKoolV2Context>().Students.ToListAsync());
        Assert.Equal("Mari", dto.FirstName);
        Assert.Equal("Tamm", dto.LastName);
    }

    [Fact]
    public async Task ShouldNot_DeleteStudent_WhenIdIsEmptyOrUnknown()
    {
        var unknownId = Guid.NewGuid();
        var emptyIdResult = await Svc<IStudentsServices>().Delete(Guid.Empty);
        var unknownIdResult = await Svc<IStudentsServices>().Delete(unknownId);

        Assert.Null(emptyIdResult);
        Assert.Null(unknownIdResult);
        Assert.Empty(await Svc<IStudentsServices>().GetAll());
        Assert.Empty(await Svc<KeelteKoolV2.Data.KeelteKoolV2Context>().Students.ToListAsync());
        Assert.NotEqual(Guid.Empty, unknownId);
    }

    [Fact]
    public async Task Should_ListStudents_OrderedByLastNameThenFirstName()
    {
        await Svc<IStudentsServices>().Create(ValidStudent("Zara", "Tamm"));
        await Svc<IStudentsServices>().Create(ValidStudent("Mari", "Aas"));
        await Svc<IStudentsServices>().Create(ValidStudent("Anna", "Aas"));
        var students = await Svc<IStudentsServices>().GetAll();

        Assert.Equal(3, students.Count);
        Assert.Equal("Anna", students[0].FirstName);
        Assert.Equal("Mari", students[1].FirstName);
        Assert.Equal("Aas", students[1].LastName);
        Assert.Equal("Tamm", students[2].LastName);
        Assert.Equal(3, await Svc<KeelteKoolV2.Data.KeelteKoolV2Context>().Students.CountAsync());
    }

    private async Task<Student> AddStudentToDatabase()
    {
        var created = await Svc<IStudentsServices>().Create(ValidStudent());
        Assert.NotNull(created);
        return created;
    }

    private static StudentDTO ValidStudent(string firstName = "Mari", string lastName = "Tamm") => new()
    {
        FirstName = firstName,
        LastName = lastName,
        UserID = "student-user-1"
    };
}
