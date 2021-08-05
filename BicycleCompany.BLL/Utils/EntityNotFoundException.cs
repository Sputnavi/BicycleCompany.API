using System;

namespace BicycleCompany.BLL.Utils
{
    /// <summary>
    /// The exception that is thrown when entity with provided id was not found.
    /// </summary>
    public class EntityNotFoundException : Exception
    {
        public EntityNotFoundException(string EntityName, Guid providedId)
            : base($"{EntityName} with provided id: {providedId} cannot be found!")
        {

        }

        public EntityNotFoundException()
            : base("Entity was not found!")
        {

        }
    }
}