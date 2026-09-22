using LabRegistry.Domain.Entities;

namespace LabRegistry.Application.InspectionObjects.GetInspectionObjectsList;

public record GetInspectionObjectsListModel(
    List<InspectionObject> InspectionObjects,
    int Page,
    int PageSize,
    int TotalCount,
    int TotalPages);
