using System.ComponentModel.DataAnnotations;

namespace Emaily.Core.DTO
{
    public class ListEmail
    {
        public int ListId { get; set; }
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
    }
}