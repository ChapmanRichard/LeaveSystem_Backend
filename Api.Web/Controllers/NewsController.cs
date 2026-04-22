using Api.Common;
using Api.Common.Providers;
using Api.Common.Queries;
using Api.Contract.Commands;
using Api.Contract.Queries;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

using Api.Web;
using Api.Common.Helper;

namespace Api.Web.Controllers;

[ApiController]
[Route("api/[controller]")]
public class NewsController : BaseController
{
    private readonly string IndexTitle = "News";
    public NewsController(IQueryProcessor queryProcessor, IWebCommandBus commandBus, IConfiguration configuration) : base(queryProcessor, commandBus)
    {
    }

    #region Api
    [HttpPost]
    [AllowAnonymous]
    public IActionResult Grid(GetNewsDataTableQuery query)
    {
        var result = this.QueryProcessor.ProcessSingleQuery(query);
        return new JsonResult(result);
    }
    [HttpPost("Create")]
    [Authorize]
    public IActionResult Create(CreateNewsCommand command)
    {
        var result = this.CommandBus.SubmitAndReturnJsonResult(command);
        return Content(result, "application/json");
    }
    [HttpPost("Edit")]
    [Authorize]
    public IActionResult Edit(EditNewsCommand command)
    {
        var result = this.CommandBus.SubmitAndReturnJsonResult(command);
        return Content(result, "application/json");
    }
    [HttpPost("Delete")]
    [Authorize]
    public IActionResult Delete(DeleteNewsCommand command)
    {
        var result = this.CommandBus.SubmitAndReturnJsonResult(command);
        return Content(result, "application/json");
    }
    #region Auto Comman Code
    #endregion

    #endregion
}

public class BaseController : ControllerBase
{
    private readonly IQueryProcessor queryProcessor;
    private readonly IWebCommandBus commandBus;

    public IQueryProcessor QueryProcessor
    {
        get { return queryProcessor; }
        //set { queryProcessor = value; }
    }

    public IWebCommandBus CommandBus { get { return commandBus; } }
    public BaseController(IQueryProcessor queryProcessor, IWebCommandBus commandBus)
    {
        this.queryProcessor = queryProcessor;
        this.commandBus = commandBus;
    }
}