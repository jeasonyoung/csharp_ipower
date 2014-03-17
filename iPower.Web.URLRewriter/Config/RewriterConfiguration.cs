using System;
using System.Web;
using System.Web.Caching;
using System.Configuration;

using System.Xml;
using System.Xml.Serialization;
using System.Xml.XPath;

namespace iPower.Web.URLRewriter.Config
{
    /// <summary>
    /// Specifies the configuration settings in the Web.config for the RewriterRule.
    /// </summary>
    /// <remarks>This class defines the structure of the Rewriter configuration file in the Web.config file.
    /// Currently, it allows only for a set of rewrite rules; however, this approach allows for customization.
    /// For example, you could provide a ruleset that <i>doesn't</i> use regular expression matching; or a set of
    /// constant names and values, which could then be referenced in rewrite rules.
    /// <p />
    /// The structure in the Web.config file is as follows:
    /// <code>
    /// &lt;configuration&gt;
    /// 	&lt;configSections&gt;
    /// 		&lt;section name="RewriterConfig" 
    /// 		            type="iPower.Web.URLRewriter.Config.JeasonRewriterSection, iPower.Web.URLRewriter" /&gt;
    ///		&lt;/configSections&gt;
    ///		
    ///		&lt;RewriterConfig&gt;
    ///		    &lt;add lookFor=<i>pattern</i> sendTo=<i>replace with</i>&gt;
    ///			&lt;add lookFor=<i>pattern</i> sendTo=<i>replace with</i>&gt;
    ///		&lt;/RewriterConfig&gt;
    ///		
    ///		&lt;system.web&gt;
    ///			...
    ///		&lt;/system.web&gt;
    ///	&lt;/configuration&gt;
    /// </code>
    /// </remarks>
    public class RewriterConfiguration
    {
        #region 成员变量，构造函数。
        const string const_cache_key = "RewriterConfig";
        JeasonRewriterSection section;
        /// <summary>
        /// 构造函数。
        /// </summary>
        public RewriterConfiguration()
        {
            this.section = ConfigurationManager.GetSection(const_cache_key) as JeasonRewriterSection;
        }
        #endregion

        #region 静态属性。
        static RewriterConfiguration mconfig;
        /// <summary>
        /// GetConfig() returns an instance of the <b>RewriterConfiguration</b> class with the values populated from
        /// the Web.config file.  It uses XML deserialization to convert the XML structure in Web.config into
        /// a <b>RewriterConfiguration</b> instance.
        /// </summary>
        /// <returns>A <see cref="RewriterConfiguration"/> instance.</returns>
        public static  RewriterConfiguration ModuleConfig
        {
            get
            {
                lock (typeof(RewriterConfiguration))
                {
                    if (mconfig == null)
                        mconfig = new RewriterConfiguration();
                    return mconfig;
                }
            }
        }
        #endregion

        #region Public Properties
        /// <summary>
        /// A <see cref="RewriterRuleCollection"/> instance that provides access to a set of <see cref="RewriterRule"/>s.
        /// </summary>
        public RewriterRuleCollection Rules
        {
            get
            {
                if (this.section != null)
                    return this.section.Rules;
                return null;
            }
        }
        #endregion
    }

    /// <summary>
    /// Deserializes the markup in Web.config into an instance of the <see cref="RewriterConfiguration"/> class.
    /// </summary>
    public class JeasonRewriterSection : ConfigurationSection
    {
        #region 构造函数，析构函数
        static readonly ConfigurationProperty propRewriterRule;
        static ConfigurationPropertyCollection properties;
        /// <summary>
        /// 静态构造函数。
        /// </summary>
        static JeasonRewriterSection()
        {
            properties = new ConfigurationPropertyCollection();
            propRewriterRule = new ConfigurationProperty(null, typeof(RewriterRuleCollection), null, ConfigurationPropertyOptions.IsDefaultCollection);
            properties.Add(propRewriterRule);
        }
        #endregion
        /// <summary>
        /// 
        /// </summary>
        [ConfigurationProperty("", Options= ConfigurationPropertyOptions.IsDefaultCollection)]
        public RewriterRuleCollection Rules
        {
            get
            {
                return (RewriterRuleCollection)base[propRewriterRule];
            }
        }

        #region 重载。
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        protected override object GetRuntimeObject()
        {
            this.SetReadOnly();
            return this;
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
