namespace Academy.Application.Cache;

public readonly struct CacheKey(string value)
{
    private readonly string _value = value;


    public static CacheKey Format(CacheKey key , params object[] args)
        => new(string.Format(key, args));

    public static implicit operator string(CacheKey cacheKey)
    {
        return cacheKey._value;
    }
    

    public override string ToString()
    {
        return _value;
    }
}