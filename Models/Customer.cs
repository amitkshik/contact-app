using System;
namespace contact_app.Models
{
    public class Customer
    {
        public int Id { get; set; }
        public DateTime? Birthday { get; set; }
        public string Email { get; set; }
         public int PersonId { get; set; }
        public virtual  Person Person { get; set; }
    }
}