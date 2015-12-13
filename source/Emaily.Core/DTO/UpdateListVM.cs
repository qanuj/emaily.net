using Emaily.Core.Data.Complex;

namespace Emaily.Core.DTO
{
    public class UpdateListVM
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public MailNote Confirmation { get; set; }
        public MailNote GoodBye { get; set; }
        public MailNote ThankYou { get; set; }
        public string SubscribedUrl { get; set; }
        public string ConfirmUrl { get; set; }
        public bool IsOptIn { get; set; }
        public bool IsUnsubcribeAllList { get; set; }
        public string UnsubscribedUrl { get; set; }
    }
}