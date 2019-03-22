using Nop.Core.Configuration;

namespace Nop.Plugin.Misc.FraudLabsPro
{
    /// <summary>
    /// Represents a FraudLabsPro settings
    /// </summary>
    public class FraudLabsProSettings : ISettings
    {
        /// <summary>
        /// Gets or sets the API key
        /// </summary>
        public string ApiKey { get; set; }

        /// <summary>
        /// Gets or sets the order status ID when order has been approved by FraudLabs Pro
        /// </summary>
        public int ApproveStatusID { get; set; }

        /// <summary>
        /// Gets or sets the order status ID when order has been review by FraudLabs Pro
        /// </summary>
        public int ReviewStatusID { get; set; }

        /// <summary>
        /// Gets or sets the order status ID when order has been reject by FraudLabs Pro
        /// </summary>
        public int RejectStatusID { get; set; }

        /// <summary>
        /// Gets or sets account queries balance
        /// </summary>
        public string Balance { get; set; }
    }
}
