using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PCLs.Exceptions
{
    public class PointCloudException : Exception
    {
        public PointCloudException() {}

        public PointCloudException(String message) : base(message) {}
    }
}
