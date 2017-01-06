using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Antlr4.Runtime.Misc;

namespace ValueFlowInterpreter
{
    class IDListener : MuParserBaseListener
    {
        private HashSet<string> ids;

        public IDListener()
        {
            ids = new HashSet<string>();
        }

        public override void EnterIdAtom([NotNull] MuParserParser.IdAtomContext context)
        {
            ids.Add(context.GetText());
        }

        public HashSet<string> GetIDs()
        {
            return ids;
        }
    }
}
