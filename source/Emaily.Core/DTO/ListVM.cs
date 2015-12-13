namespace Emaily.Core.DTO
{
    public class ListVM
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int AppId { get; set; }
        public int Actives { get; set; }
        public int Unsubscribed { get; set; }
        public int Bounced { get; set; }
        public int Total { get; set; }
        public string Key { get; set; }
        public int Spams { get; set; }
    }
}