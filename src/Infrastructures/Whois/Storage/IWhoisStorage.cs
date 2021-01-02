namespace SiteStatus.Infrastructures.Storage
{
    public interface IWhoisStorage
    {
        void Put(string json);
    }
}
