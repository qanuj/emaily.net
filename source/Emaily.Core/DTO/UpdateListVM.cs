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
        public bool IsOptIn { get; set; }
        public bool IsUnsubcribeAllList { get; set; }
    }

    public class ListInfoVM
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public MailNote Confirmation { get; set; }
        public MailNote GoodBye { get; set; }
        public MailNote ThankYou { get; set; }
        public bool IsOptIn { get; set; }
        public bool IsUnsubcribeAllList { get; set; }
        public int AppId { get; set; }
        public string Custom { get; set; }
    }
}