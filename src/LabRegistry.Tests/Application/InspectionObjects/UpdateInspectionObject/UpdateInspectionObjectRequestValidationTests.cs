using FluentValidation.TestHelper;
using LabRegistry.Application.InspectionObjects.UpdateInspectionObject;
using LabRegistry.Domain.Exceptions;

namespace LabRegistry.Tests.Application.InspectionObjects.UpdateInspectionObject;

public class UpdateInspectionObjectRequestValidationTests
{
    private readonly UpdateInspectionObjectRequestValidator _validator;
    private readonly Guid _defaultId = Guid.Parse("22ea23d9-b81c-4d58-859f-b18d340eecd0");
    private const string _1000LengthString =
        """
        Lorem ipsum dolor sit amet, consectetuer adipiscing elit. Aenean commodo ligula eget dolor. Aenean massa.
        Cum sociis natoque penatibus et magnis dis parturient montes, nascetur ridiculus mus. Donec quam felis,
        ultricies nec, pellentesque eu, pretium quis, sem. Nulla consequat massa quis enim. Donec pede justo,
        fringilla vel, aliquet nec, vulputate eget, arcu. In enim justo, rhoncus ut, imperdiet a, venenatis 
        vitae, justo. Nullam dictum felis eu pede mollis pretium. Integer tincidunt. Cras dapibus. Vivamus 
        elementum semper nisi. Aenean vulputate eleifend tellus. Aenean leo ligula, porttitor eu, consequat vitae
        , eleifend ac, enim. Aliquam lorem ante, dapibus in, viverra quis, feugiat a, tellus. Phasellus viverra 
        nulla ut metus varius laoreet. Quisque rutrum. Aenean imperdiet. Etiam ultricies nisi vel augue. Curabitur 
        ullamcorper ultricies nisi. Nam eget dui. Etiam rhoncus. Maecenas tempus, tellus eget condimentum rhoncus, 
        sem quam semper libero, sit amet adipiscing sem neq
        """;
    private const string _1001LengthString = _1000LengthString + "a";

    public UpdateInspectionObjectRequestValidationTests()
    {
        _validator = new UpdateInspectionObjectRequestValidator();
    }

    #region Id Tests

    [Theory]
    [InlineData("22ea23d9-b81c-4d58-859f-b18d340eecd0")]
    [InlineData("11111111-1111-1111-1111-111111111111")]
    public void UpdateInspectionObjectRequestValidation_ValidGuid_Success(
        string guidString)
    {
        // Arrange
        var request = new UpdateInspectionObjectRequest
        {
            Id = Guid.Parse(guidString),
            Body = new UpdateInspectionObjectRequestBody
            {
                ProductResult = null,
                Comment = null
            }
        };

        // Act
        var result = _validator.TestValidate(request);

        // Assert
        Assert.True(result.IsValid);
        Assert.Empty(result.Errors);
    }

    [Fact]
    public void UpdateInspectionObjectRequestValidation_InvalidGuid_Failure()
    {
        // Arrange
        var request = new UpdateInspectionObjectRequest
        {
            Id = Guid.Empty,
            Body = new UpdateInspectionObjectRequestBody
            {
                ProductResult = null,
                Comment = null
            }
        };

        // Act
        var result = _validator.TestValidate(request);

        // Assert
        Assert.False(result.IsValid);
        Assert.Equal(2, result.Errors.Count);
        Assert.Contains(
            result.Errors,
            e => e.ErrorMessage == ExceptionMessagesConsts.ValidationObjectIdIsRequired);
        Assert.Contains(
            result.Errors,
            e => e.ErrorMessage == ExceptionMessagesConsts.ValidationObjectIdCannotBeEmpty);
    }

    #endregion

    #region ProductResult Tests

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("В работе")]
    [InlineData("Соответствует")]
    [InlineData("Не соответствует")]
    [InlineData("не соответствует")]
    public void UpdateInspectionObjectRequestValidation_ProductResult_Success(
        string productResult)
    {
        // Arrange
        var request = new UpdateInspectionObjectRequest
        {
            Id = _defaultId,
            Body = new UpdateInspectionObjectRequestBody
            {
                ProductResult = productResult,
                Comment = null
            }
        };

        // Act
        var result = _validator.TestValidate(request);

        // Assert
        Assert.True(result.IsValid);
    }

    [Theory]
    [InlineData("В работу")]
    [InlineData("Соответствут")]
    [InlineData("Несоответствует")]
    [InlineData("any-text")]
    [InlineData(" ")]
    public void UpdateInspectionObjectRequestValidation_ProductResult_Failure(
        string productResult)
    {
        // Arrange
        var request = new UpdateInspectionObjectRequest
        {
            Id = _defaultId,
            Body = new UpdateInspectionObjectRequestBody
            {
                ProductResult = productResult,
                Comment = null
            }
        };

        // Act
        var result = _validator.TestValidate(request);

        // Assert
        Assert.False(result.IsValid);
        Assert.Single(result.Errors);
        Assert.Contains(
            result.Errors,
            e => e.ErrorMessage == ExceptionMessagesConsts.ProductResultMustBeValid);
    }

    #endregion

    #region Comment Tests

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData("Test comment")]
    [InlineData(_1000LengthString)]
    public void UpdateInspectionObjectRequestValidation_Comment_Success(
        string comment)
    {
        // Arrange
        var request = new UpdateInspectionObjectRequest
        {
            Id = _defaultId,
            Body = new UpdateInspectionObjectRequestBody
            {
                ProductResult = null,
                Comment = comment
            }
        };

        // Act
        var result = _validator.TestValidate(request);

        // Assert
        Assert.True(result.IsValid);
    }

    [Fact]
    public void UpdateInspectionObjectRequestValidation_CommentLength_Failure()
    {
        // Arrange
        var request = new UpdateInspectionObjectRequest
        {
            Id = _defaultId,
            Body = new UpdateInspectionObjectRequestBody
            {
                ProductResult = null,
                Comment = _1001LengthString
            }
        };

        // Act
        var result = _validator.TestValidate(request);

        // Assert
        Assert.False(result.IsValid);
        Assert.Single(result.Errors);
        Assert.Contains(
            result.Errors,
            e => e.ErrorMessage == ExceptionMessagesConsts.CommentLengthLimit);
    }

    #endregion
}