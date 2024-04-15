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
    public class ParishesControllerTest
    {
        private ParishesController _parishController;
        private Mock<IMediator> _mediatorMock;
        private Fixture _fixture;

        public ParishesControllerTest()
        {
            _fixture = new Fixture();
            _fixture.Customize(new AutoMoqCustomization());
            _fixture.Behaviors.Add(new OmitOnRecursionBehavior());
            _mediatorMock = new Mock<IMediator>();
            _parishController = new ParishesController(_mediatorMock.Object);
        }

        [Fact]
        public async Task GetParishes_ShouldReturnParishes()
        {
            var expectedResult = _fixture.Create<OkObjectResult>();
            _mediatorMock.Setup(x => x.Send(It.IsAny<GetParishes.Query>(), default)).ReturnsAsync(expectedResult);

            var result = await _parishController.GetParishes();

            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public async Task GetParish_ShouldReturnAParish()
        {
            var parish = _fixture.Create<Parish>();

            var expectedResult = _fixture.Create<OkObjectResult>();

            _mediatorMock.Setup(x => x.Send(It.Is<GetParish.Query>(x => x._parishId == parish.ParishId), default)).ReturnsAsync(expectedResult);

            var result = await _parishController.GetParish(parish.ParishId);

            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public async Task ParishOffice_ShouldReturnUpdatedParish()
        {
            //Arrange

            var parish = _fixture.Create<Parish>();

            JsonPatchDocument<ParishForCreationDto> data = _fixture.Create<JsonPatchDocument<ParishForCreationDto>>();

            var expectedResult = _fixture.Create<NoContentResult>();

            _mediatorMock.Setup(x => x.Send(It.Is<UpdateParish.Query>(x => x._parishId == parish.ParishId && x._data == data), default)).ReturnsAsync(expectedResult);

            //Act
            var result = await _parishController.UpdateParish(parish.ParishId, data);

            //Assert
            Assert.IsType<NoContentResult>(result);

        }

        [Fact]
        public async Task PostParish_ShouldCreateParish()
        {
            //Arrange
            var parish = _fixture.Create<ParishForCreationDto>();

            var expectedResult = _fixture.Create<CreatedAtRouteResult>();

            _mediatorMock.Setup(x => x.Send(It.Is<PostParish.Query>(x => x._parish == parish), default)).ReturnsAsync(expectedResult);

            //Act
            var result = await _parishController.PostParish(parish);

            //Assert
            Assert.IsType<CreatedAtRouteResult>(result);
        }

        [Fact]
        public async Task DeleteParish_ShouldRemoveParishFromDataBase()
        {
            //Arrange
            var parish = _fixture.Create<Parish>();

            var expectedResult = _fixture.Create<NoContentResult>();

            _mediatorMock.Setup(x => x.Send(It.Is<DeleteParish.Query>(x => x._parishId == parish.ParishId), default)).ReturnsAsync(expectedResult);

            //Act 
            var result = await _parishController.DeleteParish(parish.ParishId);

            //Assert
            Assert.IsType<NoContentResult>(result);
        }
    }
}
