namespace AJTarefasDomain.Base
{
    public class BaseResponse<T>
    {
        public bool Success { get; set; }
        public T Data { get; set; }
        public string ErrorMessage { get; set; }

        public static BaseResponse<T> SuccessResponse(T data)
        {
            return new BaseResponse<T>
            {
                Success = true,
                Data = data,
                ErrorMessage = null
            };
        }

        public static BaseResponse<T> ErrorResponse(string errorMessage)
        {
            return new BaseResponse<T>
            {
                Success = false,
                Data = default(T),
                ErrorMessage = errorMessage
            };
        }

    }
}
