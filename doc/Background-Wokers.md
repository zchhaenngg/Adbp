EmailWorker
===================

执行间隔时间
---------------
默认为**30**秒。可通过[修改配置](https://www.aspnetboilerplate.com/Pages/Documents/Setting-Management#changing-settings) `dbp.BackgroundWorkers.EmailWorker.TimerPeriodSeconds` 进行修改。

启用邮件后台服务
---------------

append below code on method **PreInitialize** in class **SampleApplicationModule**
```
Configuration.Modules.ZeroApplicationModule().IsEmailWorkerEnabled = true;
```

 


