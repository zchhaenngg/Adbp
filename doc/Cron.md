CRON
=============

CRON Expression
-------------
概述：CRON表达式在使用调度器的业务场景中得到了广泛使用, 但其实在其他业务如生成周期性报表时也能大显身手。
```
0    1    2    3    4
*    *    *    *    *
|    |    |    |    |
|    |    |    |    + — — — — — day of week ( 0 - 6 ) ( Sunday = 0 )
|    |    |    + — — — — — — month ( 1 - 12 )
|    |    + — — — — — — — day of month ( 1 - 31 )
|    + — — — — — — — hour ( 0 - 23 )
+ — — — — — — — min ( 0 - 59 )
```
注意: `/n` 必须满足`n > 0`, 如 `/1 * * * *` , 表示每隔1分钟

使用方式
-------------------
Namespace：`Adbp.Timing.Cron`  Class：`Schedule`以及`PatternPart`

让我们获取最近3次的执行时间,
```
var schedule = new Schedule();
schedule.SetPattern("0 0 1 /3 *");
var time1 = schedule.Next(DateTime.Now);
var time2 = schedule.Next(time1);
var time3 = schedule.Next(time2);
```
