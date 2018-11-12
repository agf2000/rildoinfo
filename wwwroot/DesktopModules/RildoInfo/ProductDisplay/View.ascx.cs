
using DotNetNuke.Entities.Modules;
using DotNetNuke.Web.Client.ClientResourceManagement;
using System;
using System.Text;

namespace RIW.Modules.ProductDisplay
{
    public partial class View : PortalModuleBase
    {

        private const string RiwModulePathScript = "mod_ModulePathScript";

        public void RegisterModuleInfo()
        {
            //Dim modCtrl As New Entities.Modules.ModuleController()

            dynamic scriptblock = new StringBuilder();
            scriptblock.Append("<script>");
            scriptblock.Append(string.Format("var ascxVersion = \"{0}\"; ", System.IO.File.GetLastWriteTime(string.Format("{0}DesktopModules\\{1}\\View.ascx", Request.PhysicalApplicationPath, this.ModuleConfiguration.DesktopModule.FolderName)).ToString("g")));
            scriptblock.Append(string.Format("var jsVersion = \"{0}\"; ", System.IO.File.GetLastWriteTime(string.Format("{0}DesktopModules\\{1}\\view.js", Request.PhysicalApplicationPath, this.ModuleConfiguration.DesktopModule.FolderName)).ToString("g")));
            scriptblock.Append("</script>");

            // register scripts
            if (!Page.ClientScript.IsClientScriptBlockRegistered(RiwModulePathScript))
            {
                Page.ClientScript.RegisterClientScriptBlock(GetType(), RiwModulePathScript, scriptblock.ToString());
            }
        }

        private void Page_Init(object sender, EventArgs e)
        {
            ClientResourceManager.RegisterStyleSheet(Parent.Page, "/desktopmodules/rildoinfo/webapi/content/owl-carousel/owl.carousel.css", 71);
            ClientResourceManager.RegisterStyleSheet(Parent.Page, "/desktopmodules/rildoinfo/webapi/content/owl-carousel/owl.theme.css", 72);
            ClientResourceManager.RegisterStyleSheet(Parent.Page, "/desktopmodules/rildoinfo/webapi/content/owl-carousel/owl.transitions.css", 73);


            ClientResourceManager.RegisterScript(Parent.Page, "/desktopmodules/rildoinfo/webapi/scripts/owl-carousel/owl.carousel.min.js", 21);
            ClientResourceManager.RegisterScript(Parent.Page, string.Format("DesktopModules/{0}/ViewModels/view.js", this.ModuleConfiguration.DesktopModule.FolderName), 99);
            RegisterModuleInfo();
        }

    }
}