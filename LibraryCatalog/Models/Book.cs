using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;

namespace LibraryCatalog.Models
{
    public class Book
    {
        public int Id { get; set; }
        public string Title { get; set; }
        private int copies = 10;

        public Book(string title, int id = 0)
        {

            Id = id;
            Title = title;
            copies--;
        }

        public override int GetHashCode()
        {
            return this.Title.GetHashCode();
        }

        public override bool Equals(System.Object otherBook)
        {
            if (!(otherBook is Book))
            {
                return false;
            }
            else
            {
                Book newBook = (Book)otherBook;
                bool idEquality = (this.Id == newBook.Id);
                bool titleEquality = (this.Title == newBook.Title);

                return (idEquality && titleEquality);
            }
        }

        public int GetCopies()
        {
            return copies;
        }

        public void ReturnCopy()
        {
            copies++;
        }

        public void Save()
        {
            MySqlConnection conn = DB.Connection();
            conn.Open();

            var cmd = conn.CreateCommand() as MySqlCommand;
            cmd.CommandText = @"INSERT INTO books (title, copies) VALUES (@title, @copies);";

            MySqlParameter title = new MySqlParameter();
            title.ParameterName = "@title";
            title.Value = Title;
            cmd.Parameters.Add(title);

            MySqlParameter copiesParam = new MySqlParameter();
            copiesParam.ParameterName = "@copies";
            copiesParam.Value = copies;
            cmd.Parameters.Add(copiesParam);

            cmd.ExecuteNonQuery();
            Id = (int)cmd.LastInsertedId;

            conn.Close();
            if (conn != null)
            {
                conn.Dispose();
            }
        }

        public static List<Book> GetAll()
        {
            List<Book> allBooks = new List<Book> { };
            MySqlConnection conn = DB.Connection();
            conn.Open();

            MySqlCommand cmd = conn.CreateCommand() as MySqlCommand;
            cmd.CommandText = @"SELECT * FROM books;";

            MySqlDataReader rdr = cmd.ExecuteReader() as MySqlDataReader;

            while (rdr.Read())
            {
                int id = rdr.GetInt32(0);
                string title = rdr.GetString(1);

                Book newBook = new Book(title, id);
                allBooks.Add(newBook);
            }

            conn.Close();

            if (conn != null)
            {
                conn.Dispose();
            }

            return allBooks;
        }

        public static Book Find(int id)
        {
            MySqlConnection conn = DB.Connection();
            conn.Open();

            var cmd = conn.CreateCommand() as MySqlCommand;
            cmd.CommandText = @"SELECT * FROM books WHERE id = @thisId;";

            MySqlParameter thisId = new MySqlParameter();
            thisId.ParameterName = "@thisId";
            thisId.Value = id;
            cmd.Parameters.Add(thisId);

            var rdr = cmd.ExecuteReader() as MySqlDataReader;

            int bookId = 0;
            string title = "";

            while (rdr.Read())
            {
                bookId = rdr.GetInt32(0);
                title = rdr.GetString(1);
            }

            Book foundBook = new Book(title, bookId);

            conn.Close();
            if (conn != null)
            {
                conn.Dispose();
            }

            return foundBook;
        }

        public void Edit(string newTitle)
        {
            MySqlConnection conn = DB.Connection();
            conn.Open();

            var cmd = conn.CreateCommand() as MySqlCommand;
            cmd.CommandText = @"UPDATE books SET title = @newTitle WHERE id = @searchId;";

            MySqlParameter searchId = new MySqlParameter();
            searchId.ParameterName = "@searchId";
            searchId.Value = Id;
            cmd.Parameters.Add(searchId);

            MySqlParameter title = new MySqlParameter();
            title.ParameterName = "@newTitle";
            title.Value = newTitle;
            cmd.Parameters.Add(title);

            cmd.ExecuteNonQuery();

            Title = newTitle;


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
            cmd.CommandText = @"DELETE FROM books WHERE id = @BookId; DELETE FROM catalog WHERE book_id = @BookId;";

            MySqlParameter bookIdParameter = new MySqlParameter();
            bookIdParameter.ParameterName = "@BookId";
            bookIdParameter.Value = Id;
            cmd.Parameters.Add(bookIdParameter);

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
            cmd.CommandText = @"DELETE FROM books;";

            cmd.ExecuteNonQuery();

            conn.Close();
            if (conn != null)
            {
                conn.Dispose();
            }
        }

        public void AddAuthor(Author newAuthor)
        {
            MySqlConnection conn = DB.Connection();
            conn.Open();
            var cmd = conn.CreateCommand() as MySqlCommand;
            cmd.CommandText = @"INSERT INTO catalog (book_id, author_id) VALUES (@BookId, @AuthorId);";

            MySqlParameter book_id = new MySqlParameter();
            book_id.ParameterName = "@BookId";
            book_id.Value = Id;
            cmd.Parameters.Add(book_id);

            MySqlParameter author_id = new MySqlParameter();
            author_id.ParameterName = "@AuthorId";
            author_id.Value = newAuthor.Id;
            cmd.Parameters.Add(author_id);

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
            cmd.CommandText = @"INSERT INTO copies_books (copy_id, book_id) VALUES (@CopyId, @BookId);";

            MySqlParameter copy_id = new MySqlParameter();
            copy_id.ParameterName = "@CopyId";
            copy_id.Value = newCopy.Id;
            cmd.Parameters.Add(copy_id);

            MySqlParameter book_id = new MySqlParameter();
            book_id.ParameterName = "@BookId";
            book_id.Value = Id;
            cmd.Parameters.Add(book_id);

            cmd.ExecuteNonQuery();
            conn.Close();
            if (conn != null)
            {
                conn.Dispose();
            }
        }

        public List<Author> GetAuthors()
        {
            MySqlConnection conn = DB.Connection();
            conn.Open();
            var cmd = conn.CreateCommand() as MySqlCommand;
            cmd.CommandText = @"SELECT authors.* FROM books
                                JOIN catalog ON (books.id = catalog.book_id)
                                JOIN authors ON (catalog.author_id = authors.id)
                                WHERE books.id = @bookId;";

            MySqlParameter bookIdParameter = new MySqlParameter();
            bookIdParameter.ParameterName = "@bookId";
            bookIdParameter.Value = Id;
            cmd.Parameters.Add(bookIdParameter);

            var rdr = cmd.ExecuteReader() as MySqlDataReader;

            List<Author> authors = new List<Author> { };

            while (rdr.Read())
            {
                int authorId = rdr.GetInt32(0);
                string authorName = rdr.GetString(1);
                Author newAuthor = new Author(authorName, authorId);
                authors.Add(newAuthor);
            }

            conn.Close();
            if (conn != null)
            {
                conn.Dispose();
            }

            return authors;
        }

        public List<Author> GetAllAuthors()
        {
            return Author.GetAll();
        }
    }
}
