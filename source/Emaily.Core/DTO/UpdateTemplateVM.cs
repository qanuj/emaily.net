namespace Emaily.Core.DTO
{
    public class UpdateTemplateVM
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public dynamic Custom { get; set; }
        public string Html { get; set; }
    }
}