using AutoFixture;
using AutoFixture.AutoMoq;
using MediatR;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Organization.Domain.Entity;
using Organization.Features.OfficeFeatures.Controller;
using Organization.Features.OfficeFeatures.DTO;
using Organization.Features.OfficeFeatures.Request;
using Xunit;
using Assert = Xunit.Assert;

namespace Organization.Test.Features.Addition.Controller
{
    public class OfficesControllerTest
    {
        private OfficesController _officeController;
        private Mock<IMediator> _mediatorMock;
        private Fixture _fixture;

        public OfficesControllerTest()
        {
            _fixture = new Fixture();
            _fixture.Customize(new AutoMoqCustomization());
            _fixture.Behaviors.Add(new OmitOnRecursionBehavior());
            _mediatorMock = new Mock<IMediator>();
            _officeController = new OfficesController(_mediatorMock.Object);
        }

        [Fact]
        public async Task GetOffices_ShouldReturnOffices()
        {
            //Arrange
            var expectedResult = _fixture.Create<OkObjectResult>();
            _mediatorMock.Setup(x => x.Send(It.IsAny<GetOffices.Query>(), default)).ReturnsAsync(expectedResult);

            //Act
            var result = await _officeController.GetOffices();

            //Assert
            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public async Task GetOffice_ShouldReturnAnOffice()
        {
            //Arrange
            var office = _fixture.Create<Office>();

            var expectedResult = _fixture.Create<OkObjectResult>();
            _mediatorMock.Setup(x => x.Send(It.Is<GetOffice.Query>(x => x._officeId == office.OfficeId), default)).ReturnsAsync(expectedResult);

            //Act
            var result = await _officeController.GetOffice(office.OfficeId);

            //Assert
            Assert.IsType<OkObjectResult>(result);

        }

        [Fact]
        public async Task UpdateOffice_ShouldReturnUpdatedOffice()
        {
            //Arrange

            var office = _fixture.Create<Office>();

            JsonPatchDocument<OfficeForCreationDto> data = _fixture.Create<JsonPatchDocument<OfficeForCreationDto>>();

            var expectedResult = _fixture.Create<NoContentResult>();

            _mediatorMock.Setup(x => x.Send(It.Is<UpdateOffice.Query>(x => x._officeId == office.OfficeId && x._data == data), default)).ReturnsAsync(expectedResult);

            //Act
            var result = await _officeController.UpdateOffice(office.OfficeId, data);

            //Assert
            Assert.IsType<NoContentResult>(result);

        }

        [Fact]
        public async Task PostOffice_ShouldCreateOffice()
        {
            //Arrange
            var office = _fixture.Create<OfficeForCreationDto>();

            var expectedResult = _fixture.Create<CreatedAtRouteResult>();

            _mediatorMock.Setup(x => x.Send(It.Is<PostOffice.Query>(x => x._office == office), default)).ReturnsAsync(expectedResult);

            //Act
            var result = await _officeController.PostOffice(office);

            //Assert
            Assert.IsType<CreatedAtRouteResult>(result);
        }

        [Fact]
        public async Task AddEmployeeToOffice_ShouldAddEmployeeToOffice()
        {
            //Assert
            var office = _fixture.Create<Office>();
            var employee = _fixture.Create<Employee>();

            var expectedResult = _fixture.Create<NoContentResult>();

            _mediatorMock.Setup(x => x.Send(It.Is<AddEmployeeToOffice.Query>(x => x._officeId == office.OfficeId && x._employeeId == employee.EmployeeId), default)).ReturnsAsync(expectedResult);

            //Act
            var result = await _officeController.AddEmployeeToOffice(office.OfficeId, employee.EmployeeId);

            //Assert
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task DeleteEmployeeToOffice_ShouldDeleteEmployeeFromOffice()
        {
            //Assert
            var office = _fixture.Create<Office>();
            var employee = _fixture.Create<Employee>();

            var expectedResult = _fixture.Create<NoContentResult>();

            _mediatorMock.Setup(x => x.Send(It.Is<DeleteEmployeeFromOffice.Query>(x => x._officeId == office.OfficeId && x._employeeId == employee.EmployeeId), default)).ReturnsAsync(expectedResult);

            //Act
            var result = await _officeController.DeleteEmployeeFromOffice(office.OfficeId, employee.EmployeeId);

            //Assert
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task DeleteOffice_ShouldRemoveOfficeFromDataBase()
        {
            //Arrange
            var office = _fixture.Create<Office>();

            var expectedResult = _fixture.Create<NoContentResult>();

            _mediatorMock.Setup(x => x.Send(It.Is<DeleteOffice.Query>(x => x._officeId == office.OfficeId), default)).ReturnsAsync(expectedResult);

            //Act 
            var result = await _officeController.DeleteOffice(office.OfficeId);

            //Assert
            Assert.IsType<NoContentResult>(result);
        }
    }
}
