namespace eventsapi.Cache
{
    public enum CacheTypeEnum { RedisServer, AmazonRedisOSS  }

    public class CacheType
    {
        private CacheTypeEnum cacheType;
        private string cacheTypeValue;

        public static CacheType RedisServer = new CacheType(CacheTypeEnum.RedisServer, "REDIS_SERVER");
        public static CacheType AmazonRedisOSS = new CacheType(CacheTypeEnum.AmazonRedisOSS, "REDIS_AMAZON_OSS");

        private CacheType(CacheTypeEnum type, string cacheDefinition)
        {
            this.cacheType = type;
            this.cacheTypeValue = cacheDefinition;
        }

        public override string ToString()
        {
            return this.cacheTypeValue;
        }

        public CacheTypeEnum ToCacheType()
        {
            return this.cacheType;
        }

        public bool IsCacheType(string cacheType) 
        {
            return this.cacheTypeValue.Equals(cacheType, StringComparison.InvariantCultureIgnoreCase);
        }

        public static CacheType FromString(string cacheType)
        {
            if (AmazonRedisOSS.IsCacheType(cacheType)) {
                return AmazonRedisOSS;
            }
            return RedisServer;
        }
    }
}
