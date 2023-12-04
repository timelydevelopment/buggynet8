using Microsoft.Data.Sqlite;
using System.Security.Cryptography;
using System.Text;
public static class Database
{
    public static void BuildDatabase(this SqliteConnection connection, string username, string password)
    {
      
        var command = connection.CreateCommand();
        command.CommandText =
        @"
            CREATE TABLE User(username text, passwordhash text);
            INSERT INTO User(username, passwordhash) VALUES ('bob', 'asfasfd');
            CREATE TABLE Product(name text, price number);
            INSERT INTO Product(name, price) VALUES('Apple', 30);
            INSERT INTO Product(name, price) VALUES('Banana', 30);
            INSERT INTO Product(name, price) VALUES('Orange', 40);
            INSERT INTO Product(name, price) VALUES('Mandarin', 30);
            INSERT INTO Product(name, price) VALUES('Grapes', 30);
            INSERT INTO Product(name, price) VALUES('Kiwifruit', 30);
            
        ";
        connection.Open();

        command.ExecuteNonQuery();  

        var sha1 = SHA1.Create();
        var passwordHash = Convert.ToHexString(sha1.ComputeHash(Encoding.UTF8.GetBytes(password)));

        var c2 = connection.CreateCommand();
        c2.CommandText =$"INSERT INTO User(username, passwordhash) VALUES ('{username}', '{passwordHash}');";
        c2.ExecuteNonQuery();
    }


}