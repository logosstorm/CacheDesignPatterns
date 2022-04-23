using StackExchange.Redis;

namespace CacheApp.Cache;

public class RedisService
{
    private readonly ConnectionMultiplexer _connectionMultiplexer;
    public IDatabase Database = null!;

    public RedisService()
    {
        _connectionMultiplexer = ConnectionMultiplexer.Connect("localhost");
    }

    public IDatabase GetDb(int db)
    {
        return _connectionMultiplexer.GetDatabase(db);
    }
}