using FullContact.Domain.DataTransferObject;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;

namespace FullContactDataAccessLayer
{
    public interface IFullContact
    {
         System.Threading.Tasks.Task<Person> getContactByEmailAsync(string email);
    }
    public class FullContact
    {
        public async System.Threading.Tasks.Task<Person> getContactByEmailAsync(string email)
        {
            try
            {
                var client = new HttpClient();

                HttpRequestMessage objRequest = new HttpRequestMessage(HttpMethod.Get,
                    "https://api.fullcontact.com/v2/person.json?email=" + email);

                objRequest.Headers.Add("X-FullContact-APIKey", "3478f712076cc8c0");
                HttpResponseMessage response = await client.SendAsync(objRequest);

                if (!response.IsSuccessStatusCode)
                    return null;

                HttpContent responseContent = response.Content;

                using (var reader = new StreamReader(await responseContent.ReadAsStreamAsync()))
                {
                    string result = await reader.ReadToEndAsync();
                    Person objPerson = JsonConvert.DeserializeObject<Person>(result);
                    return objPerson;
                }
            }
            catch (Exception ex)
            {
                //log error
                throw ex;
            }

        }

    }
}
