using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Soway.Query
{
    class test:System.IO.TextWriter
    {
        public override void Write(string value)
        {

            System.Diagnostics.Trace.Write(value);
        }
        public override void WriteLine(String value)
        {

            // System.Diagnostics.Trace.WriteLine(value);
        }

        public override Encoding Encoding
        {
            get { return Encoding.Unicode; }
        }
    }
}
