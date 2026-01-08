
using System.Diagnostics;

public class ApiResponse<T>
{
    public bool success;
    public T data;
    public ApiError error;
}

public class ApiError
{
    public int code;
    public string message;
}
