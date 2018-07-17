using System;

namespace contact_app.Models.Dto
{
    public class PersonDto
    {
         public int? Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public int PersonTypeId { get; set; }
        public DateTime? Birthday { get; set; }
        public string Email { get; set; }

        public string Telephone { get; set; }
    }
}