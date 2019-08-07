using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PCLs.Exceptions
{
    class IsNotDenseException : PointCloudException
    {
        public IsNotDenseException () {}

        public IsNotDenseException(String message) : base(message)
        {
        }
    }
}
