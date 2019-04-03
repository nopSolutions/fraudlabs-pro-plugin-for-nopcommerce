using System.Linq;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json.Linq;
using Nop.Core.Domain.Orders;
using Nop.Plugin.Misc.FraudLabsPro.Models.Order;
using Nop.Plugin.Misc.SendInBlue.Services;
using Nop.Services.Common;
using Nop.Services.Events;
using Nop.Services.Localization;
using Nop.Services.Orders;
using Nop.Services.Plugins;
using Nop.Web.Areas.Admin.Models.Orders;
using Nop.Web.Framework.Events;
using Nop.Web.Framework.Extensions;
using Nop.Web.Framework.UI;

namespace Nop.Plugin.Misc.FraudLabsPro.Services
{
    /// <summary>
    /// Represents a FraudLabsPro event consumer
    /// </summary>
    public class EventConsumer :
        IConsumer<AdminTabStripCreated>,
        IConsumer<OrderPlacedEvent>,
        IConsumer<PageRenderingEvent>
    {

        #region Fields

        private readonly FraudLabsProManager _fraudLabsProManager;
        private readonly IGenericAttributeService _genericAttributeService;
        private readonly ILocalizationService _localizationService;
        private readonly IOrderService _orderService;
        private readonly IPluginFinder _pluginFinder;

        #endregion

        #region Ctor

        public EventConsumer(
            FraudLabsProManager fraudLabsProManager,
            IGenericAttributeService genericAttributeService,
            ILocalizationService localizationService,
            IOrderService orderService,
            IPluginFinder pluginFinder            
            )
        {
            _fraudLabsProManager = fraudLabsProManager;
            _genericAttributeService = genericAttributeService;
            _localizationService = localizationService;
            _orderService = orderService;
            _pluginFinder = pluginFinder;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Handle admin tabstrip created event
        /// </summary>
        /// <param name="eventMessage">Event message</param>
        public void HandleEvent(AdminTabStripCreated eventMessage)
        {
            if (eventMessage?.Helper == null)
                return;

            //we need customer details page
            var fraudLabsProTabId = FraudLabsProDefaults.FraudLabsProTabId;
            var orderTabsId = "order-edit";
            if (!eventMessage.TabStripName.Equals(orderTabsId))
                return;

            //check whether the plugin is installed
            var pluginDescriptor = _pluginFinder.GetPluginDescriptorBySystemName(FraudLabsProDefaults.SystemName);
            if (pluginDescriptor == null || !pluginDescriptor.Installed)
                return;

            //get the view model
            if (!(eventMessage.Helper.ViewData.Model is OrderModel orderModel))
                return;

            //try to get data to fill model
            var order = _orderService.GetOrderById(orderModel.Id);

            //prepare model
            var model = new FraudLabsProOrderModel
            {
                Id = orderModel.Id,
                UserOrderID = order.Id.ToString()
            };

            var stringResponse = _genericAttributeService.GetAttribute<string>(order, FraudLabsProDefaults.OrderResultAttribute);

            if (!string.IsNullOrEmpty(stringResponse))
            {
                var response = JObject.Parse(stringResponse);
                var orderResultModel = response.ToObject<FraudLabsProOrderModel>();

                orderResultModel.Id = orderModel.Id;
                orderResultModel.IPAddress = order.Customer.LastIpAddress;
                orderResultModel.IPCountry = ISO3166.FromCountryCode(orderResultModel.IPCountry)?.Name ?? "-";
                orderResultModel.FraudLabsProOriginalStatus = _genericAttributeService.GetAttribute<string>(order, FraudLabsProDefaults.OrderStatusAttribute) ?? string.Empty;
                model = orderResultModel;
            }

            //compose script to create a new tab
            var fraudLabsProOrderTab = new HtmlString($@"
                <script>
                    $(document).ready(function() {{
                        $(`
                            <li>
                                <a data-tab-name='{fraudLabsProTabId}' data-toggle='tab' href='#{fraudLabsProTabId}'>
                                    {_localizationService.GetResource("Plugins.Misc.FraudLabsPro.Order")}
                                </a>
                            </li>
                        `).appendTo('#{orderTabsId} .nav-tabs:first');
                        $(`
                            <div class='tab-pane' id='{fraudLabsProTabId}'>
                                {
                                    eventMessage.Helper.Partial("~/Plugins/Misc.FraudLabsPro/Views/Order/Edit.FraudLabsPro.cshtml", model).RenderHtmlContent()
                                        .Replace("</script>", "<\\/script>") //we need escape a closing script tag to prevent terminating the script block early
                                }
                            </div>
                        `).appendTo('#{orderTabsId} .tab-content:first');
                    }});
                </script>");

            //add this tab as a block to render on the order details page
            eventMessage.BlocksToRender.Add(fraudLabsProOrderTab);
        }

        /// <summary>
        /// Handle the order placed event
        /// </summary>
        /// <param name="eventMessage">The event message.</param>
        public void HandleEvent(OrderPlacedEvent eventMessage)
        {
            //check whether the plugin is installed
            var pluginDescriptor = _pluginFinder.GetPluginDescriptorBySystemName(FraudLabsProDefaults.SystemName);
            if (pluginDescriptor == null || !pluginDescriptor.Installed)
                return;

            //handle event
            _fraudLabsProManager.HandleOrderPlacedEvent(eventMessage.Order);
        }

        /// <summary>
        /// Handle page rendering event
        /// </summary>
        /// <param name="eventMessage">Event message</param>
        public void HandleEvent(PageRenderingEvent eventMessage)
        {
            if (eventMessage?.Helper?.ViewContext == null)
                return;

            //check whether the plugin is installed
            var pluginDescriptor = _pluginFinder.GetPluginDescriptorBySystemName(FraudLabsProDefaults.SystemName);
            if (pluginDescriptor == null || !pluginDescriptor.Installed)
                return;

            //add js sсript to the one page checkout
            if (eventMessage.GetRouteNames().Any(r => r.Equals(FraudLabsProDefaults.OnePageCheckoutRouteName) || r.Equals(FraudLabsProDefaults.ConfirmCheckoutRouteName)))
            {
                eventMessage.Helper.AddScriptParts(ResourceLocation.Footer, FraudLabsProDefaults.AgentScriptPath, excludeFromBundle: true);
            }
        }

        #endregion
    }
}
