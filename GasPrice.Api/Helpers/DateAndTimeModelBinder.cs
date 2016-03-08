using System;
using System.Web.Http.Controllers;
using System.Web.Http.ModelBinding;

namespace GasPrice.Api.Helpers
{
    /// <summary>
    /// 
    /// </summary>
    public class DateAndTimeModelBinder : IModelBinder
    {
        public DateAndTimeModelBinder() { }

        public bool BindModel(HttpActionContext actionContext, ModelBindingContext bindingContext)
        {
         
            if (bindingContext == null)
            {
                throw new ArgumentNullException("bindingContext");
            }

            //Maybe we're lucky and they just want a DateTime the regular way.
            var dateTimeAttempt = GetA<DateTime>(bindingContext, "DateTime");
            if (dateTimeAttempt != null)
            {
                return true;
            }

            //If they haven't set Month,Day,Year OR Date, set "date" and get ready for an attempt
            if (MonthDayYearSet == false && DateSet == false)
            {
                Date = "Date";
            }

            //If they haven't set Hour, Minute, Second OR Time, set "time" and get ready for an attempt
            if (HourMinuteSecondSet == false && TimeSet == false)
            {
                Time = "Time";
            }

            //Did they want the Date *and* Time?
            var dateAttempt = GetA<DateTime>(bindingContext, Date);
            var timeAttempt = GetA<DateTime>(bindingContext, Time);

            //Maybe they wanted the Time via parts
            if (HourMinuteSecondSet)
            {
                timeAttempt = new DateTime(
                    DateTime.MinValue.Year, DateTime.MinValue.Month, DateTime.MinValue.Day,
                    GetA<int>(bindingContext, Hour).Value,
                    GetA<int>(bindingContext, Minute).Value,
                    GetA<int>(bindingContext, Second).Value);
            }

            //Maybe they wanted the Date via parts
            if (MonthDayYearSet)
            {
                dateAttempt = new DateTime(
                    GetA<int>(bindingContext, Year).Value,
                    GetA<int>(bindingContext, Month).Value,
                    GetA<int>(bindingContext, Day).Value,
                    DateTime.MinValue.Hour, DateTime.MinValue.Minute, DateTime.MinValue.Second);
            }

            //If we got both parts, assemble them!
            return (dateAttempt != null && timeAttempt != null);
        }

        private static T? GetA<T>(ModelBindingContext bindingContext, string key) where T : struct
        {
            if (String.IsNullOrEmpty(key)) return null;
            var valueResult = bindingContext.ValueProvider.GetValue(bindingContext.ModelName + "." + key);

            //Didn't work? Try without the prefix if needed...
            if (valueResult == null && bindingContext.FallbackToEmptyPrefix)
            {
                valueResult = bindingContext.ValueProvider.GetValue(key);
            }
            return (T?) valueResult?.ConvertTo(typeof(T));
        }
        public string Date { get; set; }
        public string Time { get; set; }

        public string Month { get; set; }
        public string Day { get; set; }
        public string Year { get; set; }

        public string Hour { get; set; }
        public string Minute { get; set; }
        public string Second { get; set; }

        public bool DateSet => !String.IsNullOrEmpty(Date);
        public bool MonthDayYearSet => !(String.IsNullOrEmpty(Month) && String.IsNullOrEmpty(Day) && String.IsNullOrEmpty(Year));

        public bool TimeSet => !String.IsNullOrEmpty(Time);
        public bool HourMinuteSecondSet => !(String.IsNullOrEmpty(Hour) && String.IsNullOrEmpty(Minute) && String.IsNullOrEmpty(Second));

       
    }

}
