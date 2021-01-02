using System.Collections.Generic;
using SiteStatus.Utils;

namespace SiteStatus.Infrastructures.Storage
{
    public interface ICertificateStorage
    {
        void Put(string json);
    }
}
