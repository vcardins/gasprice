using System.Web.Http.ModelBinding;

namespace GasPrice.Api.Helpers
{

    public class DateAndTimeAttribute : CustomModelBinderAttribute
    {
        private readonly IModelBinder _binder;

        // The user cares about a full date structure and full
        // time structure, or one or the other.
        public DateAndTimeAttribute(string date, string time)
        {
            _binder = new DateAndTimeModelBinder
            {
                Date = date,
                Time = time
            };
        }

        // The user wants to capture the date and time (or only one)
        // as individual portions.
        public DateAndTimeAttribute(string year, string month, string day,
            string hour, string minute, string second)
        {
            _binder = new DateAndTimeModelBinder
            {
                Day = day,
                Month = month,
                Year = year,
                Hour = hour,
                Minute = minute,
                Second = second
            };
        }

        // The user wants to capture the date and time (or only one)
        // as individual portions.
        public DateAndTimeAttribute(string date, string time,
            string year, string month, string day,
            string hour, string minute, string second)
        {
            _binder = new DateAndTimeModelBinder
            {
                Day = day,
                Month = month,
                Year = year,
                Hour = hour,
                Minute = minute,
                Second = second,
                Date = date,
                Time = time
            };
        }

        public override IModelBinder GetBinder() { return _binder; }
    }
}
