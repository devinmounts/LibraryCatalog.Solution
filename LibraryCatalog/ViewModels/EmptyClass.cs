using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using LibraryCatalog.Models;

namespace LibraryCatalog.ViewModels
{
    public class BookDetailsViewModel
    {
        public Book BookModel { get; set; }
        public Patron PatronModel { get; set; }

        public BookDetailsViewModel(Book book, Patron patron)
        {
            BookModel = book;
            PatronModel = patron;
        }
    }
}
