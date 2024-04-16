using AutoFixture;
using AutoFixture.AutoMoq;
using MediatR;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Organization.Domain.Entity;
using Organization.Features.EmployeeFeatures.Controller;
using Organization.Features.EmployeeFeatures.DTO;
using Organization.Features.EmployeeFeatures.Request;
using Xunit;
using Assert = Xunit.Assert;

namespace Organization.Test.Features.Addition.Controller
{
    public class EmployeesControllerTest
    {

        private EmployeesController _employeeController;
        private Mock<IMediator> _mediatorMock;
        private Fixture _fixture;

        public EmployeesControllerTest()
        {
            _fixture = new Fixture();
            _fixture.Customize(new AutoMoqCustomization());
            _fixture.Behaviors.Add(new OmitOnRecursionBehavior());
            _mediatorMock = new Mock<IMediator>();
            _employeeController = new EmployeesController(_mediatorMock.Object);

        }

        [Fact]
        public async Task GetEmployees_ShouldReturnEmployees()
        {
            //Arrange
            var expectedResult = _fixture.Create<OkObjectResult>();
            _mediatorMock.Setup(x => x.Send(It.IsAny<GetEmployees.Query>(), default)).ReturnsAsync(expectedResult);

            //Act
            var result = await _employeeController.GetEmployees();

            //Assert
            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public async Task GetEmployee_ShouldReturnAnEmployee()
        {
            //Arrange
            var employee = _fixture.Create<Employee>();
            var office = _fixture.Build<Office>().With(x => x.Employees, [employee]).Create();

            var expectedResult = _fixture.Create<OkObjectResult>();
            _mediatorMock.Setup(x => x.Send(It.Is<GetEmployee.Query>(x => x._employeeId == employee.EmployeeId && x._officeId == office.OfficeId), default)).ReturnsAsync(expectedResult);

            //Act
            var result = await _employeeController.GetEmployee(office.OfficeId, employee.EmployeeId);

            //Assert
            Assert.IsType<OkObjectResult>(result);

        }

        [Fact]
        public async Task UpdateEmployee_ShouldReturnUpdatedEmployee()
        {
            //Arrange

            var employees = _fixture.CreateMany<Employee>();
            var employee = employees.First();
            var employeeToReplace = employees.Last();

            var office = _fixture.Build<Office>().With(x => x.Employees, [employee]).Create();

            JsonPatchDocument<EmployeeForCreationDto> data = new();

            data.Replace(x => x.Age, employeeToReplace.Age)
                .Replace(x => x.FirstName, employeeToReplace.FirstName)
                .Replace(x => x.LastName, employeeToReplace.LastName);

            var expectedResult = _fixture.Create<OkObjectResult>();

            _mediatorMock.Setup(x => x.Send(It.Is<UpdateEmployee.Query>(x => x._employeeId == employee.EmployeeId && x._officeId == office.OfficeId && x._data == data), default)).ReturnsAsync(expectedResult);

            //Act
            var result = await _employeeController.UpdateEmployee(office.OfficeId, employee.EmployeeId, data);

            //Assert
            Assert.IsType<OkObjectResult>(result);

        }

        [Fact]
        public async Task PostEmployee_ShouldCreateEmployee()
        {
            //Arrange
            var employeeForCreationDto = _fixture.Create<EmployeeForCreationDto>();
            var office = _fixture.Create<Office>();
            var team = _fixture.Create<Team>();

            var expectedResult = _fixture.Create<CreatedAtRouteResult>();

            _mediatorMock.Setup(x => x.Send(It.Is<PostEmployee.Query>(x => x._employee == employeeForCreationDto && x._officeId == office.OfficeId && x._teamName == team.Name), default)).ReturnsAsync(expectedResult);

            //Act
            var result = await _employeeController.PostEmployee(employeeForCreationDto, office.OfficeId, team.Name);

            //Assert
            Assert.IsType<CreatedAtRouteResult>(result);
        }

        [Fact]
        public async Task DeleteEmployee_ShouldRemoveEmployeeFromDataBaseAndOffice()
        {
            //Arrange
            var employeeToDelete = _fixture.Create<Employee>();
            var office = _fixture.Build<Office>().With(x => x.Employees, [employeeToDelete]).Create();

            var expectedResult = _fixture.Create<NoContentResult>();

            _mediatorMock.Setup(x => x.Send(It.Is<DeleteEmployee.Query>(x => x._officeId == office.OfficeId && x._employeeId == employeeToDelete.EmployeeId), default)).ReturnsAsync(expectedResult);

            //Act 
            var result = await _employeeController.DeleteEmployee(office.OfficeId, employeeToDelete.EmployeeId);

            //Assert
            Assert.IsType<NoContentResult>(result);
        }
    }
}
