//using CassandraCSharpDriver;
using Cassandra;
using Microsoft.AspNetCore.DataProtection.KeyManagement;

namespace c_sharp_project;

public class Program
{
    public static void Main(string[] args)
    {
        Console.WriteLine("Hello Cassandra!");

        Cluster cluster = Cluster.Builder()
            .AddContactPoints("10.166.70.10")
            .WithPort(9042)
            .WithLoadBalancingPolicy(new DCAwareRoundRobinPolicy("SearchAnalytics"))
            .WithAuthProvider(new PlainTextAuthProvider("cassandra", "cassandra"))
            .Build();

        var session = cluster.Connect();

        var ps = session.Prepare("SELECT first_name FROM perftesting.users where username=?");
        var statement = ps.Bind(1).SetPageSize(1000);
        var rs = session.Execute(statement);

        foreach (var row in rs)
        {
            Console.WriteLine("- {0}", row["first_name"]);
        }
    }
}