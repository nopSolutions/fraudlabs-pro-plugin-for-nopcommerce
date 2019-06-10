using System;
using Microsoft.AspNetCore.Mvc;
using Nop.Plugin.Misc.FraudLabsPro.Factories;
using Nop.Services.Cms;
using Nop.Services.Orders;
using Nop.Web.Areas.Admin.Models.Orders;
using Nop.Web.Framework.Components;
using Nop.Web.Framework.Infrastructure;

namespace Nop.Plugin.Misc.FraudLabsPro.Components
{
    /// <summary>
    /// Represents FraudLabsPro block view component
    /// </summary>
    [ViewComponent(Name = FraudLabsProDefaults.ORDER_VIEW_COMPONENT_NAME)]
    public class FraudLabsProOrderViewComponent : NopViewComponent
    {
        #region Fields

        private readonly FraudLabsProOrderModelFactory _fraudLabsProOrderModelFactory;
        private readonly FraudLabsProSettings _fraudLabsProSettings;
        private readonly IOrderService _orderService;
        private readonly IWidgetPluginManager _widgetPluginManager;

        #endregion

        #region Ctor

        public FraudLabsProOrderViewComponent(
            FraudLabsProOrderModelFactory fraudLabsProOrderModelFactory,
            FraudLabsProSettings fraudLabsSettings,
            IOrderService orderService,
            IWidgetPluginManager widgetPluginManager
            )
        {
            _fraudLabsProOrderModelFactory = fraudLabsProOrderModelFactory;
            _fraudLabsProSettings = fraudLabsSettings;
            _orderService = orderService;
            _widgetPluginManager = widgetPluginManager;
        }

        #endregion

        #region Methods

        public IViewComponentResult Invoke(string widgetZone, object additionalData)
        {
            if (string.IsNullOrEmpty(_fraudLabsProSettings.ApiKey))
                return Content("");

            if (!widgetZone?.Equals(AdminWidgetZones.OrderDetailsBlock, StringComparison.InvariantCultureIgnoreCase) ?? true)
                return Content(string.Empty);

            //check whether the payment plugin is active
            if (!_widgetPluginManager.IsPluginActive(FraudLabsProDefaults.SystemName))
                return Content(string.Empty);

            //get the view model
            if (!(additionalData is OrderModel orderModel))
                return Content(string.Empty);

            //try to get data to fill model
            var order = _orderService.GetOrderById(orderModel.Id);

            var model = _fraudLabsProOrderModelFactory.PrepareOrderModel(orderModel, order);

            return View("~/Plugins/Misc.FraudLabsPro/Views/Order/Edit.FraudLabsPro.cshtml", model);
        }

        #endregion
    }
}
