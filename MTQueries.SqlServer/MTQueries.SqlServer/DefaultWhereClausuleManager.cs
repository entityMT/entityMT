using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using MTConfigurations.Abstractions.Attributes;
using MTQueries.Abstractions.ClausuleManagers;
using MTQueries.Abstractions.ClausuleManagers.MemberAccessHandlers;

namespace MTQueries.SqlServer;

public sealed class DefaultWhereClausuleManager<T> : ExpressionVisitor, IWhereClausuleManager<T>
{
    private StringBuilder _where = new StringBuilder();
    private StringBuilder _conditionAdder = new StringBuilder();
    private ExpressionType _binaryType;
    private List<string> _concatConditions = new List<string>();
    private string _currentConditional = null!;
    private bool _isLike = false;
    private bool _isSetFromMemberAccess = false;
    private IDictionary<string, object> _parameters = new Dictionary<string, object>();
    private readonly IEnumerable<IMemberAccessHandler> _memberAccessHandlers;

    public DefaultWhereClausuleManager(IEnumerable<IMemberAccessHandler> memberAccessHandlers)
    {
        _memberAccessHandlers = memberAccessHandlers;
    }

    public void SetCondition(Expression<Func<T, bool>> exp)
    {
        _currentConditional = default!;
        _conditionAdder = new StringBuilder("(");
        _concatConditions = new List<string>();
        _isLike = false;
        _isSetFromMemberAccess = false;
        
        this.Visit(exp);

        var aux = _conditionAdder.ToString().Trim();

        if (aux.EndsWith("AND"))
            aux = aux.Substring(0, aux.Length - 3);
        else if (aux.EndsWith("OR"))
            aux = aux.Substring(0, aux.Length - 2);

        if (_where.Length > 0)
            _where.Append(" AND ");
        
        _where.Append(aux.Trim() + ")");
    }

    public void Reset()
    {
        _conditionAdder = new StringBuilder();
        _where = new StringBuilder();
        _concatConditions = new List<string>();
        _parameters = new Dictionary<string, object>();
        _isLike = default;
        _currentConditional = default!;
    }

    public IDictionary<string, object> Parameters => _parameters;

    public string GetWhere
    {
        get
        {
            var aux = string.Empty;

            if (_where.Length > 0)
                aux = "WHERE ";
            
            return aux + _where.ToString();
        }
    }

    protected override Expression VisitMember(MemberExpression node)
    {
        var column = node.Member.GetCustomAttribute<ColumnAttribute>();

        if (column != default)
        {
            var table = node.Member.ReflectedType!.GetCustomAttribute<TableAttribute>();

            if (table == default)
                throw new ApplicationException($"{node.Member.ReflectedType!.Name} does not have table configuration.");

            _conditionAdder.Append($"{table.Name}.{column.Name} ");
        }
        else
        {
            foreach (var memberAccessHandler in _memberAccessHandlers)
            {
                if (memberAccessHandler.Handle(node, out object value))
                {
                    _isSetFromMemberAccess = true;
                    this.AppendConditional();
                    
                    if (value is not null)
                        _parameters.Add($"param_{_parameters.Count + 1}", value);
                    else
                        _conditionAdder.Append(_binaryType == ExpressionType.Equal ? "IS NULL" : "IS NOT NULL");
                    
                    _conditionAdder.Append(" ");
                }
            }
        }
            
        return base.VisitMember(node);
    }

    protected override Expression VisitConstant(ConstantExpression node)
    {
        if (_isSetFromMemberAccess)
        {
            _isSetFromMemberAccess = false;
            return base.VisitConstant(node);
        }
        
        var value = node.Value;
        var valueType = value?.GetType();
        
        if (value is not null)
        {
            this.AppendConditional();
            
            if (valueType != null && (valueType.IsPrimitive 
                                      || valueType == typeof(string) 
                                      || valueType == typeof(decimal)
                                      || valueType == typeof(Guid)
                                      || valueType == typeof(DateTime)))
                _parameters.Add($"param_{_parameters.Count + 1}", value);
            else
            {
                var field = valueType?.GetFields()[0].GetValue(value);
                _parameters.Add($"param_{_parameters.Count + 1}", field!);
            }
        }
        else
            _conditionAdder.Append(_binaryType == ExpressionType.Equal ? "IS NULL" : "IS NOT NULL");

        _conditionAdder.Append(" ");
        return base.VisitConstant(node);
    }

    protected override Expression VisitBinary(BinaryExpression node)
    {
        if (node.NodeType == ExpressionType.OrElse || node.NodeType == ExpressionType.AndAlso)
        {
            var conditionType = node.NodeType == ExpressionType.OrElse ? " OR " : " AND ";

            if (_concatConditions.Count > 0 && !_concatConditions.Any(c => c == conditionType))
                throw new ApplicationException("it is not possible to set filter with more than one logical operator type.");
            
            _concatConditions.Add(conditionType);
            _currentConditional = _concatConditions.Last();

            var inner = base.VisitBinary(node);

            return inner;
        }

        _conditionAdder.Append("(");
        _binaryType = node.NodeType;
        
        var exp = base.VisitBinary(node);

        _conditionAdder.Append(")");
        _conditionAdder.Append(_currentConditional);
        
        return exp;
    }

    protected override Expression VisitMethodCall(MethodCallExpression node)
    {
        if (node.Method.Name == "Contains")
            _isLike = true;

        _conditionAdder.Append("(");
        _binaryType = node.NodeType;
        
        var exp = base.VisitMethodCall(node);

        _conditionAdder.Append(")");
        _conditionAdder.Append(_currentConditional);

        return exp;
    }

    private void AppendConditional()
    {
        if (_binaryType == ExpressionType.Equal)
            _conditionAdder.Append($"= @param_{_parameters.Count + 1}");
        else if(_binaryType == ExpressionType.NotEqual)
            _conditionAdder.Append($"<> @param_{_parameters.Count + 1}");
        else if(_binaryType == ExpressionType.GreaterThan)
            _conditionAdder.Append($"> @param_{_parameters.Count + 1}");
        else if (_binaryType == ExpressionType.LessThan)
            _conditionAdder.Append($"< @param_{_parameters.Count + 1}");
        else if(_binaryType == ExpressionType.GreaterThanOrEqual)
            _conditionAdder.Append($">= @param_{_parameters.Count + 1}");
        else if (_binaryType == ExpressionType.LessThanOrEqual)
            _conditionAdder.Append($"<= @param_{_parameters.Count + 1}");
        else
        {
            if (_isLike)
                _conditionAdder.Append($"LIKE '%' + @param_{_parameters.Count + 1} + '%'");
            else
                throw new ApplicationException(
                    "Unsupported ExpressionType operator at ConstantExpression access in tree");
        }
    }
}