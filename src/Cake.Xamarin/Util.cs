using System;
using System.Xml;

namespace Cake.Xamarin
{
	public static class Util
	{
		public const string XAMARIN_ANDROID_PROJECT_TYPE_GUID = "{EFBA0AD7-5A72-4C68-AF49-83D382785DCF}";
		public const string XAMARIN_IOS_PROJECT_TYPE_GUID = "{FEACFBD2-3405-455C-9665-78FE426C6842}";

		public static bool IsProjectOfType (string csproj, string projectTypeGuid)
		{
			var xml = new XmlDocument();
			xml.Load(csproj);
			return xml.SelectNodes("/Project/PropertyGroup/ProjectTypeGuids[contains(@text,'" + projectTypeGuid + "')]")?.Count > 0;
		}
	}
}
