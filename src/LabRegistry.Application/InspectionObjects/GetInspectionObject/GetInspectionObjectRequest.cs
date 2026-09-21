using Microsoft.AspNetCore.Mvc;

namespace LabRegistry.Application.InspectionObjects.GetInspectionObject;

public class GetInspectionObjectRequest
{
    [FromRoute(Name = "id")]
    public Guid Id { get; set; }
}
