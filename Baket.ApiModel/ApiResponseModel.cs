
namespace Basket.ApiModel
{
    public class ApiResponseModel<T, R>
    {
        public bool IsSuccess { get; set; }
        public R ResponseCode { get; set; }
        public string ResponseMessage { get; set; }
        public T Data { get; set; }
    }
}
