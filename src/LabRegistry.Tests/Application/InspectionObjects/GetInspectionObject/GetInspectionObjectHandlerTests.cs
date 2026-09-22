using LabRegistry.Application.InspectionObjects.GetInspectionObject;
using LabRegistry.Domain.Entities;
using LabRegistry.Domain.Exceptions;
using LabRegistry.Infrastructure.Database.Repositories;
using Moq;

namespace LabRegistry.Tests.Application.InspectionObjects.GetInspectionObject;

public class GetInspectionObjectHandlerTests
{
    private readonly Mock<IInspectionObjectsRepository> _repositoryMock = new();

    [Fact]
    public async Task GetInspectionObjectHandler_ObjectExists_Success()
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
            ProductResult = "В работе",
            Comment = "Test comment"
        };

        _repositoryMock
            .Setup(r => r.Read(objectId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(inspectionObject);

        var handler = new GetInspectionObjectHandler(_repositoryMock.Object);

        var query = new GetInspectionObjectQuery(objectId);

        // Act
        var result = await handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(inspectionObject.Id, result.Id);
        Assert.Equal(inspectionObject.Name, result.Name);
        Assert.Equal(inspectionObject.Version, result.Version);
        Assert.Equal(inspectionObject.ProductType, result.ProductType);
        Assert.Equal(inspectionObject.ReceiptDate, result.ReceiptDate);
        Assert.Equal(inspectionObject.ProductResult, result.ProductResult);
        Assert.Equal(inspectionObject.Comment, result.Comment);

        _repositoryMock.Verify(
            r => r.Read(objectId, It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Fact]
    public async Task GetInspectionObjectHandler_ObjectNotFound_ThrowsHttpErrorException()
    {
        // Arrange
        var objectId = Guid.NewGuid();

        _repositoryMock
            .Setup(r => r.Read(objectId, It.IsAny<CancellationToken>()))
            .ReturnsAsync((InspectionObject?)null);

        var handler = new GetInspectionObjectHandler(_repositoryMock.Object);

        var query = new GetInspectionObjectQuery(objectId);

        // Act
        var exception = await Assert.ThrowsAsync<HttpErrorException>(
            () => handler.Handle(query, CancellationToken.None));

        // Assert
        Assert.Equal(ExceptionMessagesConsts.InspectionObjectNotFound, exception.ErrorMessage);
        Assert.Equal(System.Net.HttpStatusCode.NotFound, exception.HttpStatusCode);

        _repositoryMock.Verify(
            r => r.Read(objectId, It.IsAny<CancellationToken>()),
            Times.Once);
    }
}
