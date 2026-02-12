using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace Egresoss.Models
{
    internal class CategoryBudget
    {
        [PrimaryKey]
        public string CategoryName { get; set; }
        public decimal Limit { get; set; }
        public decimal Spent { get; set; }
    }
}
