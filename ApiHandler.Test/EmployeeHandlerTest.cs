using EmployeePortal.ApiHandler;
using EmployeePortal.ApiHandler.Model;
using NUnit.Framework;
using System.Linq;
using System.Threading.Tasks;

namespace ApiHandler.Test
{
    [TestFixture]
    public class EmployeeHandlerTest
    {
        [OneTimeSetUp]
        public void Setup()
        {
            EmployeePortal.ApiHandler.ApiHandler.InitializeClient();
        }

        //TODO: Add bad weather tests

        [Test]
        public async Task CRUDOperations_ValidParameters_ShouldSucceed()
        {
            // Arrange
            var handler = new EmployeeHandler();
            var employee = new Employee { Name = "Foo", Email = "bar@email.com", Gender = "Male", Status = "Active" };

            //Act Create
            var returnValue = await handler.CreateEmployee(employee);
            
            //Assert Create
            Assert.IsTrue(returnValue.Contains("Employee Foo created with id:"));

            var id = returnValue.Split(' ').Last();
            employee.Id = id;

            //Act Search
            var searchResult = await handler.SearchEmployees(employee);

            //Assert Search
            Assert.That(searchResult.Count, Is.EqualTo(1));
            Assert.That(employee, Is.EqualTo(searchResult.First()));

            employee.Name = "Lorem";
            returnValue = await handler.UpdateEmployee(employee);

            Assert.That(returnValue, Is.EqualTo($"Employee {employee.Name} is successfully updated."));

            //Act Delete
            returnValue = await handler.DeleteEmployee(id);

            //Assert Delete
            Assert.That(returnValue, Is.EqualTo($"Employee deleted with id: {id}"));
        }
    }
}
