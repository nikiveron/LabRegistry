using Microsoft.AspNetCore.Mvc;

namespace LabRegistry.Application.InspectionObjects.UpdateInspectionObject;

public class UpdateInspectionObjectRequest
{
    [FromRoute(Name = "id")]
    public Guid Id { get; set; }

    [FromBody]
    public UpdateInspectionObjectRequestBody Body { get; set; } = null!;
}

public class UpdateInspectionObjectRequestBody
{
    public string? ProductResult { get; set; }
    public string? Comment { get; set; }
}
