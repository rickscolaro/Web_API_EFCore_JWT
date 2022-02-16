
using Microsoft.AspNetCore.Mvc.Filters;

namespace APICatalogo_Fundamentos.Filters {

    public class ApiLogginFilter : IActionFilter {

        private readonly ILogger<ApiLogginFilter> _logger;

        // Injeção de dependência
        public ApiLogginFilter(ILogger<ApiLogginFilter> logger) {

            _logger = logger;
        }


        // Executa antes da action 
        public void OnActionExecuting(ActionExecutingContext context) {

            _logger.LogInformation("### Executando -> OnActionExecuting");
            _logger.LogInformation("###########################################################");
            _logger.LogInformation($"{DateTime.Now.ToLongTimeString()}");
            _logger.LogInformation($"ModelState: {context.ModelState.IsValid}");
            _logger.LogInformation("###########################################################");
        }


        // Executa depois da action
        public void OnActionExecuted(ActionExecutedContext context) {

            _logger.LogInformation("### Executando -> OnActionExecuted");
            _logger.LogInformation("###########################################################");
            _logger.LogInformation($"{DateTime.Now.ToLongTimeString()}");
             _logger.LogInformation("###########################################################");
        }


    }
}