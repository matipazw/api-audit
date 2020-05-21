using System;
namespace api.audit.web.example.Models
{
    public class EventScheduling
    {
        DateTime _closeDate;
        DateTime _startDate;
        int _startHour;

        public int Hours { get; set; }


        public DateTime StartDate {
            get {

                SetStartDate();
                return _startDate;
            }
            set {

                _startDate = value.Date;
                SetStartDate();
            }
        }

        public int StartHour
        {
            get
            {
                SetStartDate();
                return _startHour;
            }
            set
            {

                _startHour = value;
                SetStartDate();
            }
        }

       
        public DateTime CloseDate {
            get {

                if (_closeDate == DateTime.MinValue && _startDate != DateTime.MinValue)
                {
                    var date = StartDate;
                    _closeDate = date.AddHours(Hours);
                }
                 return _closeDate;
            }
            set {

                _closeDate = value;
            }
        }

        void SetStartDate()
        {
            if (_startDate != DateTime.MinValue)
            {
                _startDate = new DateTime(_startDate.Year, _startDate.Month, _startDate.Day, _startHour, 0, 0);
            }
        }
    }
}
