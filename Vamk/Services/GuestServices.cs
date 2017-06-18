using Microsoft.WindowsAzure.MobileServices;
using System.Threading.Tasks;
using Vamk.Models;

namespace Vamk.Services
{
    public class GuestServices
    {
        private static MobileServiceClient client;
        private IMobileServiceTable<Guest> guestTable;
        const string applicationURL = @"https://azuremobapptest.azurewebsites.net";


        public GuestServices()
        {
            client = new MobileServiceClient(applicationURL);
        }
        public async Task SaveUser(Guest guest)
        {
            guestTable = client.GetTable<Guest>();

            await guestTable.InsertAsync(guest);

        }
    }
}