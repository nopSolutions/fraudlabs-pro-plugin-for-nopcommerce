using Newtonsoft.Json;
using Nop.Plugin.Misc.FraudLabsPro.Services;
using Nop.Web.Framework.Models;
using Nop.Web.Framework.Mvc.ModelBinding;

namespace Nop.Plugin.Misc.FraudLabsPro.Models.Order
{
    /// <summary>
    /// Represents the FraudLabs Pro order model
    /// </summary>
    public class FraudLabsProOrderModel : BaseNopEntityModel
    {
        [JsonConverter(typeof(StringConverter))]
        [NopResourceDisplayName("Plugins.Misc.FraudLabsPro.Order.IsCountryMatch")]
        public string IsCountryMatch { get; set; }

        [JsonConverter(typeof(StringConverter))]
        [NopResourceDisplayName("Plugins.Misc.FraudLabsPro.Order.IsHighRiskCountry")]
        public string IsHighRiskCountry { get; set; }

        [JsonConverter(typeof(StringConverter))]
        [NopResourceDisplayName("Plugins.Misc.FraudLabsPro.Order.DistanceInKM")]
        public string DistanceInKM { get; set; }

        [JsonConverter(typeof(StringConverter))]
        [NopResourceDisplayName("Plugins.Misc.FraudLabsPro.Order.DistanceInMile")]
        public string DistanceInMile { get; set; }

        [JsonConverter(typeof(StringConverter))]
        [NopResourceDisplayName("Plugins.Misc.FraudLabsPro.Order.IPAddress")]
        public string IPAddress { get; set; }

        [JsonConverter(typeof(StringConverter))]
        [NopResourceDisplayName("Plugins.Misc.FraudLabsPro.Order.IPCountry")]
        public string IPCountry { get; set; }

        [JsonConverter(typeof(StringConverter))]
        [NopResourceDisplayName("Plugins.Misc.FraudLabsPro.Order.IPRegion")]
        public string IPRegion { get; set; }

        [JsonConverter(typeof(StringConverter))]
        [NopResourceDisplayName("Plugins.Misc.FraudLabsPro.Order.IPCity")]
        public string IPCity { get; set; }

        [JsonConverter(typeof(StringConverter))]
        [NopResourceDisplayName("Plugins.Misc.FraudLabsPro.Order.IPContinent")]
        public string IPContinent { get; set; }

        [JsonConverter(typeof(StringConverter))]
        [NopResourceDisplayName("Plugins.Misc.FraudLabsPro.Order.IPLatitude")]
        public string IPLatitude { get; set; }

        [JsonConverter(typeof(StringConverter))]
        [NopResourceDisplayName("Plugins.Misc.FraudLabsPro.Order.IPLongtitude")]
        public string IPLongtitude { get; set; }

        [JsonConverter(typeof(StringConverter))]
        [NopResourceDisplayName("Plugins.Misc.FraudLabsPro.Order.IPTimeZone")]
        public string IPTimeZone { get; set; }

        [JsonConverter(typeof(StringConverter))]
        [NopResourceDisplayName("Plugins.Misc.FraudLabsPro.Order.IPElevation")]
        public string IPElevation { get; set; }

        [JsonConverter(typeof(StringConverter))]
        [NopResourceDisplayName("Plugins.Misc.FraudLabsPro.Order.IPDomain")]
        public string IPDomain { get; set; }

        [JsonConverter(typeof(StringConverter))]
        [NopResourceDisplayName("Plugins.Misc.FraudLabsPro.Order.IPMobileMNC")]
        public string IPMobileMNC { get; set; }

        [JsonConverter(typeof(StringConverter))]
        [NopResourceDisplayName("Plugins.Misc.FraudLabsPro.Order.IPMobileMCC")]
        public string IPMobileMCC { get; set; }

        [JsonConverter(typeof(StringConverter))]
        [NopResourceDisplayName("Plugins.Misc.FraudLabsPro.Order.IPMobileBrand")]
        public string IPMobileBrand { get; set; }

        [JsonConverter(typeof(StringConverter))]
        [NopResourceDisplayName("Plugins.Misc.FraudLabsPro.Order.IPNetSpeed")]
        public string IPNetSpeed { get; set; }

