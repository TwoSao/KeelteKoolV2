using KeelteKoolV2.Core.Domain;
using KeelteKoolV2.Core.DTO;
using KeelteKoolV2.Core.ServiceInterface;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using Xunit;

namespace KeelteKoolV2.xUnitTesting
{
    public class LanguageCoursesServicesTests : TestBase
    {
        [Fact] //Käsusõna, mida testrunner tunneb, et aru saada mis on test, ja mis ei ole
        // 1 - Kirjeldatakse ära, kas test on tavaline (peaks/ei tohi teha), või negatiivne (ei tohi/peaks tegema)
        // 2 - Mida parasjagu testiga testitakse.
        // 3 - Mis tingimustel tulemust kontrollitakse, peale tegevust
        //                  1           2           3
        //                  \/          \/          \/
        public async Task Should_AddNewCourse_WhenResultIsReturned()
        {
            //ülesseade
            LanguageCourseDTO newCourseDTO = new LanguageCourseDTO();
            newCourseDTO.Nimetus = "TestKursus";
            newCourseDTO.Keel = "Eesti keel";
            newCourseDTO.Tase = "Algtase";
            newCourseDTO.Kirjeldus = "A0 tasemel eesti keele \"õpe\", tule ja raiska aega";

            //tegevus
            var result = await Svc<ILanguageCoursesServices>().Create(newCourseDTO);

            //kontroll
            Assert.NotNull(result);
            /*
             Assert on klass mille abil saab kontrollita andmete eri tingimusi, kujusid, olekuid jne.
            Antud juhul kontrollitakse eelnevat objekti ühe kontrolliga - et ei oleks tühi.
            Aga, kui meie meetod pärast selle sisu arendamist hakkab juba tagastama mingisugust objekti, 
            tuleks testi täiendada, täpsemate tingimustega, mis kontrollib näiteks, kas on samasugune, 
            sisaldab kindlal kujul andmeid, andmed on mingit kindlat tüüpi jne. Võimalusi mida kontrollida on palju,
            ning viise kuidas teste kirjutada veelgi rohkem.
             */
        }

        [Fact]
        public async Task ShouldNot_AddNewCourse_WhenFieldsEmpty() {
            //ülesseade
            LanguageCourseDTO newCourse = MockLanguageCourseDTOData();
            newCourse.Keel = string.Empty;
            newCourse.Nimetus = string.Empty;
            //tegevus
            var result = await Svc<ILanguageCoursesServices>().Create(newCourse);
            //kontroll
            Assert.Null(result); //kontrollime et teenus lükkaks objekti lisamise tagasi
            if (result != null)
            {                
                //kontrollime et keel oleks juurde lisatud, ja mitte tühi
                Assert.NotNull(result.Keel); 
                Assert.NotNull(result.Nimetus); 
                //kontrollime et muutujates oleks midagi lisatud
                Assert.True(result.Keel.Length > 0); 
                Assert.False(result.Nimetus.Length < 1); 
                //kontrollime et teenus ei kaota ära vahepeal andmeid mis me sisestasime
                Assert.Equal(newCourse.Keel, result.Keel); 
                Assert.Equal(newCourse.Nimetus, result.Nimetus);
            }
        }
        //Detailstest
        [Fact]
        public async Task Should_ReturnCourseDetails_WhenGuidIsNotNull()//??
        {
            //ülesseade
            Core.Domain.LanguageCourse createdCourse = await AddObjectToDB();

            //tegevus
            //kasutame objektis asuvat id et see objekt tagasi lugeda andmebaasist DetailsAsync meetodiga
            var result = await Svc<ILanguageCoursesServices>().DetailsAsync(createdCourse.Id);

            //kontroll
            //kontrollime et tagastati midagi
            Assert.NotNull(result);
            //kontrollime et tagastatud objekti id on sama nagu see mis andmebaasi lisatud sai
            Assert.Equal(result.Id, createdCourse.Id); //on sama kontroll nagu alumine
            Assert.True(result.Id == createdCourse.Id); //on sama kontroll nagu ülemine, kirjapilt erineb
            ////võrdleme kas objekt on sama nagu see mis me genereerisime, va. id-ga
            Assert.Equal(result, createdCourse);
        }

