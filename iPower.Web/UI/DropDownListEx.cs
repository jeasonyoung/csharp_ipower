
using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.Design.WebControls;
using System.ComponentModel;
using System.Collections;
using System.Data;
using System.Drawing;
using System.Text;
using System.Security.Permissions;

using iPower.Utility;
using iPower.Platform;
namespace iPower.Web.UI
{
	/// <summary>
	/// 允许用户从下拉列表中选择一项的控件,
	/// 扩展功能：
	///		1.DataTextFiled支持多个数据列绑定,DataTextField多个字段之间用[,]分开;
	///		2.支持树状选项列表的显示(ShowTreeView属性);
	///		3.支持显示表示未定义选项或表示全部选项的选项
	///		(在ShowUnDefine属性为true时，通过UnDefineTitle,UndefineValue的值来构造选项;
	/// </summary>
	///<example>下面例子演示了DropDownListEx控件的各种用法
	///<code>
	///<![CDATA[
	///DataSet ds=CreateDataSet();
	/// //显示树状
	///ddl.ShowTreeView=true;
	///ddl.DataValueField="SystemID";
	///ddl.DataValueParentField="ParentSystemID";
	///ddl.DataTextField="SystemName";
	///ddl.DataSource=ds;
	///ddl.DataBind();
	/// //显示平板状
	///ddl.DataValueField="SystemID";
	///ddl.DataTextField="SystemName";
	///ddl.DataSource=ds;
	///ddl.DataBind();
	/// //显示未定义选项
	///ddl.ShowUnDefine=true;
	///ddl.UnDefineTitle="=====全部======";
	///ddl.UnDefineValue="";
	///ddl.DataValueField="SystemID";
	///ddl.DataTextField="SystemName";
	///ddl.DataSource=ds;
	///ddl.DataBind();
	/// ]]>
	///</code>
	///</example>
    [DefaultProperty("DataSource")]
    [ToolboxData("<{0}:DropDownListEx runat=server></{0}:DropDownListEx>")]
    [ToolboxBitmap(typeof(DropDownListEx))]
    [AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal),
     AspNetHostingPermission(SecurityAction.InheritanceDemand, Level = AspNetHostingPermissionLevel.Minimal)]
    public class DropDownListEx : DropDownList, IValidator, IDataDropDownList
    {
        #region 成员变量，构造函数，析构函数。
        string midChar, m_strDataValueParentField;
        bool m_bShowTreeView = false;
        /// <summary>
        /// 构造函数。
        /// </summary>
        public DropDownListEx()
        {
            this.midChar = HttpUtility.HtmlDecode("&nbsp;&nbsp;&nbsp;&nbsp;");
        }
        /// <summary>
        /// 析构函数。
        /// </summary>
        ~DropDownListEx()
        {
        }
        #endregion

        #region ShowTreeView
        /// <summary>
        /// 以树状形式显示列表数据。
        /// </summary>
        [Category("Appearance")]
        [DefaultValue(false)]
        [Description("以树状形式显示列表数据。")]
        public bool ShowTreeView
        {
            get
            {
                return this.m_bShowTreeView;
            }
            set
            {
                this.m_bShowTreeView = value;
            }
        }
        #endregion

        #region DataValueParentField
        /// <summary>
        /// 树状形式显示时过滤数据的父ID字段名称。
        /// </summary>
        [Category("Data")]
        [Description("树状形式显示时过滤数据的父ID字段名称。")]
        public string DataValueParentField
        {
            get
            {
                return this.m_strDataValueParentField;
            }
            set
            {
                this.m_strDataValueParentField = value;
            }
        }
        #endregion

        #region 过滤数据。
        /// <summary>
        /// 获取或设置上级ID的字段名称。
        /// </summary>
        [Category("Data")]
        [Description("树状形式显示时过滤数据的父ID字段名称。")]
        public string TreeParentID
        {
            get
            {
                string s = (string)this.ViewState["TreeParentID"];
                return string.IsNullOrEmpty(s) ? string.Empty : s;
            }
            set
            {
                if (this.TreeParentID != value)
                    this.ViewState["TreeParentID"] = value;
            }
        }

        /// <summary>
        /// 获取或设置树结构的字段名称。
        /// </summary>
        [Category("Data")]
        [Description("树状形式显示时过滤数据的父ID字段名称。")]
        public string TreeID
        {
            get
            {
                string s = (string)this.ViewState["TreeID"];
                return string.IsNullOrEmpty(s) ? string.Empty : s;
            }
            set
            {
                if (this.TreeID != value)
                    this.ViewState["TreeID"] = value;
            }
        }
        #endregion

        #region ShowUnDefine
        /// <summary>
        /// 显示表示未定义选项或表示全部选项的项目。
        /// </summary>
        [Category("Appearance")]
        [DefaultValue(false)]
        [Description("显示表示未定义选项或表示全部选项的项目。")]
        public bool ShowUnDefine
        {
            get
            {
                object o = this.ViewState["ShowUnDefine"];
                return (o == null) ? false : (bool)o;
            }
            set
            {
                if (this.ShowUnDefine != value)
                    this.ViewState["ShowUnDefine"] = value;
            }
        }
        #endregion

        #region UnDefineTitle/UnDefineValue
        /// <summary>
        /// 表示未定义选项或表示全部选项的项目的标题。
        /// </summary>
        [Category("Appearance")]
        [DefaultValue("=====未指定=====")]
        [Description("表示未定义选项或表示全部选项的项目的标题。")]
        public string UnDefineTitle
        {
            get
            {
                string s = (string)this.ViewState["UnDefineTitle"];
                return string.IsNullOrEmpty(s) ? "=====未指定=====" : s;
            }
            set
            {
                if (this.UnDefineTitle != value)
                    this.ViewState["UnDefineTitle"] = value;
            }
        }
        /// <summary>
        /// 表示未定义选项或表示全部选项的项目的值。
        /// </summary>
        [Category("Appearance")]
        [Description("表示未定义选项或表示全部选项的项目的值。")]
        public string UnDefineValue
        {
            get
            {
                string s = (string)this.ViewState["UnDefineValue"];
                return string.IsNullOrEmpty(s) ? string.Empty : s;
            }
            set
            {
                if (this.UnDefineValue != value)
                    this.ViewState["UnDefineValue"] = value;
            }
        }
        #endregion

        #region SplitChar
        /// <summary>
        /// 数据列之间的分隔符,默认为[:]。
        /// </summary>
        [Category("Data")]
        [DefaultValue(":")]
        [Bindable(true)]
        [Description("数据列之间的分隔符,默认为[:]。")]
        public char SplitChar
        {
            get
            {
                object o = this.ViewState["SplitChar"];
                return (o == null) ? ':' : (char)o;
            }
            set
            {
                if (this.SplitChar != value)
                    this.ViewState["SplitChar"] = value;
            }
        }
        #endregion

        #region 验证
        /// <summary>
        /// 获取或设置是否允许客户端验证。
        /// </summary>
        [Category("Valid")]
        [DefaultValue(true)]
        [Description("获取或设置是否允许客户端验证。")]
        public bool EnableClientScript
        {
            get
            {
                object obj1 = this.ViewState["EnableClientScript"];
                return ((obj1 == null) ? true : ((bool)obj1));
            }
            set
            {
                this.ViewState["EnableClientScript"] = value;
            }
        }
        #region IsRequired
        /// <summary>
        /// 至少必选一项非空值。
        /// </summary>
        [Category("Valid")]
        [DefaultValue(false)]
        [Description("至少必选一项非空值。")]
        public bool IsRequired
        {
            get
            {
                object o = this.ViewState["IsRequired"];
                return (o == null) ? false : (bool)o;
            }
            set
            {
                if (this.IsRequired != value)
                    this.ViewState["IsRequired"] = value;
            }
        }
        #endregion

        #region 实现IValidator 成员

        /// <summary>
        /// 计算它检查的条件并更新 IsValid 属性。
        /// </summary>
        public void Validate()
        {
            this.IsValid = true;//默认情况下验证是成功的
            //验证必选
            if (this.IsRequired)
            {
                //下拉框没有选项//没有选中项
                if (this.Items.Count == 0 || this.SelectedIndex == -1)
                {
                    this.IsValid = false;
                }
            }
        }

        /// <summary>
        /// 获取或设置一个值，通过该值指示用户在指定控件中输入的内容是否通过验证。
        /// </summary>
        [Category("Valid")]
        [Browsable(false)]
        [DefaultValue("true")]
        public bool IsValid
        {
            get
            {
                object o = this.ViewState["IsValid"];
                return (o == null) ? true : (bool)o;
            }
            set
            {
                if (this.IsValid != value)
                    this.ViewState["IsValid"] = value;
            }
        }
        #endregion
        /// <summary>
        /// 获取或设置条件验证失败时生成的错误信息。
        /// </summary>
        [Category("Valid")]
        [Description("没有选择项时的提示信息。")]
        public string ErrorMessage
        {
            get
            {
                string s = (string)this.ViewState["ErrorMessage"];
                return string.IsNullOrEmpty(s) ? string.Empty : s;
            }
            set
            {
                if (this.ErrorMessage != value)
                    this.ViewState["ErrorMessage"] = value;
            }
        }

        #endregion
        
        #region 重载
        /// <summary>
        /// 获取或设置样式表。
        /// </summary>
        [Category("Styles")]
        [Description("获取或设置样式表。")]
        public string BorderCssClass
        {
            get
            {
                string str = this.ViewState["BorderCssClass"] as string;
                if (string.IsNullOrEmpty(str))
                    str = "BorderDropDownList";
                return str;
            }
            set
            {
                this.ViewState["BorderCssClass"] = value;
            }
        }
        /// <summary>
        /// 重载OnInit方法。
        /// </summary>
        /// <param name="e"></param>
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            if (this.Page != null)
                this.Page.Validators.Add(this);
        }
        /// <summary>
        /// 重载OnUnload。
        /// </summary>
        /// <param name="e"></param>
        protected override void OnUnload(EventArgs e)
        {
            if (this.Page != null)
                this.Page.Validators.Remove(this);
            base.OnUnload(e);
        }
        /// <summary>
        /// 重载OnDataBinding方法用于绑定多个数据表字段。
        /// </summary>
        /// <param name="e">包含事件数据的 EventArgs 对象。</param>
        protected override void OnDataBinding(EventArgs e)
        {
            if (this.DataSource == null)
                return;
            else
                this.Items.Clear();//清除选项

            //构造树状选项列表
            if (this.ShowTreeView && 
                ((this.DataSource is DataSet) || (this.DataSource is DataTable) || (this.DataSource is DataView)))
            {
                this.BuildTreeView(string.Empty, string.Empty);
            }
            else//平板状选项列表
            {
                string sDataTextField = this.DataTextField, sDataValueField = this.DataValueField;
                string[] vDataTextField = null;
                string sText = string.Empty;
                bool bValue = (sDataValueField != string.Empty) ? true : false;
                bool bText = (sDataTextField != string.Empty) ? true : false;

                if (bText)
                    vDataTextField = sDataTextField.Split(',');

                IEnumerator ie = DataUtil.DataSource2IEnumerator(this.DataSource);
                Object oCurrent;
                ListItem _ListItem = null;
                if (ie != null)
                {
                    this.Items.Clear();//清除所有旧选项

                    while (ie.MoveNext())
                    {
                        oCurrent = ie.Current;
                        _ListItem = new ListItem();
                        if (bValue)
                            _ListItem.Value = DataBinder.GetPropertyValue(oCurrent, sDataValueField, null);

                        if (bText)
                        {
                            sText = string.Empty;
                            for (int i = 0; i < vDataTextField.Length; i++)
                            {
                                sText += string.Format("{0}{1}", this.SplitChar, DataBinder.GetPropertyValue(oCurrent, vDataTextField[i].ToString(), null));
                            }

                            if (sText.Length > 0)
                                _ListItem.Text = sText.Substring(1);
                        }
                        this.Items.Add(_ListItem);
                    }
                }
            }

            //在列表最前头增加未定义选项
            if (this.ShowUnDefine)
            {
                this.Items.Insert(0, new ListItem(this.UnDefineTitle, this.UnDefineValue));
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        protected override void OnPreRender(EventArgs e)
        {
            if (this.IsRequired)
            {
                ClientScriptManager clientManager = this.Page.ClientScript;
                if (clientManager != null)
                {
                    const string validatorResourceKey = "ValidatorIncludeScript";
                    const string ValidatorIncludeScriptKey = "ValidatorIncludeScript";
                    const string ValidatorOnSubmitKey = "ValidatorOnSubmit";
                    if (!clientManager.IsClientScriptBlockRegistered(typeof(BaseValidator), validatorResourceKey))
                    {
                        clientManager.RegisterClientScriptResource(typeof(BaseValidator), "WebUIValidation.js");
                        StringBuilder ValidatorStartupScript = new StringBuilder();
                        ValidatorStartupScript.Append("<script type=\"text/javascript\">\r\n");
                        ValidatorStartupScript.Append("<!--\r\n");
                        ValidatorStartupScript.Append("var Page_ValidationActive = false;\r\n");
                        ValidatorStartupScript.Append("if (typeof(ValidatorOnLoad) == \"function\") {\r\n");
                        ValidatorStartupScript.Append("\tValidatorOnLoad();\r\n");
                        ValidatorStartupScript.Append("}\r\n");

                        ValidatorStartupScript.Append("function ValidatorOnSubmit() {\r\n");
                        ValidatorStartupScript.Append("\tif (Page_ValidationActive) {\r\n");
                        ValidatorStartupScript.Append("\t\treturn ValidatorCommonOnSubmit();\r\n");
                        ValidatorStartupScript.Append("\t}else {\r\n");
                        ValidatorStartupScript.Append("\t\treturn true;\r\n");
                        ValidatorStartupScript.Append("\t}\r\n");
                        ValidatorStartupScript.Append("}\r\n");

                        ValidatorStartupScript.Append("// -->\r\n");
                        ValidatorStartupScript.Append("</script>\r\n");
                        clientManager.RegisterStartupScript(typeof(BaseValidator), ValidatorIncludeScriptKey, ValidatorStartupScript.ToString());

                        string ValidatorOnSubmitScript = "if (typeof(ValidatorOnSubmit) == \"function\" && ValidatorOnSubmit() == false) return false;";
                        clientManager.RegisterOnSubmitStatement(typeof(BaseValidator), ValidatorOnSubmitKey, ValidatorOnSubmitScript);
                    }
                    string strScript = string.Format("{0}_RequiredFieldValidator", this.ClientID);
                    if (!clientManager.IsClientScriptBlockRegistered(strScript))
                    {
                        StringBuilder sb = new StringBuilder();
                        sb.Append("<script type=\"text/javascript\">\r\n");
                        sb.Append("<!--\r\n");
                        sb.AppendFormat("function {0}_RequiredFieldValidator(val)\r\n", this.ClientID);
                        sb.Append("{\r\n");
                        sb.Append("\tvar oSelect=document.getElementById(val.controltovalidate);\r\n");
                        sb.Append("\t//not item\r\n");
                        sb.Append("\tif (oSelect.options.length == 0)\r\n");
                        sb.Append("\t\treturn false;\r\n");
                        sb.Append("\t//not item is select\r\n");
                        sb.Append("\tif (oSelect.value == \"\")\r\n");
                        sb.Append("\t\treturn false;\r\n");

                        sb.Append("\treturn true;\r\n");
                        sb.Append("}\r\n");
                        sb.Append("// -->\r\n");
                        sb.Append("</script>\r\n");

                        clientManager.RegisterArrayDeclaration("Page_Validators", string.Format("document.getElementById(\"{0}_RequiredField\")", this.ClientID));
                        clientManager.RegisterClientScriptBlock(this.GetType(), strScript, sb.ToString());
                    }
                }
            }
            base.OnPreRender(e);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="writer"></param>
        protected override void Render(HtmlTextWriter writer)
        {
            if (string.IsNullOrEmpty(this.CssClass))
                this.CssClass = "DropDownList";

            writer.AddStyleAttribute(HtmlTextWriterStyle.Width, this.Width.ToString());
            writer.AddAttribute(HtmlTextWriterAttribute.Class, this.BorderCssClass);
            writer.RenderBeginTag(HtmlTextWriterTag.Div);
            base.Render(writer);
            writer.RenderEndTag();
            if (this.IsRequired)
            {
                writer.AddAttribute(HtmlTextWriterAttribute.Id, string.Format("{0}_RequiredField", this.ClientID));
                writer.AddAttribute("controltovalidate", this.ClientID);
                writer.AddAttribute("errormessage", this.ErrorMessage);
                writer.AddAttribute("isvalid", this.IsValid.ToString());
                writer.AddAttribute("evaluationfunction", string.Format("{0}_RequiredFieldValidator", this.ClientID));
                writer.AddAttribute(HtmlTextWriterAttribute.Nowrap, "true");
                writer.AddStyleAttribute(HtmlTextWriterStyle.Color, "red");
                writer.RenderBeginTag(HtmlTextWriterTag.Span);
                writer.Write("*");
                writer.RenderEndTag();
            }
        }
        #endregion

        #region 生成树状数据
        void BuildTreeView(string strPID, string strLeftPad)
        {
            string strValue = string.Empty, strText = string.Empty;
            string idF = this.TreeID;
            if (string.IsNullOrEmpty(idF))
            {
                idF = this.DataValueField;
            }
            string idFP = this.TreeParentID;
            if (string.IsNullOrEmpty(idFP))
            {
                idFP = this.DataValueParentField;
            }

            DataTable dtSource = DataUtil.DataSourceToDataTable(this.DataSource);
            if (dtSource != null)
            {
                //处理数据条目的父字段及子字段都是数字类型的数据过滤方法
                string idFPTypeName = dtSource.Columns[idFP].DataType.ToString();
                if (idFPTypeName == "System.Int16" || idFPTypeName == "System.Int32" || idFPTypeName == "System.Int64")
                {
                    if (!string.IsNullOrEmpty(strPID))
                        dtSource.DefaultView.RowFilter = string.Format("{0}={1}", idFP, strPID);
                    else
                        dtSource.DefaultView.RowFilter = string.Format("{0}=0", idFP);
                }
                else
                    dtSource.DefaultView.RowFilter = string.Format("isnull({0},'')='{1}'", idFP, strPID);

                DataTable dt = DataUtil.GetDataTable(dtSource.DefaultView);
                if (dt != null)
                {
                    if (string.IsNullOrEmpty(strPID) && dt.Rows.Count == 0)
                        dt = dtSource.Copy();

                    foreach (DataRow row in dt.Rows)
                    {
                        strValue = row[this.DataValueField].ToString();
                        strText = string.Format("{0}{1}", strLeftPad, row[this.DataTextField].ToString());
                        this.Items.Add(new ListItem(strText, strValue));
                        this.BuildTreeView(row[idF].ToString(), string.Format("{0}{1}", strLeftPad, this.midChar));
                    }
                }
            }
        }
        #endregion
    }
}
