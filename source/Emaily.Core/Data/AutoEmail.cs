using Emaily.Core.Abstraction;

namespace Emaily.Core.Data
{
    public class AutoEmail : Campaign
    {
        public AutoResponder AutoResponder { get; set; }
        public int AutoResponderId { get; set; }

        public string TimeCondition { get; set; }
    }
}