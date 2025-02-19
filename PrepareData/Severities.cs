using ParseTenable.DTO;

namespace ParseTenable.PrepareData
{
    internal static class Severities
    {
        /// <summary>
        /// Gets vulnerabilities by severity from a certain asset
        /// TODO: This is highly inefficient, but it works
        /// </summary>
        /// <param name="assetName"></param>
        /// <param name="severity"></param>
        /// <returns></returns>
        public static int GetVulnerabilitiesBySeverity(string assetName, Severity severity, List<TenableJSON> vulnerabilities)
        {
            var bajo = 0;
            var medio = 0;
            var alto = 0;
            var crítico = 0;
            foreach (var item in vulnerabilities)
            {
                if (assetName != item.asset.name)
                {
                    continue;
                }

                var vulnerability = (Severity)Convert.ToInt32(item.severity);

                switch (vulnerability)
                {
                    case Severity.Bajo:
                        bajo++;
                        break;
                    case Severity.Medio:
                        medio++;
                        break;
                    case Severity.Alto:
                        alto++;
                        break;
                    case Severity.Crítico:
                        crítico++;
                        break;
                    default:
                        break;
                }
            }
            switch (severity)
            {
                case Severity.Bajo:
                    return bajo;
                case Severity.Medio:
                    return medio;
                case Severity.Alto:
                    return alto;
                case Severity.Crítico:
                    return crítico;
                default:
                    return -1;
            }
        }

        /// <summary>
        /// From a severity, gets the name
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string SeverityNumberToString(Severity value)
        {
            switch (value)
            {
                case Severity.Bajo:
                    return "Bajo";
                case Severity.Medio:
                    return "Medio";
                case Severity.Alto:
                    return "Alto";
                case Severity.Crítico:
                    return "Crítico";
                default:
                    return "";
            }
        }

        /// <summary>
        /// Risk severity
        /// </summary>
        public enum Severity
        {
            Bajo = 1,
            Medio = 2,
            Alto = 3,
            Crítico = 4
        }
    }
}
