namespace ParseTenable.DTO
{
    internal class TenableJSON
    {
        public string output { get; set; }
        public string id { get; set; }
        public Asset asset { get; set; }
        public Definition definition { get; set; }
        public AssetCloudResource asset_cloud_resource { get; set; }
        public ContainerImage container_image { get; set; }
        public int severity { get; set; }
        public string state { get; set; }
        public DateTime first_observed { get; set; }
        public DateTime last_seen { get; set; }
        public string risk_modified { get; set; }
        public string protocol { get; set; }
        public int port { get; set; }
        public Scan scan { get; set; }
        public int age_in_days { get; set; }

        public class Asset
        {
            public string id { get; set; }
            public string name { get; set; }
            public List<object> tags { get; set; }
            public string display_ipv4_address { get; set; }
            public string display_ipv6_address { get; set; }
            public string display_fqdn { get; set; }
            public string host_name { get; set; }
            public string operating_system { get; set; }
            public string system_type { get; set; }
            public string display_mac_address { get; set; }
            public Network network { get; set; }
        }

        public class AssetCloudResource
        {
            public string id { get; set; }
            public string name { get; set; }
        }

        public class Cisa
        {
        }

        public class ContainerImage
        {
            public string id { get; set; }
            public string name { get; set; }
        }

        public class Cvss2
        {
            public double base_score { get; set; }
            public string base_vector { get; set; }
            public double temporal_score { get; set; }
            public string temporal_vector { get; set; }
        }

        public class Cvss3
        {
            public double? base_score { get; set; }
            public string base_vector { get; set; }
            public double? temporal_score { get; set; }
            public string temporal_vector { get; set; }
        }

        public class Definition
        {
            public int id { get; set; }
            public string name { get; set; }
            public string description { get; set; }
            public string solution { get; set; }
            public string synopsis { get; set; }
            public List<string> see_also { get; set; }
            public string family { get; set; }
            public int severity { get; set; }
            public List<string> cpe { get; set; }
            public string exploitability_ease { get; set; }
            public bool default_account { get; set; }
            public bool exploited_by_malware { get; set; }
            public bool exploited_by_nessus { get; set; }
            public bool unsupported_by_vendor { get; set; }
            public DateTime plugin_published { get; set; }
            public DateTime plugin_updated { get; set; }
            public DateTime vulnerability_published { get; set; }
            public DateTime patch_published { get; set; }
            public Vpr vpr { get; set; }
            public Cvss2 cvss2 { get; set; }
            public Cvss3 cvss3 { get; set; }
            public string type { get; set; }
            public string output { get; set; }
            public bool in_the_news { get; set; }
            public bool malware { get; set; }
            public double plugin_version { get; set; }
            public bool asset_inventory { get; set; }
            public bool inventory_live_scan { get; set; }
            public List<Reference> references { get; set; }
            public List<string> cve { get; set; }
            public List<string> cwe { get; set; }
            public List<string> bugtraq { get; set; }
            public List<object> exploit_frameworks { get; set; }
            public Cisa cisa { get; set; }
            public string stig_severity { get; set; }
            public string iavm { get; set; }
            public List<string> iava { get; set; }
            public List<string> iavb { get; set; }
        }

        public class Network
        {
            public string id { get; set; }
            public string name { get; set; }
        }

        public class Reference
        {
            public string type { get; set; }
            public List<string> ids { get; set; }
        }

        public class Scan
        {
            public string id { get; set; }
        }

        public class Vpr
        {
            public double score { get; set; }
            public int drivers_age_of_vulns_low { get; set; }
            public string drivers_exploit_code_maturity { get; set; }
            public double drivers_cvss3_impact_score { get; set; }
            public string drivers_threat_intensity { get; set; }
            public List<string> drivers_threat_sources { get; set; }
            public string drivers_product_coverage { get; set; }
            public int? drivers_threat_recency_low { get; set; }
            public int? drivers_threat_recency_high { get; set; }
        }
    }
}
