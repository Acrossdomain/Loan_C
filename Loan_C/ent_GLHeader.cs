using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Loan_C
{
    public class ent_GLHeader
    {
        public object SourceType { get; set; }
        //public object EntryType { get; set; }
        public object Description { get; set; }
       // public object TaxGroup { get; set; }
       // public object TaxBaseAmount1 { get; set; }
       // public object TaxItemClass1 { get; set; }
         public List<ent_GLDetail> JournalDetails;
    }
}
