using System;
using Amazon.S3;
using Amazon.S3.Model;
using SiteStatus.Infrastructures.Storage;

namespace SiteStatus.Infrastructures.Certificates.Storage
{
    public class S3 : ICertificateStorage
    {
        public Domains.Settings.Settings _settings { get; private set; }
        public S3(Domains.Settings.Settings settings)
        {
            // TODO: validation
            this._settings = settings;
        }

        public void Put(string json)
        {
            // TODO: get region from settings.
            using (var s3Client = new AmazonS3Client())
            {
                PutObjectRequest request = new PutObjectRequest()
                {
                    ContentBody = json,
                    BucketName = this._settings.S3.BucketName,
                    Key = this._settings.OutPut.Certifications.Destination, // TODO: replace relative path
                    ContentType = "application/json",
                    CannedACL = Convert.ToBoolean(this._settings.S3.IsPublicRead) ? S3CannedACL.PublicRead : S3CannedACL.Private
                };
                s3Client.PutObjectAsync(request).GetAwaiter().GetResult();
            }
        }
    }
}
