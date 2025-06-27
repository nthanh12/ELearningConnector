using System;

namespace ElearningConnector.Models.Common
{
    /// <summary>
    /// Danh sách mã lỗi và trạng thái của API theo spec
    /// </summary>
    public enum ApiErrorCode
    {
        /// <summary>0000 – Thành công</summary>
        Success = 0,
        /// <summary>0001 – Lỗi hệ thống</summary>
        SystemError = 1,
        /// <summary>0002 – Tài khoản hoặc mật khẩu không chính xác</summary>
        InvalidCredentials = 2
    }

    public static class ApiErrorCodeExtensions
    {
        public static string DefaultMessage(this ApiErrorCode code) => code switch
        {
            ApiErrorCode.Success => "Thành công",
            ApiErrorCode.SystemError => "Lỗi hệ thống",
            ApiErrorCode.InvalidCredentials => "Tài khoản hoặc mật khẩu không chính xác",
            _ => string.Empty
        };
    }
}