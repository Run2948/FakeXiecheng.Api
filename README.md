# ASP.Net Core�������̺��API ��͸RESTful���

### ���� docker ������ݿ������

* ���� mssql ���ݿ����

```shell
docker run -d --name asp-mssql -p 1433:1433 -e "ACCEPT_EULA=Y" -e "SA_PASSWORD=Sa123456" microsoft/mssql-server-linux

docker ps

docker logs asp-mssql
```

* ��ɻ���ע�����������

```csharp
    public class TouristRoutePicture
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [MaxLength(100)]
        public string Url { get; set; }

        [ForeignKey(nameof(TouristRouteId))]
        public Guid TouristRouteId { get; set; }

        public virtual TouristRoute TouristRoute { get; set; }
    }
```

* ���������

```xml
  <ItemGroup>
    <PackageReference Include="Microsoft.EntityFrameworkCore" Version="3.1.10" />
    <PackageReference Include="Microsoft.EntityFrameworkCore.SqlServer" Version="3.1.10" />
    <PackageReference Include="Microsoft.EntityFrameworkCore.Tools" Version="3.1.10">
      <PrivateAssets>all</PrivateAssets>
      <IncludeAssets>runtime; build; native; contentfiles; analyzers; buildtransitive</IncludeAssets>
    </PackageReference>
  </ItemGroup>
```

* �� Package Manage Console ִ�����ݿ�Ǩ������

```shell
add-migration Init

update-database
```

* ������ Command Line ���������

    * ȫ�ְ�װef���ߣ�`dotnet tool install --global dotnet-ef`

```shell
dotnet ef migrations add Init

dotnet ef database update
```

* �����������

```shell
add-migration SeedData

update-database
```

* ������ݿ�ṹ�ĸ���

```shell
add-migration updateTouristRoute

update-database
```

### ���� MySQL ���ݿ�

* ���� mysql ���ݿ���� 

```shell
docker run -itd --name asp-mysql -p 3306:3306 -e MYSQL_ROOT_PASSWORD=123456 mysql
```

* ��װ����

```xml
<ItemGroup>
    <PackageReference Include="Microsoft.EntityFrameworkCore" Version="3.1.10" />
    <PackageReference Include="Microsoft.EntityFrameworkCore.Tools" Version="3.1.10">
      <PrivateAssets>all</PrivateAssets>
      <IncludeAssets>runtime; build; native; contentfiles; analyzers; buildtransitive</IncludeAssets>
    </PackageReference>
    <PackageReference Include="Pomelo.EntityFrameworkCore.MySql" Version="3.2.4" />
  </ItemGroup>
```

* ɾ�� Migrations �ļ���ȫ�����ݣ������¿�ʼ���ݿ�Ǩ��

```shell
add-migration mysqlInit

update-database
```

### ���������� .NET Core API

* Dockerfile

```dockerfile
#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 80
# EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["FakeXiecheng.Api.csproj", "./"]
RUN dotnet restore "./FakeXiecheng.Api.csproj"
COPY . .
WORKDIR "/src/."
RUN dotnet build "FakeXiecheng.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "FakeXiecheng.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "FakeXiecheng.Api.dll"]
```

* ���������� 

```bash
docker build -t fakexiechengapi .
docker run -d --name fakexiechengapi -p8080:80 fakexiechengapi
docker ps
```

* �ҵ� docker �����е����ݿ� IP ��ַ

```bash
docker inspect bridge
```

* �޸����ݿ������ַ����е� `localhost` Ϊ `172.17.0.2`

* ɾ���������¹���������������

```bash
docker stop fakexiechengapi
docker rm fakexiechengapi
docker build -t fakexiechengapi .
docker run -d --name fakexiechengapi -p8080:80 fakexiechengapi
docker ps
```