using Api.Common;
using Api.Common.Queries;
using Api.Contract.Commands;
using Api.Contract.Queries;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace Api.Web.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class StockBaseController : BaseController
    {
        public StockBaseController(IQueryProcessor queryProcessor, IWebCommandBus commandBus, IConfiguration configuration) : base(queryProcessor, commandBus)
        {
        }

        [HttpPost]
        [AllowAnonymous]
        public IActionResult Grid(GetStockBaseDataTableQuery query)
        {
            var result = this.QueryProcessor.ProcessSingleQuery(query);
            return new JsonResult(result);
        }

        [HttpPost("Create")]
        [Authorize]
        public IActionResult Create(CreateStockBaseCommand command)
        {
            var result = this.CommandBus.SubmitAndReturnJsonResult(command);
            return Content(result, "application/json");
        }

        [HttpPost("Edit")]
        [Authorize]
        public IActionResult Edit(EditStockBaseCommand command)
        {
            var result = this.CommandBus.SubmitAndReturnJsonResult(command);
            return Content(result, "application/json");
        }

        [HttpPost("Delete")]
        [Authorize]
        public IActionResult Delete(DeleteStockBaseCommand command)
        {
            var result = this.CommandBus.SubmitAndReturnJsonResult(command);
            return Content(result, "application/json");
        }
    }
}
