
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
	/// �����û��������б���ѡ��һ��Ŀؼ�,
	/// ��չ���ܣ�
	///		1.DataTextFiled֧�ֶ�������а�,DataTextField����ֶ�֮����[,]�ֿ�;
	///		2.֧����״ѡ���б����ʾ(ShowTreeView����);
	///		3.֧����ʾ��ʾδ����ѡ����ʾȫ��ѡ���ѡ��
	///		(��ShowUnDefine����Ϊtrueʱ��ͨ��UnDefineTitle,UndefineValue��ֵ������ѡ��;
	/// </summary>
	///<example>����������ʾ��DropDownListEx�ؼ��ĸ����÷�
	///<code>
	///<![CDATA[
	///DataSet ds=CreateDataSet();
	/// //��ʾ��״
	///ddl.ShowTreeView=true;
	///ddl.DataValueField="SystemID";
	///ddl.DataValueParentField="ParentSystemID";
	///ddl.DataTextField="SystemName";
	///ddl.DataSource=ds;
	///ddl.DataBind();
	/// //��ʾƽ��״
	///ddl.DataValueField="SystemID";
	///ddl.DataTextField="SystemName";
	///ddl.DataSource=ds;
	///ddl.DataBind();
	/// //��ʾδ����ѡ��
	///ddl.ShowUnDefine=true;
	///ddl.UnDefineTitle="=====ȫ��======";
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
        #region ��Ա���������캯��������������
        string midChar, m_strDataValueParentField;
        bool m_bShowTreeView = false;
        /// <summary>
        /// ���캯����
        /// </summary>
        public DropDownListEx()
        {
            this.midChar = HttpUtility.HtmlDecode("&nbsp;&nbsp;&nbsp;&nbsp;");
        }
        /// <summary>
        /// ����������
        /// </summary>
        ~DropDownListEx()
        {
        }
        #endregion

        #region ShowTreeView
        /// <summary>
        /// ����״��ʽ��ʾ�б����ݡ�
        /// </summary>
        [Category("Appearance")]
        [DefaultValue(false)]
        [Description("����״��ʽ��ʾ�б����ݡ�")]
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
        /// ��״��ʽ��ʾʱ�������ݵĸ�ID�ֶ����ơ�
        /// </summary>
        [Category("Data")]
        [Description("��״��ʽ��ʾʱ�������ݵĸ�ID�ֶ����ơ�")]
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

        #region �������ݡ�
        /// <summary>
        /// ��ȡ�������ϼ�ID���ֶ����ơ�
        /// </summary>
        [Category("Data")]
        [Description("��״��ʽ��ʾʱ�������ݵĸ�ID�ֶ����ơ�")]
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
        /// ��ȡ���������ṹ���ֶ����ơ�
        /// </summary>
        [Category("Data")]
        [Description("��״��ʽ��ʾʱ�������ݵĸ�ID�ֶ����ơ�")]
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
        /// ��ʾ��ʾδ����ѡ����ʾȫ��ѡ�����Ŀ��
        /// </summary>
        [Category("Appearance")]
        [DefaultValue(false)]
        [Description("��ʾ��ʾδ����ѡ����ʾȫ��ѡ�����Ŀ��")]
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
        /// ��ʾδ����ѡ����ʾȫ��ѡ�����Ŀ�ı��⡣
        /// </summary>
        [Category("Appearance")]
        [DefaultValue("=====δָ��=====")]
        [Description("��ʾδ����ѡ����ʾȫ��ѡ�����Ŀ�ı��⡣")]
        public string UnDefineTitle
        {
            get
            {
                string s = (string)this.ViewState["UnDefineTitle"];
                return string.IsNullOrEmpty(s) ? "=====δָ��=====" : s;
            }
            set
            {
                if (this.UnDefineTitle != value)
                    this.ViewState["UnDefineTitle"] = value;
            }
        }
        /// <summary>
        /// ��ʾδ����ѡ����ʾȫ��ѡ�����Ŀ��ֵ��
        /// </summary>
        [Category("Appearance")]
        [Description("��ʾδ����ѡ����ʾȫ��ѡ�����Ŀ��ֵ��")]
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
        /// ������֮��ķָ���,Ĭ��Ϊ[:]��
        /// </summary>
        [Category("Data")]
        [DefaultValue(":")]
        [Bindable(true)]
        [Description("������֮��ķָ���,Ĭ��Ϊ[:]��")]
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

        #region ��֤
        /// <summary>
        /// ��ȡ�������Ƿ�����ͻ�����֤��
        /// </summary>
        [Category("Valid")]
        [DefaultValue(true)]
        [Description("��ȡ�������Ƿ�����ͻ�����֤��")]
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
        /// ���ٱ�ѡһ��ǿ�ֵ��
        /// </summary>
        [Category("Valid")]
        [DefaultValue(false)]
        [Description("���ٱ�ѡһ��ǿ�ֵ��")]
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

        #region ʵ��IValidator ��Ա

        /// <summary>
        /// �������������������� IsValid ���ԡ�
        /// </summary>
        public void Validate()
        {
            this.IsValid = true;//Ĭ���������֤�ǳɹ���
            //��֤��ѡ
            if (this.IsRequired)
            {
                //������û��ѡ��//û��ѡ����
                if (this.Items.Count == 0 || this.SelectedIndex == -1)
                {
                    this.IsValid = false;
                }
            }
        }

        /// <summary>
        /// ��ȡ������һ��ֵ��ͨ����ֵָʾ�û���ָ���ؼ�������������Ƿ�ͨ����֤��
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
        /// ��ȡ������������֤ʧ��ʱ���ɵĴ�����Ϣ��
        /// </summary>
        [Category("Valid")]
        [Description("û��ѡ����ʱ����ʾ��Ϣ��")]
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
        
        #region ����
        /// <summary>
        /// ��ȡ��������ʽ��
        /// </summary>
        [Category("Styles")]
        [Description("��ȡ��������ʽ��")]
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
        /// ����OnInit������
        /// </summary>
        /// <param name="e"></param>
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            if (this.Page != null)
                this.Page.Validators.Add(this);
        }
        /// <summary>
        /// ����OnUnload��
        /// </summary>
        /// <param name="e"></param>
        protected override void OnUnload(EventArgs e)
        {
            if (this.Page != null)
                this.Page.Validators.Remove(this);
            base.OnUnload(e);
        }
        /// <summary>
        /// ����OnDataBinding�������ڰ󶨶�����ݱ��ֶΡ�
        /// </summary>
        /// <param name="e">�����¼����ݵ� EventArgs ����</param>
        protected override void OnDataBinding(EventArgs e)
        {
            if (this.DataSource == null)
                return;
            else
                this.Items.Clear();//���ѡ��

            //������״ѡ���б�
            if (this.ShowTreeView && 
                ((this.DataSource is DataSet) || (this.DataSource is DataTable) || (this.DataSource is DataView)))
            {
                this.BuildTreeView(string.Empty, string.Empty);
            }
            else//ƽ��״ѡ���б�
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
                    this.Items.Clear();//������о�ѡ��

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

            //���б���ǰͷ����δ����ѡ��
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

        #region ������״����
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
                //����������Ŀ�ĸ��ֶμ����ֶζ����������͵����ݹ��˷���
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
