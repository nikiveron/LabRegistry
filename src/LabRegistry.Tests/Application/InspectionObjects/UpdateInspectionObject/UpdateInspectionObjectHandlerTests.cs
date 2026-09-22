using LabRegistry.Application.InspectionObjects.UpdateInspectionObject;
using LabRegistry.Domain.Entities;
using LabRegistry.Domain.Enums;
using LabRegistry.Domain.Exceptions;
using LabRegistry.Infrastructure.Database.Repositories;
using Moq;

namespace LabRegistry.Tests.Application.InspectionObjects.UpdateInspectionObject;

public class UpdateInspectionObjectHandlerTests
{
    private readonly Mock<IInspectionObjectsRepository> _repositoryMock = new();

    [Fact]
    public async Task UpdateInspectionObjectHandler_ObjectExists_Success()
    {
        // Arrange
        var objectId = Guid.NewGuid();

        var inspectionObject = new InspectionObject
        {
            Id = objectId,
            Name = "Test object",
            Version = "1.0",
            ProductType = "ПО (Программное обеспечение)",
            ReceiptDate = DateTimeOffset.UtcNow,
            ProductResult = "В работе"
        };

        var productResult = ProductResult.Passed;
        var comment = "Объект соответствует требованиям";

        _repositoryMock
            .Setup(r => r.Read(objectId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(inspectionObject);

        var handler = new UpdateInspectionObjectHandler(
            _repositoryMock.Object);

        var command = new UpdateInspectionObjectCommand(
            objectId,
            productResult,
            comment);

        // Act
        await handler.Handle(command, CancellationToken.None);

        // Assert
        _repositoryMock.Verify(
            r => r.Read(objectId, It.IsAny<CancellationToken>()),
            Times.Once);

        _repositoryMock.Verify(
            r => r.Update(
                objectId,
                productResult,
                comment,
                It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Fact]
    public async Task UpdateInspectionObjectHandler_ObjectNotFound_ThrowsHttpErrorException()
    {
        // Arrange
        var objectId = Guid.NewGuid();

        _repositoryMock
            .Setup(r => r.Read(objectId, It.IsAny<CancellationToken>()))
            .ReturnsAsync((InspectionObject?)null);

        var handler = new UpdateInspectionObjectHandler(_repositoryMock.Object);

        var command = new UpdateInspectionObjectCommand(
            objectId,
            ProductResult.InProgress,
            "Comment");

        // Act 
        var exception = await Assert.ThrowsAsync<HttpErrorException>(
            () => handler.Handle(command, CancellationToken.None));

        // Assert
        Assert.Equal(ExceptionMessagesConsts.InspectionObjectNotFound, exception.ErrorMessage);
        Assert.Equal(System.Net.HttpStatusCode.NotFound, exception.HttpStatusCode);

        _repositoryMock.Verify(
            r => r.Update(
                It.IsAny<Guid>(),
                It.IsAny<ProductResult?>(),
                It.IsAny<string?>(),
                It.IsAny<CancellationToken>()),
            Times.Never);
    }
}
