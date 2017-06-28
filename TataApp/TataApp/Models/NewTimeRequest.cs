namespace TataApp.Models
{
    using System;
    public class NewTimeRequest
    {
        public int EmployeeId { get; set; }

        public int ProjectId { get; set; }

        public int ActivityId { get; set; }

        public DateTime DateReported { get; set; }

        public DateTime From { get; set; }

        public DateTime To { get; set; }

        public double Latitude { get; set; }

        public double Longitude { get; set; }

        public string Remarks { get; set; }

        public bool IsRepeated { get; set; }

        public bool IsRepeatMonday { get; set; }

        public bool IsRepeatTuesday { get; set; }

        public bool IsRepeatWednesday { get; set; }

        public bool IsRepeatThursday { get; set; }

        public bool IsRepeatFriday { get; set; }

        public bool IsRepeatSaturday { get; set; }

        public bool IsRepeatSunday { get; set; }

        public DateTime Until { get; set; }
    }
}
