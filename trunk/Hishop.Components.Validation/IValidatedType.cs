using System.Collections.Generic;
using System.Reflection;

namespace Hishop.Components.Validation
{
    internal interface IValidatedType : IValidatedElement
    {
        IEnumerable<MethodInfo> GetSelfValidationMethods();
        IEnumerable<IValidatedElement> GetValidatedFields();
        IEnumerable<IValidatedElement> GetValidatedMethods();
        IEnumerable<IValidatedElement> GetValidatedProperties();
    }
}

