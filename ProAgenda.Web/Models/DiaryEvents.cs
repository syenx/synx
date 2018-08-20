using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Globalization; // << dont forget to add this for converting dates to localtime
using System.Data.Entity.Core.Objects;
using ProAgenda.Web.Models;

namespace ProAgenda.Web
{
    public class DiaryEvent
    {
        public int ID;
        public string Title;
        public int SomeImportantKeyID;
        public string StartDateString;
        public string EndDateString;
        public string StatusString;
        public string StatusColor;
        public string ClassName;

        public static List<DiaryEvent> LoadAllAppointmentsInDateRange(double start, double end)
        {
            var fromDate = ConvertFromUnixTimestamp(start);
            var toDate = ConvertFromUnixTimestamp(end);

            var rslt = Banco.SelectAllList();
       

            List<DiaryEvent> result = new List<DiaryEvent>();
            foreach (var item in rslt)
            {
                DiaryEvent recs = new DiaryEvent();
                recs.ID = item.ID;
                recs.SomeImportantKeyID = item.SomeImportantKey;
                recs.StartDateString = item.DateTimeScheduled.ToString("s"); // "s" is a preset format that outputs as: "2009-02-27T12:12:22"
                recs.EndDateString = item.DateTimeScheduled.AddMinutes(item.AppointmentLength).ToString("s"); // field AppointmentLength is in minutes
                recs.Title = item.Title + " - " + item.AppointmentLength.ToString() + " mins";
                recs.StatusString = Enums.GetName<AppointmentStatus>((AppointmentStatus)item.StatusENUM);
                recs.StatusColor = Enums.GetEnumDescription<AppointmentStatus>(recs.StatusString);
                string ColorCode = recs.StatusColor.Substring(0, recs.StatusColor.IndexOf(":"));
                recs.ClassName = recs.StatusColor.Substring(recs.StatusColor.IndexOf(":") + 1, recs.StatusColor.Length - ColorCode.Length - 1);
                recs.StatusColor = ColorCode;
                result.Add(recs);
            }

            return result;


        }
        
        public static List<DiaryEvent> LoadAppointmentSummaryInDateRange(double start, double end)
        {

            var fromDate = ConvertFromUnixTimestamp(start);
            var toDate = ConvertFromUnixTimestamp(end);
        
            var rslt = Banco.GetAgendas(start, end);

            List<DiaryEvent> result = new List<DiaryEvent>();
            int i = 0;
            foreach (var item in rslt)
            {
                DiaryEvent rec = new DiaryEvent();
                rec.ID = i; //we dont link this back to anything as its a group summary but the fullcalendar needs unique IDs for each event item (unless its a repeating event)
                rec.SomeImportantKeyID = -1;
                string StringDate = string.Format("{0:yyyy-MM-dd}", item.DateTimeScheduled);
                rec.StartDateString = StringDate + "T00:00:00"; //ISO 8601 format
                rec.EndDateString = StringDate + "T23:59:59";
                //rec.Title = "Booked: " + item.Count.ToString();
                result.Add(rec);
                i++;
            }

            return result;


        }

        public static void UpdateDiaryEvent(int id, string NewEventStart, string NewEventEnd)
        {
            // EventStart comes ISO 8601 format, eg:  "2000-01-10T10:00:00Z" - need to convert to DateTime
            Banco.UpdateAgenda(id, NewEventStart, NewEventEnd);
        }

        private static DateTime ConvertFromUnixTimestamp(double timestamp)
        {
            var origin = new DateTime(1970, 1, 1, 0, 0, 0, 0);
            return origin.AddSeconds(timestamp);
        }

        public static bool CreateNewEvent(string Title, string NewEventDate, string NewEventTime, string NewEventDuration)
        {
            Banco.CreateNewAgenda(Title, NewEventDate, NewEventTime, NewEventDuration);
            return true;
        }
    }
}