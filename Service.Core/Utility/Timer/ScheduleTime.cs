namespace Service.Core.Utility.Timer
{
	public enum DayOfWeek
	{
		None = -1,
		Sunday = 0,
		Monday = 1,
		Tuesday = 2,
		Wednesday = 3,
		Thursday = 4,
		Friday = 5,
		Saturday = 6
	}

	public enum ScheduleType
	{
		MonthDay,
		Weekday
	}

	public class ScheduleTime
	{
		public DayOfWeek Weekday { get; set; }

		public int MonthDay { get; set; }

		public int Hour { get; set; }

		public int Minute { get; set; }

		public ScheduleType ScheduleType { get; set; }

		/// <summary>
		/// Parses to day of week.
		/// </summary>
		/// <param name="dayOfWeek">The day of week. Values for this parameter: Su,M,Tu,W,Th,F,Sa</param>
		/// <returns></returns>
		public static DayOfWeek ParseToDayOfWeek(string dayOfWeek)
		{
			switch (dayOfWeek.ToUpper())
			{
				case "SU":
					return DayOfWeek.Sunday;
				case "M":
					return DayOfWeek.Monday;
				case "TU":
					return DayOfWeek.Tuesday;
				case "W":
					return DayOfWeek.Wednesday;
				case "TH":
					return DayOfWeek.Thursday;
				case "F":
					return DayOfWeek.Friday;
				case "SA":
					return DayOfWeek.Saturday;
				default:
					return DayOfWeek.None;
			}
		}
	}
}