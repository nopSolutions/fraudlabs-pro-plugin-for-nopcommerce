using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;
using Nop.Web.Framework.Mvc.ModelBinding;

namespace Nop.Plugin.Misc.FraudLabsPro.Models
{
    /// <summary>
    /// Represents a configuration model
    /// </summary>
    public class ConfigurationModel
    {
        #region Ctor

        public ConfigurationModel()
        {
            AvailableStatusLists = new List<SelectListItem>();
        }
        #endregion

        #region Properties

        [NopResourceDisplayName("Plugins.Misc.FraudLabsPro.Fields.ApiKey")]
        public string ApiKey { get; set; }

        [NopResourceDisplayName("Plugins.Misc.FraudLabsPro.Fields.ApproveStatusID")]
        public int ApproveStatusID { get; set; }

        [NopResourceDisplayName("Plugins.Misc.FraudLabsPro.Fields.ReviewStatusID")]
        public int ReviewStatusID { get; set; }

        [NopResourceDisplayName("Plugins.Misc.FraudLabsPro.Fields.RejectStatusID")]
        public int RejectStatusID { get; set; }

        [NopResourceDisplayName("Plugins.Misc.FraudLabsPro.Fields.Balance")]
        public string Balance { get; set; }

        public List<SelectListItem> AvailableStatusLists { get; set; }

        #endregion
    }
}
