using System;

namespace BicycleCompany.BLL.Utils
{
    /// <summary>
    /// The exception that is thrown when user try to make action when he dosn't have permissions.
    /// </summary>
    public class ForbiddenException : Exception
    {
        public ForbiddenException()
            : base("You don't have permission to access")
        {

        }

        public ForbiddenException(string message)
            : base(message)
        {

        }
    }
}
