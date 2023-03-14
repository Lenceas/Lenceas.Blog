namespace Lenceas.Core.Common
{
    /// <summary>
    /// 配置文件帮助类
    /// </summary>
    public static class ConfigHelper
    {
        /// <summary>
        /// JWT密钥
        /// </summary>
        public static string JwtSecretString
        {
            get
            {
                return DifDBConnOfSecurity(@"./jwt_Secret.txt", @"F:\my-file\jwt_Secret.txt", @"D:\my-file\jwt_Secret.txt", @"C:\my-file\jwt_Secret.txt"); ;
            }
        }

        /// <summary>
        /// MySql连接字符串
        /// </summary>
        public static string MySqlConnectionString
        {
            get
            {
                return DifDBConnOfSecurity(@"./mysql_Conn.txt", @"F:\my-file\mysql_Conn.txt", @"D:\my-file\mysql_Conn.txt", @"C:\my-file\mysql_Conn.txt");
            }
        }

        /// <summary>
        /// Redis连接字符串
        /// </summary>
        public static string RedisConnectionString
        {
            get
            {
                return DifDBConnOfSecurity(@"./redis_Conn.txt", @"F:\my-file\redis_Conn.txt", @"D:\my-file\redis_Conn.txt", @"C:\my-file\redis_Conn.txt");
            }
        }

        /// <summary>
        /// Mongo连接字符串
        /// </summary>
        public static string MongoDBConnectionString
        {
            get
            {
                return DifDBConnOfSecurity(@"./mongo_Conn.txt", @"F:\my-file\mongo_Conn.txt", @"D:\my-file\mongo_Conn.txt", @"C:\my-file\mongo_Conn.txt");
            }
        }

        /// <summary>
        /// Mongo数据库名称
        /// </summary>
        public static string MongoDBNameString { get { return "Blog"; } }

        /// <summary>
        /// 获取文本内容
        /// </summary>
        /// <param name="conn">txt文件路径集合</param>
        /// <returns>返回第一个有值的文本内容</returns>
        private static string DifDBConnOfSecurity(params string[] conn)
        {
            foreach (var item in conn)
            {
                try
                {
                    if (File.Exists(item))
                    {
                        return File.ReadAllText(item).Trim();
                    }
                }
                catch (System.Exception) { }
            }
            return conn[^1];
        }
    }
}