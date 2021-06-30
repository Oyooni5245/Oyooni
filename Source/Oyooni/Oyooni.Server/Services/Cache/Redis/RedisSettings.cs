namespace Oyooni.Server.Services.Cache.Redis
{
    /// <summary>
    /// Represents a redis settings data
    /// </summary>
    public class RedisSettings
    {
        /// <summary>
        /// Default constructor
        /// </summary>
        public RedisSettings() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="RedisSettings"/> class using the passed parameters
        /// </summary>
        public RedisSettings(string host = "localhost", int port = 3679, int defaultDatabase = -1)
        {
            Host = host;
            Port = port;
            DefaultDatabase = defaultDatabase;
        }

        /// <summary>
        /// The host the redis service is running on
        /// </summary>
        public string Host { get; set; }

        /// <summary>
        /// The port of the related redis service
        /// </summary>
        public int Port { get; set; }

        /// <summary>
        /// The default database identifier to be used
        /// </summary>
        public int DefaultDatabase { get; set; }
    }
}
