using ParseTenable.DTO;

namespace ParseTenable.PrepareData
{
    internal static class ReplaceOS
    {
        /// <summary>
        /// Replaces OS through all the posibilities
        /// </summary>
        /// <param name="assetName"></param>
        /// <param name="operatingSystem"></param>
        /// <returns></returns>
        public static string OS(string assetName, string operatingSystem, List<OSReplaceByAD> replaceByAD, List<OSRealVersion> realVersion, List<OSManualnput> manualInput)
        {
            var value = ReplaceOSByAD(assetName, operatingSystem, replaceByAD);
            value = ReplaceOSByName(value, realVersion);
            value = ReplaceOSByManualInput(assetName, value, manualInput);

            return value;
        }

        /// <summary>		
        /// Replace OS in case it is unidentified by Tenable
        /// </summary>
        /// <param name="assetName"></param>
        /// <param name="operatingSystem"></param>
        /// <returns></returns>
        private static string ReplaceOSByAD(string assetName, string operatingSystem, List<OSReplaceByAD> replaceByAD)
        {
            var replace = replaceByAD.FirstOrDefault(x => x.Name == assetName);

            if (replace != null)
            {
                return replace.OperatingSystem + " " + replace.OperatingSystemVersion;
            }

            return operatingSystem;
        }

        /// <summary>
        /// Replace OS identified by Tenable to real OS
        /// </summary>
        /// <param name="operatingSystem"></param>
        /// <returns></returns>
        private static string ReplaceOSByName(string operatingSystem, List<OSRealVersion> realVersion)
        {
            var replace = realVersion.FirstOrDefault(x => x.OperatingSystem == operatingSystem);

            if (replace != null)
            {
                return replace.OperatingSystemReal;
            }

            return operatingSystem;
        }

        /// <summary>
        /// Replace OS in servers through manual input
        /// </summary>
        /// <param name="assetName"></param>
        /// <param name="operatingSystem"></param>
        /// <returns></returns>
        private static string ReplaceOSByManualInput(string assetName, string operatingSystem, List<OSManualnput> manualInput)
        {
            var replace = manualInput.FirstOrDefault(x => x.Name == assetName);

            if (replace != null)
            {
                return replace.OperatingSystem;
            }

            return operatingSystem;
        }
    }
}
