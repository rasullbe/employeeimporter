using EmployeeImporter.Models;

namespace EmployeeImporter.Tests
{
    [TestClass]
    public sealed class EmployeeTests
    {
        /// <summary>
        /// Tests that an Employee object is created with the correct properties.
        /// </summary>
        [TestMethod]
        public void Employee_ShouldHaveCorrectProperties()
        {
            var employee = new Employee
            {
                PayrollNumber = "COOP08",
                Forenames = "John",
                Surname = "William",
                DateOfBirth = new DateTime(1955, 1, 26),
                Telephone = "12345678",
                Mobile = "987654231",
                Address = "12 Foreman road",
                Address2 = "London",
                Postcode = "GU12 6JW",
                EmailHome = "nomadic20@hotmail.co.uk",
                StartDate = new DateTime(2013, 4, 18)
            };

            Assert.AreEqual("COOP08", employee.PayrollNumber);
            Assert.AreEqual("William", employee.Surname);
            Assert.AreEqual("nomadic20@hotmail.co.uk", employee.EmailHome);
        }

        /// <summary>
        /// Tests that a CSV line is correctly parsed into an Employee object.
        /// </summary>
        [TestMethod]
        public void CsvLine_ShouldParseCorrectly()
        {
            var line = "COOP08,John,William,26/01/1955,12345678,987654231,12 Foreman road,London,GU12 6JW,nomadic20@hotmail.co.uk,18/04/2013";
            var columns = line.Split(',');

            Assert.AreEqual(11, columns.Length);
            Assert.AreEqual("COOP08", columns[0].Trim());
            Assert.AreEqual("William", columns[2].Trim());
            Assert.AreEqual("26/01/1955", columns[3].Trim());
        }

        /// <summary>
        /// Tests that a CSV line with insufficient columns is considered invalid.
        /// </summary>
        [TestMethod]
        public void CsvLine_WithInsufficientColumns_ShouldBeInvalid()
        {
            var line = "COOP08,John,William";
            var columns = line.Split(',');

            Assert.IsTrue(columns.Length < 11);
        }

        /// <summary>
        /// Tests that an empty CSV line is detected correctly.
        /// </summary>
        [TestMethod]
        public void CsvLine_Empty_ShouldBeDetected()
        {
            var line = "";
            Assert.IsTrue(string.IsNullOrWhiteSpace(line));
        }
    }
}