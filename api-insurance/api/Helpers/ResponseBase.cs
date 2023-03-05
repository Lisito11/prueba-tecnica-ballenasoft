using System;
namespace api.Helpers
{
	public class ResponseBase<T>
	{
        public ResponseBase()
        {
            Succeeded = true;
            Error = string.Empty;
            StatusCode = 200;
        }

        public ResponseBase(T data)
        {
            Succeeded = true;
            Error = string.Empty;
            StatusCode = 200;
            Data = data;
        }

        public T? Data { get; set; }
        public bool Succeeded { get; set; }
        public string? Error { get; set; }
        public int? StatusCode { get; set; }

    }
}

