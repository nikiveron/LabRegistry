using LabRegistry.Domain.Enums;
using LabRegistry.Infrastructure.Database.Repositories;
using MediatR;

namespace LabRegistry.Application.InspectionObjects.UpdateInspectionObject;

public record UpdateInspectionObjectCommand(Guid Id, ProductResult? ProductResult, string? Comment) : IRequest;

public class UpdateInspectionObjectHandler(
    IInspectionObjectsRepository inspectionObjectsRepository
) : IRequestHandler<UpdateInspectionObjectCommand>
{
    public async Task Handle(UpdateInspectionObjectCommand request, CancellationToken cancellationToken)
    {
        await inspectionObjectsRepository.Update(request.Id, request.ProductResult, request.Comment, cancellationToken);
    }
}
