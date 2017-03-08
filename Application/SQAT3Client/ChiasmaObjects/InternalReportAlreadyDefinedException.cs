using System;
using System.Collections.Specialized;
using System.Text;

namespace Molmed.SQAT.ChiasmaObjects
{
    public class InternalReportAlreadyDefinedException : DataAlreadyDefinedException
    {

        public InternalReportAlreadyDefinedException(StringCollection internalReportIdentifiers)
            : base(internalReportIdentifiers)
        {
        }

        public override Type GetDataType()
        {
            return typeof(InternalReportAlreadyDefinedException);
        }

    }
}
