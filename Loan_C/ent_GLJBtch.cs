using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Loan_C
{
    public class ent_GLJBtch
    {
        public object Description { get; set; }
        public List<ent_GLHeader> JournalHeaders;
        public object UpdateOperation { get; set; }
    }
}
