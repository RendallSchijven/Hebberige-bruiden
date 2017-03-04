using System;
using System.Collections.Generic;
using System.Web;
using WebMatrix.Data;


/// <summary>
/// Every action that takes place on an authentication basis.
/// </summary>
public static class Security
{
    /// <summary>
    /// Gets random salt from bcrypt, then hashes password
    /// </summary>
    private static string HashPassword(string password)
    {
        //workfactor 13 seems the sweet spot ~1-2sec
        var salt = BCrypt.Net.BCrypt.GenerateSalt(13);
        return BCrypt.Net.BCrypt.HashPassword(password, salt);
    }

    /// <summary>
    /// Creates an account
    /// </summary>
    public static string CreateAccount(string password)
    {
        return HashPassword(password);
    }

    /// <summary>
    /// Login to blyat
    /// </summary>
    public static int Login(string email, string password)
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// Resets password and sends email with reset key
    /// </summary>
    public static bool ResetPassword()
    {
        //we need a webserver for this.
        throw new NotImplementedException();
    }

    /// <summary>
    /// Check for sessions
    /// </summary>
    public static bool IsAuthenticated(int session_id)
    {
        throw new NotImplementedException();
    }
}
