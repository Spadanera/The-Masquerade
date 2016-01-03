using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;

namespace The_Masquerade.Test
{
    /// <summary>
    /// Summary description for GetPdf
    /// </summary>
    public class GetPdf : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            var mobile = context.Request.QueryString["mobile"];
            context.Response.ContentType = "application/pdf";
            WebClient MyCli = new WebClient();
            byte[] bytes = MyCli.DownloadData(HttpContext.Current.Server.MapPath("~/Test/RIT_Ferrero_ASSI000106819.pdf"));
            var base64EncodedPDF = System.Convert.ToBase64String(bytes);

            if (mobile == "Y")
            {
                context.Response.AddHeader("Content-Length", base64EncodedPDF.Length.ToString());
                context.Response.Write(base64EncodedPDF);
            }
            else
            {
                context.Response.AddHeader("Content-Length", bytes.Length.ToString());
                context.Response.BinaryWrite(bytes);
            }
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}