# User Service
[![Build .NET API](https://github.com/Quarter-Break/user_service/actions/workflows/build_test.yml/badge.svg)](https://github.com/Quarter-Break/user_service/actions/workflows/build_test.yml)
[![Deploy .NET API](https://github.com/Quarter-Break/user_service/actions/workflows/docker-publish.yml/badge.svg)](https://github.com/Quarter-Break/user_service/actions/workflows/docker-publish.yml)

.NET Core 5.0 service for authentication and authorization.

## Getting Started
```zsh
dotnet build
```
```zsh
dotnet restore
```
```zsh
dotnet run
```

## Run with Docker
```
docker-compose up --build
```

## Run RabbitMQ
```
docker run --network qbreak-network -it --rm --name rabbitmq -p 5672:5672 -p 15672:15672 rabbitmq:3-management
```

#### Error: Docker Network Missing
If you get the following error:
Network `qbreak-network` declared as external, but could not be found. Run the following:
```zsh
docker network create qbreak-network
```
<i>Note: a Docker network is required to allow the container to communicate with other containers.</i>

#### Resources

Repository pattern in .NET: https://www.programmingwithwolfgang.com/repository-pattern-net-core/

Testing with Moq: https://softchris.github.io/pages/dotnet-moq.html#instruct-our-mock

Authentication/authorization: https://github.com/cornflourblue/aspnet-core-3-jwt-authentication-api/

Variant generic interfaces: https://docs.microsoft.com/en-us/dotnet/csharp/programming-guide/concepts/covariance-contravariance/creating-variant-generic-interfaces

RabbitMQ implementation: https://www.programmingwithwolfgang.com/rabbitmq-in-an-asp-net-core-3-1-microservice/#implement-rabbitmq-with-net-core
