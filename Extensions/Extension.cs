namespace ParseTenable.Extensions
{
	internal static class Extension
	{
		public static string ToSN(this bool value)
		{
			return value ? "Sí" : "No";
		}
	}
}
