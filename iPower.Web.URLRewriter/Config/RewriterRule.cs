using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

using System.Configuration;
namespace iPower.Web.URLRewriter.Config
{
    /// <summary>
    /// Represents a rewriter rule.  A rewriter rule is composed of a pattern to search for and a string to replace
    /// the pattern with (if matched).
    /// </summary>
    public class RewriterRule : ConfigurationElement
    {
        #region 成员变量，构造函数。
        static ConfigurationPropertyCollection properties;
        static ConfigurationProperty propLookFor, propSendTo;
        /// <summary>
        /// 静态构造函数。
        /// </summary>
        static RewriterRule()
        {
            properties = new ConfigurationPropertyCollection();
            propLookFor = new ConfigurationProperty("lookFor", typeof(string), null, ConfigurationPropertyOptions.IsKey | ConfigurationPropertyOptions.IsRequired);
            propSendTo = new ConfigurationProperty("sendTo", typeof(string), null, ConfigurationPropertyOptions.None);

            properties.Add(propLookFor);
            properties.Add(propSendTo);
        }
        /// <summary>
        /// 构造函数。
        /// </summary>
        public RewriterRule()
        {
        }
        /// <summary>
        /// 构造函数。
        /// </summary>
        public RewriterRule(string lookFor)
        {
            this.LookFor = lookFor;
        }
        /// <summary>
        /// 构造函数。
        /// </summary>
        /// <param name="lookFor"></param>
        /// <param name="sendTo"></param>
        public RewriterRule(string lookFor, string sendTo)
            : this(lookFor)
        {
            this.SendTo = sendTo;
        }
        #endregion

        #region Public Properties
        /// <summary>
        /// Gets or sets the pattern to look for.
        /// </summary>
        /// <remarks><b>LookFor</b> is a regular expression pattern.  Therefore, you might need to escape
        /// characters in the pattern that are reserved characters in regular expression syntax (., ?, ^, $, etc.).
        /// <p />
        /// The pattern is searched for using the <b>System.Text.RegularExpression.Regex</b> class's <b>IsMatch()</b>
        /// method.  The pattern is case insensitive.</remarks>
        [ConfigurationProperty("lookFor", Options = ConfigurationPropertyOptions.IsKey | ConfigurationPropertyOptions.IsRequired)]
        public string LookFor
        {
            get
            {
                return (string)this[propLookFor];
            }
            set
            {
                this[propLookFor] = value;
            }
        }

        /// <summary>
        /// The string to replace the pattern with, if found.
        /// </summary>
        /// <remarks>The replacement string may use grouping symbols, like $1, $2, etc.  Specifically, the
        /// <b>System.Text.RegularExpression.Regex</b> class's <b>Replace()</b> method is used to replace
        /// the match in <see cref="LookFor"/> with the value in <b>SendTo</b>.</remarks>
        [ConfigurationProperty("sendTo")]
        public string SendTo
        {
            get
            {
                return (string)this[propSendTo];
            }
            set
            {
                this[propSendTo] = value;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        internal string Key
        {
            get { return this.LookFor; }
        }
        #endregion
    }

    /// <summary>
    /// The RewriterRuleCollection models a set of RewriterRules in the Web.config file.
    /// </summary>
    /// <remarks>
    /// The RewriterRuleCollection is expressed in XML as:
    /// <code>
    /// &lt;add LookFor='' SendTo=''&gt;
    /// </code>
    /// </remarks>
    [ConfigurationCollection(typeof(RewriterRule))]
    public class RewriterRuleCollection : ConfigurationElementCollection
    {
        #region 成员变量，构造函数。
        static ConfigurationPropertyCollection properties;
        /// <summary>
        /// 静态构造函数。
        /// </summary>
        static RewriterRuleCollection()
        {
            properties = new ConfigurationPropertyCollection();
        }
        /// <summary>
        /// 构造函数。
        /// </summary>
        public RewriterRuleCollection()
            : base(StringComparer.OrdinalIgnoreCase)
        {
        }
        #endregion

        /// <summary>
        /// 
        /// </summary>
        /// <param name="rule"></param>
        public void Add(RewriterRule rule)
        {
            this.BaseAdd(rule);
        }
        /// <summary>
        /// 
        /// </summary>
        public void Clear()
        {
            base.BaseClear();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="rule"></param>
        /// <returns></returns>
        public int IndexOf(RewriterRule rule)
        {
            return base.BaseIndexOf(rule);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="rule"></param>
        public void Remove(RewriterRule rule)
        {
            if (base.BaseIndexOf(rule) >= 0)
                base.BaseRemove(rule.Key);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="lookFor"></param>
        public void Remove(string lookFor)
        {
            base.BaseRemove(lookFor);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="index"></param>
        public void RemoveAt(int index)
        {
            base.BaseRemoveAt(index);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="lookFor"></param>
        /// <returns></returns>
        public RewriterRule this[string lookFor]
        {
            get { return (RewriterRule)base.BaseGet(lookFor); }
        }
        public RewriterRule this[int index]
        {
            get { return (RewriterRule)base.BaseGet(index); }
            set
            {
                if (base.BaseGet(index) != null)
                    base.BaseRemoveAt(index);
                this.BaseAdd(index, value);
            }
        }
        #region 重载。
        /// <summary>
        /// 
        /// </summary>
        /// <param name="index"></param>
        /// <param name="element"></param>
        protected override void BaseAdd(int index, ConfigurationElement element)
        {
            if (index == -1)
                base.BaseAdd(element, false);
            else
                base.BaseAdd(index, element);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        protected override ConfigurationElement CreateNewElement()
        {
            return new RewriterRule();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="element"></param>
        /// <returns></returns>
        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((RewriterRule)element).Key;
        }
        /// <summary>
        /// 
        /// </summary>
        protected override ConfigurationPropertyCollection Properties
        {
            get
            {
                return properties;
            }
        }
        #endregion
    }
}
