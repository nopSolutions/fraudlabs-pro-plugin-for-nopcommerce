using Nop.Core.Domain.Orders;
using Nop.Services.Cms;
using Nop.Services.Events;
using Nop.Web.Framework.Events;
using Nop.Web.Framework.UI;

namespace Nop.Plugin.Misc.FraudLabsPro.Services
{
    /// <summary>
    /// Represents a FraudLabsPro event consumer
    /// </summary>
    public class EventConsumer :
        IConsumer<OrderPlacedEvent>,
        IConsumer<PageRenderingEvent>
    {
        #region Fields

        private readonly FraudLabsProManager _fraudLabsProManager;
        private readonly IWidgetPluginManager _widgetPluginManager;

        #endregion

        #region Ctor

        public EventConsumer(
            FraudLabsProManager fraudLabsProManager,
            IWidgetPluginManager widgetPluginManager
            )
        {
            _fraudLabsProManager = fraudLabsProManager;
            _widgetPluginManager = widgetPluginManager;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Handle the order placed event
        /// </summary>
        /// <param name="eventMessage">The event message.</param>
        public void HandleEvent(OrderPlacedEvent eventMessage)
        {
            //check whether the plugin is active
            if (!_widgetPluginManager.IsPluginActive(FraudLabsProDefaults.SystemName))
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

            //check whether the plugin is active
            if (!_widgetPluginManager.IsPluginActive(FraudLabsProDefaults.SystemName))
                return;

            //add js sсript to the one page checkout
            var routeName = eventMessage.GetRouteName() ?? string.Empty;
            if (routeName == FraudLabsProDefaults.OnePageCheckoutRouteName || routeName == FraudLabsProDefaults.ConfirmCheckoutRouteName)
            {
                eventMessage.Helper.AddScriptParts(ResourceLocation.Footer, FraudLabsProDefaults.AgentScriptPath, excludeFromBundle: true);
            }
        }

        #endregion
    }
}
