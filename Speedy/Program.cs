using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Speedy
{
    class Program
    {
        static async Task Main(string[] args)
        {
            Console.OutputEncoding = Encoding.UTF8;
            Console.Write("Your email: ");
            string userName = Console.ReadLine();
            Console.Write("Your password: ");
            string password = Console.ReadLine();
            Console.Write("Phone number to lookup: ");
            string phoneNumber = Console.ReadLine();
            Speedy searchSite = new Speedy();
            await searchSite.openLogin();
            var loginResult = await searchSite.login(userName, password);
            var addresses = await searchSite.Search(phoneNumber);
            var nonOffices = addresses.Where(a => !a.IsOffice);
            var latestAddress = nonOffices.LastOrDefault();
            if (latestAddress != null)
            {
                Console.WriteLine($"Latest address: {latestAddress.FullAddress}");
            }
        }


        
    }
}
