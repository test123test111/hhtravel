using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace HHTravel.CRM.Booking_Online.DataAccess
{
    public class ActionTextWriter : TextWriter
    {
        private Action<string> action;

        public ActionTextWriter(Action<string> action)
        {
            this.action = action;
        }

        public override void Write(char[] buffer, int index, int count)
        {
            Write(new string(buffer, index, count));
        }

        public override void Write(string value)
        {
            action.Invoke(value);
        }

        public override Encoding Encoding
        {
            get { return System.Text.Encoding.Default; }
        }
    }
}
