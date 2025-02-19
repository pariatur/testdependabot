namespace ParseTenable.PrepareData
{
	internal class ExceptionServer
	{
		/// <summary>
		/// Checks if a server has a exception
		/// </summary>
		/// <param name="ip"></param>
		/// <returns></returns>
		public static bool ServerHasException(string ip, List<DTO.ExceptionServer> list)
		{
			var exceptionServer = list.FirstOrDefault(x => x.Ip == ip);
			var exceptionServerValue = false;

			if (exceptionServer != null)
			{
				exceptionServerValue = true;
			}

			return exceptionServerValue;
		}
	}
}
