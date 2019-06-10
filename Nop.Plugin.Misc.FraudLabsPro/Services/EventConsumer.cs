using System.Linq;
using Nop.Core.Domain.Orders;
using Nop.Services.Events;
using Nop.Services.Plugins;
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
        private readonly IPluginService _pluginService;

        #endregion

        #region Ctor

        public EventConsumer(
            FraudLabsProManager fraudLabsProManager,
            IPluginService pluginService
            )
        {
            _fraudLabsProManager = fraudLabsProManager;
            _pluginService = pluginService;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Handle the order placed event
        /// </summary>
        /// <param name="eventMessage">The event message.</param>
        public void HandleEvent(OrderPlacedEvent eventMessage)
        {
            //check whether the plugin is installed
            var pluginDescriptor = _pluginService.GetPluginDescriptorBySystemName<IPlugin>(FraudLabsProDefaults.SystemName, LoadPluginsMode.InstalledOnly);
            if (pluginDescriptor == null)
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
            var pluginDescriptor = _pluginService.GetPluginDescriptorBySystemName<IPlugin>(FraudLabsProDefaults.SystemName, LoadPluginsMode.InstalledOnly);
            if (pluginDescriptor == null)
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
