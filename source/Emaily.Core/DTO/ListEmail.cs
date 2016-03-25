using System;
using System.ComponentModel.DataAnnotations;

namespace Emaily.Core.DTO
{
    public class ListEmail
    {
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        public string Name { get; set; }
        public string Custom { get; set; }
        public int ListId { get; set; }
    }

    public class UpdateSubscriberVM  : ListEmail
    {
        public bool IsConfirmed { get; set; }
    }

    public class CreateSubscriberVM
    {
        public string Data { get; set; } 
    }

    public class SubscriberVM : ListEmail
    {
        public int Id { get; set; }
        public DateTime Created { get; set; }
        public bool IsUnsubscribed { get; set; }
        public bool IsBounced { get; set; }
        public bool IsSoftBounce { get; set; }
        public bool IsComplaint { get; set; }
        public bool IsConfirmed { get; set; }
    }

    public class ImportResult
    {
        public int Added { get; set; }
        public int Failed { get; set; }
    }
}