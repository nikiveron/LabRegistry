using LabRegistry.Application.InspectionObjects.GetInspectionObjectsList;
using LabRegistry.Domain.Entities;
using LabRegistry.Domain.Enums;
using LabRegistry.Infrastructure.Database.Repositories;
using Moq;

namespace LabRegistry.Tests.Application.InspectionObjects.GetInspectionObjectsList;

public class GetInspectionObjectsListHandlerTests
{
    private readonly Mock<IInspectionObjectsRepository> _repositoryMock = new();

    [Fact]
    public async Task GetInspectionObjectsListHandler_OneObject_Success()
    {
        // Arrange
        var items = new List<InspectionObject>
        {
            new()
            {
                Name = "Object 1",
                Version = "1.0",
                ProductType = "ПО (Программное обеспечение)",
                ReceiptDate = DateTimeOffset.UtcNow,
                ProductResult = "В работе"
            }
        };

        _repositoryMock
            .Setup(r => r.ReadList(
                "test",
                ProductType.Software,
                ProductResult.InProgress,
                2,
                10,
                It.IsAny<CancellationToken>()))
            .ReturnsAsync((items, 25));

        var handler = new GetInspectionObjectsListHandler(
            _repositoryMock.Object);

        var query = new GetInspectionObjectsListQuery(
            "test",
            ProductType.Software,
            ProductResult.InProgress,
            2,
            10);

        // Act
        var result = await handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.Equal(items, result.InspectionObjects);
        Assert.Equal(2, result.Page);
        Assert.Equal(10, result.PageSize);
        Assert.Equal(25, result.TotalCount);
        Assert.Equal(3, result.TotalPages);
    }

    [Fact]
    public async Task GetInspectionObjectsListHandler_NoObjects_Success()
    {
        // Arrange
        _repositoryMock
            .Setup(r => r.ReadList(
                null,
                null,
                null,
                1,
                10,
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(([], 0));

        var handler = new GetInspectionObjectsListHandler(
            _repositoryMock.Object);

        var query = new GetInspectionObjectsListQuery(
            null,
            null,
            null,
            1,
            10);

        // Act
        var result = await handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.Empty(result.InspectionObjects);
        Assert.Equal(0, result.TotalCount);
        Assert.Equal(1, result.TotalPages);
    }
}
