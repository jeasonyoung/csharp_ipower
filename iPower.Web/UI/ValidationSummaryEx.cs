using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.ComponentModel;
using System.Drawing;

namespace iPower.Web.UI
{
    /// <summary>
    /// 显示所有验证错误摘要的扩展。
    /// </summary>
    [ToolboxData("<{0}:ValidationSummaryEx runat=server/>")]
    [ToolboxBitmap(typeof(ValidationSummaryEx))]
    public class ValidationSummaryEx : ValidationSummary
    {
        #region 成员变量，构造函数。
        /// <summary>
        /// 构造函数。
        /// </summary>
        public ValidationSummaryEx()
            : base()
        {
        }
        #endregion
    }

    /// <summary>
    /// 必选验证。
    /// </summary>
    public class RequiredFieldValidatorEx : RequiredFieldValidator
    {
        #region 成员变量，构造函数。
        /// <summary>
        /// 构造函数。
        /// </summary>
        public RequiredFieldValidatorEx()
        {
            base.Display = ValidatorDisplay.None;
            this.Text = string.Empty;
        }
        #endregion
    }

    /// <summary>
    /// 比较验证。
    /// </summary>
    public class CompareValidatorEx : CompareValidator
    {
        #region 成员变量，构造函数。
        /// <summary>
        /// 构造函数。
        /// </summary>
        public CompareValidatorEx()
        {
            base.Display = ValidatorDisplay.None;
            this.Text = string.Empty;
        }
        #endregion
    }

    /// <summary>
    /// 正则表达式验证。
    /// </summary>
    public class RegularExpressionValidatorEx : RegularExpressionValidator
    {
        #region 成员变量，构造函数。
        /// <summary>
        /// 构造函数。
        /// </summary>
        public RegularExpressionValidatorEx()
        {
            base.Display = ValidatorDisplay.None;
            this.Text = string.Empty;
        }
        #endregion
    }

    /// <summary>
    /// 范围验证。
    /// </summary>
    public class RangeValidatorEx : RangeValidator
    {
         #region 成员变量，构造函数。
        /// <summary>
        /// 构造函数。
        /// </summary>
        public RangeValidatorEx()
        {
            base.Display = ValidatorDisplay.None;
            this.Text = string.Empty;
        }
        #endregion
    }
}
