using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MarketManagement.Models.Response
{
    public class UserAuthenticationResponse
    {
        public string PhoneNumber;
        public AuthResponseType AuthResponseType;

        public UserAuthenticationResponse()
        {
        }

        public UserAuthenticationResponse(string phoneNumber)
        {
            PhoneNumber = phoneNumber;
        }
    }

    public enum AuthResponseType: byte
    {
        UserNotFound = 1,
        UserCredentialsInvalid = 2,
        UserFound = 3
    }
}
