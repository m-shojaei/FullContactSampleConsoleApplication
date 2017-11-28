using FullContact.Domain.DataTransferObject;
using FullContactDataAccessLayer;
using Nito.AsyncEx;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace FullContact_API_integration
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {

                AsyncContext.Run(() =>
                {
                    PrepareSearch();
                    preformSearch();
                });
            }
            catch (Exception ex)
            {
                WriteWithColor("an error occured during the proccess", ConsoleColor.DarkRed);
            }
        }

        private async static void preformSearch()
        {
            try
            {
                var enterdEmail = Console.ReadLine();

                if (enterdEmail == "0")//closing the console on user entering 0
                    Environment.Exit(0);

                var retuernedContact = await ContactByEmail(enterdEmail);

                Console.Clear();

                ShowResult(enterdEmail, retuernedContact);
                PrepareSearch();
                preformSearch();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        private static void PrepareSearch()
        {
            Console.WriteLine("-------------------------------------------------");
            Console.WriteLine("write down the email of your contact and press enter");
            Console.WriteLine("for exit enter 0");
        }

        private static void ShowResult(string enterdEmail, Person personResult)
        {
            #region no contact reurned
            if (personResult == null)
            {
                WriteWithColor("no result for " + enterdEmail, ConsoleColor.Red);
                return;
            }
            #endregion

            #region valid contact returned
            WriteWithColor("Here is your search result for " + enterdEmail + ":", ConsoleColor.Green);
            Console.WriteLine("Likelihood: {0}", personResult.Likelihood);
            Console.WriteLine("FullName: {0}", personResult.ContactInfo.FullName);
            Console.WriteLine("GivenName: {0}", personResult.ContactInfo.GivenName);

            if (personResult.ContactInfo.WebSites != null)
                Console.WriteLine("Website: {0}", personResult.ContactInfo.WebSites[0]);

            if (personResult.SocialProfiles != null)
                Console.WriteLine("Profile: {0} url: {1}", personResult.SocialProfiles[0].TypeName,
                    personResult.SocialProfiles[0].Url);
            #endregion
        }

        static void WriteWithColor(string message, ConsoleColor color)
        {
            Console.ForegroundColor = color;
            Console.WriteLine(message);
            Console.ResetColor();
        }

        public static async Task<Person> ContactByEmail(string email)
        {
            try
            {
                FullContactDataAccessLayer.FullContact fullContact = new FullContactDataAccessLayer.FullContact();
                Person result = await fullContact.getContactByEmailAsync(email);
                return result;
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }
    }
}
