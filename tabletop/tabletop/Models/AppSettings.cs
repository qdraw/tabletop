using System;
using System.IO;

namespace tabletop.Models
{
	public class AppSettings
	{
		public enum DatabaseTypeList
		{
			Mysql = 1,
			Sqlite = 2,
			InMemoryDatabase = 3,
			SqlServer = 4
		}

		/// <summary>
		/// Replaces a SQLite url with a full directory path in the connection string
		/// </summary>
		/// <param name="databaseType"></param>
		/// <param name="connectionString">SQLite</param>
		/// <param name="baseDirectoryProject">path</param>
		/// <returns></returns>
		/// <exception cref="ArgumentException">The 'DatabaseConnection' field is null or empty or missing Data Source in connection string</exception>
		public string SqLiteFullPath(DatabaseTypeList databaseType, string connectionString, string baseDirectoryProject)
		{
			if ((databaseType == DatabaseTypeList.Mysql || databaseType == DatabaseTypeList.SqlServer) && string.IsNullOrWhiteSpace(connectionString)) 
				throw  new ArgumentException("The 'DatabaseConnection' field is null or empty");

			if(databaseType != DatabaseTypeList.Sqlite) return connectionString; // mysql does not need this

			if(!connectionString.Contains("Data Source=")) throw 
				new ArgumentException("missing Data Source in connection string");

			var databaseFileName = connectionString.Replace("Data Source=", "");
            
			// Check if path is not absolute already
			if (databaseFileName.Contains("/") || databaseFileName.Contains("\\")) return connectionString;

			// Return if running in Microsoft.EntityFrameworkCore.Sqlite (location is now root folder)
			if(baseDirectoryProject.Contains("entityframeworkcore")) return connectionString;

			var dataSource = "Data Source=" + baseDirectoryProject + 
			                 Path.DirectorySeparatorChar+  databaseFileName;
			return dataSource;
		}
	}
}
