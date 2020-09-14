// Decompiled with JetBrains decompiler
// Type: WTW.ESB.Model.Message`1
// Assembly: WTW.ESB.Model, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 70AFDC6C-72F5-498B-987E-4F2D6C6BE33F
// Assembly location: C:\tfs\WTWESB\Release18.2\Lib\WTW.ESB.Model.dll

namespace WTW.ESB.Model
{
    /// <summary>Class Message.</summary>
    /// <typeparam name="T"></typeparam>
    public class Message<T> where T : class, new()
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="T:WTW.ESB.Model.Message`1" /> class.
        /// </summary>
        public Message()
        {
            this.SystemProperty = new SystemProperty();
            this.UserDefinedProperty = new UserDefinedProperty();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:WTW.ESB.Model.Message`1" /> class.
        /// </summary>
        /// <param name="message">The message.</param>
        public Message(T message)
        {
            this.SystemProperty = new SystemProperty();
            this.UserDefinedProperty = new UserDefinedProperty();
            this.EntityBody = message;
        }

        /// <summary>Gets or sets the system property.</summary>
        /// <value>The system property.</value>
        public SystemProperty SystemProperty { set; get; }

        /// <summary>Gets or sets the user defined property.</summary>
        /// <value>The user defined property.</value>
        public UserDefinedProperty UserDefinedProperty { set; get; }

        /// <summary>Gets or sets the entity body.</summary>
        /// <value>The entity body.</value>
        public T EntityBody { set; get; }
    }
}