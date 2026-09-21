using LabRegistry.Application.InspectionObjects.CreateInspectionObject;
using LabRegistry.Domain.Exceptions;
using FluentValidation.TestHelper;
using LabRegistry.Infrastructure.Tools;
using LabRegistry.Domain.Enums;

namespace LabRegistry.Tests.Application.InspectionObjects.CreateInspectionObject;

public class CreateInspectionObjectRequestValidationTests
{
    private readonly CreateInspectionObjectRequestValidator _validator;


    private const string _200LengthString = "Lorem ipsum dolor sit amet, consectetuer adipiscing elit. " +
                                            "Aenean commodo ligula eget dolor. Aenean massa. Cum sociis " +
                                            "natoque penatibus et magnis dis parturient montes, nascetur " +
                                            "ridiculus mus. Donec qu";
    private const string _201LengthString = _200LengthString + "a";
    private const string _50LengthString = "Lorem ipsum dolor sit amet, consectetuer adipiscin";
    private const string _51LengthString = _50LengthString + "a";
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
    private readonly string _defaultProductType = EnumExtensions.GetEnumMemberValue(ProductType.Software);
    private readonly string _defaultVersion = "1.0";
    private DateTimeOffset _defaultDate = DateTimeOffset.UtcNow.AddDays(-1);

    public CreateInspectionObjectRequestValidationTests()
    {
        _validator = new CreateInspectionObjectRequestValidator();
    }

    #region Name Tests

