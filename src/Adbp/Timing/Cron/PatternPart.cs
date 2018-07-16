using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace Adbp.Timing.Cron
{
    public class PatternPart
    {
        public static object _obj = new object();
        /// <summary>
        /// 缓存结果
        /// </summary>
        private List<int> _schedules;
        public int LowerThreshold { get; set; }
        public int UpperThreshold { get; set; }
        
        public PatternPart(int lowerThreshold, int upperThreshold)
        {
            LowerThreshold = lowerThreshold;
            UpperThreshold = upperThreshold;
        }

        private string _part;
        public string Part
        {
            get
            {
                return _part;
            }
            set
            {
                _part = value;
                lock (_obj)
                {
                    _schedules = null;
                }
            }
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public virtual void Check()
        {
            if (!IsValid)
            {
                throw new Exception($"The Part format is incorrect { Part }");
            }
        }

        public virtual bool IsValid
        {
            get
            {
                if (string.IsNullOrWhiteSpace(Part))
                {
                    return false;
                }
                if (IsUsingAsterisk)
                {
                    return true;
                }
                if (IsUsingBackslash)
                {
                    if (int.TryParse(Part.Substring(1), out int result))
                    {
                        return LowerThreshold <= result && result <= UpperThreshold;
                    }
                }
                else
                {
                    if (int.TryParse(Part, out int result))
                    {
                        return LowerThreshold <= result && result <= UpperThreshold;
                    }
                }
                return false;
            }
        }
        
        /// <summary>
        /// 是否直接使用数字 -> 固定时刻
        /// </summary>
        public virtual bool IsFixedMoment
            => !IsUsingAsterisk && !IsUsingBackslash;

        /// <summary>
        /// 分析Part得出的数值部分, 表示间隔多久,或某个固定时刻
        /// </summary>
        public virtual int Value
            => IsUsingAsterisk ? LowerThreshold :
                IsUsingBackslash ? int.Parse(Part.Substring(1)) : int.Parse(Part);

        protected virtual bool IsUsingAsterisk
            => Part == "*";

        protected virtual bool IsUsingBackslash
            => Part.StartsWith("/");
        
        public List<int> Schedules()
        {
            IEnumerable<int> getSchedules()
            {
                Check();
                if (IsFixedMoment)
                {
                    yield return Value;
                }
                else
                {
                    var interval = Value == 0 ? Value + 1 : Value;
                    for (int i = LowerThreshold; i <= UpperThreshold; i = i + interval)
                    {
                        yield return i;
                    }
                }
            }

            if (_schedules == null)
            {
                lock (_obj)
                {
                    if (_schedules != null)
                    {
                        return _schedules;
                    }
                    _schedules = getSchedules().ToList();
                }
            }
            return _schedules;
        }

        public int MaxSchedule
            => Schedules().Last();

        public int MinimumSchedule
            => Schedules().First();

        public bool IsScheduleValue(int value)
        {
            return Schedules().Any(x => x == value);
        }
    }
} 
