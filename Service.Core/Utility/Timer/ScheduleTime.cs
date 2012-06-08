#region

// -----------------------------------------------------
// MIT License
// Copyright (C) 2012 John M. Baughman (jbaughmanphoto.com)
//
// Permission is hereby granted, free of charge, to any person obtaining a copy of this software and
// associated documentation files (the "Software"), to deal in the Software without restriction,
// including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense,
// and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so,
// subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in all copies or substantial
// portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.
// IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY,
// WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE
// SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
// -----------------------------------------------------

#endregion

namespace Service.Core.Utility.Timer {
	public enum DayOfWeek {
		None = -1,
		Sunday = 0,
		Monday = 1,
		Tuesday = 2,
		Wednesday = 3,
		Thursday = 4,
		Friday = 5,
		Saturday = 6
	}

	public enum ScheduleType {
		MonthDay,
		Weekday
	}

	public class ScheduleTime {

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
		public static DayOfWeek ParseToDayOfWeek(string dayOfWeek) {
			switch (dayOfWeek.ToUpper()) {
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