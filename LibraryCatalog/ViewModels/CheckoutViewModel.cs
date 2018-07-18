using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using LibraryCatalog.Models;

namespace LibraryCatalog.ViewModels
{
    public class CheckoutViewModel
    {
        public Patron PatronModel { get; set; }

        public CheckoutViewModel(Patron patron)
        {
            PatronModel = patron;
        }

        public List<Book> GetAll()
        {
            return Book.GetAll();
        }
    }
}
