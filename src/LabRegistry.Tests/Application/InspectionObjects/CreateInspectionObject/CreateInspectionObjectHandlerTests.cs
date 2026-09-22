using LabRegistry.Application.InspectionObjects.CreateInspectionObject;
using LabRegistry.Domain.Entities;
using LabRegistry.Domain.Enums;
using LabRegistry.Domain.Exceptions;
using LabRegistry.Infrastructure.Database.Repositories;
using LabRegistry.Infrastructure.Tools;
using Moq;

namespace LabRegistry.Tests.Application.InspectionObjects.CreateInspectionObject;

public class CreateInspectionObjectHandlerTests
{
    private readonly Mock<IInspectionObjectsRepository> _repositoryMock = new();

    [Fact]
    public async Task CreateInspectionObjectHandler_ObjectCreated_Success()
    {
        // Arrange
        var command = new CreateInspectionObjectCommand(
            "Test object",
            "1.0",
            EnumExtensions.GetEnumMemberValue(ProductType.Software),
            DateTimeOffset.UtcNow.AddDays(-1),
            null);

        _repositoryMock
            .Setup(r => r.Create(
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<ProductType>(),
                It.IsAny<DateTimeOffset>(),
                It.IsAny<string?>(),
                It.IsAny<CancellationToken>()
            ))
            .ReturnsAsync(new InspectionObject()
            {
                Name = command.Name,
                ProductResult = EnumExtensions.GetEnumMemberValue(ProductResult.InProgress),
                ProductType = command.ProductType,
                Version = command.Version
            });


        var handler = new CreateInspectionObjectHandler(_repositoryMock.Object);

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.Equal(command.Name, result.Name);
        Assert.Equal(command.Version, result.Version);
        Assert.Equal(command.ProductType, result.ProductType);

        _repositoryMock.Verify(
            r => r.Create(
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<ProductType>(),
                It.IsAny<DateTimeOffset>(),
                It.IsAny<string?>(),
                It.IsAny<CancellationToken>()
            ),
            Times.Once);
    }

    [Fact]
    public async Task CreateInspectionObjectHandler_InvalidProductType_ThrowsHttpErrorException()
    {
        // Arrange
        var request = new CreateInspectionObjectCommand(
            "Test object",
            "1.0",
            "Invalid type",
            DateTimeOffset.UtcNow,
            null);

        var handler = new CreateInspectionObjectHandler(_repositoryMock.Object);

        // Act 
        var exception = await Assert.ThrowsAsync<HttpErrorException>(
            () => handler.Handle(request, CancellationToken.None));

        // Assert
        Assert.Equal(ExceptionMessagesConsts.ProductTypeIsInvalid, exception.ErrorMessage);
        Assert.Equal(System.Net.HttpStatusCode.BadRequest, exception.HttpStatusCode);

        _repositoryMock.Verify(
            r => r.Create(
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<ProductType>(),
                It.IsAny<DateTimeOffset>(),
                It.IsAny<string?>(),
                It.IsAny<CancellationToken>()),
            Times.Never);
    }
}
