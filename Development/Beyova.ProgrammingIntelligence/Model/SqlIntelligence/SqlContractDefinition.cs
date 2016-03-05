using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Beyova.ProgrammingIntelligence
{
    public class SqlContractDefinition
    {
        public bool IsBaseObject { get; set; }

        public bool IsSimpleBaseObject { get; set; }

        public SqlContractDefinition ParentContractDefinition { get; set; }
    }
}
