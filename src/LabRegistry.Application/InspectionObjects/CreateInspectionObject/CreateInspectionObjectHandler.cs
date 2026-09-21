using LabRegistry.Domain.Enums;
using LabRegistry.Domain.Exceptions;
using LabRegistry.Infrastructure.Database.Repositories;
using LabRegistry.Infrastructure.Tools;
using MediatR;

namespace LabRegistry.Application.InspectionObjects.CreateInspectionObject;

public record CreateInspectionObjectCommand(
    string Name,
    string Version,
    string ProductType,
    DateTimeOffset RecieptDate,
    string? Comment
) : IRequest<CreateInspectionObjectModel>;

public class CreateInspectionObjectHandler(
    IInspectionObjectsRepository inspectionObjectsRepository
) : IRequestHandler<CreateInspectionObjectCommand, CreateInspectionObjectModel>
{
    public async Task<CreateInspectionObjectModel> Handle(CreateInspectionObjectCommand request, CancellationToken cancellationToken)
    {
        var productType = EnumExtensions.ParseFromEnumMember<ProductType>(request.ProductType)
            ?? throw new HttpErrorException(ExceptionMessagesConsts.ProductTypeIsInvalid, System.Net.HttpStatusCode.BadRequest);

        var createdInspectionObject = await inspectionObjectsRepository.Create(request.Name, request.Version, productType, request.RecieptDate, request.Comment, cancellationToken);

        return new CreateInspectionObjectModel(
            createdInspectionObject.Id,
            createdInspectionObject.Name,
            createdInspectionObject.Version,
            createdInspectionObject.ProductType,
            createdInspectionObject.ReceiptDate,
            createdInspectionObject.ProductResult,
            createdInspectionObject.Comment
        );
    }
}
