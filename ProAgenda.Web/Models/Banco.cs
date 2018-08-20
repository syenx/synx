using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Web;

namespace ProAgenda.Web.Models
{
    public static class Banco
    {

        public static string ConectionString()
        {
            return @"Data Source=SYENX;Initial Catalog=FullCalendarMVC_Demo;Integrated Security=True;MultipleActiveResultSets=True;Application Name=EntityFramework";

            //  return @"Data Source=SYENX;Initial Catalog=FullCalendarMVC_Demo;Integrated Security=True;MultipleActiveResultSets=True;Application Name=EntityFramework;Application Name=EntityFramework";
        }

        public static List<AppointmentDiary> SelectAllList()
        {
            SqlConnection con = new SqlConnection(ConectionString());
            con.Close();
            con.Open();
            SqlCommand cmd = new SqlCommand("SELECT * FROM [dbo].[AppointmentDiary]", con);

            var result = new List<AppointmentDiary>();
            using (var reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    var foo = new AppointmentDiary
                    {
                        ID = Convert.ToInt32(reader["ID"]),
                        AppointmentLength = Convert.ToInt32(reader["AppointmentLength"]),
                        DateTimeScheduled = Convert.ToDateTime(reader["DateTimeScheduled"]),
                        SomeImportantKey = Convert.ToInt32(reader["SomeImportantKey"]),
                        StatusENUM = Convert.ToInt32(reader["StatusENUM"]),
                        Title = reader["Title"].ToString(),
                    };
                    result.Add(foo);
                }
            }
            return result;

        }
    

        public static List<AppointmentDiary> GetAgendas(double start, double end)
        {
            SqlConnection con = new SqlConnection(ConectionString());
            con.Close();
            con.Open();
            SqlCommand cmd = new SqlCommand("SELECT * FROM AppointmentDiary ", con);
            //cmd.Parameters.Add("@start", SqlDbType.Int).Value = start;
            //cmd.Parameters.Add("@end", SqlDbType.Int).Value = end;
            var agends = new List<AppointmentDiary>();

            using (var reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    agends.Add(new AppointmentDiary()
                    {
                        ID = Convert.ToInt32(reader["ID"]),
                        AppointmentLength = Convert.ToInt32(reader["AppointmentLength"]),
                        DateTimeScheduled = Convert.ToDateTime(reader["DateTimeScheduled"]),
                        SomeImportantKey = Convert.ToInt32(reader["SomeImportantKey"]),
                        StatusENUM = Convert.ToInt32(reader["StatusENUM"]),
                        Title = reader["Title"].ToString(),
                    });
                }
            }
            return agends;
        }

        public static void UpdateAgenda(int id, string NewEventStart, string NewEventEnd)
        {
            SqlConnection con = new SqlConnection(ConectionString());
            con.Close();
            con.Open();
            SqlCommand cmmd = new SqlCommand("UPDATE [dbo].[AppointmentDiary]   ,[DateTimeScheduled] = "+ NewEventStart + "      ,[StatusENUM] = "+ NewEventStart + " WHERE ID = "+id+"", con);
            
            cmmd.ExecuteNonQuery();
          
        }


        public static void CreateNewAgenda(string Title, string NewEventDate, string NewEventTime, string NewEventDuration)
        {

            NewEventDate.Replace('/', '-') ;
            var duração = NewEventDuration;
            var data = NewEventDate + " " + NewEventTime;

            SqlConnection con = new SqlConnection(ConectionString());
            con.Close();
            con.Open();
            SqlCommand cmd = new SqlCommand("INSERT AppointmentDiary VALUES (@Title ,1 ,@DateTimeScheduled , @AppointmentLength , @StatusENUM) ;", con );
            cmd.Parameters.Add("@Title", SqlDbType.VarChar, 255).Value = Title;
            cmd.Parameters.Add("@DateTimeScheduled", SqlDbType.DateTime).Value = data;
            cmd.Parameters.Add("@AppointmentLength", SqlDbType.Int).Value = int.Parse(duração);
            cmd.Parameters.Add("@StatusENUM", SqlDbType.Int).Value = 1;

            cmd.ExecuteNonQuery();

        }

    }
}