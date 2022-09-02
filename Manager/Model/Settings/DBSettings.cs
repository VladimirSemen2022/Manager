
namespace Work_with_SQL_Table__HW_4_
{
    class DBSettings
    {
        public string Server { get; set; }
        public string Database { get; set; }
        public bool Trusted_Connection { get; set; }

        public DBSettings (string Server, string Database, bool Trusted_Connection)
        {
            this.Server = Server;
            this.Database = Database;
            this.Trusted_Connection = Trusted_Connection;
        }

        public override string ToString()
        {
            return $"Server={this.Server};Database={this.Database};Trusted_Connection={this.Trusted_Connection}";
        }
    }
}
