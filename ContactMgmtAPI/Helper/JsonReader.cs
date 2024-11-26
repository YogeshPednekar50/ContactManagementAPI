using ContactMgmtAPI.Model;
using Newtonsoft.Json;
using System.Text.Json;

namespace ContactMgmtAPI.Helper
{
    public static class clsJsonReader
    {
        public static List<Contact> GetJsonData()
        {
            try
            {
                string jsonResult = File.ReadAllText(Directory.GetCurrentDirectory() + @"\Json\contacts.json");

                return JsonConvert.DeserializeObject<List<Contact>>(jsonResult);
            }
            catch(Exception ex)
            {
                throw ex;
            }
            
        }

    }
}
