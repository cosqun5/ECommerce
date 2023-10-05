using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Concrate

{
    public class Product
    {
        public int Id { get; set; }
        [Required, MaxLength(100)]

        public string Name { get; set; }

        public string Description { get; set; }
        public double Price { get; set; }
        public DateTime Created { get; set; }
        public string ImagePath { get; set; }
        //public int? CategoryId { get; set; }
        //public Category? Category { get; set; }
    }
}