        [JsonConverter(typeof(StringConverter))]
        [NopResourceDisplayName("Plugins.Misc.FraudLabsPro.Order.IPISPName")]
        public string IPISPName { get; set; }

        [JsonConverter(typeof(StringConverter))]
        [NopResourceDisplayName("Plugins.Misc.FraudLabsPro.Order.IPUsageType")]
        public string IPUsageType { get; set; }

        [JsonConverter(typeof(StringConverter))]
        [NopResourceDisplayName("Plugins.Misc.FraudLabsPro.Order.IsFreeEmail")]
        public string IsFreeEmail { get; set; }

        [JsonConverter(typeof(StringConverter))]
        [NopResourceDisplayName("Plugins.Misc.FraudLabsPro.Order.IsDisposableEmail")]
        public string IsDisposableEmail { get; set; }

        [JsonConverter(typeof(StringConverter))]
        [NopResourceDisplayName("Plugins.Misc.FraudLabsPro.Order.IsNewDomainName")]
        public string IsNewDomainName { get; set; }

        [JsonConverter(typeof(StringConverter))]
        [NopResourceDisplayName("Plugins.Misc.FraudLabsPro.Order.IsDomainExists")]
        public string IsDomainExists { get; set; }

        [JsonConverter(typeof(StringConverter))]
        [NopResourceDisplayName("Plugins.Misc.FraudLabsPro.Order.IsProxyIPAddress")]
        public string IsProxyIPAddress { get; set; }

        [JsonConverter(typeof(StringConverter))]
        [NopResourceDisplayName("Plugins.Misc.FraudLabsPro.Order.IsBinFound")]
        public string IsBinFound { get; set; }

        [JsonConverter(typeof(StringConverter))]
        [NopResourceDisplayName("Plugins.Misc.FraudLabsPro.Order.IsBinCountryMatch")]
        public string IsBinCountryMatch { get; set; }

        [JsonConverter(typeof(StringConverter))]
        [NopResourceDisplayName("Plugins.Misc.FraudLabsPro.Order.IsBinNameMatch")]
        public string IsBinNameMatch { get; set; }

        [JsonConverter(typeof(StringConverter))]
        [NopResourceDisplayName("Plugins.Misc.FraudLabsPro.Order.IsBinPhoneMatch")]
        public string IsBinPhoneMatch { get; set; }

        [JsonConverter(typeof(StringConverter))]
        [NopResourceDisplayName("Plugins.Misc.FraudLabsPro.Order.IsBinPrepaid")]
        public string IsBinPrepaid { get; set; }

        [JsonConverter(typeof(StringConverter))]
        [NopResourceDisplayName("Plugins.Misc.FraudLabsPro.Order.IsAddressShipForward")]
        public string IsAddressShipForward { get; set; }

        [JsonConverter(typeof(StringConverter))]
        [NopResourceDisplayName("Plugins.Misc.FraudLabsPro.Order.IsBillShipCityMatch")]
        public string IsBillShipCityMatch { get; set; }

        [JsonConverter(typeof(StringConverter))]
        [NopResourceDisplayName("Plugins.Misc.FraudLabsPro.Order.IsBillShipStateMatch")]
        public string IsBillShipStateMatch { get; set; }

        [JsonConverter(typeof(StringConverter))]
        [NopResourceDisplayName("Plugins.Misc.FraudLabsPro.Order.IsBillShipCountryMatch")]
        public string IsBillShipCountryMatch { get; set; }

        [JsonConverter(typeof(StringConverter))]
        [NopResourceDisplayName("Plugins.Misc.FraudLabsPro.Order.IsBillShipPostalMatch")]
        public string IsBillShipPostalMatch { get; set; }

        [JsonConverter(typeof(StringConverter))]
        [NopResourceDisplayName("Plugins.Misc.FraudLabsPro.Order.IsIPBlacklist")]
        public string IsIPBlacklist { get; set; }

        [JsonConverter(typeof(StringConverter))]
        [NopResourceDisplayName("Plugins.Misc.FraudLabsPro.Order.IsEmailBlacklist")]
        public string IsEmailBlacklist { get; set; }

