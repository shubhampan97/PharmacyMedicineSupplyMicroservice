using MedicineSupplyMicroservice.Controllers;
using MedicineSupplyMicroservice.Models;
using MedicineSupplyMicroservice.Repository;
using Microsoft.Extensions.Configuration;
using Moq;
using NUnit.Framework;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MedicineSupplyTesting
{
    public class Tests
    {
        public SupplyRepo sr = new SupplyRepo();
        List<string> dataObject = new List<string>();
        MedicineStock medicineStock = new MedicineStock();
        List<PharmacyMedicineSupply> pharMedSup = new List<PharmacyMedicineSupply>();
        public static IEnumerable<List<MedicineDemand>> mockMedicineDemands()
        {
            yield return new List<MedicineDemand>
            {
                new MedicineDemand() { Medicine = "Medicine1", DemandCount = 20 },
                new MedicineDemand() { Medicine = "Medicine2", DemandCount = 25 },
                new MedicineDemand() { Medicine = "Medicine3", DemandCount = 30 },
                new MedicineDemand() { Medicine = "Medicine4", DemandCount = 35 },
                new MedicineDemand() { Medicine = "Medicine5", DemandCount = 40 },
                new MedicineDemand() { Medicine = "Medicine6", DemandCount = 45 }
            };
        }

        [SetUp]
        public void Setup()
        {


        }




        [TestCase]
        public void RepoTest_Pass()
        {
            Mock<ISupplyRepo> supRepMock = new Mock<ISupplyRepo>();
            SupplyRepo supplyRepoObject = new SupplyRepo();
            supRepMock.Setup(x => x.GetListOfPharmacy()).Returns(dataObject);
            List<string> listOfPharma = supplyRepoObject.GetListOfPharmacy();
            Assert.Contains("Pharmacy1", listOfPharma);
        }




        [TestCase]
        public void RepoTest_Fail()
        {
            Mock<ISupplyRepo> supRepMock = new Mock<ISupplyRepo>();
            SupplyRepo supplyRepoObject = new SupplyRepo();
            supRepMock.Setup(x => x.GetListOfPharmacy()).Returns(dataObject);
            List<string> listOfPharma = supplyRepoObject.GetListOfPharmacy();
            Assert.That(listOfPharma, Has.No.Member("Pharmacy8"));
        }




        [Test, TestCaseSource(nameof(mockMedicineDemands))]
        public async Task Test_GetPharmacySupply(List<MedicineDemand> mockMedicineDemands)
        {
            Mock<ISupplyRepo> supplyMock = new Mock<ISupplyRepo>();
            SupplyController sc = new SupplyController(supplyMock.Object);
            var result = await (sr.GetPharmacySupply(mockMedicineDemands) as Task<List<PharmacyMedicineSupply>>);
            Assert.Multiple(() =>
                {
                    Assert.AreEqual(4, result[0].medicineAndSupply[0].supplyCount);
                    Assert.AreEqual(5, result[1].medicineAndSupply[1].supplyCount);
                    Assert.AreEqual(6, result[2].medicineAndSupply[2].supplyCount);
                    Assert.AreEqual(7, result[3].medicineAndSupply[3].supplyCount);
                    Assert.AreEqual(8, result[4].medicineAndSupply[4].supplyCount);
                });
        }


        [TestCase]
        public void Test_GetMedicineStock()
        {
            Mock<ISupplyRepo> supplyMock = new Mock<ISupplyRepo>();
            
            SupplyController sc = new SupplyController(supplyMock.Object);
            var result = sc.GetMedicineStock() as Task<List<MedicineStock>>;
            Assert.AreEqual(true, result.IsCompletedSuccessfully);
            Assert.That(result,Is.Not.Null);
        }

    }


}