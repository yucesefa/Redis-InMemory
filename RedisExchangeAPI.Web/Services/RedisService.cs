using StackExchange.Redis;

namespace RedisExchangeAPI.Web.Services
{
    public class RedisService
    {
        private readonly string _redisHost; 
        private readonly string _redisPort;
        private ConnectionMultiplexer _redisConnection;

        public IDatabase Db { get;  set; }

        public RedisService(IConfiguration configuration)
        {
            _redisHost = configuration["RedisHost"];
            _redisPort = configuration["RedisPort"];
        }

        public void Connect()
        {
            var configString = $"{_redisHost}:{_redisPort}";

            _redisConnection = ConnectionMultiplexer.Connect(configString); 
        }
        
        public IDatabase GetDb(int db)
        {
            return _redisConnection.GetDatabase(db);
        }
    }
}
