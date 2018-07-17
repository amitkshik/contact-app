namespace contact_app.Models
{
    public class Person
    {
         public int Id { get; set; }
        public string FirstName { get; set; }
           public string LastName { get; set; }
     
        public bool IsDeleted { get; set; }
        public int PersonTypeId { get; set; }

        public virtual  Supplier Supplier { get; set; }
        public virtual  Customer Customer { get; set; }
      
    }
}