    [Theory]
    [InlineData("Test")]
    [InlineData("Inspection object")]
    [InlineData(_200LengthString)]
    public void CreateInspectionObjectRequestValidation_Name_Success(string name)
    {
        // Arrange
        var request = new CreateInspectionObjectRequest
        {
            Name = name,
            Version = _defaultVersion,
            ProductType = _defaultProductType,
            RecieptDate = _defaultDate
        };

        // Act
        var result = _validator.TestValidate(request);

        // Assert
        Assert.True(result.IsValid);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    public void CreateInspectionObjectRequestValidation_NameRequired_Failure(string name)
    {
        // Arrange
        var request = new CreateInspectionObjectRequest
        {
            Name = name,
            Version = _defaultVersion,
            ProductType = _defaultProductType,
            RecieptDate = _defaultDate
        };

        // Act
        var result = _validator.TestValidate(request);

        // Assert
        Assert.False(result.IsValid);
        Assert.Contains(
            result.Errors,
            e => e.ErrorMessage == ExceptionMessagesConsts.NameIsRequired);
    }

    [Fact]
    public void CreateInspectionObjectRequestValidation_NameLength_Failure()
    {
        // Arrange
        var request = new CreateInspectionObjectRequest
        {
            Name = _201LengthString,
            Version = _defaultVersion,
            ProductType = _defaultProductType,
            RecieptDate = _defaultDate
        };

        // Act
        var result = _validator.TestValidate(request);

        // Assert
        Assert.False(result.IsValid);
        Assert.Single(result.Errors);
        Assert.Contains(
            result.Errors,
            e => e.ErrorMessage == ExceptionMessagesConsts.NameLengthLimit);
    }

    #endregion

    #region Version Tests

    [Theory]
    [InlineData("1.0")]
    [InlineData("1.2.3")]
    [InlineData(_50LengthString)]
    public void CreateInspectionObjectRequestValidation_Version_Success(string version)
    {
        // Arrange
        var request = new CreateInspectionObjectRequest
        {
            Name = "Test",
            Version = version,
            ProductType = _defaultProductType,
            RecieptDate = _defaultDate
        };

        // Act
        var result = _validator.TestValidate(request);

        // Assert
        Assert.True(result.IsValid);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    public void CreateInspectionObjectRequestValidation_VersionRequired_Failure(string version)
    {
        // Arrange
        var request = new CreateInspectionObjectRequest
        {
            Name = "Test",
            Version = version,
            ProductType = _defaultProductType,
            RecieptDate = _defaultDate
        };

        // Act
        var result = _validator.TestValidate(request);

        // Assert
        Assert.False(result.IsValid);
        Assert.Contains(
            result.Errors,
            e => e.ErrorMessage == ExceptionMessagesConsts.VersionIsRequired);
    }

    [Fact]
    public void CreateInspectionObjectRequestValidation_VersionLength_Failure()
    {
        // Arrange
        var request = new CreateInspectionObjectRequest
        {
            Name = "Test",
            Version = _51LengthString,
            ProductType = _defaultProductType,
            RecieptDate = _defaultDate
        };

        // Act
        var result = _validator.TestValidate(request);

        // Assert
        Assert.False(result.IsValid);
        Assert.Single(result.Errors);
        Assert.Contains(
            result.Errors,
            e => e.ErrorMessage == ExceptionMessagesConsts.VersionLengthLimit);
    }

    #endregion

    #region ProductType Tests

    [Theory]
    [InlineData("ПО (Программное обеспечение)")]
    [InlineData("ПАК (Программно-аппаратный комплекс)")]
    public void CreateInspectionObjectRequestValidation_ProductType_Success(string productType)
    {
        // Arrange
        var request = new CreateInspectionObjectRequest
        {
            Name = "Test",
            Version = _defaultVersion,
            ProductType = productType,
            RecieptDate = _defaultDate
        };

        // Act
        var result = _validator.TestValidate(request);

        // Assert
        Assert.True(result.IsValid);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    public void CreateInspectionObjectRequestValidation_ProductTypeRequired_Failure(string productType)
    {
        // Arrange
        var request = new CreateInspectionObjectRequest
        {
            Name = "Test",
            Version = _defaultVersion,
            ProductType = productType,
            RecieptDate = _defaultDate
        };

        // Act
        var result = _validator.TestValidate(request);

        // Assert
        Assert.False(result.IsValid);
        Assert.Contains(
            result.Errors,
            e => e.ErrorMessage == ExceptionMessagesConsts.ProductTypeIsRequired);
    }

    [Theory]
    [InlineData("ПО")]
    [InlineData("ПАК")]
    [InlineData("any-text")]
    public void CreateInspectionObjectRequestValidation_ProductTypeInvalid_Failure(string productType)
    {
        // Arrange
        var request = new CreateInspectionObjectRequest
        {
            Name = "Test",
            Version = _defaultVersion,
            ProductType = productType,
            RecieptDate = _defaultDate
        };

        // Act
        var result = _validator.TestValidate(request);

        // Assert
        Assert.False(result.IsValid);
        Assert.Single(result.Errors);
        Assert.Contains(
            result.Errors,
            e => e.ErrorMessage == ExceptionMessagesConsts.ProductTypeMustBeValid);
    }

    #endregion

    #region RecieptDate Tests

    [Theory]
    [InlineData(-1)]
    [InlineData(-30)]
    [InlineData(-365)]
    public void CreateInspectionObjectRequestValidation_RecieptDate_Success(int days)
    {
        // Arrange
        var request = new CreateInspectionObjectRequest
        {
            Name = "Test",
            Version = _defaultVersion,
            ProductType = _defaultProductType,
            RecieptDate = DateTimeOffset.UtcNow.AddDays(days)
        };

        // Act
        var result = _validator.TestValidate(request);

        // Assert
        Assert.True(result.IsValid);
    }

    [Fact]
    public void CreateInspectionObjectRequestValidation_RecieptDateRequired_Failure()
    {
        // Arrange
        var request = new CreateInspectionObjectRequest
        {
            Name = "Test",
            Version = _defaultVersion,
            ProductType = _defaultProductType,
            RecieptDate = default
        };

        // Act
        var result = _validator.TestValidate(request);

        // Assert
        Assert.False(result.IsValid);
        Assert.Contains(
            result.Errors,
            e => e.ErrorMessage == ExceptionMessagesConsts.ReceiptDateIsRequired);
    }

    [Fact]
    public void CreateInspectionObjectRequestValidation_RecieptDateFuture_Failure()
    {
        // Arrange
        var request = new CreateInspectionObjectRequest
        {
            Name = "Test",
            Version = _defaultVersion,
            ProductType = _defaultProductType,
            RecieptDate = DateTimeOffset.UtcNow.AddMinutes(1)
        };

        // Act
        var result = _validator.TestValidate(request);

        // Assert
        Assert.False(result.IsValid);
        Assert.Single(result.Errors);
        Assert.Contains(
            result.Errors,
            e => e.ErrorMessage == ExceptionMessagesConsts.ReceiptDateMustBeValid);
    }

    #endregion

    #region Comment Tests

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData("Test comment")]
    [InlineData(_1000LengthString)]
    public void CreateInspectionObjectRequestValidation_Comment_Success(string comment)
    {
        // Arrange
        var request = new CreateInspectionObjectRequest
        {
            Name = "Test",
            Version = _defaultVersion,
            ProductType = _defaultProductType,
            RecieptDate = _defaultDate,
            Comment = comment
        };

        var length = _1000LengthString.Length;

        // Act
        var result = _validator.TestValidate(request);

        // Assert
        Assert.True(result.IsValid);
    }

    [Fact]
    public void CreateInspectionObjectRequestValidation_CommentLength_Failure()
    {
        // Arrange
        var request = new CreateInspectionObjectRequest
        {
            Name = "Test",
            Version = _defaultVersion,
            ProductType = _defaultProductType,
            RecieptDate = _defaultDate,
            Comment = _1001LengthString
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