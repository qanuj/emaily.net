namespace Emaily.Services
{
    public interface IAmazonConfigProvider : ICloudConfigProvider
    {
        string Region { get; }
        string AccessKey { get;}
        string Secret { get;}
        string BounceTopic { get;}
        string ComplaintTopic { get;}
    }
}