        private async Task<Core.Domain.LanguageCourse> AddObjectToDB()
        {
            //tekitame uue objekti
            LanguageCourseDTO course = MockLanguageCourseDTOData();
            //lisame andmebaasi
            var createdCourse = await Svc<ILanguageCoursesServices>().Create(course);
            return createdCourse;
        }

        //test peab kontrollima et andmete muutmisel õigesti andmed ka lisatakse
        // 1 - Kirjeldatakse ära, kas test on tavaline (peaks/ei tohi teha), või negatiivne (ei tohi/peaks tegema)
        // 2 - Mida parasjagu testiga testitakse.
        // 3 - Mis tingimustel tulemust kontrollitakse, peale tegevust
        //                  1           2           3
        //                  \/          \/          \/
        [Fact]
        public async Task Should_UpdateNimetusWithNewData_WhenDataIsDifferentFromDB() 
        {
            //ülesseade
            var data = MockLanguageCourseDTOData();
            var result = await Svc<ILanguageCoursesServices>().Create(data);
            data.Nimetus = "Võro Kieli";
            data.Tase = "C6";
            data.Keel = "Eesti (Võro)";
            data.Kirjeldus = "Räägi nagu maakas";

            var dto = new LanguageCourseDTO();

            dto.Id = result.Id;
            dto.Nimetus = data.Nimetus;
            dto.Keel = data.Keel;
            dto.Kirjeldus = data.Kirjeldus;
            dto.Tase = data.Tase;
            dto.CreatedAt = result.CreatedAt;
            dto.ModifiedAt = data.ModifiedAt;
            //tegevus
            var result2 = await Svc<ILanguageCoursesServices>().Update(dto);

            //kontroll
            Assert.NotNull(result);
            Assert.Equal(dto.Id, result2.Id);
            Assert.Equal(dto.Keel, result2.Keel);
            Assert.Equal(dto.Kirjeldus, result2.Kirjeldus);
            Assert.Matches(dto.Tase, result2.Tase);
            Assert.Matches(dto.Nimetus, result2.Nimetus);
        }
        //test peab kontrollima et andmete muutmisel õigesti andmed ka lisatakse
        // 1 - Kirjeldatakse ära, kas test on tavaline (peaks/ei tohi teha), või negatiivne (ei tohi/peaks tegema)
        // 2 - Mida parasjagu testiga testitakse.
        // 3 - Mis tingimustel tulemust kontrollitakse, peale tegevust
        //                  1           2           3
        //                  \/          \/          \/
        [Fact]
        public async Task Should_DeleteDataFromDB_WhenValidIDIsGiven() 
        {
            //ülesseade
            Core.Domain.LanguageCourse createdCourse = await AddObjectToDB();

            //tegevus
            var deletedCourse = await Svc<ILanguageCoursesServices>().Delete(createdCourse.Id);
            var result = await Svc<ILanguageCoursesServices>().DetailsAsync(createdCourse.Id);

            //Kontroll
            Assert.Null(result);
            Assert.Equal(createdCourse, deletedCourse);
            //Mõtle välja veel üks kontrollimisviis testile.
            Assert.Equal(createdCourse.Id, deletedCourse.Id);
            Assert.NotEqual(deletedCourse, result);
        }

        private LanguageCourseDTO MockLanguageCourseDTOData()
        {
            return new LanguageCourseDTO
            {
                Nimetus = "TestKursus",
                Keel = "Eesti keel",
                Tase = "Algtase",
                Kirjeldus = "A0 tasemel eesti keele \"õpe\", tule ja raiska aega"
            };
        }

    }
}
