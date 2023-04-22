using System;
using Microsoft.AspNetCore.Http;
using MtTenants.Abstractions;

namespace MtTenants.Implementation
{
    internal sealed class HttpHeaderTenantProvider : ITenantProvider
    {
        private readonly HttpContext _httpContext;
        private readonly ITenantFactory _tenantFactory;
        
        public HttpHeaderTenantProvider(
            IHttpContextAccessor httpContextAcessor,
            ITenantFactory tenantFactory)
        {
            _httpContext = httpContextAcessor.HttpContext;
            _tenantFactory = tenantFactory;
        }
        
        public ITenant GetTenant()
        {
            Guid tenantId = Guid.Parse(_httpContext.Request.Headers["TENANT_ID"]);
            string tenantName = _httpContext.Request.Headers["TENANT_NAME"];
            var tenant = _tenantFactory.Create(tenantId, tenantName);

            return tenant;
        }
    }
}