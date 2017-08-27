using System;
using DFM.Entities.Enums;

namespace DFM.MVC.Models
{
    public class SafeModel : BaseModel
    {
        internal void Disable(String token)
        {
            Safe.DisableToken(token);
        }

        internal void TestUserVerificationToken(String token)
        {
            Safe.TestSecurityToken(token, SecurityAction.UserVerification);
        }

        internal void ActivateUser(String token)
        {
            Safe.ActivateUser(token);
        }

        internal void SendUserVerify(String email)
        {
            Safe.SendUserVerify(email);
        }



    }
}