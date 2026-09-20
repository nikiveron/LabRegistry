using LabRegistry.Domain.Enums;
using LabRegistry.Infrastructure.Database.Repositories;
using MediatR;

namespace LabRegistry.Application.InspectionObjects.GetInspectionObjectsList;

public record GetInspectionObjectsListQuery(string? NamePart, ProductType? ProductType, ProductResult? ProductResult) : IRequest<GetInspectionObjectsListModel>;

public class GetInspectionObjectsListHandler(
    IInspectionObjectsRepository inspectionObjectsRepository
) : IRequestHandler<GetInspectionObjectsListQuery, GetInspectionObjectsListModel>
{
    public async Task<GetInspectionObjectsListModel> Handle(GetInspectionObjectsListQuery request, CancellationToken cancellationToken)
    {
        var list = await inspectionObjectsRepository.ReadList(request.NamePart, request.ProductType, request.ProductResult, cancellationToken);
        return new GetInspectionObjectsListModel(list);
    }
}
