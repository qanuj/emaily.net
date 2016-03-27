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
        public int Id { get; set; }
        public bool IsConfirmed { get; set; }
    }

    public class CreateDataSubscriberVM
    {
        public string Data { get; set; }
    }

    public class CreateNameSubscriberVM
    {
        public string Name { get; set; }
        [DataType(DataType.EmailAddress),Required]
        public string Email { get; set; }
    }

    public class ProcessSubscriberFileVM
    {
        public string FileName { get; set; }
    }

    public class SubscriberReportVM
    {
        public DateTime X { get; set; }
        public int Y { get; set; }
    }

    public class SubscriberCountReportVM
    {
        public int All { get; set; }
        public int Active { get; set; }
        public int Unconfirmed { get; set; }
        public int Unsubscribed { get; set; }
        public int Bounced { get; set; }
        public int Spam { get; set; }
    }

    public class SubscriberVM : ListEmail
    {
        public int Id { get; set; }
        public DateTime Created { get; set; }
        public bool IsActive { get; set; }
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