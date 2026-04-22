using Newtonsoft.Json;
using System.Collections.Generic;


namespace Api.Common.Json
{
    public class JsonAPIResult
    {
        public string RESULT { get; set; }
        public string ERROR_CODE { get; set; }
        public object DATA { get; set; }
    }
    public class JsonResultFactory
    {
        public static string CreateSuccessResult(object data)
        {
            return JsonConvert.SerializeObject(new { RESULT = "TRUE", ERROR_CODE = "", DATA = data }, Formatting.Indented,
                                                    new JsonSerializerSettings
                                                    {
                                                        ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                                                    });
        }

        public static string CreateFailResult(string err_code, string message)
        {
            return JsonConvert.SerializeObject(new { RESULT = "FAILURE", ERROR_CODE = err_code, MESSAGE = message });
        }
        public static string CreateFailResult(string err_code, string message, List<string> errorMsgList)
        {
            return JsonConvert.SerializeObject(new { RESULT = "FAILURE", ERROR_CODE = err_code, MESSAGE = message, ERROR_MSG_LIST = errorMsgList });
        }

        public static string CreateAPISingleSuccessResult(object data, object viewModel, string message)
        {
            return JsonConvert.SerializeObject(new { Result = "SUCCESS", MESSAGE = message, DATA = new { RECORD = data, VIEW_MODEL = viewModel } }, Formatting.Indented,
                                                    new JsonSerializerSettings
                                                    {
                                                        ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                                                    });
        }

        public static string CreateAPIListSuccessResult(object data, object viewModel, int totalRecord, string message)
        {
            return JsonConvert.SerializeObject(new { RESULT = "SUCCESS", MESSAGE = message, TOTAL_RECORD = totalRecord, DATA = new { RECORD = data, VIEW_MODEL = viewModel } }, Formatting.Indented,
                                                    new JsonSerializerSettings
                                                    {
                                                        ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                                                    });
        }


        public static string CreateAPIFailResult(string message)
        {
            return JsonConvert.SerializeObject(new { RESULT = "FAILURE", MESSAGE = message }, Formatting.Indented,
                                                    new JsonSerializerSettings
                                                    {
                                                        ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                                                    });
        }


    }

}