using System.Net;

namespace LabRegistry.Domain.Exceptions;

public class HttpErrorException(string ErrorMessage, HttpStatusCode HttpStatusCode) : Exception
{
    public string ErrorMessage { get; set; } = ErrorMessage;
    public HttpStatusCode HttpStatusCode { get; set; } = HttpStatusCode;
}