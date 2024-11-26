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
            try
            {
                var result = await pagamentoController.ObterPagamentoPorPedidoAsync(pedidoId, cancellationToken);
                return string.IsNullOrEmpty(result)
                    ? NotFound(new { Success = false, Errors = new[] { "Pagamento não encontrado" } })
                    : CustomResponseGet(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Success = false, Errors = new[] { ex.Message } });
            }
        }

        [AllowAnonymous]
        [HttpPost("checkout/{pedidoId:guid}")]
        public async Task<IActionResult> Checkout([FromRoute] Guid pedidoId, CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
            {
                return ErrorBadRequestModelState(ModelState);
            }

            try
            {
                var result = await pagamentoController.EfetuarCheckoutAsync(pedidoId, cancellationToken);
                return !result
                    ? BadRequest(new { Success = false, Errors = new[] { "Erro ao efetuar checkout" } })
                    : CustomResponsePutPatch(pedidoId, result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Success = false, Errors = new[] { ex.Message } });
            }
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

            try
            {
                var result = await pagamentoController.NotificarPagamentoAsync(pedidoId, cancellationToken);
                return !result
                    ? BadRequest(new { Success = false, Errors = new[] { "Erro ao notificar pagamento" } })
                    : CustomResponsePutPatch(pedidoId, result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Success = false, Errors = new[] { ex.Message } });
            }
        }
    }
}
