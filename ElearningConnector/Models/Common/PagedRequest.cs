using System.ComponentModel.DataAnnotations;

namespace ElearningConnector.Models.Requests
{
    public class PagedRequest
    {
        [Range(1, int.MaxValue, ErrorMessage = "Page phải >= 1")]
        public int Page { get; set; } = 1;

        [Range(1, 100, ErrorMessage = "PageSize phải từ 1 đến 100")]
        public int PageSize { get; set; } = 10;

        [StringLength(100, ErrorMessage = "Keywords tối đa 100 ký tự")]
        public string? Keywords { get; set; }
    }
} 