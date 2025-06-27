using System;
using ElearningConnector.Models.Common;

namespace ElearningConnector.Models.Common
{
    /// <summary>
    /// Response API
    /// </summary>
    public class ApiResponse<T>
    {
        /// <summary>Mã lỗi</summary>
        public string Code { get; set; }
        /// <summary>Mô tả mã lỗi</summary>
        public string Message { get; set; }
        /// <summary>Kết quả dữ liệu</summary>
        public T Result { get; set; }

        public static ApiResponse<T> Success(T result, string message = null, ApiErrorCode code = ApiErrorCode.Success)
        {
            return new ApiResponse<T>
            {
                Code = ((int)code).ToString("D4"),
                Message = message ?? code.DefaultMessage(),
                Result = result
            };
        }

        public static ApiResponse<T> Error(ApiErrorCode code = ApiErrorCode.SystemError, string message = null)
        {
            return new ApiResponse<T>
            {
                Code = ((int)code).ToString("D4"),
                Message = message ?? code.DefaultMessage(),
                Result = default
            };
        }
    }
}