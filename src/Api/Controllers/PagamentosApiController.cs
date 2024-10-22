using Controllers;
using Core.Domain.Notificacoes;
using Core.WebApi.Controller;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [Authorize(Policy = "AdminRole")]
    [Route("pagamentos")]
    public class PagamentosApiController(IPagamentoController pagamentoController, INotificador notificador) : MainController(notificador)
    {
        [HttpGet("{pedidoId:guid}")]
        public async Task<IActionResult> ObterPagamentoPorPedido([FromRoute] Guid pedidoId, CancellationToken cancellationToken)
        {
            var result = await pagamentoController.ObterPagamentoPorPedidoAsync(pedidoId, cancellationToken);

            return CustomResponseGet(result);
        }

        [AllowAnonymous]
        [HttpPost("checkout/{pedidoId:guid}")]
        public async Task<IActionResult> Checkout([FromRoute] Guid pedidoId, CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
            {
                return ErrorBadRequestModelState(ModelState);
            }

            var result = await pagamentoController.EfetuarCheckoutAsync(pedidoId, cancellationToken);

            return CustomResponsePutPatch(pedidoId, result);
        }

        // WebHook
        [AllowAnonymous]
        [HttpPost("notificacoes/{pedidoId:guid}")]
        public async Task<IActionResult> Notificacoes([FromRoute] Guid pedidoId, CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
            {
                return ErrorBadRequestModelState(ModelState);
            }

            var result = await pagamentoController.NotificarPagamentoAsync(pedidoId, cancellationToken);

            return CustomResponsePutPatch(pedidoId, result);
        }
    }
}
