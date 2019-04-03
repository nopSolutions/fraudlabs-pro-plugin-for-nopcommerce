using System.Collections.Generic;
using System.Linq;
using Nop.Core;
using Nop.Core.Domain.Orders;
using Nop.Core.Plugins;
using Nop.Data.Extensions;
using Nop.Services;
using Nop.Services.Cms;
using Nop.Services.Common;
using Nop.Services.Configuration;
using Nop.Services.Localization;
using Nop.Services.Orders;
using Nop.Web.Framework.Infrastructure;

namespace Nop.Plugin.Misc.FraudLabsPro
{
    /// <summary>
    /// Represents the FraudLabsPro plugin
    /// </summary>
    public class FraudLabsProPlugin : BasePlugin, IMiscPlugin, IWidgetPlugin
    {

        #region Fields

        private readonly IGenericAttributeService _genericAttributeService;
        private readonly ILocalizationService _localizationService;
        private readonly IOrderService _orderService;
        private readonly ISettingService _settingService;
        private readonly IWebHelper _webHelper;

        #endregion

        #region Ctor

        public FraudLabsProPlugin(
            IGenericAttributeService genericAttributeService,
            ILocalizationService localizationService,
            IOrderService orderService,
            ISettingService settingService,
            IWebHelper webHelper
            )
        {
            _genericAttributeService = genericAttributeService;
            _localizationService = localizationService;
            _orderService = orderService;
            _settingService = settingService;
            _webHelper = webHelper;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Gets widget zones where this widget should be rendered
        /// </summary>
        /// <returns>Widget zones</returns>
        public IList<string> GetWidgetZones()
        {
            return new List<string> { PublicWidgetZones.OpcContentAfter, PublicWidgetZones.CheckoutConfirmBottom };
        }

        /// <summary>
        /// Gets a name of a view component for displaying widget
        /// </summary>
        /// <param name="widgetZone">Name of the widget zone</param>
        /// <returns>View component name</returns>
        public string GetWidgetViewComponentName(string widgetZone)
        {
            return FraudLabsProDefaults.ViewComponentName;
        }

        /// <summary>
        /// Gets a configuration page URL
        /// </summary>
        public override string GetConfigurationPageUrl()
        {
            return $"{_webHelper.GetStoreLocation()}Admin/FraudLabsPro/Configure";
        }

        /// <summary>
        /// Install the plugin
        /// </summary>
        public override void Install()
        {
            //settings
            _settingService.SaveSetting(new FraudLabsProSettings() {
                ApproveStatusID = (int)OrderStatus.Processing,
                ReviewStatusID = (int)OrderStatus.Pending,
                RejectStatusID = (int)OrderStatus.Cancelled
            });

            //locales
            _localizationService.AddOrUpdatePluginLocaleResource("Plugins.Misc.FraudLabsPro.Fields.ApiKey", "API key");
            _localizationService.AddOrUpdatePluginLocaleResource("Plugins.Misc.FraudLabsPro.Fields.ApiKey.Hint", "Input your FraudLabs Pro account API key.");
            _localizationService.AddOrUpdatePluginLocaleResource("Plugins.Misc.FraudLabsPro.Fields.ApproveStatusID", "Approve Status");
            _localizationService.AddOrUpdatePluginLocaleResource("Plugins.Misc.FraudLabsPro.Fields.ApproveStatusID.Hint", "Change order status when order has been approved by FraudLabs Pro, or FraudLabs Pro Approve button has been pressed in order details page.");
            _localizationService.AddOrUpdatePluginLocaleResource("Plugins.Misc.FraudLabsPro.Fields.Balance", "Balance");
            _localizationService.AddOrUpdatePluginLocaleResource("Plugins.Misc.FraudLabsPro.Fields.Balance.Hint", "Balance of queries in your account after last transaction.");
            _localizationService.AddOrUpdatePluginLocaleResource("Plugins.Misc.FraudLabsPro.Fields.Balance.Unknown", "Balance will be available after the first scan of the order");
            _localizationService.AddOrUpdatePluginLocaleResource("Plugins.Misc.FraudLabsPro.Fields.Balance.Upgrade", "Upgrade");
            _localizationService.AddOrUpdatePluginLocaleResource("Plugins.Misc.FraudLabsPro.Fields.ReviewStatusID", "Review Status");
            _localizationService.AddOrUpdatePluginLocaleResource("Plugins.Misc.FraudLabsPro.Fields.ReviewStatusID.Hint", "Change order status when order has been marked as REVIEW by FraudLabs Pro.");
            _localizationService.AddOrUpdatePluginLocaleResource("Plugins.Misc.FraudLabsPro.Fields.RejectStatusID", "Reject Status");
            _localizationService.AddOrUpdatePluginLocaleResource("Plugins.Misc.FraudLabsPro.Fields.RejectStatusID.Hint", "Change order status when order has been reject by FraudLabs Pro, or FraudLabs Pro Reject button has been pressed in order details page.");
            //Order
            _localizationService.AddOrUpdatePluginLocaleResource("Plugins.Misc.FraudLabsPro.Order.Instructions", "<p>Please login to <a href='{0}' target='_blank'>FraudLabs Pro Merchant Area</a> for more information about this order.</p>");
            _localizationService.AddOrUpdatePluginLocaleResource("Plugins.Misc.FraudLabsPro.Order.Screen", "Screen Order");
            _localizationService.AddOrUpdatePluginLocaleResource("Plugins.Misc.FraudLabsPro.Order.Approve", "Approve");
            _localizationService.AddOrUpdatePluginLocaleResource("Plugins.Misc.FraudLabsPro.Order.BlackList", "BlackList");
            _localizationService.AddOrUpdatePluginLocaleResource("Plugins.Misc.FraudLabsPro.Order.Reject", "Reject");

            _localizationService.AddOrUpdatePluginLocaleResource("Plugins.Misc.FraudLabsPro.Order", "FraudLabs Pro Details");
            _localizationService.AddOrUpdatePluginLocaleResource("Plugins.Misc.FraudLabsPro.Order.IsCountryMatch", "Country match");
            _localizationService.AddOrUpdatePluginLocaleResource("Plugins.Misc.FraudLabsPro.Order.IsCountryMatch.Hint", "Whether country of IP address matches billing address country.");
            _localizationService.AddOrUpdatePluginLocaleResource("Plugins.Misc.FraudLabsPro.Order.IsHighRiskCountry", "High risk country");
            _localizationService.AddOrUpdatePluginLocaleResource("Plugins.Misc.FraudLabsPro.Order.IsHighRiskCountry.Hint", "Whether IP address or billing address country is in the latest high risk list.");
            _localizationService.AddOrUpdatePluginLocaleResource("Plugins.Misc.FraudLabsPro.Order.DistanceInKM", "IP Distance");
            _localizationService.AddOrUpdatePluginLocaleResource("Plugins.Misc.FraudLabsPro.Order.DistanceInKM.Hint", "Distance of location between IP address and bill.");
            _localizationService.AddOrUpdatePluginLocaleResource("Plugins.Misc.FraudLabsPro.Order.DistanceInMile", "IP Distance in Mile");
            _localizationService.AddOrUpdatePluginLocaleResource("Plugins.Misc.FraudLabsPro.Order.DistanceInMile.Hint", "Distance of location between IP address and bill. Value in mile.");
            _localizationService.AddOrUpdatePluginLocaleResource("Plugins.Misc.FraudLabsPro.Order.IPAddress", "IP Address");
            _localizationService.AddOrUpdatePluginLocaleResource("Plugins.Misc.FraudLabsPro.Order.IPAddress.Hint", "Estimated of the IP address.");
            _localizationService.AddOrUpdatePluginLocaleResource("Plugins.Misc.FraudLabsPro.Order.IPCountry", "IP Country");
            _localizationService.AddOrUpdatePluginLocaleResource("Plugins.Misc.FraudLabsPro.Order.IPCountry.Hint", "Estimated country of the IP address.");
            _localizationService.AddOrUpdatePluginLocaleResource("Plugins.Misc.FraudLabsPro.Order.IPRegion", "IP Region");
            _localizationService.AddOrUpdatePluginLocaleResource("Plugins.Misc.FraudLabsPro.Order.IPRegion.Hint", "Estimated region of the IP address.");
            _localizationService.AddOrUpdatePluginLocaleResource("Plugins.Misc.FraudLabsPro.Order.IPCity", "IP City");
            _localizationService.AddOrUpdatePluginLocaleResource("Plugins.Misc.FraudLabsPro.Order.IPCity.Hint", "Estimated city of the IP address.");
            _localizationService.AddOrUpdatePluginLocaleResource("Plugins.Misc.FraudLabsPro.Order.IPContinent", "IP Location");
            _localizationService.AddOrUpdatePluginLocaleResource("Plugins.Misc.FraudLabsPro.Order.IPContinent.Hint", "Estimated of the IP address.");
            _localizationService.AddOrUpdatePluginLocaleResource("Plugins.Misc.FraudLabsPro.Order.IPLatitude", "IP Latitude");
            _localizationService.AddOrUpdatePluginLocaleResource("Plugins.Misc.FraudLabsPro.Order.IPLatitude.Hint", "Estimated latitude of the IP address.");
            _localizationService.AddOrUpdatePluginLocaleResource("Plugins.Misc.FraudLabsPro.Order.IPLongtitude", "IP Longtitude");
            _localizationService.AddOrUpdatePluginLocaleResource("Plugins.Misc.FraudLabsPro.Order.IPLongtitude.Hint", "Estimated longitude of the IP address.");
            _localizationService.AddOrUpdatePluginLocaleResource("Plugins.Misc.FraudLabsPro.Order.IPTimeZone", "IP Time Zone");
            _localizationService.AddOrUpdatePluginLocaleResource("Plugins.Misc.FraudLabsPro.Order.IPTimeZone.Hint", "Estimated timezone of the IP address.");
            _localizationService.AddOrUpdatePluginLocaleResource("Plugins.Misc.FraudLabsPro.Order.IPElevation", "IP Elevation");
            _localizationService.AddOrUpdatePluginLocaleResource("Plugins.Misc.FraudLabsPro.Order.IPElevation.Hint", "Estimated elevation of the IP address.");
            _localizationService.AddOrUpdatePluginLocaleResource("Plugins.Misc.FraudLabsPro.Order.IPDomain", "IP Domain");
            _localizationService.AddOrUpdatePluginLocaleResource("Plugins.Misc.FraudLabsPro.Order.IPDomain.Hint", "Estimated domain name of the IP address.");
            _localizationService.AddOrUpdatePluginLocaleResource("Plugins.Misc.FraudLabsPro.Order.IPMobileMNC", "IP Mobile MNC");
            _localizationService.AddOrUpdatePluginLocaleResource("Plugins.Misc.FraudLabsPro.Order.IPMobileMNC.Hint", "Estimated mobile mnc information of the IP address, if it is a mobile network.");
            _localizationService.AddOrUpdatePluginLocaleResource("Plugins.Misc.FraudLabsPro.Order.IPMobileMCC", "IP Mobile MCC");
            _localizationService.AddOrUpdatePluginLocaleResource("Plugins.Misc.FraudLabsPro.Order.IPMobileMCC.Hint", "Estimated mobile mcc information of the IP address, if it is a mobile network.");
            _localizationService.AddOrUpdatePluginLocaleResource("Plugins.Misc.FraudLabsPro.Order.IPMobileBrand", "IP Mobile brand");
            _localizationService.AddOrUpdatePluginLocaleResource("Plugins.Misc.FraudLabsPro.Order.IPMobileBrand.Hint", "Estimated mobile brand information of the IP address, if it is a mobile network.");
            _localizationService.AddOrUpdatePluginLocaleResource("Plugins.Misc.FraudLabsPro.Order.IPNetSpeed", "IP Net Speed");
            _localizationService.AddOrUpdatePluginLocaleResource("Plugins.Misc.FraudLabsPro.Order.IPNetSpeed.Hint", "Estimated netspeed of the IP address.");
            _localizationService.AddOrUpdatePluginLocaleResource("Plugins.Misc.FraudLabsPro.Order.IPISPName", "IP ISP Name");
            _localizationService.AddOrUpdatePluginLocaleResource("Plugins.Misc.FraudLabsPro.Order.IPISPName.Hint", "Estimated ISP name of the IP address.");
            _localizationService.AddOrUpdatePluginLocaleResource("Plugins.Misc.FraudLabsPro.Order.IPUsageType", "IP Usage Type");
            _localizationService.AddOrUpdatePluginLocaleResource("Plugins.Misc.FraudLabsPro.Order.IPUsageType.Hint", "Estimated usage type of the IP address.");
            _localizationService.AddOrUpdatePluginLocaleResource("Plugins.Misc.FraudLabsPro.Order.IsFreeEmail", "Free Email");
            _localizationService.AddOrUpdatePluginLocaleResource("Plugins.Misc.FraudLabsPro.Order.IsFreeEmail.Hint", "Whether the email is from free email provider.");
            _localizationService.AddOrUpdatePluginLocaleResource("Plugins.Misc.FraudLabsPro.Order.IsDisposableEmail", "Disposable Email");
            _localizationService.AddOrUpdatePluginLocaleResource("Plugins.Misc.FraudLabsPro.Order.IsDisposableEmail.Hint", "Whether the email is a disposable email.");
            _localizationService.AddOrUpdatePluginLocaleResource("Plugins.Misc.FraudLabsPro.Order.IsNewDomainName", "New Domain Name");
            _localizationService.AddOrUpdatePluginLocaleResource("Plugins.Misc.FraudLabsPro.Order.IsNewDomainName.Hint", "Whether the email domain name a newly registered name. Only applicable for non-free email domain.");
            _localizationService.AddOrUpdatePluginLocaleResource("Plugins.Misc.FraudLabsPro.Order.IsDomainExists", "Domain Exists");
            _localizationService.AddOrUpdatePluginLocaleResource("Plugins.Misc.FraudLabsPro.Order.IsDomainExists.Hint", "Whether the email domain name is a valid domain.");
            _localizationService.AddOrUpdatePluginLocaleResource("Plugins.Misc.FraudLabsPro.Order.IsProxyIPAddress", "Using Proxy");
            _localizationService.AddOrUpdatePluginLocaleResource("Plugins.Misc.FraudLabsPro.Order.IsProxyIPAddress.Hint", "Whether the IP address is from a known anonymous proxy server.");
            _localizationService.AddOrUpdatePluginLocaleResource("Plugins.Misc.FraudLabsPro.Order.IsBinFound", "Bin Found");
            _localizationService.AddOrUpdatePluginLocaleResource("Plugins.Misc.FraudLabsPro.Order.IsBinFound.Hint", "Whether the BIN information matches our BIN list.");
            _localizationService.AddOrUpdatePluginLocaleResource("Plugins.Misc.FraudLabsPro.Order.IsBinCountryMatch", "Bin Country Match");
            _localizationService.AddOrUpdatePluginLocaleResource("Plugins.Misc.FraudLabsPro.Order.IsBinCountryMatch.Hint", "Whether the country of issuing bank matches BIN country code.");
            _localizationService.AddOrUpdatePluginLocaleResource("Plugins.Misc.FraudLabsPro.Order.IsBinNameMatch", "Bin Name Match");
            _localizationService.AddOrUpdatePluginLocaleResource("Plugins.Misc.FraudLabsPro.Order.IsBinNameMatch.Hint", "Whether the name of issuing bank matches BIN bank name. ");
            _localizationService.AddOrUpdatePluginLocaleResource("Plugins.Misc.FraudLabsPro.Order.IsBinPhoneMatch", "Bin Phone Match");
            _localizationService.AddOrUpdatePluginLocaleResource("Plugins.Misc.FraudLabsPro.Order.IsBinPhoneMatch.Hint", "Whether the customer service phone number matches BIN phone. ");
            _localizationService.AddOrUpdatePluginLocaleResource("Plugins.Misc.FraudLabsPro.Order.IsBinPrepaid", "Bin Prepaid");
            _localizationService.AddOrUpdatePluginLocaleResource("Plugins.Misc.FraudLabsPro.Order.IsBinPrepaid.Hint", "Whether the credit card is a type of prepaid card.");
            _localizationService.AddOrUpdatePluginLocaleResource("Plugins.Misc.FraudLabsPro.Order.IsAddressShipForward", "Ship Forward");
            _localizationService.AddOrUpdatePluginLocaleResource("Plugins.Misc.FraudLabsPro.Order.IsAddressShipForward.Hint", "Whether the shipping address is in database of known mail drops.");
            _localizationService.AddOrUpdatePluginLocaleResource("Plugins.Misc.FraudLabsPro.Order.IsBillShipCityMatch", "Bill Ship City Match");
            _localizationService.AddOrUpdatePluginLocaleResource("Plugins.Misc.FraudLabsPro.Order.IsBillShipCityMatch.Hint", "Whether the billing city matches the shipping city.");
            _localizationService.AddOrUpdatePluginLocaleResource("Plugins.Misc.FraudLabsPro.Order.IsBillShipStateMatch", "Bill Ship State Match");
            _localizationService.AddOrUpdatePluginLocaleResource("Plugins.Misc.FraudLabsPro.Order.IsBillShipStateMatch.Hint", "Whether the billing state matches the shipping state.");
            _localizationService.AddOrUpdatePluginLocaleResource("Plugins.Misc.FraudLabsPro.Order.IsBillShipCountryMatch", "Bill Ship Country Match");
            _localizationService.AddOrUpdatePluginLocaleResource("Plugins.Misc.FraudLabsPro.Order.IsBillShipCountryMatch.Hint", "Whether the billing country matches the shipping country.");
            _localizationService.AddOrUpdatePluginLocaleResource("Plugins.Misc.FraudLabsPro.Order.IsBillShipPostalMatch", "Bill Ship Postal Match");
            _localizationService.AddOrUpdatePluginLocaleResource("Plugins.Misc.FraudLabsPro.Order.IsBillShipPostalMatch.Hint", "Whether the billing postal/zip code matches the shipping postal/zip code.");
            _localizationService.AddOrUpdatePluginLocaleResource("Plugins.Misc.FraudLabsPro.Order.IsIPBlacklist", "IP Blacklist");
            _localizationService.AddOrUpdatePluginLocaleResource("Plugins.Misc.FraudLabsPro.Order.IsIPBlacklist.Hint", "Whether the IP address is in our blacklist database.");
            _localizationService.AddOrUpdatePluginLocaleResource("Plugins.Misc.FraudLabsPro.Order.IsEmailBlacklist", "Email Blacklist");
            _localizationService.AddOrUpdatePluginLocaleResource("Plugins.Misc.FraudLabsPro.Order.IsEmailBlacklist.Hint", "Whether the email address is in our blacklist database.");
            _localizationService.AddOrUpdatePluginLocaleResource("Plugins.Misc.FraudLabsPro.Order.IsCreditCardBlacklist", "Credit Card Blacklist");
            _localizationService.AddOrUpdatePluginLocaleResource("Plugins.Misc.FraudLabsPro.Order.IsCreditCardBlacklist.Hint", "Whether the credit card is in our blacklist database.");
            _localizationService.AddOrUpdatePluginLocaleResource("Plugins.Misc.FraudLabsPro.Order.IsDeviceBlacklist", "Device Blacklist");
            _localizationService.AddOrUpdatePluginLocaleResource("Plugins.Misc.FraudLabsPro.Order.IsDeviceBlacklist.Hint", "Whether the device Id is in our blacklist database.");
            _localizationService.AddOrUpdatePluginLocaleResource("Plugins.Misc.FraudLabsPro.Order.IsUserBlacklist", "User Blacklist");
            _localizationService.AddOrUpdatePluginLocaleResource("Plugins.Misc.FraudLabsPro.Order.IsUserBlacklist.Hint", "Whether the username is in our blacklist database.");
            _localizationService.AddOrUpdatePluginLocaleResource("Plugins.Misc.FraudLabsPro.Order.IsShipAddressBlackList", "Ship Address BlackList");
            _localizationService.AddOrUpdatePluginLocaleResource("Plugins.Misc.FraudLabsPro.Order.IsShipAddressBlackList.Hint", "Whether the ship address is in our blacklist database.");
            _localizationService.AddOrUpdatePluginLocaleResource("Plugins.Misc.FraudLabsPro.Order.IsPhoneBlacklist", "Phone Blacklist");
            _localizationService.AddOrUpdatePluginLocaleResource("Plugins.Misc.FraudLabsPro.Order.IsPhoneBlacklist.Hint", "Whether the user's phone number is in our blacklist database.");
            _localizationService.AddOrUpdatePluginLocaleResource("Plugins.Misc.FraudLabsPro.Order.IsHighRiskUsernamePassword", "High Risk Username Password");
            _localizationService.AddOrUpdatePluginLocaleResource("Plugins.Misc.FraudLabsPro.Order.IsHighRiskUsernamePassword.Hint", "Whether the username and password is in our high risk database.");
            _localizationService.AddOrUpdatePluginLocaleResource("Plugins.Misc.FraudLabsPro.Order.IsMalwareExploit", "Malware Exploit");
            _localizationService.AddOrUpdatePluginLocaleResource("Plugins.Misc.FraudLabsPro.Order.IsMalwareExploit.Hint", "Whether the machine is infected.");
            _localizationService.AddOrUpdatePluginLocaleResource("Plugins.Misc.FraudLabsPro.Order.IsExportControlledCountry", "Export Controlled Country");
            _localizationService.AddOrUpdatePluginLocaleResource("Plugins.Misc.FraudLabsPro.Order.IsExportControlledCountry.Hint", "Whether the country is from an embargoed country.");
            _localizationService.AddOrUpdatePluginLocaleResource("Plugins.Misc.FraudLabsPro.Order.UserOrderID", "User Order ID");
            _localizationService.AddOrUpdatePluginLocaleResource("Plugins.Misc.FraudLabsPro.Order.UserOrderID.Hint", "Return the order identifier given by merchant earlier.");
            _localizationService.AddOrUpdatePluginLocaleResource("Plugins.Misc.FraudLabsPro.Order.UserOrderMemo", "User Order Memo");
            _localizationService.AddOrUpdatePluginLocaleResource("Plugins.Misc.FraudLabsPro.Order.UserOrderMemo.Hint", "Return the order description given by merchant earlier.");
            _localizationService.AddOrUpdatePluginLocaleResource("Plugins.Misc.FraudLabsPro.Order.FraudLabsProScore", "FraudLabs Pro Score");
            _localizationService.AddOrUpdatePluginLocaleResource("Plugins.Misc.FraudLabsPro.Order.FraudLabsProScore.Hint", "Overall score between 1 and 100. 100 is the highest risk and 1 is the lowest risk.");
            _localizationService.AddOrUpdatePluginLocaleResource("Plugins.Misc.FraudLabsPro.Order.FraudLabsProDistribution", "FraudLabs Pro Distribution");
            _localizationService.AddOrUpdatePluginLocaleResource("Plugins.Misc.FraudLabsPro.Order.FraudLabsProDistribution.Hint", "Return the distribution of the risk rate range from 1 to 100. Distribution score of 90 means it is at top 10% high score in sample.");
            _localizationService.AddOrUpdatePluginLocaleResource("Plugins.Misc.FraudLabsPro.Order.FraudLabsProStatus", "FraudLabs Pro Status");
            _localizationService.AddOrUpdatePluginLocaleResource("Plugins.Misc.FraudLabsPro.Order.FraudLabsProStatus.Hint", "Final action based on the rules analysis.");
            _localizationService.AddOrUpdatePluginLocaleResource("Plugins.Misc.FraudLabsPro.Order.FraudLabsProID", "Transaction ID");
            _localizationService.AddOrUpdatePluginLocaleResource("Plugins.Misc.FraudLabsPro.Order.FraudLabsProID.Hint", "System own unique identifier to identify this API transaction.");
            _localizationService.AddOrUpdatePluginLocaleResource("Plugins.Misc.FraudLabsPro.Order.FraudLabsProVersion", "FraudLabs Pro Version");
            _localizationService.AddOrUpdatePluginLocaleResource("Plugins.Misc.FraudLabsPro.Order.FraudLabsProVersion.Hint", "Version of the fraud analysis engine used in this transaction.");
            _localizationService.AddOrUpdatePluginLocaleResource("Plugins.Misc.FraudLabsPro.Order.FraudLabsProErrorCode", "Error");
            _localizationService.AddOrUpdatePluginLocaleResource("Plugins.Misc.FraudLabsPro.Order.FraudLabsProErrorCode.Hint", "Error code in this transaction. Please refer to Error Codes for complete list.");
            _localizationService.AddOrUpdatePluginLocaleResource("Plugins.Misc.FraudLabsPro.Order.FraudLabsProMessage", "Message");
            _localizationService.AddOrUpdatePluginLocaleResource("Plugins.Misc.FraudLabsPro.Order.FraudLabsProMessage.Hint", "More information about the status of this transaction.");
            _localizationService.AddOrUpdatePluginLocaleResource("Plugins.Misc.FraudLabsPro.Order.FraudLabsProCredit", "Balance");
            _localizationService.AddOrUpdatePluginLocaleResource("Plugins.Misc.FraudLabsPro.Order.FraudLabsProCredit.Hint", "Balance of queries in your account after this transaction.");

            base.Install();
        }

        /// <summary>
        /// Uninstall the plugin
        /// </summary>
        public override void Uninstall()
        {
            //settings
            _settingService.DeleteSetting<FraudLabsProSettings>();

            //generic attributes
            var orders = _orderService.SearchOrders();
            foreach (var order in orders)
            {
                var genericAttributes = _genericAttributeService.GetAttributesForEntity(order.Id, order.GetUnproxiedEntityType().Name).ToList()
                    .Where(w => w.Key.Equals(FraudLabsProDefaults.OrderResultAttribute) || w.Key.Equals(FraudLabsProDefaults.OrderStatusAttribute))
                    .ToArray();
                if (genericAttributes.Any())
                    _genericAttributeService.DeleteAttributes(genericAttributes);
            }

            //locales
            _localizationService.DeletePluginLocaleResource("Plugins.Misc.FraudLabsPro.Fields.ApiKey");
            _localizationService.DeletePluginLocaleResource("Plugins.Misc.FraudLabsPro.Fields.ApiKey.Hint");
            _localizationService.DeletePluginLocaleResource("Plugins.Misc.FraudLabsPro.Fields.ApproveStatusID");
            _localizationService.DeletePluginLocaleResource("Plugins.Misc.FraudLabsPro.Fields.ApproveStatusID.Hint");
            _localizationService.DeletePluginLocaleResource("Plugins.Misc.FraudLabsPro.Fields.Balance");
            _localizationService.DeletePluginLocaleResource("Plugins.Misc.FraudLabsPro.Fields.Balance.Hint");
            _localizationService.DeletePluginLocaleResource("Plugins.Misc.FraudLabsPro.Fields.Balance.Unknown");
            _localizationService.DeletePluginLocaleResource("Plugins.Misc.FraudLabsPro.Fields.Balance.Upgrade");
            _localizationService.DeletePluginLocaleResource("Plugins.Misc.FraudLabsPro.Fields.ReviewStatusID");
            _localizationService.DeletePluginLocaleResource("Plugins.Misc.FraudLabsPro.Fields.ReviewStatusID.Hint");
            _localizationService.DeletePluginLocaleResource("Plugins.Misc.FraudLabsPro.Fields.RejectStatusID");
            _localizationService.DeletePluginLocaleResource("Plugins.Misc.FraudLabsPro.Fields.RejectStatusID.Hint");
            //Order
            _localizationService.DeletePluginLocaleResource("Plugins.Misc.FraudLabsPro.Order.Instructions");
            _localizationService.DeletePluginLocaleResource("Plugins.Misc.FraudLabsPro.Order.Screen");
            _localizationService.DeletePluginLocaleResource("Plugins.Misc.FraudLabsPro.Order.Approve");
            _localizationService.DeletePluginLocaleResource("Plugins.Misc.FraudLabsPro.Order.BlackList");
            _localizationService.DeletePluginLocaleResource("Plugins.Misc.FraudLabsPro.Order.Reject");

            _localizationService.DeletePluginLocaleResource("Plugins.Misc.FraudLabsPro.Order");
            _localizationService.DeletePluginLocaleResource("Plugins.Misc.FraudLabsPro.Order.IsCountryMatch");
            _localizationService.DeletePluginLocaleResource("Plugins.Misc.FraudLabsPro.Order.IsCountryMatch.Hint");
            _localizationService.DeletePluginLocaleResource("Plugins.Misc.FraudLabsPro.Order.IsHighRiskCountry");
            _localizationService.DeletePluginLocaleResource("Plugins.Misc.FraudLabsPro.Order.IsHighRiskCountry.Hint");
            _localizationService.DeletePluginLocaleResource("Plugins.Misc.FraudLabsPro.Order.DistanceInKM");
            _localizationService.DeletePluginLocaleResource("Plugins.Misc.FraudLabsPro.Order.DistanceInKM.Hint");
            _localizationService.DeletePluginLocaleResource("Plugins.Misc.FraudLabsPro.Order.DistanceInMile");
            _localizationService.DeletePluginLocaleResource("Plugins.Misc.FraudLabsPro.Order.DistanceInMile.Hint");
            _localizationService.DeletePluginLocaleResource("Plugins.Misc.FraudLabsPro.Order.IPAddress");
            _localizationService.DeletePluginLocaleResource("Plugins.Misc.FraudLabsPro.Order.IPAddress.Hint");
            _localizationService.DeletePluginLocaleResource("Plugins.Misc.FraudLabsPro.Order.IPCountry");
            _localizationService.DeletePluginLocaleResource("Plugins.Misc.FraudLabsPro.Order.IPCountry.Hint");
            _localizationService.DeletePluginLocaleResource("Plugins.Misc.FraudLabsPro.Order.IPRegion");
            _localizationService.DeletePluginLocaleResource("Plugins.Misc.FraudLabsPro.Order.IPRegion.Hint");
            _localizationService.DeletePluginLocaleResource("Plugins.Misc.FraudLabsPro.Order.IPCity");
            _localizationService.DeletePluginLocaleResource("Plugins.Misc.FraudLabsPro.Order.IPCity.Hint");
            _localizationService.DeletePluginLocaleResource("Plugins.Misc.FraudLabsPro.Order.IPContinent");
            _localizationService.DeletePluginLocaleResource("Plugins.Misc.FraudLabsPro.Order.IPContinent.Hint");
            _localizationService.DeletePluginLocaleResource("Plugins.Misc.FraudLabsPro.Order.IPLatitude");
            _localizationService.DeletePluginLocaleResource("Plugins.Misc.FraudLabsPro.Order.IPLatitude.Hint");
            _localizationService.DeletePluginLocaleResource("Plugins.Misc.FraudLabsPro.Order.IPLongtitude");
            _localizationService.DeletePluginLocaleResource("Plugins.Misc.FraudLabsPro.Order.IPLongtitude.Hint");
            _localizationService.DeletePluginLocaleResource("Plugins.Misc.FraudLabsPro.Order.IPTimeZone");
            _localizationService.DeletePluginLocaleResource("Plugins.Misc.FraudLabsPro.Order.IPTimeZone.Hint");
            _localizationService.DeletePluginLocaleResource("Plugins.Misc.FraudLabsPro.Order.IPElevation");
            _localizationService.DeletePluginLocaleResource("Plugins.Misc.FraudLabsPro.Order.IPElevation.Hint");
            _localizationService.DeletePluginLocaleResource("Plugins.Misc.FraudLabsPro.Order.IPDomain");
            _localizationService.DeletePluginLocaleResource("Plugins.Misc.FraudLabsPro.Order.IPDomain.Hint");
            _localizationService.DeletePluginLocaleResource("Plugins.Misc.FraudLabsPro.Order.IPMobileMNC");
            _localizationService.DeletePluginLocaleResource("Plugins.Misc.FraudLabsPro.Order.IPMobileMNC.Hint");
            _localizationService.DeletePluginLocaleResource("Plugins.Misc.FraudLabsPro.Order.IPMobileMCC");
            _localizationService.DeletePluginLocaleResource("Plugins.Misc.FraudLabsPro.Order.IPMobileMCC.Hint");
            _localizationService.DeletePluginLocaleResource("Plugins.Misc.FraudLabsPro.Order.IPMobileBrand");
            _localizationService.DeletePluginLocaleResource("Plugins.Misc.FraudLabsPro.Order.IPMobileBrand.Hint");
            _localizationService.DeletePluginLocaleResource("Plugins.Misc.FraudLabsPro.Order.IPNetSpeed");
            _localizationService.DeletePluginLocaleResource("Plugins.Misc.FraudLabsPro.Order.IPNetSpeed.Hint");
            _localizationService.DeletePluginLocaleResource("Plugins.Misc.FraudLabsPro.Order.IPISPName");
            _localizationService.DeletePluginLocaleResource("Plugins.Misc.FraudLabsPro.Order.IPISPName.Hint");
            _localizationService.DeletePluginLocaleResource("Plugins.Misc.FraudLabsPro.Order.IPUsageType");
            _localizationService.DeletePluginLocaleResource("Plugins.Misc.FraudLabsPro.Order.IPUsageType.Hint");
            _localizationService.DeletePluginLocaleResource("Plugins.Misc.FraudLabsPro.Order.IsFreeEmail");
            _localizationService.DeletePluginLocaleResource("Plugins.Misc.FraudLabsPro.Order.IsFreeEmail.Hint");
            _localizationService.DeletePluginLocaleResource("Plugins.Misc.FraudLabsPro.Order.IsDisposableEmail");
            _localizationService.DeletePluginLocaleResource("Plugins.Misc.FraudLabsPro.Order.IsDisposableEmail.Hint");
            _localizationService.DeletePluginLocaleResource("Plugins.Misc.FraudLabsPro.Order.IsNewDomainName");
            _localizationService.DeletePluginLocaleResource("Plugins.Misc.FraudLabsPro.Order.IsNewDomainName.Hint");
            _localizationService.DeletePluginLocaleResource("Plugins.Misc.FraudLabsPro.Order.IsDomainExists");
            _localizationService.DeletePluginLocaleResource("Plugins.Misc.FraudLabsPro.Order.IsDomainExists.Hint");
            _localizationService.DeletePluginLocaleResource("Plugins.Misc.FraudLabsPro.Order.IsProxyIPAddress");
            _localizationService.DeletePluginLocaleResource("Plugins.Misc.FraudLabsPro.Order.IsProxyIPAddress.Hint");
            _localizationService.DeletePluginLocaleResource("Plugins.Misc.FraudLabsPro.Order.IsBinFound");
            _localizationService.DeletePluginLocaleResource("Plugins.Misc.FraudLabsPro.Order.IsBinFound.Hint");
            _localizationService.DeletePluginLocaleResource("Plugins.Misc.FraudLabsPro.Order.IsBinCountryMatch");
            _localizationService.DeletePluginLocaleResource("Plugins.Misc.FraudLabsPro.Order.IsBinCountryMatch.Hint");
            _localizationService.DeletePluginLocaleResource("Plugins.Misc.FraudLabsPro.Order.IsBinNameMatch");
            _localizationService.DeletePluginLocaleResource("Plugins.Misc.FraudLabsPro.Order.IsBinNameMatch.Hint");
            _localizationService.DeletePluginLocaleResource("Plugins.Misc.FraudLabsPro.Order.IsBinPhoneMatch");
            _localizationService.DeletePluginLocaleResource("Plugins.Misc.FraudLabsPro.Order.IsBinPhoneMatch.Hint");
            _localizationService.DeletePluginLocaleResource("Plugins.Misc.FraudLabsPro.Order.IsBinPrepaid");
            _localizationService.DeletePluginLocaleResource("Plugins.Misc.FraudLabsPro.Order.IsBinPrepaid.Hint");
            _localizationService.DeletePluginLocaleResource("Plugins.Misc.FraudLabsPro.Order.IsAddressShipForward");
            _localizationService.DeletePluginLocaleResource("Plugins.Misc.FraudLabsPro.Order.IsAddressShipForward.Hint");
            _localizationService.DeletePluginLocaleResource("Plugins.Misc.FraudLabsPro.Order.IsBillShipCityMatch");
            _localizationService.DeletePluginLocaleResource("Plugins.Misc.FraudLabsPro.Order.IsBillShipCityMatch.Hint");
            _localizationService.DeletePluginLocaleResource("Plugins.Misc.FraudLabsPro.Order.IsBillShipStateMatch");
            _localizationService.DeletePluginLocaleResource("Plugins.Misc.FraudLabsPro.Order.IsBillShipStateMatch.Hint");
            _localizationService.DeletePluginLocaleResource("Plugins.Misc.FraudLabsPro.Order.IsBillShipCountryMatch");
            _localizationService.DeletePluginLocaleResource("Plugins.Misc.FraudLabsPro.Order.IsBillShipCountryMatch.Hint");
            _localizationService.DeletePluginLocaleResource("Plugins.Misc.FraudLabsPro.Order.IsBillShipPostalMatch");
            _localizationService.DeletePluginLocaleResource("Plugins.Misc.FraudLabsPro.Order.IsBillShipPostalMatch.Hint");
            _localizationService.DeletePluginLocaleResource("Plugins.Misc.FraudLabsPro.Order.IsIPBlacklist");
            _localizationService.DeletePluginLocaleResource("Plugins.Misc.FraudLabsPro.Order.IsIPBlacklist.Hint");
            _localizationService.DeletePluginLocaleResource("Plugins.Misc.FraudLabsPro.Order.IsEmailBlacklist");
            _localizationService.DeletePluginLocaleResource("Plugins.Misc.FraudLabsPro.Order.IsEmailBlacklist.Hint");
            _localizationService.DeletePluginLocaleResource("Plugins.Misc.FraudLabsPro.Order.IsCreditCardBlacklist");
            _localizationService.DeletePluginLocaleResource("Plugins.Misc.FraudLabsPro.Order.IsCreditCardBlacklist.Hint");
            _localizationService.DeletePluginLocaleResource("Plugins.Misc.FraudLabsPro.Order.IsDeviceBlacklist");
            _localizationService.DeletePluginLocaleResource("Plugins.Misc.FraudLabsPro.Order.IsDeviceBlacklist.Hint");
            _localizationService.DeletePluginLocaleResource("Plugins.Misc.FraudLabsPro.Order.IsUserBlacklist");
            _localizationService.DeletePluginLocaleResource("Plugins.Misc.FraudLabsPro.Order.IsUserBlacklist.Hint");
            _localizationService.DeletePluginLocaleResource("Plugins.Misc.FraudLabsPro.Order.IsShipAddressBlackList");
            _localizationService.DeletePluginLocaleResource("Plugins.Misc.FraudLabsPro.Order.IsShipAddressBlackList.Hint");
            _localizationService.DeletePluginLocaleResource("Plugins.Misc.FraudLabsPro.Order.IsPhoneBlacklist");
            _localizationService.DeletePluginLocaleResource("Plugins.Misc.FraudLabsPro.Order.IsPhoneBlacklist.Hint");
            _localizationService.DeletePluginLocaleResource("Plugins.Misc.FraudLabsPro.Order.IsHighRiskUsernamePassword");
            _localizationService.DeletePluginLocaleResource("Plugins.Misc.FraudLabsPro.Order.IsHighRiskUsernamePassword.Hint");
            _localizationService.DeletePluginLocaleResource("Plugins.Misc.FraudLabsPro.Order.IsMalwareExploit");
            _localizationService.DeletePluginLocaleResource("Plugins.Misc.FraudLabsPro.Order.IsMalwareExploit.Hint");
            _localizationService.DeletePluginLocaleResource("Plugins.Misc.FraudLabsPro.Order.IsExportControlledCountry");
            _localizationService.DeletePluginLocaleResource("Plugins.Misc.FraudLabsPro.Order.IsExportControlledCountry.Hint");
            _localizationService.DeletePluginLocaleResource("Plugins.Misc.FraudLabsPro.Order.UserOrderID");
            _localizationService.DeletePluginLocaleResource("Plugins.Misc.FraudLabsPro.Order.UserOrderID.Hint");
            _localizationService.DeletePluginLocaleResource("Plugins.Misc.FraudLabsPro.Order.UserOrderMemo");
            _localizationService.DeletePluginLocaleResource("Plugins.Misc.FraudLabsPro.Order.UserOrderMemo.Hint");
            _localizationService.DeletePluginLocaleResource("Plugins.Misc.FraudLabsPro.Order.FraudLabsProScore");
            _localizationService.DeletePluginLocaleResource("Plugins.Misc.FraudLabsPro.Order.FraudLabsProScore.Hint");
            _localizationService.DeletePluginLocaleResource("Plugins.Misc.FraudLabsPro.Order.FraudLabsProDistribution");
            _localizationService.DeletePluginLocaleResource("Plugins.Misc.FraudLabsPro.Order.FraudLabsProDistribution.Hint");
            _localizationService.DeletePluginLocaleResource("Plugins.Misc.FraudLabsPro.Order.FraudLabsProStatus");
            _localizationService.DeletePluginLocaleResource("Plugins.Misc.FraudLabsPro.Order.FraudLabsProStatus.Hint");
            _localizationService.DeletePluginLocaleResource("Plugins.Misc.FraudLabsPro.Order.FraudLabsProID");
            _localizationService.DeletePluginLocaleResource("Plugins.Misc.FraudLabsPro.Order.FraudLabsProID.Hint");
            _localizationService.DeletePluginLocaleResource("Plugins.Misc.FraudLabsPro.Order.FraudLabsProVersion");
            _localizationService.DeletePluginLocaleResource("Plugins.Misc.FraudLabsPro.Order.FraudLabsProVersion.Hint");
            _localizationService.DeletePluginLocaleResource("Plugins.Misc.FraudLabsPro.Order.FraudLabsProErrorCode");
            _localizationService.DeletePluginLocaleResource("Plugins.Misc.FraudLabsPro.Order.FraudLabsProErrorCode.Hint");
            _localizationService.DeletePluginLocaleResource("Plugins.Misc.FraudLabsPro.Order.FraudLabsProMessage");
            _localizationService.DeletePluginLocaleResource("Plugins.Misc.FraudLabsPro.Order.FraudLabsProMessage.Hint");
            _localizationService.DeletePluginLocaleResource("Plugins.Misc.FraudLabsPro.Order.FraudLabsProCredit");
            _localizationService.DeletePluginLocaleResource("Plugins.Misc.FraudLabsPro.Order.FraudLabsProCredit.Hint");

            base.Uninstall();
        }

        #endregion

    }
}
