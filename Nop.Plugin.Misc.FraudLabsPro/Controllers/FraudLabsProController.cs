using Microsoft.AspNetCore.Mvc;
using Nop.Core.Domain.Orders;
using Nop.Plugin.Misc.FraudLabsPro.Models;
using Nop.Services;
using Nop.Services.Configuration;
using Nop.Services.Localization;
using Nop.Services.Messages;
using Nop.Web.Framework;
using Nop.Web.Framework.Controllers;
using Nop.Web.Framework.Mvc.Filters;

namespace Nop.Plugin.Misc.FraudLabsPro.Controllers
{
    public class FraudLabsProController : BasePluginController
    {
        #region Fields

        private readonly FraudLabsProSettings _fraudLabsProSettings;
        private readonly ILocalizationService _localizationService;
        private readonly INotificationService _notificationService;
        private readonly ISettingService _settingService;

        #endregion

        #region Ctor

        public FraudLabsProController(
            FraudLabsProSettings fraudLabsProSettings,
            ILocalizationService localizationService,
            INotificationService notificationService,
            ISettingService settingService
            )
        {
            _fraudLabsProSettings = fraudLabsProSettings;
            _localizationService = localizationService;
            _notificationService = notificationService;
            _settingService = settingService;
        }

        #endregion

        #region Utilities

        /// <summary>
        /// Prepare FraudLabsProModel
        /// </summary>
        /// <param name="model">Model</param>
        protected void PrepareModel(ConfigurationModel model)
        {
            //prepare common properties
            model.ApiKey = _fraudLabsProSettings.ApiKey;
            model.ApproveStatusID = _fraudLabsProSettings.ApproveStatusID;
            model.ReviewStatusID = _fraudLabsProSettings.ReviewStatusID;
            model.RejectStatusID = _fraudLabsProSettings.RejectStatusID;
            model.Balance = _fraudLabsProSettings.Balance;

            //prepare available order statuses
            var availableStatusItems = OrderStatus.Pending.ToSelectList(false);
            foreach (var statusItem in availableStatusItems)
            {
                model.AvailableStatusLists.Add(statusItem);
            }
        }

        #endregion

        #region Methods

        [AuthorizeAdmin]
        [Area(AreaNames.Admin)]
        public IActionResult Configure()
        {
            var model = new ConfigurationModel();
            PrepareModel(model);

            return View("~/Plugins/Misc.FraudLabsPro/Views/Configure.cshtml", model);
        }

        [AuthorizeAdmin]
        [Area(AreaNames.Admin)]
        [HttpPost, ActionName("Configure")]
        [FormValueRequired("save")]
        public IActionResult Configure(ConfigurationModel model)
        {
            if (!ModelState.IsValid)
                return Configure();

            //set API key
            _fraudLabsProSettings.ApiKey = model.ApiKey;
            _settingService.SaveSetting(_fraudLabsProSettings, x => x.ApiKey, clearCache: false);

            //set Approve status
            _fraudLabsProSettings.ApproveStatusID = model.ApproveStatusID;
            _settingService.SaveSetting(_fraudLabsProSettings, x => x.ApproveStatusID, clearCache: false);

            //set Review status
            _fraudLabsProSettings.ReviewStatusID = model.ReviewStatusID;
            _settingService.SaveSetting(_fraudLabsProSettings, x => x.ReviewStatusID, clearCache: false);

            //set Reject status
            _fraudLabsProSettings.RejectStatusID = model.RejectStatusID;
            _settingService.SaveSetting(_fraudLabsProSettings, x => x.RejectStatusID, clearCache: false);

            //now clear settings cache
            _settingService.ClearCache();

            _notificationService.SuccessNotification(_localizationService.GetResource("Admin.Plugins.Saved"));

            return Configure();
        }

        #endregion

    }
}
