初始化数据
==============

Migrations
-----------
+ 创建 **class** `SampleDbContextCreatorBase`
```
internal class SampleDbContextCreatorBase : ZeroDbContextCreatorBase
{
    public SampleDbContextCreatorBase(SampleDbContext context) : base(context)
    {
    }
}
```

+ 创建数据初始化器
```
internal class SettingCreateor : SampleDbContextCreatorBase
{
    public SettingCreateor(SampleDbContext context)
            : base(context)
    {

    }
    internal void Create()
    {
        //todo
    }
}
```

```
public class SampleDbContextCreator
{
    internal SettingCreateor SettingCreateor { get; set; }
    public SampleDbContextCreator(SampleDbContext context)
    {
        SettingCreateor = new SettingCreateor(context);
    }
    internal void Create()
    {
        SettingCreateor.Create();
        //todo
    }
}
```

+ Seed
```
public sealed class Configuration : ZeroConfiguration<Adbp.Sample.EntityFramework.SampleDbContext>, IMultiTenantSeed
{
    public AbpTenantBase Tenant { get; set; }
    public Configuration()
        :base("AdbpSample")
    {

    }
    
    protected override void Seed(Adbp.Sample.EntityFramework.SampleDbContext context)
    {
        ZeroDbContextCreator.AuthorizationProviders.Add(new SampleAuthorizationProvider());
        base.Seed(context);

        new SampleDbContextCreator(context).Create();
    }
}
```




