using AutoFixture;
using AutoFixture.AutoMoq;
using MediatR;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Organization.Domain.Entity;
using Organization.Features.Addition.Controller;
using Organization.Features.Addition.DTO;
using Organization.Features.Addition.Request;
using Xunit;
using Assert = Xunit.Assert;

namespace Organization.Test.Features.Addition.Controller
{
    public class TeamsControllerTest
    {
        private TeamsController _teamController;
        private Mock<IMediator> _mediatorMock;
        private Fixture _fixture;

        public TeamsControllerTest()
        {
            _fixture = new Fixture();
            _fixture.Customize(new AutoMoqCustomization());
            _fixture.Behaviors.Add(new OmitOnRecursionBehavior());
            _mediatorMock = new Mock<IMediator>();
            _teamController = new TeamsController(_mediatorMock.Object);
        }

        [Fact]
        public async Task GetTeams_ShouldReturnTeams()
        {
            // Arrange
            var expectedResult = _fixture.Create<OkObjectResult>();

            _mediatorMock.Setup(x => x.Send(It.IsAny<GetTeams.Query>(), default)).ReturnsAsync(expectedResult);

            // Act
            var result = await _teamController.GetTeams();

            //Assert
            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public async Task GetTeam_ShouldReturnATeam()
        {
            // Arrange
            var team = _fixture.Create<Team>();
            var expectedResult = _fixture.Create<OkObjectResult>();

            _mediatorMock.Setup(x => x.Send(It.Is<GetTeam.Query>(x => x._teamId == team.TeamId), default)).ReturnsAsync(expectedResult);

            // Act
            var result = await _teamController.GetTeam(team.TeamId);

            // Assert
            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public async Task UpdateTeam_ShouldUpdatedTeam()
        {
            // Arrange
            var team = _fixture.Create<Team>();
            var data = _fixture.Create<JsonPatchDocument<TeamForCreationDto>>();

            var expectedResult = _fixture.Create<NoContentResult>();

            _mediatorMock.Setup(x => x.Send(It.Is<UpdateTeam.Query>(x => x._teamId == team.TeamId && x._data == data), default)).ReturnsAsync(expectedResult);

            // Act 
            var result = await _teamController.UpdateTeam(team.TeamId, data);

            // Assert

            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task PostOffice_ShouldCreateOffice()
        {
            //Arrange
            var team = _fixture.Create<TeamForCreationDto>();

            var expectedResult = _fixture.Create<CreatedAtRouteResult>();

            _mediatorMock.Setup(x => x.Send(It.Is<PostTeam.Query>(x => x._team == team), default)).ReturnsAsync(expectedResult);

            //Act
            var result = await _teamController.PostTeam(team);

            //Assert
            Assert.IsType<CreatedAtRouteResult>(result);
        }

        [Fact]
        public async Task AddEmployeeToTeam_ShouldAddEmployeeToTeam()
        {
            //Assert
            var team = _fixture.Create<Team>();
            var employee = _fixture.Create<Employee>();

            var expectedResult = _fixture.Create<NoContentResult>();

            _mediatorMock.Setup(x => x.Send(It.Is<AddEmployeeToTeam.Query>(x => x._teamId == team.TeamId && x._employeeId == employee.EmployeeId), default)).ReturnsAsync(expectedResult);

            //Act
            var result = await _teamController.AddEmployeeToTeam(team.TeamId, employee.EmployeeId);

            //Assert
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task DeleteEmployeeToTeam_ShouldRemoveEmployeeFromTeam()
        {
            //Assert
            var team = _fixture.Create<Team>();
            var employee = _fixture.Create<Employee>();

            var expectedResult = _fixture.Create<NoContentResult>();

            _mediatorMock.Setup(x => x.Send(It.Is<DeleteEmployeeFromTeam.Query>(x => x._teamId == team.TeamId && x._employeeId == employee.EmployeeId), default)).ReturnsAsync(expectedResult);

            //Act
            var result = await _teamController.DeleteEmployeeFromTeam(team.TeamId, employee.EmployeeId);

            //Assert
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task DeleteTeam_ShouldRemoveTeamFromDataBase()
        {
            //Arrange
            var team = _fixture.Create<Team>();

            var expectedResult = _fixture.Create<NoContentResult>();

            _mediatorMock.Setup(x => x.Send(It.Is<DeleteTeam.Query>(x => x._teamId == team.TeamId), default)).ReturnsAsync(expectedResult);

            //Act 
            var result = await _teamController.DeleteTeam(team.TeamId);

            //Assert
            Assert.IsType<NoContentResult>(result);
        }


    }
}
