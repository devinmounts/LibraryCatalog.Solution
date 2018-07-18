using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;

namespace LibraryCatalog.Models
{
        public class Author
        {
            public int Id { get; set; }
            public string Name { get; set; }

            public Author(string name, int id = 0)
            {
                
                Id = id;
                Name = name;
            }

            public override int GetHashCode()
            {
                return this.Name.GetHashCode();
            }

            public override bool Equals(System.Object otherAuthor)
            {
                if (!(otherAuthor is Author))
                {
                    return false;
                }
                else
                {
                    Author newAuthor = (Author)otherAuthor;
                    bool idEquality = (this.Id == newAuthor.Id);
                    bool nameEquality = (this.Name == newAuthor.Name);

                    return (idEquality && nameEquality);
                }
            }

     

            public void Save()
            {
                MySqlConnection conn = DB.Connection();
                conn.Open();

                var cmd = conn.CreateCommand() as MySqlCommand;
                cmd.CommandText = @"INSERT INTO authors (name) VALUES (@name);";

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
           
            public static List<Author> GetAll()
            {
                List<Author> allAuthors = new List<Author> { };
                MySqlConnection conn = DB.Connection();
                conn.Open();

                MySqlCommand cmd = conn.CreateCommand() as MySqlCommand;
                cmd.CommandText = @"SELECT * FROM authors;";

                MySqlDataReader rdr = cmd.ExecuteReader() as MySqlDataReader;

                while (rdr.Read())
                {
                    int id = rdr.GetInt32(0);
                    string name = rdr.GetString(1);

                    Author newAuthor = new Author(name, id);
                    allAuthors.Add(newAuthor);
                }

                conn.Close();

                if (conn != null)
                {
                    conn.Dispose();
                }

                return allAuthors;
            }

            public static Author Find(int id)
            {
                MySqlConnection conn = DB.Connection();
                conn.Open();

                var cmd = conn.CreateCommand() as MySqlCommand;
                cmd.CommandText = @"SELECT * FROM `authors` WHERE id = @thisId;";

                MySqlParameter thisId = new MySqlParameter();
                thisId.ParameterName = "@thisId";
                thisId.Value = id;
                cmd.Parameters.Add(thisId);

                var rdr = cmd.ExecuteReader() as MySqlDataReader;

                int authorId = 0;
                string name = string.Empty;

                while (rdr.Read())
                {
                    authorId = rdr.GetInt32(0);
                    name = rdr.GetString(1);
                }

                Author foundAuthor = new Author(name, authorId);

                conn.Close();
                if (conn != null)
                {
                    conn.Dispose();
                }

                return foundAuthor;
            }

            public void Edit(string newName)
            {
                MySqlConnection conn = DB.Connection();
                conn.Open();

                var cmd = conn.CreateCommand() as MySqlCommand;
                cmd.CommandText = @"UPDATE authors SET name = @newName WHERE id = @searchId;";

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
                cmd.CommandText = @"DELETE FROM authors WHERE id = @AuthorId; DELETE FROM catalog WHERE author_id = @AuthorId;";

                MySqlParameter authorIdParameter = new MySqlParameter();
                authorIdParameter.ParameterName = "@AuthorId";
                authorIdParameter.Value = Id;
                cmd.Parameters.Add(authorIdParameter);

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
                cmd.CommandText = @"DELETE FROM authors;";

                cmd.ExecuteNonQuery();

                conn.Close();
                if (conn != null)
                {
                    conn.Dispose();
                }
            }

            public void AddBook(Book newBook)
            {
                MySqlConnection conn = DB.Connection();
                conn.Open();
                var cmd = conn.CreateCommand() as MySqlCommand;
                cmd.CommandText = @"INSERT INTO catalog (book_id, author_id) VALUES (@BookId, @AuthorId);";

                MySqlParameter book_id = new MySqlParameter();
                book_id.ParameterName = "@BookId";
                book_id.Value = newBook.Id;
                cmd.Parameters.Add(book_id);

                MySqlParameter author_id = new MySqlParameter();
                author_id.ParameterName = "@AuthorId";
                author_id.Value = Id;
                cmd.Parameters.Add(author_id);

                cmd.ExecuteNonQuery();
                conn.Close();
                if (conn != null)
                {
                    conn.Dispose();
                }
            }

            public List<Book> GetBooks()
            {
                MySqlConnection conn = DB.Connection();
                conn.Open();
                var cmd = conn.CreateCommand() as MySqlCommand;
                cmd.CommandText = @"SELECT books.* FROM authors
                                JOIN catalog ON (authors.id = catalog.author_id)
                                JOIN books ON (catalog.book_id = books.id)
                                WHERE authors.id = @authorId;";

                MySqlParameter authorIdParameter = new MySqlParameter();
                authorIdParameter.ParameterName = "@authorId";
                authorIdParameter.Value = Id;
                cmd.Parameters.Add(authorIdParameter);

                var rdr = cmd.ExecuteReader() as MySqlDataReader;

                List<Book> books = new List<Book> { };

                while (rdr.Read())
                {
                    int bookId = rdr.GetInt32(0);
                    string bookName = rdr.GetString(1);
                    Book newBook = new Book(bookName, bookId);
                    books.Add(newBook);
                }

                conn.Close();
                if (conn != null)
                {
                    conn.Dispose();
                }

                return books;
            }

            public List<Book> GetAllBooks()
            {
                return Book.GetAll();
            }
           
        }
    }
}
