# SiteStatus

Batch application for check & validate domains.

Create whois(SLD only) & serverside certificate status to JSON. And store it to local storage or AWS S3.

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

# Example of result

```json
// certificate
[
  {
    "domain":"yoshinorin.net",
    "status":"success",
    "issuer":"CN=R3, O=Let's Encrypt, C=US",
    "validFrom":1609238175,
    "validTo":1617014175,
    "checkedAt":1609590374
  },
  ...
]

// whois (SLD only)
[
  {
    "DomainName":"yoshinorin.net",
    "status":"success",
    "Registered":"1463150894",
    "Updated":"1512391744",
    "Expiration":"1810219694",
    "Registrar":{
      "Name":"Amazon Registrar, Inc."
    },
    "NameServers":[
      "ns-1080.awsdns-07.org",
      "ns-1584.awsdns-06.co.uk",
      "ns-503.awsdns-62.com",
      "ns-588.awsdns-09.net"
    ],
    "CheckedAt":1609590371
  },
  ...
]
```

# Using Libraries

||LICENSE|
|---|---|
|[AWS SDK .NET](https://github.com/aws/aws-sdk-net)|[Apache License, Version 2.0](https://github.com/aws/aws-sdk-net/blob/master/License.txt)|
|[Whois](https://github.com/flipbit/whois)|[MIT](https://www.nuget.org/packages/Whois/2.0.2/License)|
|[xUnit](https://github.com/xunit/xunit)|[Apache License, Version 2.0](https://github.com/xunit/xunit/blob/main/LICENSE)|