        [JsonConverter(typeof(StringConverter))]
        [NopResourceDisplayName("Plugins.Misc.FraudLabsPro.Order.IsCreditCardBlacklist")]
        public string IsCreditCardBlacklist { get; set; }

        [JsonConverter(typeof(StringConverter))]
        [NopResourceDisplayName("Plugins.Misc.FraudLabsPro.Order.IsDeviceBlacklist")]
        public string IsDeviceBlacklist { get; set; }

        [JsonConverter(typeof(StringConverter))]
        [NopResourceDisplayName("Plugins.Misc.FraudLabsPro.Order.IsUserBlacklist")]
        public string IsUserBlacklist { get; set; }

        [JsonConverter(typeof(StringConverter))]
        [NopResourceDisplayName("Plugins.Misc.FraudLabsPro.Order.IsShipAddressBlackList")]
        public string IsShipAddressBlackList { get; set; }

        [JsonConverter(typeof(StringConverter))]
        [NopResourceDisplayName("Plugins.Misc.FraudLabsPro.Order.IsPhoneBlacklist")]
        public string IsPhoneBlacklist { get; set; }

        [JsonConverter(typeof(StringConverter))]
        [NopResourceDisplayName("Plugins.Misc.FraudLabsPro.Order.IsHighRiskUsernamePassword")]
        public string IsHighRiskUsernamePassword { get; set; }

        [JsonConverter(typeof(StringConverter))]
        [NopResourceDisplayName("Plugins.Misc.FraudLabsPro.Order.IsMalwareExploit")]
        public string IsMalwareExploit { get; set; }

        [JsonConverter(typeof(StringConverter))]
        [NopResourceDisplayName("Plugins.Misc.FraudLabsPro.Order.IsExportControlledCountry")]
        public string IsExportControlledCountry { get; set; }

        [JsonConverter(typeof(StringConverter))]
        [NopResourceDisplayName("Plugins.Misc.FraudLabsPro.Order.UserOrderID")]
        public string UserOrderID { get; set; }

        [JsonConverter(typeof(StringConverter))]
        [NopResourceDisplayName("Plugins.Misc.FraudLabsPro.Order.UserOrderMemo")]
        public string UserOrderMemo { get; set; }

        [JsonConverter(typeof(StringConverter))]
        [NopResourceDisplayName("Plugins.Misc.FraudLabsPro.Order.FraudLabsProScore")]
        public string FraudLabsProScore { get; set; }

        [JsonConverter(typeof(StringConverter))]
        [NopResourceDisplayName("Plugins.Misc.FraudLabsPro.Order.FraudLabsProDistribution")]
        public string FraudLabsProDistribution { get; set; }

        [JsonConverter(typeof(StringConverter))]
        [NopResourceDisplayName("Plugins.Misc.FraudLabsPro.Order.FraudLabsProStatus")]
        public string FraudLabsProStatus { get; set; }

        [JsonConverter(typeof(StringConverter))]
        [NopResourceDisplayName("Plugins.Misc.FraudLabsPro.Order.FraudLabsProID")]
        public string FraudLabsProID { get; set; }

        [JsonConverter(typeof(StringConverter))]
        [NopResourceDisplayName("Plugins.Misc.FraudLabsPro.Order.FraudLabsProVersion")]
        public string FraudLabsProVersion { get; set; }

        [JsonConverter(typeof(StringConverter))]
        [NopResourceDisplayName("Plugins.Misc.FraudLabsPro.Order.FraudLabsProErrorCode")]
        public string FraudLabsProErrorCode { get; set; }

        [JsonConverter(typeof(StringConverter))]
        [NopResourceDisplayName("Plugins.Misc.FraudLabsPro.Order.FraudLabsProMessage")]
        public string FraudLabsProMessage { get; set; }

        [JsonConverter(typeof(StringConverter))]
        [NopResourceDisplayName("Plugins.Misc.FraudLabsPro.Order.FraudLabsProCredit")]
        public string FraudLabsProCredit { get; set; }

        public string FraudLabsProOriginalStatus { get; set; }
    }
}