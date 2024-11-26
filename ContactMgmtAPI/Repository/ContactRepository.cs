using ContactMgmtAPI.Model;
using System.Collections;
using System.Runtime.CompilerServices;
using Newtonsoft.Json;
using ContactMgmtAPI.Helper;


namespace ContactMgmtAPI.Repository
{

    //Exception handling - global


    public class ContactRepository : IContactRepository
    {
        private List<Contact> contacts = new List<Contact>();

        public ContactRepository() 
        {
            contacts = clsJsonReader.GetJsonData();

            //contacts.Add(new Contact { Id = 8, FirstName = "Yogesh2", LastName = "Pednekar2", Email = "yogeshpednekar6@gmail.com" });

        }

       

        public void AddContact(Contact contact)
        {
            int nextId = contacts.Max(x => x.Id) + 1;
            contact.Id = nextId;
            contacts.Add(contact);

        }

        public void DeleteContact(int id)
        {
            var contact = contacts.FirstOrDefault(x => x.Id == id);
            if (contact != null)
            {
                contacts.Remove(contact);
            }
        }

        public Contact? GetContactById(int id)
        {
            var contact = contacts.FirstOrDefault(x => x.Id == id);
            if (contact != null)
            {
                return new Contact
                {
                    Id = contact.Id,
                    FirstName = contact.FirstName,
                    LastName = contact.LastName,
                    Email = contact.Email
                };
            }

            return null;
           
        }

        public List<Contact> GetContacts() => contacts;


        public void UpdateContact(int id, Contact contact)
        {
            if (id != contact.Id) return;
            var contactUpdate = contacts.FirstOrDefault(x => x.Id == id);
            if (contactUpdate != null)
            {
                contactUpdate.Email = contact.Email;
                contactUpdate.FirstName = contact.FirstName;
                contactUpdate.LastName = contact.LastName;
            }
        }
    }
}
