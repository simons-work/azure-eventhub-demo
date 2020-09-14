// Decompiled with JetBrains decompiler
// Type: WTW.ESB.Model.UserDefinedProperty
// Assembly: WTW.ESB.Model, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 70AFDC6C-72F5-498B-987E-4F2D6C6BE33F
// Assembly location: C:\tfs\WTWESB\Release18.2\Lib\WTW.ESB.Model.dll

using System.Collections.Generic;
using System.Net.Http;

namespace WTW.ESB.Model
{
    /// <summary>Class UserDefinedProperty.</summary>
    public class UserDefinedProperty
    {
        /// <summary>Gets or sets the external resource URI.</summary>
        /// <value>The external resource URI.</value>
        public string ExternalResourceUri { set; get; }

        /// <summary>Gets or sets the name of the external resource.</summary>
        /// <value>The name of the external resource.</value>
        public string ExternalResourceName { set; get; }

        /// <summary>Gets or sets the resource natural key.</summary>
        /// <value>The resource natural key.</value>
        public string ResourceNaturalKey { set; get; }

        /// <summary>Gets or sets the HTTP method.</summary>
        /// <value>The HTTP method.</value>
        public HttpMethod HttpMethod { set; get; }

        /// <summary>Gets or sets the custom key values.</summary>
        /// <value>The custom key values.</value>
        public Dictionary<string, string> CustomKeyValues { set; get; }
    }
}
