using FluentValidation.TestHelper;
using LabRegistry.Application.InspectionObjects.GetInspectionObjectsList;
using LabRegistry.Domain.Exceptions;

namespace LabRegistry.Tests.Application.InspectionObjects.GetInspectionObjectsList;

public class GetInspectionObjectsListValidationTests
{
    private readonly GetInspectionObjectsListRequestValidator _validator;
    private const string _200LengthString = "Lorem ipsum dolor sit amet, consectetuer adipiscing elit. " +
                     "Aenean commodo ligula eget dolor. Aenean massa. Cum sociis " +
                     "natoque penatibus et magnis dis parturient montes, nascetur " +
                     "ridiculus mus. Donec qu";
    private const string _201LengthString = _200LengthString + "a";

    public GetInspectionObjectsListValidationTests()
    {
        _validator = new GetInspectionObjectsListRequestValidator();
    }

    #region NamePart Tests

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData("any-text")]
    [InlineData(_200LengthString)]
    public void GetInspectionObjectsListValidation_NamePart_Success(string namePart)
    {
        // Arrange
        var request = new GetInspectionObjectsListRequest
        {
            NamePart = namePart,
            ProductType = null,
            ProductResult = null
        };

        // Act
        var result = _validator.Validate(request);

        // Assert
        Assert.True(result.IsValid);
    }

    [Fact]
    public void GetInspectionObjectsListValidation_NamePart_Failure()
    {
        // Arrange
        var request = new GetInspectionObjectsListRequest
        {
            NamePart = _201LengthString,
            ProductType = null,
            ProductResult = null
        };

        // Act
        var result = _validator.Validate(request);

        // Assert
        Assert.False(result.IsValid);
        Assert.Single(result.Errors);
        Assert.Contains(result.Errors, e => e.ErrorMessage == ExceptionMessagesConsts.NameLengthLimit);
    }

    #endregion

    #region ProductType Tests

    [Theory]
    [InlineData(null)]
    [InlineData("ПО (Программное обеспечение)")]
    [InlineData("ПАК (Программно-аппаратный комплекс)")]
    public void GetInspectionObjectsListValidation_ProductType_Success(string productType)
    {
        // Arrange
        var request = new GetInspectionObjectsListRequest
        {
            NamePart = null,
            ProductType = productType,
            ProductResult = null
        };

        // Act
        var result = _validator.TestValidate(request);

        // Assert
        Assert.True(result.IsValid);
    }

    [Theory]
    [InlineData("ПО")]
    [InlineData("ПАК")]
    [InlineData("any-text")]
    public void GetInspectionObjectsListValidation_ProductType_Failure(string productType)
    {
        // Arrange
        var request = new GetInspectionObjectsListRequest
        {
            NamePart = null,
            ProductType = productType,
            ProductResult = null
        };

        // Act
        var result = _validator.TestValidate(request);

        // Assert
        Assert.False(result.IsValid);
        Assert.Single(result.Errors);
        Assert.Contains(result.Errors, e => e.ErrorMessage == ExceptionMessagesConsts.ProductTypeMustBeValid);
    }

    #endregion

    #region ProductResult Tests

    [Theory]
    [InlineData(null)]
    [InlineData("В работе")]
    [InlineData("Соответствует")]
    [InlineData("Не соответствует")]
    [InlineData("не соответствует")]
    public void GetInspectionObjectsListValidation_ProductResult_Success(string productResult)
    {
        // Arrange
        var request = new GetInspectionObjectsListRequest
        {
            NamePart = null,
            ProductType = null,
            ProductResult = productResult
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
    [InlineData(" ")]
    public void GetInspectionObjectsListValidation_ProductResult_Failure(string productResult)
    {
        // Arrange
        var request = new GetInspectionObjectsListRequest
        {
            NamePart = null,
            ProductType = null,
            ProductResult = productResult
        };

        // Act
        var result = _validator.TestValidate(request);

        // Assert
        Assert.False(result.IsValid);
        Assert.Single(result.Errors);
        Assert.Contains(result.Errors, e => e.ErrorMessage == ExceptionMessagesConsts.ProductResultMustBeValid);
    }

    #endregion

    #region Page Number Tests

    [Theory]
    [InlineData(1)]
    [InlineData(2)]
    [InlineData(10)]
    [InlineData(100)]
    public void GetInspectionObjectsListValidation_Page_Success(int page)
    {
        // Arrange
        var request = new GetInspectionObjectsListRequest
        {
            NamePart = null,
            ProductType = null,
            ProductResult = null,
            Page = page,
            PageSize = 10
        };

        // Act
        var result = _validator.TestValidate(request);

        // Assert
        Assert.True(result.IsValid);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    [InlineData(-100)]
    public void GetInspectionObjectsListValidation_Page_Failure(int page)
    {
        // Arrange
        var request = new GetInspectionObjectsListRequest
        {
            NamePart = null,
            ProductType = null,
            ProductResult = null,
            Page = page,
            PageSize = 10
        };

        // Act
        var result = _validator.TestValidate(request);

        // Assert
        Assert.False(result.IsValid);
        Assert.Single(result.Errors);
        Assert.Contains(
            result.Errors,
            e => e.ErrorMessage == ExceptionMessagesConsts.PageMustBePositive);
    }

    #endregion

    #region PageSize Tests

    [Theory]
    [InlineData(1)]
    [InlineData(2)]
    [InlineData(10)]
    [InlineData(50)]
    [InlineData(100)]
    public void GetInspectionObjectsListValidation_PageSize_Success(int pageSize)
    {
        // Arrange
        var request = new GetInspectionObjectsListRequest
        {
            NamePart = null,
            ProductType = null,
            ProductResult = null,
            Page = 1,
            PageSize = pageSize
        };

        // Act
        var result = _validator.TestValidate(request);

        // Assert
        Assert.True(result.IsValid);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    [InlineData(101)]
    [InlineData(200)]
    public void GetInspectionObjectsListValidation_PageSize_Failure(int pageSize)
    {
        // Arrange
        var request = new GetInspectionObjectsListRequest
        {
            NamePart = null,
            ProductType = null,
            ProductResult = null,
            Page = 1,
            PageSize = pageSize
        };

        // Act
        var result = _validator.TestValidate(request);

        // Assert
        Assert.False(result.IsValid);
        Assert.Single(result.Errors);
        Assert.Contains(
            result.Errors,
            e => e.ErrorMessage == ExceptionMessagesConsts.PageSizeMustBeBetween1And100);
    }

    #endregion
}
