using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace Adbp.Timing.Cron
{
    public class Schedule
    {
        private string _pattern;

        /// <summary>
        /// 在最近的20年中,找符合执行计划的
        /// </summary>
        public const int FIND_YEARS = 20;

        private PatternPart _minutePart = new PatternPart(0, 59);
        private PatternPart _hourPart = new PatternPart(0, 23);
        private PatternPart _dayOfMonthPart = new PatternPart(1, 31);
        private PatternPart _monthPart = new PatternPart(1, 12);
        private PatternPart _dayOfWeekPart = new PatternPart(0, 6);


        public Schedule() { }

        public void SetPattern(string pattern)
        {
            _pattern = pattern;
            SetParts(pattern);
        }

        private void SetParts(string pattern)
        {
            var partArr = pattern.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            if (partArr.Length < 5)
            {
                throw new Exception($"Split pattern, parts array's length should greater than or equal to 5.");
            }
            _minutePart.Part = partArr[0];
            _hourPart.Part = partArr[1];
            _dayOfMonthPart.Part = partArr[2];
            _monthPart.Part = partArr[3];
            _dayOfWeekPart.Part = partArr[4];

            Check();
        }

        public void Check()
        {
            if (string.IsNullOrWhiteSpace(_pattern))
            {
                throw new Exception("please set pattern!");
            }
            _minutePart.Check();
            _hourPart.Check();
            _dayOfMonthPart.Check();
            _monthPart.Check();
            _dayOfWeekPart.Check();
        }

        public bool IsValid => _minutePart.IsValid
            && _hourPart.IsValid
            && _dayOfMonthPart.IsValid
            && _monthPart.IsValid
            && _dayOfWeekPart.IsValid;
        
        /// <summary>
        /// 下一个执行时间,返回值>from
        /// </summary>
        /// <returns></returns>
        public DateTime Next(DateTime from)
        {
            Check();
            int year = from.Year,
                month = from.Month,
                day = from.Day,
                hour = from.Hour;
            for (int y = year; y < year + 20; y++)
            {
                foreach (var M in _monthPart.Schedules())
                {
                    if (y == year && M < month)
                    {
                        continue;
                    }
                    foreach (var d in _dayOfMonthPart.Schedules())
                    {
                        if (y == year && M == month && d < day)
                        {
                            continue;
                        }
                        if (d > DateTime.DaysInMonth(y, M))
                        {//日期不存在,退出循环
                            break;
                        }
                        if (!_dayOfWeekPart.IsScheduleValue((int)new DateTime(y, M, d).DayOfWeek))
                        {
                            continue;
                        }
                        foreach (var h in _hourPart.Schedules())
                        {
                            if (y == year && M == month && d == day && h < hour)
                            {
                                continue;
                            }
                            foreach (var m in _minutePart.Schedules())
                            {
                                var dt = new DateTime(y, M, d, h, m, 0, from.Kind);
                                if (dt > from)
                                {
                                    return dt;
                                }
                            }
                        }
                    }
                }
            }
            throw new Exception($"The next Schedule Time has not been found for the next 20 years.");
        }
        
    }
}
