# ToyRobot.API

REST API for ToyRobot project.
The current implementation uses JWT authentication, SQL Server database and exposes objects and endpoints with Swagger on development mode.


## Controllers

- Command: commands list and execute method
- User: login and create user

## Project setup

To create and install the localhost certificate, 
customize $certKeyPath and execute in powershell:

```
$certKeyPath = "d:\Certificates\localhost.pfx"
$cert = New-SelfSignedCertificate -DnsName @("localhost") -CertStoreLocation "cert:\LocalMachine\My"
$password = ConvertTo-SecureString 'secretPassword' -AsPlainText -Force
$cert | Export-PfxCertificate -FilePath $certKeyPath -Password $password
$rootCert = $(Import-PfxCertificate -FilePath $certKeyPath -CertStoreLocation 'Cert:\LocalMachine\Root' -Password $password)
```

Install IIS on localhost.

Add localhost website, add application "toyrobotapi" with localhost.pfx certificate on https and "ToyRobot.API" folder as physical path



