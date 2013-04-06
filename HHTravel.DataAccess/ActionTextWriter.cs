using System;
using System.IO;
using System.Text;

namespace HHTravel.DataAccess
{
    public class ActionTextWriter : TextWriter
    {
        private Action<string> action;

        public ActionTextWriter(Action<string> action)
        {
            this.action = action;
        }

        public override Encoding Encoding
        {
            get { return System.Text.Encoding.Default; }
        }

        public override void Write(char[] buffer, int index, int count)
        {
            Write(new string(buffer, index, count));
        }

        public override void Write(string value)
        {
            action.Invoke(value);
        }
    }
}