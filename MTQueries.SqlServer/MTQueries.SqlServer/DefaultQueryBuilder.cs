using System.Linq.Expressions;
using System.Reflection;
using MTConfigurations.Abstractions.Attributes;
using MTQueries.Abstractions;
using MTQueries.Abstractions.ClausuleManagers;

namespace MTQueries.SqlServer;

public sealed class DefaultQueryBuilder<T> : IQueryBuilder<T>
{
    private readonly IGroupByClausuleManager<T> _groupByClausuleManager;
    private readonly IOrderByClausuleManager<T> _orderByClausuleManager;
    private readonly IWhereClausuleManager<T> _whereClausuleManager;
    private Task<string[]> _colunsSelected;
    private Task<string[]> _joins;
    private string _select = null!;
    private CancellationToken _cancellationToken;

    public DefaultQueryBuilder(
        IGroupByClausuleManager<T> groupByClausuleManager,
        IOrderByClausuleManager<T> orderByClausuleManager,
        IWhereClausuleManager<T> whereClausuleManager,
        IQuerySelectedColumnsProvider<T> columnsProvider,
        IQueryJoinsGenerator<T> queryJoinsGenerator)
    {
        _whereClausuleManager = whereClausuleManager;
        _groupByClausuleManager = groupByClausuleManager;
        _orderByClausuleManager = orderByClausuleManager;

        _colunsSelected = columnsProvider.GetColumnsAsync(_cancellationToken);
        _joins = queryJoinsGenerator.GetJoinsAsync(_cancellationToken);
    }

    public IQueryBuilder<T> SetGroup<P>(Expression<Func<T, P>> exp)
    {
        _groupByClausuleManager.AddGroup(exp);
        return this;
    }
    
    public IQueryBuilder<T> SetFilter(Expression<Func<T, bool>> exp)
    {
        _whereClausuleManager.SetCondition(exp);
        return this;
    }

    public IQueryBuilder<T> SetOrder<P>(Expression<Func<T, P>> exp, bool asc = true)
    {
        _orderByClausuleManager.AddOrder(exp, asc);
        return this;
    }

    public IQueryBuilder<T> SetDistinct()
    {
        _select = "select distinct ";
        return this;
    }

    public IQueryBuilder<T> SetCancellationToken(CancellationToken cancellationToken)
    {
        _cancellationToken = cancellationToken;
        return this;
    }

    public IQuery Build()
    {
        Task.WaitAll(
            new Task[] {_colunsSelected, _joins}, _cancellationToken);
        
        var columns = _colunsSelected.Result;
        var joins = _joins.Result;

        if (string.IsNullOrEmpty(_select))
            _select = "select ";

        var table = typeof(T).GetCustomAttribute<TableAttribute>();

        var columnsStr = string.Join(",", columns);

        var joinsStr = string.Empty;

        if (joins.Any())
            joinsStr = string.Join(" ", joins);

        _select += 
            columnsStr +
            " from " +
            table?.Name +
            " " +
            joinsStr +
            " " +
            _whereClausuleManager.GetWhere +
            " " +
            _groupByClausuleManager.GetGroupBy +
            " " +
            _orderByClausuleManager.GetOrderBy;

        return new DefaultQuery(_select, _whereClausuleManager.Parameters);
    }

    public void Reset()
    {
        if (_joins.Status == TaskStatus.RanToCompletion)
        {
            var joins = _joins.Result;
            joins = null;
        }

        if (_colunsSelected.Status == TaskStatus.RanToCompletion)
        {
            var selectedColumns = _colunsSelected.Result;
            selectedColumns = null;
        }
        
        _whereClausuleManager.Reset();
        _orderByClausuleManager.Reset();
        _groupByClausuleManager.Reset();
        _select = null!;
    }
}