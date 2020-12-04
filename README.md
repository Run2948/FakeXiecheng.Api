# ASP.Net Core开发电商后端API 吃透RESTful风格

### 基于 docker 完成数据库的启动

* 创建 mssql 数据库服务

```shell
docker run -d --name asp-mssql -p 1433:1433 -e "ACCEPT_EULA=Y" -e "SA_PASSWORD=Sa123456" microsoft/mssql-server-linux

docker ps

docker logs asp-mssql
```

* 完成基于注解的属性配置

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

* 添加依赖项

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

* 在 Package Manage Console 执行数据库迁移命令

```shell
add-migration Init

update-database
```

* 或者在 Command Line 中输入命令：

    * 全局安装ef工具：`dotnet tool install --global dotnet-ef`

```shell
dotnet ef migrations add Init

dotnet ef database update
```

* 添加种子数据

```shell
add-migration SeedData

update-database
```

