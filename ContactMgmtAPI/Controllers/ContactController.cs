using ContactMgmtAPI.Helper;
using ContactMgmtAPI.Model;
using ContactMgmtAPI.Model.Response;
using ContactMgmtAPI.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace ContactMgmtAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ContactController : ControllerBase
    {
        private readonly ILogger<Contact> _logger;
        private IContactRepository contactRepository;
        public ContactController(IContactRepository _contactRepository, ILogger<Contact> logger)
        {
            
            contactRepository = _contactRepository;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllContact()
        {

            try
            {
                var contacts = contactRepository.GetContacts();

                if (contacts.Count == 0)
                {
                    _logger.LogWarning("GetAllContact: No contacts found!");
                    return NotFound(new BaseResponse()
                    {
                        Success = false,
                        StatusCode = ApplicationStatusCodes.NoDataFound,
                        Errors = new List<string>
                            {
                                "No contacts found!"
                            }
                    }); ;

                }
                return Ok(new BaseResponse()
                {
                    Result = contacts,
                    Success = true

                });


            }
            catch (Exception ex)
            {
                _logger.LogError("GetAllContact: " + ex.Message);
                return BadRequest(new BaseResponse()
                {
                    StatusCode = ApplicationStatusCodes.Exception,
                    Success = false,
                    Errors = new List<string>
                        {
                           ex.Message
                        }
                });

            }

        }

        [HttpGet]
        [Route("GetAllContactPage")]
        public async Task<IActionResult> GetAllContactPage(int pageNumber, int itemsPerPage)
        {

            try
            {
                var contacts = contactRepository.GetContacts().AsQueryable();

                var pageData = contacts
                                .OrderBy(x => x.Id)
                                .Skip((pageNumber - 1) * itemsPerPage)
                                .Take(itemsPerPage);

                var totalCount = contacts.Count();

                if (contacts.Count() == 0)
                {
                    _logger.LogWarning("GetAllContactPage: No contacts found!");
                    return NotFound(new BaseResponse()
                    {
                        Success = false,
                        StatusCode = ApplicationStatusCodes.NoDataFound,
                        Errors = new List<string>
                            {
                                "No contacts found!"
                            }
                    }); ;

                }
                return Ok(new BaseResponse()
                {
                    Result = new { 
                        contacts= pageData,
                        totalContacts = totalCount
                    },
                    Success = true

                });


            }
            catch (Exception ex)
            {
                _logger.LogError("GetAllContactPage: " + ex.Message);
                return BadRequest(new BaseResponse()
                {
                    StatusCode = ApplicationStatusCodes.Exception,
                    Success = false,
                    Errors = new List<string>
                        {
                           ex.Message
                        }
                });

            }

        }


        [HttpPost]
        [Route("Contact")]
        public async Task<IActionResult> Contact([FromBody] Contact contact)
        {

            if (ModelState.IsValid)
            {

                try
                {
                    var existingContact = contactRepository.GetContacts().FirstOrDefault(x => x.Email == contact.Email);

                    if (existingContact != null)
                    {
                        _logger.LogWarning("Contact with same email is already exists.");

                        return BadRequest(new BaseResponse()
                        {
                            Success = false,
                            Errors = new List<string>
                            {
                                "Contact with same email is already exists."
                            }
                        });


                    }

                    contactRepository.AddContact(contact);

                    return Ok(new BaseResponse()
                    {
                        Success = true,
                    });


                }
                catch (Exception ex)
                {
                    _logger.LogError("Contact: " + ex.Message);
                    return BadRequest(new BaseResponse()
                    {
                        Success = false,
                        Errors = new List<string>
                        {
                            ex.Message
                        }
                    });

                }

            }
            else
            {
                _logger.LogWarning("Invalid payload");
                return BadRequest(new BaseResponse()
                {
                    Success = false,
                    Errors = new List<string>
                {
                    "Invalid payload"
                }
                });

            }



        }


        [HttpPut]//("{id:int}")
        [Route("UpdateContact")]
        public async Task<IActionResult> UpdateContact(int id, Contact contact)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    _logger.LogWarning("Invalid payload");

                    return BadRequest(new BaseResponse()
                    {
                        Success = false,
                        Errors = new List<string>
                            {
                                "Invalid payload"
                            }
                    });

                }

                if (contact.Id == 0 || (id != contact.Id))
                {
                    _logger.LogWarning("Id is not valid");
                    return BadRequest(new BaseResponse()
                    {
                        Success = false,
                        Errors = new List<string>
                            {
                                "Id is not valid"
                            }
                    });
                }

                var _contact = contactRepository.GetContactById(contact.Id);


                if (_contact == null)
                {
                    _logger.LogWarning($"Contact not found with id {contact.Id}");
                    return NotFound(new BaseResponse()
                    {
                        Success = false,
                        Errors = new List<string>
                            {
                                $"Contact not found with id {contact.Id}"
                            }
                    });

                }


                contactRepository.UpdateContact(id, contact);

                return Ok(new BaseResponse()
                {
                    Success = true,
                });

            }
            catch (Exception ex)
            {
                _logger.LogError("UpdateContact: " + ex.Message);
                return BadRequest(new BaseResponse()
                {
                    Success = false,
                    Errors = new List<string>
                            {
                                ex.Message
                            }
                });
            }

        }


        [HttpDelete]//("{id}")
        [Route("DeleteContact")]
        public async Task<IActionResult> DeleteContact([FromQuery] int id)
        {
            try
            {
                var contact = contactRepository.GetContactById(id);

                if (contact == null)
                {
                    _logger.LogWarning($"Contact details not found with id {id}");
                    return NotFound(new BaseResponse()
                    {
                        Success = false,
                        Errors = new List<string>
                            {
                                $"Contact details not found with id {id}"
                            }
                    });

                }

                contactRepository.DeleteContact(id);

                return Ok(new BaseResponse()
                {
                    Success = true,
                });

            }
            catch (Exception ex)
            {
                _logger.LogError("DeleteContact: " + ex.Message);
                return BadRequest(new BaseResponse()
                {
                    Success = false,
                    Errors = new List<string>
                            {
                                ex.Message
                            }
                });
            }

        }

    }
}
