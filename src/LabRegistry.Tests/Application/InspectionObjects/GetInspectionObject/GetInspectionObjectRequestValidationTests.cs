using LabRegistry.Application.InspectionObjects.GetInspectionObject;
using LabRegistry.Domain.Exceptions;

namespace LabRegistry.Tests.Application.InspectionObjects.GetInspectionObject;

public class GetInspectionObjectRequestValidationTests
{
    private readonly GetInspectionObjectRequestValidator _validator;

    public GetInspectionObjectRequestValidationTests()
    {
        _validator = new GetInspectionObjectRequestValidator();
    }

    [Theory]
    [InlineData("22ea23d9-b81c-4d58-859f-b18d340eecd0")]
    [InlineData("11111111-1111-1111-1111-111111111111")]
    public void GetInspectionObjectRequestValidation_ValidGuid_Success(string guidString)
    {
        // Arrange 
        var request = new GetInspectionObjectRequest
        {
            Id = Guid.Parse(guidString)
        };
        var validator = new GetInspectionObjectRequestValidator();

        // Act
        var result = validator.Validate(request);

        // Assert
        Assert.True(result.IsValid);
        Assert.Empty(result.Errors);
    }

    [Fact]
    public void GetInspectionObjectRequestValidation_InvalidGuid_Failure()
    {
        // Arrange
        var request = new GetInspectionObjectRequest
        {
            Id = Guid.Parse("00000000-0000-0000-0000-000000000000")
        };
        var validator = new GetInspectionObjectRequestValidator();

        // Act 
        var result = validator.Validate(request);

        // Assert
        Assert.False(result.IsValid);
        Assert.Equal(2, result.Errors.Count);
        Assert.Contains(result.Errors, e => e.ErrorMessage == ExceptionMessagesConsts.ValidationObjectIdIsRequired);
        Assert.Contains(result.Errors, e => e.ErrorMessage == ExceptionMessagesConsts.ValidationObjectIdCannotBeEmpty);
    }

}
