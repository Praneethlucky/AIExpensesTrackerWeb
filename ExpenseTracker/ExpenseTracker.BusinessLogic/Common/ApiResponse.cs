namespace ExpenseTracker.BusinessLogic.Common;

public class ApiResponse<T>
{
    public bool success { get; set; }
    public string message { get; set; }
    public T data { get; set; }
    public string errorCode { get; set; }

    public static ApiResponse<T> SuccessResponse(
        T data,
        string message = "")
    {
        return new ApiResponse<T>
        {
            success = true,
            message = message,
            data = data,
        };
    }

    public static ApiResponse<T> FailureResponse(
        string errors,
        string message = "")
    {
        return new ApiResponse<T>
        {
            success = false,
            message = message,
            data = default,
            errorCode = errors
        };
    }
}