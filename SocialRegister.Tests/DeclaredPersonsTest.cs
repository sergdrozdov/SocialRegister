using Microsoft.VisualStudio.TestTools.UnitTesting;
using SocialRegister.Lib;

namespace SocialRegister.Tests
{
    [TestClass]
    public class DeclaredPersonsTest
    {
        private readonly DeclaredPersons _declaredPersons;

        public DeclaredPersonsTest()
        {
            _declaredPersons = new DeclaredPersons();
        }

        [TestMethod]
        public void DeclaredPersons_AverageCount_IsEqual()
        {
            _declaredPersons.Parameters.GroupBy = "ym";
            _declaredPersons.RawData.Add(new DeclaredPersonInfoExtApi { Year = 2019, Month = 1, PersonsCount = 21008938 });
            _declaredPersons.RawData.Add(new DeclaredPersonInfoExtApi { Year = 2019, Month = 2, PersonsCount = 18967475 });
            _declaredPersons.RawData.Add(new DeclaredPersonInfoExtApi { Year = 2019, Month = 3, PersonsCount = 20987135 });
            _declaredPersons.RawData.Add(new DeclaredPersonInfoExtApi { Year = 2019, Month = 4, PersonsCount = 20293663 });

            _declaredPersons.ProcessDataObject();

            Assert.AreEqual(_declaredPersons.Datasheet.Summary.AveragePersonsCount, 20314303);
        }

        [TestMethod]
        public void DeclaredPersons_MaxPersonsCount_IsCorrect()
        {
            _declaredPersons.Parameters.GroupBy = "ym";
            _declaredPersons.RawData.Add(new DeclaredPersonInfoExtApi { Year = 2019, Month = 1, PersonsCount = 21008938 });
            _declaredPersons.RawData.Add(new DeclaredPersonInfoExtApi { Year = 2019, Month = 2, PersonsCount = 18967475 });
            _declaredPersons.RawData.Add(new DeclaredPersonInfoExtApi { Year = 2019, Month = 3, PersonsCount = 20987135 });
            _declaredPersons.RawData.Add(new DeclaredPersonInfoExtApi { Year = 2019, Month = 4, PersonsCount = 20293663 });

            _declaredPersons.ProcessDataObject();
            _declaredPersons.ProcessDatasheetSummary();

            Assert.AreEqual(_declaredPersons.Datasheet.Summary.MaxPersonsCount, 21008938);
        }

        [TestMethod]
        public void DeclaredPersons_MinPersonsCount_IsCorrect()
        {
            _declaredPersons.Parameters.GroupBy = "ym";
            _declaredPersons.RawData.Add(new DeclaredPersonInfoExtApi { Year = 2019, Month = 1, PersonsCount = 21008938 });
            _declaredPersons.RawData.Add(new DeclaredPersonInfoExtApi { Year = 2019, Month = 2, PersonsCount = 18967475 });
            _declaredPersons.RawData.Add(new DeclaredPersonInfoExtApi { Year = 2019, Month = 3, PersonsCount = 20987135 });
            _declaredPersons.RawData.Add(new DeclaredPersonInfoExtApi { Year = 2019, Month = 4, PersonsCount = 20293663 });

            _declaredPersons.ProcessDataObject();
            _declaredPersons.ProcessDatasheetSummary();

            Assert.AreEqual(_declaredPersons.Datasheet.Summary.MinPersonsCount, 18967475);
        }
    }
}
