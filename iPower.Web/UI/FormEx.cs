using System;
using System.Collections.Generic;
using System.Text;

using System.Web;
using System.Web.UI;
using System.Web.UI.Design;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.ComponentModel;
using System.Security.Permissions;
namespace iPower.Web.UI
{
    /// <summary>
    /// The Form class extends the HtmlForm HTML control by overriding its RenderAttributes()
    /// method and NOT emitting an action attribute.
    /// </summary>
    [ToolboxData("<{0}:FormEx runat=server></{0}:FormEx>")]
    [Designer(typeof(FormExControlDesigner))]
    [AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal), 
    AspNetHostingPermission(SecurityAction.InheritanceDemand, Level = AspNetHostingPermissionLevel.Minimal)]
    public class FormEx : HtmlForm, INamingContainer
    {
        /// <summary>
        /// The RenderAttributes method adds the attributes to the rendered &lt;form&gt; tag.
        /// We override this method so that the action attribute is not emitted.
        /// </summary>
        protected override void RenderAttributes(HtmlTextWriter writer)
        {
            // write the form's name
            writer.WriteAttribute("name", this.Name);
            base.Attributes.Remove("name");

            // write the form's method
            writer.WriteAttribute("method", this.Method);
            base.Attributes.Remove("method");

            // remove the action attribute
            base.Attributes.Remove("action");

            // finally write all other attributes
            this.Attributes.Render(writer);

            if (base.ID != null)
                writer.WriteAttribute("id", base.ClientID);
        }
    }

    /// <summary>
    /// FormEx的设计时支持。
    /// </summary>
    public class FormExControlDesigner : ContainerControlDesigner
    {
        #region 成员变量，构造函数。
        //const string FormExNoCaptionDesignTimeHtml = "<div style=\"{0}{2}{3}{4}{6}{10}\" class=\"{11}\" {7}=0></div>";
        /// <summary>
        /// 构造函数。
        /// </summary>
        public FormExControlDesigner()
        {
        }
        #endregion

        #region 重载。
        /// <summary>
        /// 
        /// </summary>
        public override string FrameCaption
        {
            get
            {
                return string.Empty;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public override Style FrameStyle
        {
            get
            {
                if (string.IsNullOrEmpty(this.FrameCaption))
                    return new Style();
                return base.FrameStyle;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        protected override bool UsePreviewControl
        {
            get
            {
                return true;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public override bool AllowResize
        {
            get
            {
                return true;
            }
        }
        #endregion
    }
}
