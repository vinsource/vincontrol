using System;
using System.Runtime.Serialization.Json;
using System.Web.Mvc;

namespace WhitmanEnterpriseMVC.HelperClass
{
    public class DataContractJsonResult : JsonResult
    {
        public DataContractJsonResult(Object data) { Data = data; }
        public override void ExecuteResult(ControllerContext context)
        {
            var serializer = new DataContractJsonSerializer(Data.GetType());
            context.HttpContext.Response.ContentType = "application/json";
            serializer.WriteObject(context.HttpContext.Response.OutputStream,
                                   Data);
        }
    }
}