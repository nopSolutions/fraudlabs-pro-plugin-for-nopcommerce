namespace Nop.Plugin.Misc.FraudLabsPro
{
    /// <summary>
    /// Represents constants of the FraudLabsPro plugin
    /// </summary>
    public static class FraudLabsProDefaults
    {
        /// <summary>
        /// FraudLabs Pro system name
        /// </summary>
        public static string SystemName => "Misc.FraudLabsPro";

        /// <summary>
        /// Name of the view component to display seal in public store
        /// </summary>
        public const string SEAL_VIEW_COMPONENT_NAME = "FraudLabsPro.Secured.Seal";

        /// <summary>
        /// Name of the view component to disaply FraudLabsPro block on the order details page
        /// </summary>
        public const string ORDER_VIEW_COMPONENT_NAME = "FraudLabsPro.OrderDetails";

        /// <summary>
        /// Generic attribute name to hide FraudLabsPro order block on the order details page
        /// </summary>
        public static string HideBlockAttribute = "OrderPage.HideFraudLabsProBlock";

        /// <summary>
        /// Gets Secured Seal link url
        /// </summary>
        public static string SecuredSealHrefUrl => "https://www.fraudlabspro.com/?ref=15876#secured-seal-1";

        /// <summary>
        /// Gets Secured Seal image src
        /// </summary>
        public static string SecuredSealLinkSrc => "//www.fraudlabspro.com/images/secured-seals/seal.png?ref=15876";

        /// <summary>
        /// One page checkout route name
        /// </summary>
        public static string OnePageCheckoutRouteName => "CheckoutOnePage";

        /// <summary>
        /// Confirm checkout route name
        /// </summary>
        public static string ConfirmCheckoutRouteName => "CheckoutConfirm";

        /// <summary>
        /// Gets a key of the Oredr result
        /// </summary>
        public static string OrderResultAttribute => "FraudLabsProOrderResult";

        /// <summary>
        /// Gets a key of the Oredr status
        /// </summary>
        public static string OrderStatusAttribute => "FraudLabsProOrderStatus";

        /// <summary>
        /// Gets cookies name for device validation
        /// </summary>
        public static string CookiesName => "flp_checksum";

        /// <summary>
        /// Gets FraudLabs tab name
        /// </summary>
        public static string FraudLabsProPanelId => "panel-fraudlabspro";

        /// <summary>
        /// Gets path of agent js script
        /// </summary>
        public static string AgentScriptPath => "~/Plugins/Misc.FraudLabsPro/Scripts/agent_javascript.js";

        /// <summary>
        /// Gets FraudLabs Pro Merchant Area url
        /// </summary>
        public static string FraudLabsProMerchantArea => "https://www.fraudlabspro.com/merchant/";

        /// <summary>
        /// Gets FraudLabs Pro scope image url
        /// </summary>
        public static string FraudLabsProImageUrl => "https://www.fraudlabspro.com/images/fraudscore/fraudlabsproscore_a";

        /// <summary>
        /// Gets FraudLabs Pro upgrade url
        /// </summary>
        public static string FraudLabsProUpgrageUrl => "https://www.fraudlabspro.com/pricing";
    }
}