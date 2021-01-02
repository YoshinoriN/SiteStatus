# [WIP]: SiteStatus

Batch application for check & validate domains.

# Requirements

* .NET Core 5.0

# Usage

* Write configuration
* Execute `SiteStatus.ext`

# Configuration

Put `settings.json` in the directory where` SiteStatus.exe` exists.

```
{
  // set domains you want to check.
  "domains": [
    "example.com",
    "sub.example.com"
  ],
  // local or s3
  "storageType": "local",
  "output": {
    "secondLevelDomain": {
      // please DO NOT WRITE relative path if storageType is s3
      "destination": "./second-level-domains.json"
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

```
{
  "domains": [
    "example.com",
    "sub.example.com"
  ],
  "storageType": "s3",
  "output": {
    "secondLevelDomain": {
      "destination": "sites/second-level-domains.json"
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