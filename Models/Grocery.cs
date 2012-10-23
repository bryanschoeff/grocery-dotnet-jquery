using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace GroceryList.Models
{
    public class Grocery
    {
        public int Id { get; set; }
        public string Item { get; set; }
        public string Section { get; set; }
        public bool Done { get; set; }
    }

    public class GroceryContext : DbContext
    {
        public DbSet<Grocery> Db { get; set; }

    }
}