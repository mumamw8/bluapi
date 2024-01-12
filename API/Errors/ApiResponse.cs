using System;
namespace API.Errors;

public class ApiResponse
{
    public ApiResponse(int statusCode, string message = null)
    {
        StatusCode = statusCode;
        Message = message ?? GetDefaultMessageForStatusCode(statusCode);
    }

    public int StatusCode { get; set; }
    public string Message { get; set; }

    private string GetDefaultMessageForStatusCode(int statusCode)
    {
        return statusCode switch
        {
            400 => "Bad request made",
            401 => "Not Authorized",
            404 => "Not found",
            500 => "Internal server error",
            _ => ""
        };
    }
}

