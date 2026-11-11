using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using Xunit;

namespace KeelteKoolV2.SeleniumTesting
{
    /// <summary>
    /// Lektorite (Lecturers) kasutajaliidese Selenium testid: CRUD (order 1-4) ning neli lisatesti (order 5-8).
    /// Rakendus peab testide käivitamise ajal töötama aadressil <see cref="BaseUrl"/>.
    /// CRUD testid jagavad ühte unikaalset lektorit (<see cref="thisRunLastName"/>), seega käivituvad need kindlas järjekorras.
    /// Lisatestid loovad ja kustutavad oma andmed ise ning ei sõltu CRUD testidest.
    /// </summary>
    [TestCaseOrderer("KeelteKoolV2.SeleniumTesting.PriorityOrderer", "KeelteKoolV2.SeleniumTesting")]
    public class LecturerFrontendTests
    {
        private const string BaseUrl = "https://localhost:7191/";

        // algandmed (create) ja uued andmed (update)
        private const string FirstNameCreate = "Mari";
        private const string QualificationsCreate = "Magister, inglise keele õpetaja";
        private const string FirstNameUpdate = "Liisa";
        private const string QualificationsUpdate = "Doktor, soome-ugri keeleteadus";

        /// <summary>Selle testijooksu unikaalne perenimi (Guid), mille järgi lektorit tabelist otsitakse.</summary>
        public static string thisRunLastName { get; set; } = "";

        /// <summary>
        /// Test, mis loob uue lektori unikaalse perenimega ning kontrollib, et lektor ilmub nimekirja
        /// õigete andmetega (eesnimi, perenimi, kvalifikatsioon).
        /// </summary>
        [Fact]
        [Order(1)]
        public void Should_NavigateToIndex_AddLecturerWithUniqueData_ReturnToIndex()
        {
            /* ülesseade */
            IWebDriver driver = TestSetup_NavigateToIndex();
            try
            {
                /* tegevus */
                driver.FindElement(By.Id("IndexCreateNewLecturer_BTN")).Click();
                string uniqueLastName = DataEntry_Lecturer(driver, FirstNameCreate, QualificationsCreate);
                thisRunLastName = uniqueLastName;
                driver.FindElement(By.Id("CU_Create_BTN")).Click();

                /* kontroll */
                IWebElement? row = FindRowByLastName(driver, uniqueLastName);
                Assert.NotNull(row);
                Assert.Equal(FirstNameCreate, row!.FindElement(By.Id("IndexTableLecturerFirstName")).Text);
                Assert.Equal(uniqueLastName, row.FindElement(By.Id("IndexTableLecturerLastName")).Text);
                Assert.Equal(QualificationsCreate, row.FindElement(By.Id("IndexTableLecturerQualifications")).Text);
            }
            finally
            {
                driver.Quit();
            }
        }

        /// <summary>
        /// Test, mis avab eelnevalt loodud lektori andmete vaate ning kontrollib, et kõik väljad
        /// (id, eesnimi, perenimi, kvalifikatsioon) on täidetud ja vastavad loomisel sisestatule.
        /// </summary>
        [Fact]
        [Order(2)]
        public void Should_NavigateToDetailsOfALecturer_WithPreviouslyCreatedUniqueData()
        {
            /* ülesseade */
            IWebDriver driver = TestSetup_NavigateToIndex();
            try
            {
                /* tegevus */
                ClickRowAction(driver, thisRunLastName, "IndexTableLecturerDetails_BTN");

                string detailsId = driver.FindElement(By.Id("DD_LecturerGuid")).Text;
                string detailsFirstName = driver.FindElement(By.Id("DD_LecturerFirstName")).Text;
                string detailsLastName = driver.FindElement(By.Id("DD_LecturerLastName")).Text;
                string detailsQualifications = driver.FindElement(By.Id("DD_LecturerQualifications")).Text;

                /* kontroll */
                Assert.True(Guid.TryParse(detailsId, out Guid parsedId));
                Assert.NotEqual(Guid.Empty, parsedId);
                Assert.Equal(FirstNameCreate, detailsFirstName);
                Assert.Equal(thisRunLastName, detailsLastName);
                Assert.Equal(QualificationsCreate, detailsQualifications);
            }
            finally
            {
                driver.Quit();
            }
        }

