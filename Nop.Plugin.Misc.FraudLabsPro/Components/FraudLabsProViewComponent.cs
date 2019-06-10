using Microsoft.AspNetCore.Mvc;
using Nop.Plugin.Misc.FraudLabsPro.Models;
using Nop.Web.Framework.Components;

namespace Nop.Plugin.Misc.FraudLabsPro.Components
{
    /// <summary>
    /// Represents view component
    /// </summary>
    [ViewComponent(Name = FraudLabsProDefaults.SEAL_VIEW_COMPONENT_NAME)]
    public class FraudLabsProViewComponent : NopViewComponent
    {
        #region Fields

        private readonly FraudLabsProSettings _fraudLabsProSettings;

        #endregion

        #region Ctor

        public FraudLabsProViewComponent(FraudLabsProSettings fraudLabsSettings)
        {
            _fraudLabsProSettings = fraudLabsSettings;
        }

        #endregion

        #region Methods

        public IViewComponentResult Invoke(string widgetZone, object additionalData)
        {
            if (string.IsNullOrEmpty(_fraudLabsProSettings.ApiKey))
                return Content("");

            var model = new PublicInfoModel
            {
                HrefUrl = FraudLabsProDefaults.SecuredSealHrefUrl,
                LinkSrc = FraudLabsProDefaults.SecuredSealLinkSrc
            };

            return View("~/Plugins/Misc.FraudLabsPro/Views/PublicInfo.cshtml", model);
        }

        #endregion
    }
}
