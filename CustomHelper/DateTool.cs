using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomHelper
{
    public class DateTool
    {
        /// <summary>
        /// 取得當前系統時間
        /// </summary>
        /// <returns></returns>
        public DateTime GetCurrentDate()
        {
            return DateTime.Now;
        }

        /// <summary>
        /// 取得當前月份最後一天
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public DateTime GetCurrentMonthFinalDay(DateTime date)
        {
            var nextMonthFirstDay = this.GetCurrentMonthFirstDay(date.AddMonths(1));
            var CurrentMonthFinalDay = nextMonthFirstDay.AddDays(-1);
            return CurrentMonthFinalDay;
        }

        /// <summary>
        /// 取得當前月份第一天
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public DateTime GetCurrentMonthFirstDay(DateTime date)
        {
            var tempDate = new DateTime(date.Year, date.Month, 1);
            return tempDate;
        }
    }
}