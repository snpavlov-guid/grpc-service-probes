# ASP.NET Core gRPC example project



Project contains two VisualStudio solutions: 

- GrpcAppService.sln - contains ASP.NET Core services' project
- GrpcServiceClient.sln - contains console and PowerShell cmdlet's clients



## GrpcServiceApp project

Implements several gRPC services:

- **greet** - [default Microsoft's example of greeting service](https://docs.microsoft.com/en-us/aspnet/core/grpc/basics?view=aspnetcore-5.0#proto-file-1).
- **greet v1** - greeting service with additional methods. Service demonstrates gRPC service versioning.  
- **transfe**r - file transfer service. Service demonstrates gRPC [server streaming calls](https://docs.microsoft.com/en-us/aspnet/core/grpc/client?view=aspnetcore-5.0#server-streaming-call) and [client streaming calls](https://docs.microsoft.com/en-us/aspnet/core/grpc/client?view=aspnetcore-5.0#client-streaming-call).
- **authentication** - authentication service. Service demonstrates JWT based [gRPC service authentication](https://docs.microsoft.com/en-us/aspnet/core/grpc/authn-and-authz?view=aspnetcore-5.0).


![gRPCServiceApp-Services](.\Documents\Pictures\gRPCServiceApp-Services.PNG)

Service app implements a demo file repository for transfer service.

<img src=".\Documents\Pictures\gRPCServiceApp-FileRepo.PNG" alt="gRPCServiceApp-FileRepo" style="zoom:33%;" />



## GrpcConsoleClient project

.Net Core console application that demonstrates simple gRPC client app.

Makes both anonymous and authenticated calls to GrpcServiceApp.

<img src=".\Documents\Pictures\GrpcConsoleClient-Run.PNG" alt="GrpcConsoleClient-Run" style="zoom: 33%;" />





## GrpcServiceClient project

.Net Core management automation application. Implements set of coded PowerShell cmdlets to demonstrate calls of gRPC service methods.

 ![GrpcServiceClient-Run](.\Documents\Pictures\GrpcServiceClient-Run.PNG)



