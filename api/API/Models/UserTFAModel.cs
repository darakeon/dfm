using System;

namespace DFM.API.Models
{
    public class UserTFAModel : BaseApiModel
    {
        public UserTFAModel(string code)
        {
            Code = code;
        }

        public string Code { get; }

        internal void Validate()
        {
            auth.ValidateTicketTFA(Code);
        }
    }
}
