using System;
using System.Web;

namespace iPower.Web.URLRewriter
{
    /// <summary>
    /// The base class for module rewriting.  This class is abstract, and therefore must be derived from.
    /// </summary>
    /// <remarks>Provides the essential base functionality for a rewriter using the HttpModule approach.</remarks>
    public abstract class BaseModuleRewriter : IHttpModule
    {
        /// <summary>
        /// Executes when the module is initialized.
        /// </summary>
        /// <param name="app">A reference to the HttpApplication object processing this request.</param>
        /// <remarks>Wires up the HttpApplication's AuthorizeRequest event to the</remarks>
        public virtual void Init(HttpApplication app)
        {
            // WARNING!  This does not work with Windows authentication!
            // If you are using Windows authentication, change to app.BeginRequest
            if (app != null)
                app.AuthorizeRequest += new EventHandler(delegate(object sender, EventArgs e)
                {
                    HttpApplication a = (HttpApplication)sender;
                    this.Rewrite(a.Request.Path, a);
                });
        }

        /// <summary>
        /// 
        /// </summary>
        public virtual void Dispose()
        {
        }      

        /// <summary>
        /// The <b>Rewrite</b> method must be overriden.  It is where the logic for rewriting an incoming
        /// URL is performed.
        /// </summary>
        /// <param name="requestedPath">The requested RawUrl.  (Includes full path and querystring.)</param>
        /// <param name="app">The HttpApplication instance.</param>
        protected abstract void Rewrite(string requestedPath, HttpApplication app);
    }
}
