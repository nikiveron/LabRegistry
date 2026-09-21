using LabRegistry.Domain.Exceptions;
using LabRegistry.Infrastructure.Database.Repositories;
using MediatR;

namespace LabRegistry.Application.InspectionObjects.GetInspectionObject;

public record GetInspectionObjectQuery(Guid InspectionObjectId) : IRequest<GetInspectionObjectModel>;

public class GetInspectionObjectHandler(
    IInspectionObjectsRepository inspectionObjectsRepository
) : IRequestHandler<GetInspectionObjectQuery, GetInspectionObjectModel>
{
    public async Task<GetInspectionObjectModel> Handle(GetInspectionObjectQuery request, CancellationToken cancellationToken)
    {
        var inspectionObjectResult = await inspectionObjectsRepository.Read(request.InspectionObjectId, cancellationToken)
            ?? throw new HttpErrorException("Ошибка! Объект проверки не найден", System.Net.HttpStatusCode.NotFound);
        return new GetInspectionObjectModel(
            inspectionObjectResult.Id,
            inspectionObjectResult.Name,
            inspectionObjectResult.Version,
            inspectionObjectResult.ProductType,
            inspectionObjectResult.ReceiptDate,
            inspectionObjectResult.ProductResult,
            inspectionObjectResult.Comment);
    }
}
