using System;
using MTConfigurations.Abstractions.Attributes;

namespace MtPersistency.SqlServer.UnitTests.Fakers
{
    [Schema("dbo")]
    [Table("DeleteTable")]
    public sealed class FakeDeleteEntity
    {
        [Key]
        public Guid Id { get; set; }
    }
}