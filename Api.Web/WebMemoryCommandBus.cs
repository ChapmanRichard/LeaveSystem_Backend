using Api.Common.Commands;
using Api.Common.Json;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.ComponentModel.DataAnnotations;

namespace Api.Web
{
    public interface IWebCommandBus : ICommandBus
    {
        string SubmitAndReturnJsonResult<TResult>(ICommand<TResult> command);
        TResult SubmitAndReturnResult<TResult>(ICommand<TResult> command);
    }

    public class WebMemoryCommandBus : MemoryCommandBus, IWebCommandBus
    {

        public WebMemoryCommandBus(IServiceScopeFactory scopeFactory) : base(scopeFactory)
        {
        }

        public string SubmitAndReturnJsonResult<TResult>(ICommand<TResult> command)
        {
            //var commandbase  = command as CommandBase;
            //if(commandbase != null)
            //{
            //    commandbase.CreatedUser = SessionUtil.LOGIN.HRSD_NAME;
            //    commandbase.CreatedRole = SessionUtil.CURRENT_USER_POST;
            //}


            string ErrorMsg = string.Empty;
            object Data = null;
            try
            {
                Data = base.Submit(command);
            }
            catch (ValidationException e)
            {
                ErrorMsg = e.Message;
            }
            catch (InvalidOperationException e)
            {
                ErrorMsg = e.Message;
            }
            catch
            {
                //if ((bool)e.InnerException?.Message.Contains("REFERENCE 约束"))
                //{
                //    ErrorMsg = "Cannot delete as other data exists.";
                //}
                //else
                //    ErrorMsg = e.Message;
                ErrorMsg = "System error. Please contact your system administrator.";
            }

            if (ErrorMsg == string.Empty)
            {
                return JsonResultFactory.CreateSuccessResult(Data);
                //new { Result = "TRUE", ERROR_CODE = "", DATA = data  }
            }
            else
            {
                return JsonResultFactory.CreateFailResult("ERR", ErrorMsg);
                // new { Result = "FAILURE", ERROR_CODE = err_code, MESSAGE = message }
            }

        }
        public TResult SubmitAndReturnResult<TResult>(ICommand<TResult> command)
        {
            string ErrorMsg = string.Empty;
            TResult Data = default(TResult);
            try
            {
                Data = base.Submit(command);
            }
            catch (ValidationException e)
            {
                ErrorMsg = e.Message;
                throw;
            }
            catch (InvalidOperationException e)
            {
                ErrorMsg = e.Message;
                throw;
            }
            catch
            {
                //if ((bool)e.InnerException?.Message.Contains("REFERENCE 约束"))
                //{
                //    ErrorMsg = "Cannot delete as other data exists.";
                //}
                //else
                //    ErrorMsg = e.Message;
                ErrorMsg = "System error. Please contact your system administrator.";
                throw;
            }
            return Data;
        }
    }
}
