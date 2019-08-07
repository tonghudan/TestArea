using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PCLs.Exceptions
{

    class PCDReaderException : PointCloudException
    {
        public PCDReaderException() { }

        public PCDReaderException(String message) : base(message)
        {
        }
    }
}
