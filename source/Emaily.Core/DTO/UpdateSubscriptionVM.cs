using System.ComponentModel.DataAnnotations;

namespace Emaily.Core.DTO
{
    public class UpdateSubscriptionVM
    {
        public int Id { get; set; }
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        public string Token { get; set; }
    }
}