namespace MyFirstApp.Models
{
    public class CalendarModel
    {
        public void OnGet()
        {
        }
        public static DateTime Today;
        public static DateTime firstDay;
        public static int daysInCurrentMonth;
        static DateTime lastDay;
        public static int dayOfWeekFirst;
        public static int dayOfWeekLast;
        public static int daysInLastMonth;
        public static int firstDayInTable;

        public static void Set()
        {
            DateTime firstDay = new DateTime(Today.Year, Today.Month, 1);
            daysInCurrentMonth = DateTime.DaysInMonth(firstDay.Year, firstDay.Month);
            lastDay = new DateTime(firstDay.Year, firstDay.Month, daysInCurrentMonth);
            dayOfWeekFirst = ((int)firstDay.DayOfWeek > 0) ? (int)firstDay.DayOfWeek : 7;
            dayOfWeekLast = ((int)lastDay.DayOfWeek > 0) ? (int)lastDay.DayOfWeek : 7;
            daysInLastMonth = (firstDay.Month == 1) ? DateTime.DaysInMonth(firstDay.Year - 1, 12) : DateTime.DaysInMonth(firstDay.Year, firstDay.Month - 1);
            firstDayInTable = CalendarModel.daysInLastMonth - CalendarModel.dayOfWeekFirst + 2;
        }
    }
}
