<%@ WebHandler Language="C#" Class="UEditorHandler" %>

using System;
using System.Web;
using System.IO;
using System.Collections;
using Newtonsoft.Json;
public class UEditorHandler : IHttpHandler
{
    public void ProcessRequest(HttpContext context)
    {
        Handler action = null;
        int supplierId = 0;
        if (!string.IsNullOrEmpty(context.Request.Form["supplierId"]))
            supplierId = int.Parse(context.Request.Form["supplierId"]);
        switch (context.Request["action"])
        {
            case "config":
                action = new ConfigHandler(context);
                break;
            case "uploadimage":
                var imgPath = Config.GetString("imagePathFormat");
                if (supplierId > 0)
                {
                    imgPath = Config.GetString("imagePathFormat4Supplier");
                }
                action = new UploadHandler(context, new UploadConfig()
                {
                    AllowExtensions = Config.GetStringList("imageAllowFiles"),
                    PathFormat = imgPath,
                    SizeLimit = Config.GetInt("imageMaxSize"),
                    UploadFieldName = Config.GetString("imageFieldName")
                }, supplierId);
                break;
            case "uploadtemplateimage":
                action = new UploadTemplateImagesHandler(context, new UploadConfig()
                {
                    AllowExtensions = Config.GetStringList("templateimageAllowFiles"),
                    PathFormat = Config.GetString("templateimagePathFormat"),
                    SizeLimit = Config.GetInt("templateimageMaxSize"),
                    UploadFieldName = Config.GetString("templateimageFieldName")
                });
                break;
            case "uploadscrawl":
                action = new UploadHandler(context, new UploadConfig()
                {
                    AllowExtensions = new string[] { ".png" },
                    PathFormat = Config.GetString("scrawlPathFormat"),
                    SizeLimit = Config.GetInt("scrawlMaxSize"),
                    UploadFieldName = Config.GetString("scrawlFieldName"),
                    Base64 = true,
                    Base64Filename = "scrawl.png"
                }, supplierId);
                break;
            case "uploadvideo":
                action = new UploadHandler(context, new UploadConfig()
                {
                    AllowExtensions = Config.GetStringList("videoAllowFiles"),
                    PathFormat = Config.GetString("videoPathFormat"),
                    SizeLimit = Config.GetInt("videoMaxSize"),
                    UploadFieldName = Config.GetString("videoFieldName")
                }, supplierId);
                break;
            case "uploadfile":
                action = new UploadHandler(context, new UploadConfig()
                {
                    AllowExtensions = Config.GetStringList("fileAllowFiles"),
                    PathFormat = Config.GetString("filePathFormat"),
                    SizeLimit = Config.GetInt("fileMaxSize"),
                    UploadFieldName = Config.GetString("fileFieldName")
                }, supplierId);
                break;
            case "listimage":
                action = new ListFileManager(context, Config.GetString("imageManagerListPath"), Config.GetStringList("imageManagerAllowFiles"));
                break;
            case "listfile":
                action = new ListFileManager(context, Config.GetString("fileManagerListPath"), Config.GetStringList("fileManagerAllowFiles"));
                break;
            case "catchimage":
                action = new CrawlerHandler(context);
                break;
            default:
                action = new NotSupportedHandler(context);
                break;
        }
        action.Process();
    }

    public bool IsReusable
    {
        get
        {
            return false;
        }
    }
}