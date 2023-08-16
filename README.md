# Influxdb2 demo

dotnet add package Influxdb2.Client
```
### 服务注册
```
services.AddInfuxdb(o =>
{
    o.Host = new Uri("http://localhost:8086");
    o.Token = "base64 token value";
    o.DefaultOrg = "my-org";
    o.DefaultBucket = "my-bucket";
});
```

### 服务获取
```
MyService(IInfuxdb infuxdb)
{
}
```
###

### 写入数据
```
var book = new Book();
await infuxdb.WriteAsync(book);
```
或者
```
var book = new PointBuilder("Book")
    .SetTag("key", "value")
    .SetField("field", "value")
    .Build();
await infuxdb.WriteAsync(book);
```

数据属性需要ColumnType标记
```
class Book
{
    [ColumnType(ColumnType.Tag)]
    public string Serie { get; set; }


    [ColumnType(ColumnType.Field)]
    public string Name { get; set; }


    [ColumnType(ColumnType.Field)]
    public double? Price { get; set; }


    [ColumnType(ColumnType.Field)]
    public bool? SpecialOffer { get; set; }


    [ColumnType(ColumnType.Timestamp)]
    public DateTimeOffset? CreateTime { get; set; } 
}
```

