using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HHTravel.CRM.Booking_Online.Model
{
    public abstract class BrokenRuleMessages
    {
        private Dictionary<string, string> messages;

        protected Dictionary<string, string> Messages
        {
            get { return this.messages; }
        }

        protected BrokenRuleMessages()
        {
            this.messages = new Dictionary<string, string>();
            this.PopulateMessages();
        }

        protected abstract void PopulateMessages();

        public string GetRuleDescription(string messageKey)
        {
            string description = string.Empty;
            if (this.messages.ContainsKey(messageKey))
            {
                description = this.messages[messageKey];
            }
            return description;
        }
    }
}
