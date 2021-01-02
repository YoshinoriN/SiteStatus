# [WIP]: SiteStatus

Batch application for check & validate domains.

# Requirements

* .NET Core 5.0

# Usage

* Write configuration
* Execute `SiteStatus.ext`

# Configuration

Put `settings.json` in the directory where` SiteStatus.exe` exists.

```json
{
  // set domains you want to check.
  "domains": [
    "example.com",
    "sub.example.com"
  ],
  // local or s3
  "storageType": "local",
  "output": {
    "whois": {
      // please DO NOT WRITE relative path if storageType is s3
      "destination": "./whois.json"
    },
    "certifications": {
      // please DO NOT WRITE relative path if storageType is s3
      "destination": "./certifications.json"
    }
  },
  "s3": {
    "bucketName": "bucket.example.com",
    "isPublicRead": "true"
  }
}
```

## S3 config example

Currently, can not set `region`, `profile` from `settings.json`. The program uses the default user's profile.

Certificate result (JSON) will put onto `bucket.example.com/sites/certifications.json` as below.

```json
{
  "domains": [
    "example.com",
    "sub.example.com"
  ],
  "storageType": "s3",
  "output": {
    "whois": {
      "destination": "sites/whois.json"
    },
    "certifications": {
      "destination": "sites/certifications.json"
    }
  },
  "s3": {
    "bucketName": "bucket.example.com",
    "isPublicRead": "true"
  }
}
```