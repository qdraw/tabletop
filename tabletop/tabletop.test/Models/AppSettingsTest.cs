using System;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using tabletop.Models;

namespace tabletop.tests.Models
{
	[TestClass]
	public class AppSettingsTest
	{
		private readonly AppSettings _appSettings;

		public AppSettingsTest()
		{
			_appSettings = new AppSettings();
		}
		[TestMethod]
		public void AppSettingsProviderTest_SqLiteFullPathTest()
		{
			var dataSource = _appSettings.SqLiteFullPath(AppSettings.DatabaseTypeList.Sqlite, "Data Source=data.db", string.Empty);
			Assert.AreEqual(true, dataSource.Contains("data.db") );
			Assert.AreEqual(true, dataSource.Contains("Data Source="));
		}


		[TestMethod]
		public void AppSettingsProviderTest_SqLiteFullPathStarskyCliTest()
		{
			var datasource = _appSettings.SqLiteFullPath(AppSettings.DatabaseTypeList.Sqlite, 
				"Data Source=data.db", Path.DirectorySeparatorChar + "starsky");
			Assert.AreEqual(true, datasource.Contains("data.db"));
			Assert.AreEqual(true, datasource.Contains("Data Source="));
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentException))]
		public void AppSettingsProviderTest_SQLite_ExpectException()
		{
			var datasource = _appSettings.SqLiteFullPath(AppSettings.DatabaseTypeList.Sqlite, string.Empty, null);
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentException))]
		public void AppSettingsProviderTest_MySQL_ExpectException()
		{
			_appSettings.SqLiteFullPath(AppSettings.DatabaseTypeList.Mysql, string.Empty, null);
		}
	}
}
