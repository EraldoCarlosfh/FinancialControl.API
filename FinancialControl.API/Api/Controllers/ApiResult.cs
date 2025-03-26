using FinancialControl.API.Api.Notifications;
using Newtonsoft.Json;

namespace FinancialControl.API.Api.Controllers
{
    public class ApiResult
    {
        public ApiResult()
        {

        }

        [JsonProperty(PropertyName = "isSuccess")]
        public bool IsSuccess { get; private set; }

        [JsonProperty(PropertyName = "message")]
        public string Message { get; private set; }

        [JsonProperty(PropertyName = "errors")]
        public List<Notification> Errors { get; private set; }

        [JsonProperty(PropertyName = "data")]
        public object Data { get; private set; }


        public ApiResult(bool isSuccess, string message, object data)
        {
            IsSuccess = isSuccess;
            Message = message;
            Data = data;
        }

        public ApiResult(bool isSuccess, string message, List<Notification> erros)
        {
            IsSuccess = isSuccess;
            Message = message;
            Errors = erros;
        }
    }
}
