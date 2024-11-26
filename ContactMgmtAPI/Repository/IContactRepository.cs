using ContactMgmtAPI.Model;

namespace ContactMgmtAPI.Repository
{
    public interface IContactRepository
    {

        void AddContact(Contact contact);
        List<Contact> GetContacts();

        Contact? GetContactById(int id);

        void UpdateContact(int id, Contact contact);

        void DeleteContact(int id);

    }
}
