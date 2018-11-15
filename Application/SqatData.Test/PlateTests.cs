using NUnit.Framework;
using SqatData.Repositories;

namespace SqatData.Test
{
    [TestFixture]
    class PlateTests
    {
        [Test]
        public void GetPlates_WithKnownSessionName_NumberPlatesOk()
        {
            //Arrange
            var platRepo = new PlateRepository("qc_devel");

            //Act
            var plates = platRepo.GetPlatesBySession("Session name");

            //Assert
            Assert.AreEqual(1, plates.Count);

        }
    }
}
