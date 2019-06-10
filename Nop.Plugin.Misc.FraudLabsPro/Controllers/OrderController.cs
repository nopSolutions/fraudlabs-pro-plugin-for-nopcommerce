using FraudLabsPro.NetCore2.FraudLabsPro;
using Microsoft.AspNetCore.Mvc;
using Nop.Core;
using Nop.Plugin.Misc.FraudLabsPro.Services;
using Nop.Services.Orders;
using Nop.Services.Security;
using Nop.Web.Areas.Admin.Controllers;

namespace Nop.Plugin.Misc.FraudLabsPro.Controllers
{
    public class OrderController : BaseAdminController
    {
        #region Fields

        private readonly FraudLabsProManager _fraudLabsProManager;
        private readonly IOrderService _orderService;
        private readonly IPermissionService _permissionService;

        #endregion

        #region Ctor

        public OrderController(
            FraudLabsProManager fraudLabsProManager,
            IOrderService orderService,
            IPermissionService permissionService
            )
        {
            _fraudLabsProManager = fraudLabsProManager;
            _orderService = orderService;
            _permissionService = permissionService;
        }

        #endregion

        #region Methods

        [HttpPost]
        public IActionResult FraudLabsProOrderScreen(int orderId)
        {
            //whether user has the authority
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageOrders))
                return AccessDeniedView();

            var order = _orderService.GetOrderById(orderId);

            if (order != null)
            {
                var orderResult = _fraudLabsProManager.ScreenOrder(order);

                if (orderResult == null)
                    throw new NopException("FraundLabs Pro order screen error: Screen result returned null.");
            }

            return Json(new { Result = true });
        }

        [HttpPost]
        public IActionResult FraudLabsProOrderApprove(string transactionId, int orderId)
        {
            //whether user has the authority
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageOrders))
                return AccessDeniedView();

            if (!string.IsNullOrEmpty(transactionId))
            {
                var orderResult = _fraudLabsProManager.OrderFeedback(orderId, transactionId, Order.Action.APPROVE);

                if (orderResult == null)
                    throw new NopException("FraundLabs Pro approve error: approve result returned null.");
            }

            return Json(new { Result = true });
        }

        [HttpPost]
        public IActionResult FraudLabsProOrderReject(string transactionId, int orderId)
        {
            //whether user has the authority
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageOrders))
                return AccessDeniedView();

            if (!string.IsNullOrEmpty(transactionId))
            {
                var orderResult = _fraudLabsProManager.OrderFeedback(orderId, transactionId, Order.Action.REJECT);

                if (orderResult == null)
                    throw new NopException("FraundLabs Pro feedback error: feedback result returned null.");
            }

            return Json(new { Result = true });
        }

        [HttpPost]
        public IActionResult FraudLabsProOrderBlackList(string transactionId, int orderId)
        {
            //whether user has the authority
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageOrders))
                return AccessDeniedView();

            if (!string.IsNullOrEmpty(transactionId))
            {
                var orderResult = _fraudLabsProManager.OrderFeedback(orderId, transactionId, Order.Action.REJECT_BLACKLIST);

                if (orderResult == null)
                    throw new NopException("FraundLabs Pro black list error: black list result returned null.");
            }

            return Json(new { Result = true });
        }

        #endregion
    }
}