namespace contact_app.Models
{
    public class Supplier
    {
        public int Id { get; set; }
        public int PersonId { get; set; }
        public virtual  Person Person { get; set; }
        public string Telephone { get; set; }

    }
}