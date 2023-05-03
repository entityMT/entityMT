using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using MTConfigurations.Abstractions.Attributes;
using MTQueries.Abstractions.ClausuleManagers;

namespace MTQueries.SqlServer
{
    public sealed class DefaultGroupByClausuleManager<T> : ExpressionVisitor, IGroupByClausuleManager<T>
    {
        private StringBuilder _groups = new StringBuilder();
        
        public void AddGroup<P>(Expression<Func<T, P>> exp)
        {
            this.Visit(exp);
        }

        protected override Expression VisitMember(MemberExpression node)
        {
            var column = node.Member.GetCustomAttribute<ColumnAttribute>();

            if (column != default)
            {
                var table = node.Member.ReflectedType!.GetCustomAttribute<TableAttribute>();

                if (table == default)
                    throw new ApplicationException($"{node.Member.ReflectedType!.Name} does not have table configuration.");

                if (_groups.Length == 0)
                    _groups.Append("GROUP BY ");
                
                _groups.Append($"{table.Name}.{column.Name}, ");
            }
            
            return base.VisitMember(node);
        }

        public void Reset()
        {
            _groups = new StringBuilder();
        }

        public string GetGroupBy
        {
            get
            {
                if (_groups.Length > 0)
                    _groups = _groups.Remove(_groups.Length - 2, 2);

                return _groups.ToString();
            }
        }
    }
}