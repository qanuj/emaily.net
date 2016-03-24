using System;
using System.Linq;
using Emaily.Core.Abstraction.Services;
using Emaily.Core.DTO;

namespace Emaily.Services
{
    public class UtilService : IUtilService
    {
        public EnumList Enums()
        {
            var types = ReflectionHelper.GetEnumTypesInNamespace("Emaily.Core.Enumerations");
            var vals = new EnumList();
            foreach (var type in types)
            {
                vals.Add(type.Name.ToLower(), Enum.GetNames(type).Select(x => Enum.Parse(type, x)).OrderBy(x => x));
            }
            return vals;
        }
        public object BuildModel(string id)
        {
            var type = ReflectionHelper.GetModelByName(id);
            return type == null ? null : Activator.CreateInstance(type);
        }
    }
}