using System;
using System.Linq.Expressions;

namespace EntityGraphQL.Compiler.Util
{
    /// <summary>
    /// As people build schema fields they are against a different parameter, this visitor lets us change it to the one used in compiling the EQL
    /// </summary>
    internal class ParameterReplacer : ExpressionVisitor
    {
        private Expression newParam;
        private Type toReplaceType;
        private ParameterExpression toReplace;
        internal Expression Replace(Expression node, ParameterExpression toReplace, Expression newParam)
        {
            this.newParam = newParam;
            this.toReplace = toReplace;
            return Visit(node);
        }

        internal Expression ReplaceByType(Expression node, Type toReplaceType, Expression newParam)
        {
            this.newParam = newParam;
            this.toReplaceType = toReplaceType;
            return Visit(node);
        }

        protected override Expression VisitParameter(ParameterExpression node)
        {
            if (toReplace != null && toReplace == node)
                return newParam;
            if (toReplaceType != null && node.NodeType == ExpressionType.Parameter && toReplaceType == node.Type)
                return newParam;
            return node;
        }
    }
}