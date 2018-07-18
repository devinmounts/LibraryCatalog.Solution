using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;

namespace LibraryCatalog.Models
{
    public class Patron
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public Patron(string name, int id = 0)
        {

            Id = id;
            Name = name;
        }

        public override int GetHashCode()
        {
            return this.Name.GetHashCode();
        }

        public override bool Equals(System.Object otherPatron)
        {
            if (!(otherPatron is Patron))
            {
                return false;
            }
            else
            {
                Patron newPatron = (Patron)otherPatron;
                bool idEquality = (this.Id == newPatron.Id);
                bool nameEquality = (this.Name == newPatron.Name);

                return (idEquality && nameEquality);
            }
        }



        public void Save()
        {
            MySqlConnection conn = DB.Connection();
            conn.Open();

            var cmd = conn.CreateCommand() as MySqlCommand;
            cmd.CommandText = @"INSERT INTO patrons (name) VALUES (@name);";

            MySqlParameter name = new MySqlParameter();
            name.ParameterName = "@name";
            name.Value = Name;
            cmd.Parameters.Add(name);

            cmd.ExecuteNonQuery();
            Id = (int)cmd.LastInsertedId;

            conn.Close();
            if (conn != null)
            {
                conn.Dispose();
            }
        }

        public static List<Patron> GetAll()
        {
            List<Patron> allPatrons = new List<Patron> { };
            MySqlConnection conn = DB.Connection();
            conn.Open();

            MySqlCommand cmd = conn.CreateCommand() as MySqlCommand;
            cmd.CommandText = @"SELECT * FROM patrons;";

            MySqlDataReader rdr = cmd.ExecuteReader() as MySqlDataReader;

            while (rdr.Read())
            {
                int id = rdr.GetInt32(0);
                string name = rdr.GetString(1);

                Patron newPatron = new Patron(name, id);
                allPatrons.Add(newPatron);
            }

            conn.Close();

            if (conn != null)
            {
                conn.Dispose();
            }

            return allPatrons;
        }

        public static Patron Find(int id)
        {
            MySqlConnection conn = DB.Connection();
            conn.Open();

            var cmd = conn.CreateCommand() as MySqlCommand;
            cmd.CommandText = @"SELECT * FROM `patrons` WHERE id = @thisId;";

            MySqlParameter thisId = new MySqlParameter();
            thisId.ParameterName = "@thisId";
            thisId.Value = id;
            cmd.Parameters.Add(thisId);

            var rdr = cmd.ExecuteReader() as MySqlDataReader;

            int patronId = 0;
            string name = string.Empty;

            while (rdr.Read())
            {
                patronId = rdr.GetInt32(0);
                name = rdr.GetString(1);
            }

            Patron foundPatron = new Patron(name, patronId);

            conn.Close();
            if (conn != null)
            {
                conn.Dispose();
            }

            return foundPatron;
        }

        public void Edit(string newName)
        {
            MySqlConnection conn = DB.Connection();
            conn.Open();

            var cmd = conn.CreateCommand() as MySqlCommand;
            cmd.CommandText = @"UPDATE patrons SET name = @newName WHERE id = @searchId;";

            MySqlParameter searchId = new MySqlParameter();
            searchId.ParameterName = "@searchId";
            searchId.Value = Id;
            cmd.Parameters.Add(searchId);

            MySqlParameter name = new MySqlParameter();
            name.ParameterName = "@newName";
            name.Value = newName;
            cmd.Parameters.Add(name);

            cmd.ExecuteNonQuery();

            Name = newName;


            conn.Close();
            if (conn != null)
            {
                conn.Dispose();
            }
        }

        public void Delete()
        {
            MySqlConnection conn = DB.Connection();
            conn.Open();
            var cmd = conn.CreateCommand() as MySqlCommand;
            cmd.CommandText = @"DELETE FROM patrons WHERE id = @PatronId; DELETE FROM checkout WHERE patron_id = @PatronId;";

            MySqlParameter patronIdParameter = new MySqlParameter();
            patronIdParameter.ParameterName = "@PatronId";
            patronIdParameter.Value = Id;
            cmd.Parameters.Add(patronIdParameter);

            cmd.ExecuteNonQuery();
            if (conn != null)
            {
                conn.Close();
            }
        }

        public static void DeleteAll()
        {
            MySqlConnection conn = DB.Connection();
            conn.Open();

            var cmd = conn.CreateCommand() as MySqlCommand;
            cmd.CommandText = @"DELETE FROM patrons;";

            cmd.ExecuteNonQuery();

            conn.Close();
            if (conn != null)
            {
                conn.Dispose();
            }
        }

        public void AddCopy(Copy newCopy)
        {
            MySqlConnection conn = DB.Connection();
            conn.Open();
            var cmd = conn.CreateCommand() as MySqlCommand;
            cmd.CommandText = @"INSERT INTO checkout (copy_id, patron_id) VALUES (@CopyId, @PatronId);";

            MySqlParameter copy_id = new MySqlParameter();
            copy_id.ParameterName = "@CopyId";
            copy_id.Value = newCopy.Id;
            cmd.Parameters.Add(copy_id);

            MySqlParameter author_id = new MySqlParameter();
            author_id.ParameterName = "@PatronId";
            author_id.Value = Id;
            cmd.Parameters.Add(author_id);

            cmd.ExecuteNonQuery();
            conn.Close();
            if (conn != null)
            {
                conn.Dispose();
            }
        }

        public List<Copy> GetCopys()
        {
            MySqlConnection conn = DB.Connection();
            conn.Open();
            var cmd = conn.CreateCommand() as MySqlCommand;
            cmd.CommandText = @"SELECT copies.* FROM patrons
                                JOIN checkout ON (patrons.id = checkout.patron_id)
                                JOIN copies ON (checkout.copy_id = copies.id)
                                WHERE patrons.id = @patronId;";

            MySqlParameter patronIdParameter = new MySqlParameter();
            patronIdParameter.ParameterName = "@PatronId";
            patronIdParameter.Value = Id;
            cmd.Parameters.Add(patronIdParameter);

            var rdr = cmd.ExecuteReader() as MySqlDataReader;

            List<Copy> copies = new List<Copy> { };

            while (rdr.Read())
            {
                int copyId = rdr.GetInt32(0);
                DateTime dueDate = rdr.GetDateTime(1);
                Copy newCopy = new Copy(dueDate, copyId);
                copies.Add(newCopy);
            }

            conn.Close();
            if (conn != null)
            {
                conn.Dispose();
            }

            return copies;
        }

        public List<Copy> GetAllCopies()
        {
            return Copy.GetAll();
        }

    }
}
}
