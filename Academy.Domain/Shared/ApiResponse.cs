using System.Net;

namespace Academy.Domain.Shared;

public record ApiResponse<TResult>
{
    public bool IsSuccess { get; set; }

    public HttpStatusCode StatusCode { get; set; }
    public TResult Result { get; set; }

    public string Message { get; set; } = "";

    public static ApiResponse<TResult> FailResponse(string message) => new()
        { IsSuccess = false, StatusCode = HttpStatusCode.InternalServerError, Message = message };

    public static ApiResponse<TResult> SuccessResponse(HttpStatusCode httpStatusCode, TResult result) =>
        new() { IsSuccess = true, StatusCode = httpStatusCode, Result = result };
}

public record ApiResponse<TSuccess, TError>
{
    public bool IsSuccess { get; set; }

    public HttpStatusCode StatusCode { get; set; }

    public TSuccess? SuccessData { get; set; }

    public TError? ErrorData { get; set; }

    public string Message { get; set; } = "";


    public static ApiResponse<TSuccess, TError> FailResponse(TError error, string message) => new()
        { IsSuccess = false, ErrorData = error, StatusCode = HttpStatusCode.InternalServerError, Message = message };

    public static ApiResponse<TSuccess, TError> FailResponse(TError error, HttpStatusCode httpStatusCode) =>
        new() { IsSuccess = false, ErrorData = error, StatusCode = httpStatusCode };

    public static ApiResponse<TSuccess, TError> SuccessResponse(TSuccess success, HttpStatusCode httpStatusCode) =>
        new() { IsSuccess = true, SuccessData = success, StatusCode = httpStatusCode };
}