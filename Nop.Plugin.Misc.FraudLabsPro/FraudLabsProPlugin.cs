using System;
using System.Collections.Generic;
using System.Linq;
using Nop.Core;
using Nop.Core.Domain.Orders;
using Nop.Services.Cms;
using Nop.Services.Common;
using Nop.Services.Configuration;
using Nop.Services.Localization;
using Nop.Services.Orders;
using Nop.Services.Plugins;
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
            return new List<string>
            {
                AdminWidgetZones.OrderDetailsBlock,
                PublicWidgetZones.OpcContentAfter,
                PublicWidgetZones.CheckoutConfirmBottom
            };
        }

        /// <summary>
        /// Gets a name of a view component for displaying widget
        /// </summary>
        /// <param name="widgetZone">Name of the widget zone</param>
        /// <returns>View component name</returns>
        public string GetWidgetViewComponentName(string widgetZone)
        {
            if (!widgetZone?.Equals(AdminWidgetZones.OrderDetailsBlock, StringComparison.InvariantCultureIgnoreCase) ?? true)
            {
                return FraudLabsProDefaults.SEAL_VIEW_COMPONENT_NAME;
            }
            else
            {
                return FraudLabsProDefaults.ORDER_VIEW_COMPONENT_NAME;
            }
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
            _settingService.SaveSetting(new FraudLabsProSettings()
            {
                ApproveStatusID = (int)OrderStatus.Processing,
                ReviewStatusID = (int)OrderStatus.Pending,
                RejectStatusID = (int)OrderStatus.Cancelled
            });

            //locales
            _localizationService.AddPluginLocaleResource(new Dictionary<string, string>
            {
                ["Plugins.Misc.FraudLabsPro.Fields.ApiKey"] = "API key",
                ["Plugins.Misc.FraudLabsPro.Fields.ApiKey.Hint"] = "Input your FraudLabs Pro account API key.",
                ["Plugins.Misc.FraudLabsPro.Fields.ApproveStatusID"] = "Approve Status",
                ["Plugins.Misc.FraudLabsPro.Fields.ApproveStatusID.Hint"] = "Change order status when order has been approved by FraudLabs Pro, or FraudLabs Pro Approve button has been pressed in order details page.",
                ["Plugins.Misc.FraudLabsPro.Fields.Balance"] = "Balance",
                ["Plugins.Misc.FraudLabsPro.Fields.Balance.Hint"] = "Balance of queries in your account after last transaction.",
                ["Plugins.Misc.FraudLabsPro.Fields.Balance.Unknown"] = "Balance will be available after the first scan of the order",
                ["Plugins.Misc.FraudLabsPro.Fields.Balance.Upgrade"] = "Upgrade",
                ["Plugins.Misc.FraudLabsPro.Fields.ReviewStatusID"] = "Review Status",
                ["Plugins.Misc.FraudLabsPro.Fields.ReviewStatusID.Hint"] = "Change order status when order has been marked as REVIEW by FraudLabs Pro.",
                ["Plugins.Misc.FraudLabsPro.Fields.RejectStatusID"] = "Reject Status",
                ["Plugins.Misc.FraudLabsPro.Fields.RejectStatusID.Hint"] = "Change order status when order has been reject by FraudLabs Pro, or FraudLabs Pro Reject button has been pressed in order details page.",
                //order
                ["Plugins.Misc.FraudLabsPro.Order.Instructions"] = "<p>Please login to <a href='{0}' target='_blank'>FraudLabs Pro Merchant Area</a> for more information about this order.</p>",
                ["Plugins.Misc.FraudLabsPro.Order.Screen"] = "Screen Order",
                ["Plugins.Misc.FraudLabsPro.Order.Approve"] = "Approve",
                ["Plugins.Misc.FraudLabsPro.Order.BlackList"] = "BlackList",
                ["Plugins.Misc.FraudLabsPro.Order.Reject"] = "Reject",
            
                ["Plugins.Misc.FraudLabsPro.Order"] = "FraudLabs Pro Details",
                ["Plugins.Misc.FraudLabsPro.Order.IsCountryMatch"] = "Country match",
                ["Plugins.Misc.FraudLabsPro.Order.IsCountryMatch.Hint"] = "Whether country of IP address matches billing address country.",
                ["Plugins.Misc.FraudLabsPro.Order.IsHighRiskCountry"] = "High risk country",
                ["Plugins.Misc.FraudLabsPro.Order.IsHighRiskCountry.Hint"] = "Whether IP address or billing address country is in the latest high risk list.",
                ["Plugins.Misc.FraudLabsPro.Order.DistanceInKM"] = "IP Distance",
                ["Plugins.Misc.FraudLabsPro.Order.DistanceInKM.Hint"] = "Distance of location between IP address and bill.",
                ["Plugins.Misc.FraudLabsPro.Order.DistanceInMile"] = "IP Distance in Mile",
                ["Plugins.Misc.FraudLabsPro.Order.DistanceInMile.Hint"] = "Distance of location between IP address and bill. Value in mile.",
                ["Plugins.Misc.FraudLabsPro.Order.IPAddress"] = "IP Address",
                ["Plugins.Misc.FraudLabsPro.Order.IPAddress.Hint"] = "Estimated of the IP address.",
                ["Plugins.Misc.FraudLabsPro.Order.IPCountry"] = "IP Country",
                ["Plugins.Misc.FraudLabsPro.Order.IPCountry.Hint"] = "Estimated country of the IP address.",
                ["Plugins.Misc.FraudLabsPro.Order.IPRegion"] = "IP Region",
                ["Plugins.Misc.FraudLabsPro.Order.IPRegion.Hint"] = "Estimated region of the IP address.",
                ["Plugins.Misc.FraudLabsPro.Order.IPCity"] = "IP City",
                ["Plugins.Misc.FraudLabsPro.Order.IPCity.Hint"] = "Estimated city of the IP address.",
                ["Plugins.Misc.FraudLabsPro.Order.IPContinent"] = "IP Location",
                ["Plugins.Misc.FraudLabsPro.Order.IPContinent.Hint"] = "Estimated of the IP address.",
                ["Plugins.Misc.FraudLabsPro.Order.IPLatitude"] = "IP Latitude",
                ["Plugins.Misc.FraudLabsPro.Order.IPLatitude.Hint"] = "Estimated latitude of the IP address.",
                ["Plugins.Misc.FraudLabsPro.Order.IPLongtitude"] = "IP Longtitude",
                ["Plugins.Misc.FraudLabsPro.Order.IPLongtitude.Hint"] = "Estimated longitude of the IP address.",
                ["Plugins.Misc.FraudLabsPro.Order.IPTimeZone"] = "IP Time Zone",
                ["Plugins.Misc.FraudLabsPro.Order.IPTimeZone.Hint"] = "Estimated timezone of the IP address.",
                ["Plugins.Misc.FraudLabsPro.Order.IPElevation"] = "IP Elevation",
                ["Plugins.Misc.FraudLabsPro.Order.IPElevation.Hint"] = "Estimated elevation of the IP address.",
                ["Plugins.Misc.FraudLabsPro.Order.IPDomain"] = "IP Domain",
                ["Plugins.Misc.FraudLabsPro.Order.IPDomain.Hint"] = "Estimated domain name of the IP address.",
                ["Plugins.Misc.FraudLabsPro.Order.IPMobileMNC"] = "IP Mobile MNC",
                ["Plugins.Misc.FraudLabsPro.Order.IPMobileMNC.Hint"] = "Estimated mobile mnc information of the IP address, if it is a mobile network.",
                ["Plugins.Misc.FraudLabsPro.Order.IPMobileMCC"] = "IP Mobile MCC",
                ["Plugins.Misc.FraudLabsPro.Order.IPMobileMCC.Hint"] = "Estimated mobile mcc information of the IP address, if it is a mobile network.",
                ["Plugins.Misc.FraudLabsPro.Order.IPMobileBrand"] = "IP Mobile brand",
                ["Plugins.Misc.FraudLabsPro.Order.IPMobileBrand.Hint"] = "Estimated mobile brand information of the IP address, if it is a mobile network.",
                ["Plugins.Misc.FraudLabsPro.Order.IPNetSpeed"] = "IP Net Speed",
                ["Plugins.Misc.FraudLabsPro.Order.IPNetSpeed.Hint"] = "Estimated netspeed of the IP address.",
                ["Plugins.Misc.FraudLabsPro.Order.IPISPName"] = "IP ISP Name",
                ["Plugins.Misc.FraudLabsPro.Order.IPISPName.Hint"] = "Estimated ISP name of the IP address.",
                ["Plugins.Misc.FraudLabsPro.Order.IPUsageType"] = "IP Usage Type",
                ["Plugins.Misc.FraudLabsPro.Order.IPUsageType.Hint"] = "Estimated usage type of the IP address.",
                ["Plugins.Misc.FraudLabsPro.Order.IsFreeEmail"] = "Free Email",
                ["Plugins.Misc.FraudLabsPro.Order.IsFreeEmail.Hint"] = "Whether the email is from free email provider.",
                ["Plugins.Misc.FraudLabsPro.Order.IsDisposableEmail"] = "Disposable Email",
                ["Plugins.Misc.FraudLabsPro.Order.IsDisposableEmail.Hint"] = "Whether the email is a disposable email.",
                ["Plugins.Misc.FraudLabsPro.Order.IsNewDomainName"] = "New Domain Name",
                ["Plugins.Misc.FraudLabsPro.Order.IsNewDomainName.Hint"] = "Whether the email domain name a newly registered name. Only applicable for non-free email domain.",
                ["Plugins.Misc.FraudLabsPro.Order.IsDomainExists"] = "Domain Exists",
                ["Plugins.Misc.FraudLabsPro.Order.IsDomainExists.Hint"] = "Whether the email domain name is a valid domain.",
                ["Plugins.Misc.FraudLabsPro.Order.IsProxyIPAddress"] = "Using Proxy",
                ["Plugins.Misc.FraudLabsPro.Order.IsProxyIPAddress.Hint"] = "Whether the IP address is from a known anonymous proxy server.",
                ["Plugins.Misc.FraudLabsPro.Order.IsBinFound"] = "Bin Found",
                ["Plugins.Misc.FraudLabsPro.Order.IsBinFound.Hint"] = "Whether the BIN information matches our BIN list.",
                ["Plugins.Misc.FraudLabsPro.Order.IsBinCountryMatch"] = "Bin Country Match",
                ["Plugins.Misc.FraudLabsPro.Order.IsBinCountryMatch.Hint"] = "Whether the country of issuing bank matches BIN country code.",
                ["Plugins.Misc.FraudLabsPro.Order.IsBinNameMatch"] = "Bin Name Match",
                ["Plugins.Misc.FraudLabsPro.Order.IsBinNameMatch.Hint"] = "Whether the name of issuing bank matches BIN bank name. ",
                ["Plugins.Misc.FraudLabsPro.Order.IsBinPhoneMatch"] = "Bin Phone Match",
                ["Plugins.Misc.FraudLabsPro.Order.IsBinPhoneMatch.Hint"] = "Whether the customer service phone number matches BIN phone. ",
                ["Plugins.Misc.FraudLabsPro.Order.IsBinPrepaid"] = "Bin Prepaid",
                ["Plugins.Misc.FraudLabsPro.Order.IsBinPrepaid.Hint"] = "Whether the credit card is a type of prepaid card.",
                ["Plugins.Misc.FraudLabsPro.Order.IsAddressShipForward"] = "Ship Forward",
                ["Plugins.Misc.FraudLabsPro.Order.IsAddressShipForward.Hint"] = "Whether the shipping address is in database of known mail drops.",
                ["Plugins.Misc.FraudLabsPro.Order.IsBillShipCityMatch"] = "Bill Ship City Match",
                ["Plugins.Misc.FraudLabsPro.Order.IsBillShipCityMatch.Hint"] = "Whether the billing city matches the shipping city.",
                ["Plugins.Misc.FraudLabsPro.Order.IsBillShipStateMatch"] = "Bill Ship State Match",
                ["Plugins.Misc.FraudLabsPro.Order.IsBillShipStateMatch.Hint"] = "Whether the billing state matches the shipping state.",
                ["Plugins.Misc.FraudLabsPro.Order.IsBillShipCountryMatch"] = "Bill Ship Country Match",
                ["Plugins.Misc.FraudLabsPro.Order.IsBillShipCountryMatch.Hint"] = "Whether the billing country matches the shipping country.",
                ["Plugins.Misc.FraudLabsPro.Order.IsBillShipPostalMatch"] = "Bill Ship Postal Match",
                ["Plugins.Misc.FraudLabsPro.Order.IsBillShipPostalMatch.Hint"] = "Whether the billing postal/zip code matches the shipping postal/zip code.",
                ["Plugins.Misc.FraudLabsPro.Order.IsIPBlacklist"] = "IP Blacklist",
                ["Plugins.Misc.FraudLabsPro.Order.IsIPBlacklist.Hint"] = "Whether the IP address is in our blacklist database.",
                ["Plugins.Misc.FraudLabsPro.Order.IsEmailBlacklist"] = "Email Blacklist",
                ["Plugins.Misc.FraudLabsPro.Order.IsEmailBlacklist.Hint"] = "Whether the email address is in our blacklist database.",
                ["Plugins.Misc.FraudLabsPro.Order.IsCreditCardBlacklist"] = "Credit Card Blacklist",
                ["Plugins.Misc.FraudLabsPro.Order.IsCreditCardBlacklist.Hint"] = "Whether the credit card is in our blacklist database.",
                ["Plugins.Misc.FraudLabsPro.Order.IsDeviceBlacklist"] = "Device Blacklist",
                ["Plugins.Misc.FraudLabsPro.Order.IsDeviceBlacklist.Hint"] = "Whether the device Id is in our blacklist database.",
                ["Plugins.Misc.FraudLabsPro.Order.IsUserBlacklist"] = "User Blacklist",
                ["Plugins.Misc.FraudLabsPro.Order.IsUserBlacklist.Hint"] = "Whether the username is in our blacklist database.",
                ["Plugins.Misc.FraudLabsPro.Order.IsShipAddressBlackList"] = "Ship Address BlackList",
                ["Plugins.Misc.FraudLabsPro.Order.IsShipAddressBlackList.Hint"] = "Whether the ship address is in our blacklist database.",
                ["Plugins.Misc.FraudLabsPro.Order.IsPhoneBlacklist"] = "Phone Blacklist",
                ["Plugins.Misc.FraudLabsPro.Order.IsPhoneBlacklist.Hint"] = "Whether the user's phone number is in our blacklist database.",
                ["Plugins.Misc.FraudLabsPro.Order.IsHighRiskUsernamePassword"] = "High Risk Username Password",
                ["Plugins.Misc.FraudLabsPro.Order.IsHighRiskUsernamePassword.Hint"] = "Whether the username and password is in our high risk database.",
                ["Plugins.Misc.FraudLabsPro.Order.IsMalwareExploit"] = "Malware Exploit",
                ["Plugins.Misc.FraudLabsPro.Order.IsMalwareExploit.Hint"] = "Whether the machine is infected.",
                ["Plugins.Misc.FraudLabsPro.Order.IsExportControlledCountry"] = "Export Controlled Country",
                ["Plugins.Misc.FraudLabsPro.Order.IsExportControlledCountry.Hint"] = "Whether the country is from an embargoed country.",
                ["Plugins.Misc.FraudLabsPro.Order.UserOrderID"] = "User Order ID",
                ["Plugins.Misc.FraudLabsPro.Order.UserOrderID.Hint"] = "Return the order identifier given by merchant earlier.",
                ["Plugins.Misc.FraudLabsPro.Order.UserOrderMemo"] = "User Order Memo",
                ["Plugins.Misc.FraudLabsPro.Order.UserOrderMemo.Hint"] = "Return the order description given by merchant earlier.",
                ["Plugins.Misc.FraudLabsPro.Order.FraudLabsProScore"] = "FraudLabs Pro Score",
                ["Plugins.Misc.FraudLabsPro.Order.FraudLabsProScore.Hint"] = "Overall score between 1 and 100. 100 is the highest risk and 1 is the lowest risk.",
                ["Plugins.Misc.FraudLabsPro.Order.FraudLabsProDistribution"] = "FraudLabs Pro Distribution",
                ["Plugins.Misc.FraudLabsPro.Order.FraudLabsProDistribution.Hint"] = "Return the distribution of the risk rate range from 1 to 100. Distribution score of 90 means it is at top 10% high score in sample.",
                ["Plugins.Misc.FraudLabsPro.Order.FraudLabsProStatus"] = "FraudLabs Pro Status",
                ["Plugins.Misc.FraudLabsPro.Order.FraudLabsProStatus.Hint"] = "Final action based on the rules analysis.",
                ["Plugins.Misc.FraudLabsPro.Order.FraudLabsProID"] = "Transaction ID",
                ["Plugins.Misc.FraudLabsPro.Order.FraudLabsProID.Hint"] = "System own unique identifier to identify this API transaction.",
                ["Plugins.Misc.FraudLabsPro.Order.FraudLabsProVersion"] = "FraudLabs Pro Version",
                ["Plugins.Misc.FraudLabsPro.Order.FraudLabsProVersion.Hint"] = "Version of the fraud analysis engine used in this transaction.",
                ["Plugins.Misc.FraudLabsPro.Order.FraudLabsProErrorCode"] = "Error",
                ["Plugins.Misc.FraudLabsPro.Order.FraudLabsProErrorCode.Hint"] = "Error code in this transaction. Please refer to Error Codes for complete list.",
                ["Plugins.Misc.FraudLabsPro.Order.FraudLabsProMessage"] = "Message",
                ["Plugins.Misc.FraudLabsPro.Order.FraudLabsProMessage.Hint"] = "More information about the status of this transaction.",
                ["Plugins.Misc.FraudLabsPro.Order.FraudLabsProCredit"] = "Balance",
                ["Plugins.Misc.FraudLabsPro.Order.FraudLabsProCredit.Hint"] = "Balance of queries in your account after this transaction."
            });

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
                var genericAttributes = _genericAttributeService.GetAttributesForEntity(order.Id, order.GetType().Name).ToList()
                    .Where(w => w.Key.Equals(FraudLabsProDefaults.OrderResultAttribute) || w.Key.Equals(FraudLabsProDefaults.OrderStatusAttribute))
                    .ToArray();
                if (genericAttributes.Any())
                    _genericAttributeService.DeleteAttributes(genericAttributes);
            }

            //locales
            _localizationService.DeletePluginLocaleResources("Plugins.Misc.FraudLabsPro");            

            base.Uninstall();
        }

        #endregion

        /// <summary>
        /// Gets a value indicating whether to hide this plugin on the widget list page in the admin area
        /// </summary>
        public bool HideInWidgetList => false;

    }
}
