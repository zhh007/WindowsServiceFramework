using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Service.Core.Log.Configuration;

namespace Service.Core.Log
{
	public class SQLLogger : ILogger
	{
		public static void ExceptionToSQL(string exception, string exceptionType, SQLLoggerConfiguration settings)
		{
			if (settings.SystemId == LoggerConfigurationBase.DefaultSystemId)
			{
				return;
			}

			//Instantiate a new connection/command
			SqlConnection conn = new SqlConnection(settings.ConnectionString);
			SqlCommand commandIssue = new SqlCommand("InsertIntoIssueTable", conn);
			commandIssue.CommandType = CommandType.StoredProcedure;
			commandIssue.Parameters.Add("@SystemID", SqlDbType.Int).Value = settings.SystemId;
			commandIssue.Parameters.Add("@LocationID", SqlDbType.Int).Value = settings.LocationId;
			commandIssue.Parameters.Add("@ApplicationID", SqlDbType.Int).Value = settings.ApplicationId;
			commandIssue.Parameters.Add("@Summary", SqlDbType.VarChar).Value = exceptionType;
			commandIssue.Parameters.Add("@ReportedBy", SqlDbType.VarChar).Value = settings.ReportedBy;
			commandIssue.Parameters.Add("@ReturnIssueID", SqlDbType.Int).Direction = ParameterDirection.ReturnValue;

			try
			{
				// Open the connection
				conn.Open();

				//Call ExecuteNonQuery to send commands
				commandIssue.ExecuteNonQuery();
				int IssueID = (int)commandIssue.Parameters["@ReturnIssueID"].Value;

				SqlCommand commandNotes = new SqlCommand("InsertIntoNotesTable", conn);
				commandNotes.CommandType = CommandType.StoredProcedure;
				commandNotes.Parameters.Add("@SystemID", SqlDbType.Int).Value = settings.SystemId;
				commandNotes.Parameters.Add("@LocationID", SqlDbType.Int).Value = settings.LocationId;
				commandNotes.Parameters.Add("@ApplicationID", SqlDbType.Int).Value = settings.ApplicationId;
				commandNotes.Parameters.Add("@IssueID", SqlDbType.Int).Value = IssueID;
				commandNotes.Parameters.Add("@Symptom", SqlDbType.VarChar).Value = exceptionType;
				commandNotes.Parameters.Add("@Problem", SqlDbType.VarChar).Value = exception;
				commandNotes.Parameters.Add("@CreatedBy", SqlDbType.VarChar).Value = settings.ReportedBy;

				commandNotes.ExecuteNonQuery();
			}
			catch
			{
				throw;
			}
			finally
			{
				// Close the connection
				if (conn != null)
				{
					conn.Close();
				}
			}
		}

		public List<LogRecord> Parse()
		{
			return null;
		}

		public List<LogRecord> Parse(LogLevelEnum logLevel)
		{
			return null;
		}
	}
}