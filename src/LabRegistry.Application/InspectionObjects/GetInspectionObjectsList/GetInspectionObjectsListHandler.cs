using LabRegistry.Domain.Enums;
using LabRegistry.Infrastructure.Database.Repositories;
using MediatR;

namespace LabRegistry.Application.InspectionObjects.GetInspectionObjectsList;

public record GetInspectionObjectsListQuery(
    string? NamePart,
    ProductType? ProductType,
    ProductResult? ProductResult,
    int Page,
    int PageSize
) : IRequest<GetInspectionObjectsListModel>;

public class GetInspectionObjectsListHandler(
    IInspectionObjectsRepository inspectionObjectsRepository
) : IRequestHandler<GetInspectionObjectsListQuery, GetInspectionObjectsListModel>
{
    public async Task<GetInspectionObjectsListModel> Handle(GetInspectionObjectsListQuery request, CancellationToken cancellationToken)
    {
        var (items, totalCount) = await inspectionObjectsRepository.ReadList(
            request.NamePart,
            request.ProductType,
            request.ProductResult,
            request.Page,
            request.PageSize,
            cancellationToken
        );

        var totalPages = Math.Max(1, (int)Math.Ceiling(totalCount / (double)request.PageSize));

        return new GetInspectionObjectsListModel(items, request.Page, request.PageSize, totalCount, totalPages);
    }
}
