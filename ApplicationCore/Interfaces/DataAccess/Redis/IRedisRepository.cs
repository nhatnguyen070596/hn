using System;
namespace ApplicationCore.Interfaces.DataAccess.Redis
{
    public interface IRedisRepository
    {
        void SetValue(string key, string value);
        string GetValue(string key);
    }
}

