using Moq;
using NUnit.Framework;
using UI.ViewModels;
using EmployeePortal.ApiHandler;
using EmployeePortal.ApiHandler.Model;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace UI.Test
{
    [TestFixture]
    public class EmployeeViewModelTest
    {
        [Test]
        public void EditEmployee_CallsEmployeeHandler()
        {
            //Arrange
            var employee = new Employee { Id = "1", Name = "Foo", Email = "Bar", Gender = "Male", Status = "Active" };
            var employeeHandlerMock = new Mock<IEmployeeHandler>();
            employeeHandlerMock.Setup(m => m.UpdateEmployee(It.IsAny<Employee>())).Returns(Task.FromResult("Edit successful"));
            var viewModel = new EmployeeViewModel(employeeHandlerMock.Object, null);
            viewModel.SelectedEmployee = employee;

            //Act
            viewModel.EditEmployee();

            //Assert
            employeeHandlerMock.Verify(m => m.UpdateEmployee(employee), Times.Once);
            Assert.That(viewModel.Status, Is.EqualTo("Edit successful"));
        }

        [Test]
        public void DeleteEmployee_CallsEmployeeHandler()
        {
            //Arrange
            var employee = new Employee { Id = "1" };
            var employeeHandlerMock = new Mock<IEmployeeHandler>();
            employeeHandlerMock.Setup(m => m.DeleteEmployee(employee.Id)).Returns(Task.FromResult("Delete successful"));
            var viewModel = new EmployeeViewModel(employeeHandlerMock.Object, null);
            viewModel.SelectedEmployee = employee;

            //Act
            viewModel.DeleteEmployee();

            //Assert
            employeeHandlerMock.Verify(m => m.DeleteEmployee(employee.Id), Times.Once);
            Assert.That(viewModel.Status, Is.EqualTo("Delete successful"));
        }

        [Test]
        public void SearchEmployee_CallsEmployeeHandler()
        {
            //Arrange
            var employee = new Employee { Id = "1", Name = "Foo", Email = "Bar", Gender = "Male", Status = "Active" };
            var employeeHandlerMock = new Mock<IEmployeeHandler>();
            employeeHandlerMock.Setup(m => m.SearchEmployees(employee)).Returns(Task.FromResult(GetStubEmployeeCollection()));
            var viewModel = new EmployeeViewModel(employeeHandlerMock.Object, null);
            viewModel.EmployeeToSearch = employee;

            //Act
            viewModel.SearchEmployee();

            //Assert
            employeeHandlerMock.Verify(m => m.SearchEmployees(employee), Times.Once);
            Assert.That(viewModel.Status, Is.EqualTo("Search yielded 2 results."));
        }

        [Test]
        public void LoadEmployeeData_CallsEmployeeHandler([Values(1u, 2u)] uint pageNumber)
        {
            //Arrange
            var employeeHandlerMock = new Mock<IEmployeeHandler>();
            var employees = 
            employeeHandlerMock.Setup(m => m.ListEmployees(pageNumber)).Returns(Task.FromResult((0, GetStubEmployeeCollection())));

            var viewModel = new EmployeeViewModel(employeeHandlerMock.Object, null);
            viewModel.CurrentPage = pageNumber;

            //Act
            viewModel.LoadEmployeeData().Wait();

            employeeHandlerMock.Verify(m => m.ListEmployees(pageNumber), Times.Once);
            Assert.That(viewModel.Status, Is.EqualTo("2 employees loaded"));
        }

        [Test]
        public void AddEmployee_ValidEmployee_CallsEmployeeHandler()
        {
            //Arrange
            var employee = new Employee { Id = "1", Name = "Foo", Email = "Bar", Gender = "Male", Status = "Active" };
            var employeeHandlerMock = new Mock<IEmployeeHandler>();
            employeeHandlerMock.Setup(m => m.CreateEmployee(employee)).Returns(Task.FromResult("Create successful"));
            var viewModel = new EmployeeViewModel(employeeHandlerMock.Object, null);
            viewModel.SelectedEmployee = employee;

            //Act
            viewModel.AddEmployee();

            employeeHandlerMock.Verify(m => m.CreateEmployee(employee), Times.Once);
            Assert.That(viewModel.Status, Is.EqualTo("Create successful"));
        }

        private IEnumerable<Employee> GetStubEmployeeCollection()
        {
            return new[] { new Employee(), new Employee() };
        }
    }
}
