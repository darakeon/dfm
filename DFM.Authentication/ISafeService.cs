using System;
using DFM.Entities;

namespace DFM.Authentication
{
    public interface ISafeService
    {
        User SelectUserByEmail(String username);

        String ValidateAndGet(String username, String password);


    }
}
