using System;
using ApplicationCore.Aggregates;

namespace ApplicationCore.Entites
{
	public abstract class EntityBase : AggregateRoot
	{
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}

