using System;
using Emaily.Core.Enumerations;

namespace Emaily.Core.DTO
{
    public class CreateAutoResponderVM
    {                                  
        public string Name { get; set; }
        public AutoResponderEnum Mode { get; set; }
    }

    public class AutoResponderVM
    {
        public int ListId { get; set; }
        public int Id { get; set; }
        public string Name { get; set; }
        public AutoResponderEnum Mode { get; set; }
        public DateTime Created { get; set; }
    }
}