using System;
using ApplicationCore.Interfaces.DataAccess.Redis;
using StackExchange.Redis;

namespace Infrastructure.Persistence.DataAccess
{
	public class RedisRepository : IRedisRepository
    {
		private readonly IConnectionMultiplexer _connectionMultiplexer;
        public RedisRepository(IConnectionMultiplexer connectionMultiplexer)
		{
			_connectionMultiplexer = connectionMultiplexer;
        }

        public string GetValue(string key)
        {
            var db = _connectionMultiplexer.GetDatabase();
            return db.StringGet(key);
        }

        public void SetValue(string key, string value)
        {
            var db = _connectionMultiplexer.GetDatabase();
            db.StringSet(key, value);
        }
    }
}
