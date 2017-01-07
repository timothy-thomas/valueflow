using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Antlr4.Runtime.Misc;

namespace ValueFlowInterpreter
{
    class MuParserToPythonVisitor : MuParserBaseVisitor<string>
    {
        public override string VisitProgExpr([NotNull] MuParserParser.ProgExprContext context)
        {
            var testCases = new StringBuilder();
            testCases.Append("import math\n");
            foreach (var expr in context.expr())
            {
                testCases.Append("\nprint ").Append(Visit(expr));
            }
            return testCases.ToString();
        }

        public override string VisitPowExpr([NotNull] MuParserParser.PowExprContext context)
        {
            string expBase = Visit(context.expr(0));
            string expExp = Visit(context.expr(1));

            return "(" + expBase + "**" + expExp + ")";
        }

        public override string VisitUnaryMinusExpr([NotNull] MuParserParser.UnaryMinusExprContext context)
        {
            string expr = Visit(context.expr());

            return "(-" + expr + ")";
        }

        public override string VisitMulDivExpr([NotNull] MuParserParser.MulDivExprContext context)
        {
            string left = Visit(context.expr(0));
            string right = Visit(context.expr(1));

            if (context.op.Type == MuParserLexer.MUL)
            {
                return "(" + left + "*" + right + ")";
            }
            else
            {
                return "(" + left + "/float(" + right + "))";
            }
        }

        public override string VisitAddSubExpr([NotNull] MuParserParser.AddSubExprContext context)
        {
            string left = Visit(context.expr(0));
            string right = Visit(context.expr(1));

            if (context.op.Type == MuParserLexer.ADD)
            {
                return "(" + left + "+" + right + ")";
            }
            else
            {
                return "(" + left + "-" + right + ")";
            }
        }

        public override string VisitRelationalExpr([NotNull] MuParserParser.RelationalExprContext context)
        {
            string left = Visit(context.expr(0));
            string right = Visit(context.expr(1));

            switch (context.op.Type)
            {
                case MuParserLexer.LTEQ:
                    return "(" + left + "<=" + right + ")";
                case MuParserLexer.GTEQ:
                    return "(" + left + ">=" + right + ")";
                case MuParserLexer.LT:
                    return "(" + left + "<" + right + ")";
                case MuParserLexer.GT:
                    return "(" + left + ">" + right + ")";
                default:
                    return "False";
            }
        }

        public override string VisitEqualityExpr([NotNull] MuParserParser.EqualityExprContext context)
        {
            string left = Visit(context.expr(0));
            string right = Visit(context.expr(1));

            switch (context.op.Type)
            {
                case MuParserLexer.EQ:
                    return "(" + left + "==" + right + ")";
                case MuParserLexer.NEQ:
                    return "(" + left + "!=" + right + ")";
                default:
                    return "False";
            }

        }

        public override string VisitAndExpr([NotNull] MuParserParser.AndExprContext context)
        {
            string left = Visit(context.expr(0));
            string right = Visit(context.expr(1));

            return "(" + left + " and " + right + ")";
        }

        public override string VisitOrExpr([NotNull] MuParserParser.OrExprContext context)
        {
            string left = Visit(context.expr(0));
            string right = Visit(context.expr(1));

            return "(" + left + " or " + right + ")";
        }

        public override string VisitIteExpr([NotNull] MuParserParser.IteExprContext context)
        {
            string condition = Visit(context.expr(0));
            string true_action = Visit(context.expr(1));
            string false_action = Visit(context.expr(2));

            return "(" + true_action + " if " + condition + " else " + false_action + ")";
        }

        public override string VisitFunctionExpr([NotNull] MuParserParser.FunctionExprContext context)
        {
            string expr = Visit(context.expr());

            string function = "";
            string close = "";
            switch (context.op.Text)
            {
                case "sin":
                case "cos":
                case "tan":
                case "asin":
                case "acos":
                case "atan":
                case "sinh":
                case "cosh":
                case "tanh":
                case "asinh":
                case "acosh":
                case "atanh":
                    function = "math." + context.op.Text;
                    break;
                case "log2":
                    function = "math.log(";
                    close = ",2)";
                    break;
                case "log10":
                case "log":
                    function = "math.log(";
                    close = ",10)";
                    break;
                case "ln":
                    function = "math.log";
                    break;
                case "exp":
                    function = "math.e**";
                    break;
                case "sqrt":
                    function = "pow(";
                    close = ",0.5)";
                    break;
                case "sign":
                    function = "-1 if ";
                    close = " < 0 else 1";
                    break;
                case "rint":
                    function = "(int) (round";
                    close = ")";
                    break;
                case "abs":
                    function = "abs";
                    break;
                default:
                    function = context.op.Text;
                    break;
            }

            return "(" + function + expr + close + ")";
        }

        public override string VisitFunctionMultiExpr([NotNull] MuParserParser.FunctionMultiExprContext context)
        {
            var vals = String.Join(",", context.expr().Select(c => Visit(c)));

            switch (context.op.Text)
            {
                case "min":
                case "max":
                    return "(" + context.op.Text + "(" + vals + "))";
                case "sum":
                    return "(" + context.op.Text + "([" + vals + "]))";
                case "avg":
                    return "(sum([" + vals + "])/float(len([" + vals + "])))"; //TODO: replace with a simple "avg" function in the output file.
                default:
                    return null; // Shouldn't happen.
            }
        }

        public override string VisitAtom([NotNull] MuParserParser.AtomContext context)
        {
            return base.VisitAtom(context);
        }

        public override string VisitParExpr([NotNull] MuParserParser.ParExprContext context)
        {
            return Visit(context.expr());
        }

        public override string VisitNumberAtom([NotNull] MuParserParser.NumberAtomContext context)
        {
            return "(" + context.GetText() + ")";
        }

        public override string VisitBooleanAtom([NotNull] MuParserParser.BooleanAtomContext context)
        {
            if (context.GetText().ToLower().Equals("true"))
            {
                return "(True)";
            }
            else
            {
                return "(False)";
            }
        }

        public override string VisitIdAtom([NotNull] MuParserParser.IdAtomContext context)
        {
            return "(" + context.GetText() + ")";
        }

        public override string VisitPredefinedConstantAtom([NotNull] MuParserParser.PredefinedConstantAtomContext context)
        {
            if (context.GetText().ToLower().Equals("_pi"))
            {
                return "(math.pi)";
            }
            else
            {
                return "(math.e)";
            }
        }
    }
}