        /* ---------- abimeetodid ---------- */

        /// <summary>Loob Chrome draiveri, mis aktsepteerib arenduse enda allkirjastatud HTTPS sertifikaati.</summary>
        private static IWebDriver CreateDriver()
        {
            var options = new ChromeOptions { AcceptInsecureCertificates = true };
            IWebDriver driver = new ChromeDriver(options);
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(5);
            return driver;
        }

        /// <summary>Ülesseade: loob draiveri, avab avalehe ja vajutab menüüs lingile "Lektorid".</summary>
        private static IWebDriver TestSetup_NavigateToIndex()
        {
            IWebDriver driver = CreateDriver();
            driver.Url = BaseUrl;
            driver.FindElement(By.Id("Nav_Lecturers_LNK")).Click();
            return driver;
        }

        /// <summary>Tagastab nimekirja tabeli andmeread (päisrida välja arvatud).</summary>
        private static List<IWebElement> GetRows(IWebDriver driver)
        {
            return driver.FindElement(By.Id("IndexTable")).FindElements(By.CssSelector("tbody tr")).ToList();
        }

        /// <summary>Otsib tabelist rea perenime järgi, tagastab null kui ei leia.</summary>
        private static IWebElement? FindRowByLastName(IWebDriver driver, string lastName)
        {
            return GetRows(driver).FirstOrDefault(r => r.FindElement(By.Id("IndexTableLecturerLastName")).Text == lastName);
        }

        /// <summary>Vajutab etteantud perenimega rea tegevusnupule (andmed/muuda/kustuta).</summary>
        private static void ClickRowAction(IWebDriver driver, string lastName, string buttonId)
        {
            IWebElement? row = FindRowByLastName(driver, lastName);
            Assert.True(row != null, $"Lektorit perenimega '{lastName}' ei leitud nimekirjast.");
            row!.FindElement(By.Id(buttonId)).Click();
        }

        /// <summary>Asendab välja sisu uue väärtusega.</summary>
        private static void SetField(IWebDriver driver, string fieldId, string value)
        {
            IWebElement field = driver.FindElement(By.Id(fieldId));
            field.Clear();
            field.SendKeys(value);
        }

        /// <summary>Täidab lisamisvormi, perenimeks pannakse uus Guid. Tagastab perenime.</summary>
        private static string DataEntry_Lecturer(IWebDriver driver, string firstName, string qualifications)
        {
            string uniqueLastName = Guid.NewGuid().ToString();
            SetField(driver, "CU_Lecturer_FirstName", firstName);
            SetField(driver, "CU_Lecturer_LastName", uniqueLastName);
            SetField(driver, "CU_Lecturer_Qualifications", qualifications);
            return uniqueLastName;
        }

        /// <summary>Loob lektori kasutajaliidese kaudu (nimekirjast alustades) ja tagastab tema unikaalse perenime.</summary>
        private static string CreateLecturerViaUi(IWebDriver driver, string firstName, string qualifications)
        {
            driver.FindElement(By.Id("IndexCreateNewLecturer_BTN")).Click();
            string lastName = DataEntry_Lecturer(driver, firstName, qualifications);
            driver.FindElement(By.Id("CU_Create_BTN")).Click();
            return lastName;
        }

        /// <summary>Kustutab testi ajutise lektori (kui on veel olemas), et testid andmebaasi ei risustaks.</summary>
        private static void CleanupLecturer(IWebDriver driver, string lastName)
        {
            try
            {
                driver.Url = BaseUrl + "Lecturers";
                IWebElement? row = FindRowByLastName(driver, lastName);
                if (row == null) return;
                row.FindElement(By.Id("IndexTableLecturerDelete_BTN")).Click();
                driver.FindElement(By.Id("DeleteDelete_BTN")).Click();
            }
            catch (WebDriverException)
            {
                // koristamine ei tohi testi tulemust varjata
            }
        }
    }
}
