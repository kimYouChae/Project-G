
public class ApiResponse<T>
{
    public bool success { get; set; }
    public T data { get; set; }
    public ApiError error { get; set; }
}

public class ApiError
{
    public int code;
    public string message;
}
