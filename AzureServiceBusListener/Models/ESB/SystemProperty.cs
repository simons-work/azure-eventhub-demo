// Decompiled with JetBrains decompiler
// Type: WTW.ESB.Model.SystemProperty
// Assembly: WTW.ESB.Model, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 70AFDC6C-72F5-498B-987E-4F2D6C6BE33F
// Assembly location: C:\tfs\WTWESB\Release18.2\Lib\WTW.ESB.Model.dll

using System;
using System.Collections.Generic;

namespace WTW.ESB.Model
{
    /// <summary>Class SystemProperty.</summary>
    public class SystemProperty
    {
        /// <summary>Gets or sets the tenant.</summary>
        /// <value>The tenant.</value>
        public string Tenant { set; get; }

        /// <summary>Gets or sets the correlation identifier.</summary>
        /// <value>The correlation identifier.</value>
        public string CorrelationId { get; set; }

        /// <summary>Gets or sets the message identifier.</summary>
        /// <value>The message identifier.</value>
        public Guid MessageId { set; get; }

        /// <summary>Gets or sets the session identifier.</summary>
        /// <value>The session identifier.</value>
        public string SessionId { set; get; }

        /// <summary>Gets or sets the type of the message.</summary>
        /// <value>The type of the message.</value>
        public MessageType MessageType { set; get; }

        /// <summary>Gets or sets the message topic.</summary>
        /// <value>The message topic.</value>
        public string MessageTopic { set; get; }

        /// <summary>Gets or sets the message subscription.</summary>
        /// <value>The message subscription.</value>
        public string MessageSubscription { set; get; }

        /// <summary>Gets or sets the message queue.</summary>
        /// <value>The message queue.</value>
        public string MessageQueue { set; get; }

        /// <summary>Gets or sets the expiration date.</summary>
        /// <value>The expiration date.</value>
        public DateTime ExpirationDate { set; get; }

        /// <summary>Gets or sets the system key values.</summary>
        /// <value>The system key values.</value>
        public Dictionary<string, string> SystemKeyValues { set; get; }

        /// <summary>Gets or sets the reply to topic.</summary>
        /// <value>The reply to topic.</value>
        public string ReplyToTopic { get; set; }
    }
}
