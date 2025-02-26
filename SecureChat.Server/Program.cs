using NTDLS.SqliteDapperWrapper;

namespace SecureChat.Server
{
    internal class Program
    {
        static void Main(string[] args)
        {
            ManagedDataStorageFactory factory = new("Data Source=C:\\Users\\ntdls\\Desktop\\SecureChat\\test.db");

            factory.Execute("create table a(id int)");
        }
    }
}
