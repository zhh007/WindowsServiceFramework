using System;
using System.Data;
using System.Data.SQLite;
using System.Diagnostics;
using Service.Core.Log;
using Service.Core.Log.Configuration;
using Service.Core.StatusDatabase.Utility;

namespace Service.Core.StatusDatabase
{
	internal class SQLite
	{
		public static SQLiteConnection InitializeConnection(string databasePath, FileLoggerConfiguration loggerConfiguration)
		{
			SQLiteConnectionStringBuilder sqliteConnectionStringBuilder = new SQLiteConnectionStringBuilder
																						{
																							DataSource = databasePath,
																							PageSize = 1024,
																							Pooling = true
																						};
			Logging.Log(LogLevelEnum.Debug, "SQLite Connection string initialized", loggerConfiguration);
			Logging.Log(LogLevelEnum.Debug, string.Format("Connection string: {0}", sqliteConnectionStringBuilder.ConnectionString), loggerConfiguration);

			return new SQLiteConnection(sqliteConnectionStringBuilder.ConnectionString);
		}

		public static void ExecuteNonQuery(string sqlStatement, SQLiteConnection sqliteConnection, FileLoggerConfiguration loggerConfiguration)
		{
			try
			{
				using (SQLiteCommand sqliteCommand = new SQLiteCommand(sqlStatement, sqliteConnection))
				{
					sqliteCommand.CommandType = CommandType.Text;
					Logging.Log(LogLevelEnum.Debug, string.Format("SQL statement:\n\n{0}\n\n", sqlStatement), loggerConfiguration);
					sqliteCommand.ExecuteNonQuery();
				}
			}
			catch (Exception ex)
			{
				StackTrace stackTrace = new StackTrace();
				Logging.Log(LogLevelEnum.Error, string.Format("{0}: {1}", stackTrace.GetFrame(1).GetMethod().Name, FileLogger.GetInnerException(ex).Message), loggerConfiguration);
				Logging.HandleException(ex);
			}
		}

		public static void ExecuteNonQuery(SQLiteCommand sqliteCommand, SQLiteConnection sqliteConnection, FileLoggerConfiguration loggerConfiguration)
		{
			try
			{
				using (sqliteCommand)
				{
					sqliteCommand.Connection = sqliteConnection;
					ExecuteNonQuery(sqliteCommand, loggerConfiguration);
				}
			}
			catch (Exception ex)
			{
				StackTrace stackTrace = new StackTrace();
				Logging.Log(LogLevelEnum.Error, string.Format("{0}: {1}", stackTrace.GetFrame(1).GetMethod().Name, FileLogger.GetInnerException(ex).Message), loggerConfiguration);
				Logging.HandleException(ex);
			}
		}

		public static void ExecuteNonQuery(SQLiteCommand sqliteCommand, FileLoggerConfiguration loggerConfiguration)
		{
			try
			{
				using (sqliteCommand)
				{
					sqliteCommand.CommandType = CommandType.Text;
					Logging.Log(LogLevelEnum.Debug, string.Format("SQL statement:\n\n{0}\n\n", sqliteCommand.CommandText), loggerConfiguration);
					sqliteCommand.ExecuteNonQuery();
				}
			}
			catch (Exception ex)
			{
				StackTrace stackTrace = new StackTrace();
				Logging.Log(LogLevelEnum.Error, string.Format("{0}: {1}", stackTrace.GetFrame(1).GetMethod().Name, FileLogger.GetInnerException(ex).Message), loggerConfiguration);
				Logging.HandleException(ex);
			}
		}

		public static SQLiteDataReader ExecuteReader(string sqlStatement, SQLiteConnection sqliteConnection, FileLoggerConfiguration loggerConfiguration)
		{
			SQLiteDataReader reader = null;
			try
			{
				using (SQLiteCommand cmd = new SQLiteCommand(sqlStatement, sqliteConnection))
				{
					cmd.CommandType = CommandType.Text;
					Logging.Log(LogLevelEnum.Debug, string.Format("SQL statement:\n\n{0}\n\n", sqlStatement), loggerConfiguration);
					reader = cmd.ExecuteReader();
				}
			}
			catch (Exception ex)
			{
				StackTrace stackTrace = new StackTrace();
				Logging.Log(LogLevelEnum.Error, string.Format("{0}: {1}", stackTrace.GetFrame(1).GetMethod().Name, FileLogger.GetInnerException(ex).Message), loggerConfiguration);
				Logging.HandleException(ex);
			}
			return reader;
		}

		public static object ExecuteScalar(string sqlStatement, SQLiteConnection sqliteConnection, FileLoggerConfiguration loggerConfiguration)
		{
			object returnObject = null;
			try
			{
				using (SQLiteCommand cmd = new SQLiteCommand(sqlStatement, sqliteConnection))
				{
					cmd.CommandType = CommandType.Text;
					Logging.Log(LogLevelEnum.Debug, string.Format("SQL statement:\n\n{0}\n\n", sqlStatement), loggerConfiguration);
					returnObject = cmd.ExecuteScalar();
				}
			}
			catch (Exception ex)
			{
				StackTrace stackTrace = new StackTrace();
				Logging.Log(LogLevelEnum.Error, string.Format("{0}: {1}", stackTrace.GetFrame(1).GetMethod().Name, FileLogger.GetInnerException(ex).Message), loggerConfiguration);
				Logging.HandleException(ex);
			}

			return returnObject;
		}
	}
}