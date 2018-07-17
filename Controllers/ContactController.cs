using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using contact_app.Models;
using contact_app.Models.Dto;
using contact_app.Models.Enum;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace contact_app.Controllers
{
    // set route attribute to make request as 'api/contact'
    [Route("api/[controller]")]
    public class ContactController : Controller
    {
        private readonly ContactAppContext _context;

        // initiate database context
        public ContactController(ContactAppContext context)
        {
            _context = context;
        }

        [HttpGet]
        [Route("getAllContact")]
      //  public IEnumerable<Contact> GetAll()
       // {
            // fetch all contact records 
        //    return _context.Contact.ToList();
       // }
 public IEnumerable<PersonDto> GetAll()
        {
            // fetch all contact records
            var people = _context.Person.Where(t => t.IsDeleted == false)
            .Include(p => p.Customer)
            .Include(p => p.Supplier)
            .ToList();
        
            var result = new List<PersonDto>();
         
            people.ForEach(t =>  {

            var personDto = new PersonDto{
            Id = t.Id,
            FirstName = t.FirstName,
            LastName = t.LastName,
            
            
            PersonTypeId = t.PersonTypeId
            };

            if(t.PersonTypeId == 0){
             personDto.Birthday = t.Customer?.Birthday;
             personDto.Email = t.Customer.Email;
            }

            if(t.PersonTypeId == 1){
            personDto.Telephone = t.Supplier?.Telephone;
            }

            result.Add(personDto);
            });

          return result;
        }

        [HttpGet("{id}")]
        [Route("getContact")]
        public IActionResult GetById(long id)
        {
            // filter contact records by contact id
            var item = _context.Person.FirstOrDefault(t => t.Id == id);
            if (item == null)
            {
                return NotFound();
            }

            var personDto = new PersonDto{
                Id = item.Id,
                FirstName = item.FirstName,
                LastName = item.LastName,
                Telephone = item.Supplier.Telephone,
                Birthday = item.Customer.Birthday,
                Email = item.Customer.Email
                };

            return new ObjectResult(personDto);
        }
        [HttpPost]
        [Route("addContact")]
        public IActionResult Create([FromBody] PersonDto item)
        {
            // set bad request if contact data is not provided in body
            if (item == null)
            {
                return BadRequest();
            }
            var person = new Person{
            FirstName = item.FirstName,
            LastName = item.LastName,
            PersonTypeId = item.PersonTypeId
            };

            person.Supplier = new Supplier{
                Telephone = item.Telephone
            };

            person.Customer = new Customer{
                Birthday = item.Birthday,
                Email = item.Email
            };          
            
            _context.Person.Add(person);
            _context.SaveChanges();

            return Ok( new { message= "Person is added successfully."});
        }

        [HttpPut("{id}")]
        [Route("updateContact")]
        public IActionResult Update(long id, [FromBody] PersonDto item)
        {
            // set bad request if contact data is not provided in body
            if (item == null || id == 0)
            {
                return BadRequest();
            }

            var contact = _context.Person.FirstOrDefault(t => t.Id == id);
            if (contact == null)
            {
                return NotFound();
            }

            var person = new Person{
            FirstName = item.FirstName,
            LastName = item.LastName,
            PersonTypeId = item.PersonTypeId
            };
            
            if((PersonTypes)item.PersonTypeId == PersonTypes.Supplier){
                    person.Supplier = new Supplier{
                        Telephone = item.Telephone
                    };
            }
            
            if((PersonTypes)item.PersonTypeId == PersonTypes.Customer){
                person.Customer = new Customer{
                Birthday = item.Birthday,
                Email = item.Email
                };
            }

            _context.Person.Update(person);
            _context.SaveChanges();
            return Ok( new { message= "Contact is updated successfully."});
        }


        [HttpDelete("{id}")]
        [Route("deleteContact")]
        public IActionResult Delete(long id)
        {
            var contact = _context.Person.FirstOrDefault(t => t.Id == id);
            if (contact == null)
            {
                return NotFound();
            }
            contact.IsDeleted = true;
            _context.Person.Update(contact);
            _context.SaveChanges();
            return Ok( new { message= "Contact is deleted successfully."});
        }
    }
}
