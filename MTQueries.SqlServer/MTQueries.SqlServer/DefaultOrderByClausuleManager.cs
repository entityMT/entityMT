using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using MTConfigurations.Abstractions.Attributes;
using MTQueries.Abstractions.ClausuleManagers;

namespace MTQueries.SqlServer;

public sealed class DefaultOrderByClausuleManager<T> : ExpressionVisitor, IOrderByClausuleManager<T>
{
    private StringBuilder _orders = new StringBuilder();
    private bool _isAscOrder;
    
    public void AddOrder<P>(Expression<Func<T, P>> exp, bool asc = true)
    {
        _isAscOrder = asc;
        this.Visit(exp);
    }

    public void Reset()
    {
        _orders = new StringBuilder();
        _isAscOrder = false;
    }

    public string GetOrderBy => _orders.ToString();

    protected override Expression VisitMember(MemberExpression node)
    {
        var column = node.Member.GetCustomAttribute<ColumnAttribute>();

        if (column != default)
        {
            var table = node.Member.ReflectedType!.GetCustomAttribute<TableAttribute>();

            if (table == default)
                throw new ApplicationException($"{node.Member.ReflectedType!.Name} does not have table configuration.");

            if (_orders.Length > 0)
                _orders.Append(",");

            if (_orders.Length == 0)
                _orders.Append("ORDER BY ");
            
            _orders.Append($"{table.Name}.{column.Name}");

            if (_isAscOrder)
                _orders.Append(" ASC");
            else
                _orders.Append(" DESC");
        }
        
        return base.VisitMember(node);
    }
}