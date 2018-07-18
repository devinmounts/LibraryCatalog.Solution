using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using LibraryCatalog.Models;

namespace LibraryCatalog.ViewModels
{
    public class CatalogViewModel
    {
        public Book BookModel { get; set; }
        public Author AuthorModel { get; set; }

        public CatalogViewModel(Book book, Author author)
        {
            BookModel = book;
            AuthorModel = author;
        }
    }
}
