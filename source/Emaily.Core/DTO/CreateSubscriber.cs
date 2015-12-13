namespace Emaily.Core.DTO
{
    public class CreateSubscriber
    {
        public int ListId { get; set; }
        public string Key { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public dynamic Custom { get; set; }
    }
}