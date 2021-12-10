using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab2
{
    public class Threat
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string SourceOfAttack { get; set; }
        public string ObjectOfAttack { get; set; }
        public bool ViolatesConfidentiality { get; set; }
        public bool ViolatesIntegrity { get; set; }
        public bool ViolatesAccessibility { get; set; }

        public Threat(int id, string name, string description, string sourceOfAttack, string objectOfAttack, bool violatesConfidentiality, bool violatesIntegrity, bool violatesAccessibility)
        {
            Id = id;
            Name = name;
            Description = description;
            SourceOfAttack = sourceOfAttack;
            ObjectOfAttack = objectOfAttack;
            ViolatesConfidentiality = violatesConfidentiality;
            ViolatesIntegrity = violatesIntegrity;
            ViolatesAccessibility = violatesAccessibility;
        }
    }
}
