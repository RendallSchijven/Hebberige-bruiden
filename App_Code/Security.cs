using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI.WebControls;
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
    public static string CreateAccount(string email, string password)
    {
        string hash_pass = HashPassword(password);
        var db = Database.Open("hebberige-bruiden");

        string GetQuery = "SELECT email FROM bruidspaar WHERE email=@0";

        //test for existing email
        if (db.QuerySingle(GetQuery, email) != null) return "Email already set";
        
        string InsertQuery = "INSERT INTO bruidspaar (email,password) VALUES(@0, @1); ";
        db.Execute(InsertQuery, email, hash_pass);
        return "succes";

    }

    /// <summary>
    /// Login to blyat
    /// </summary>
    public static string Login(string email, string password)
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// Resets password and sends email with reset key
    /// </summary>
    public static string ResetPassword()
    {
        //we need a webserver for this.
        throw new NotImplementedException();
    }

    /// <summary>
    /// Check for sessions
    /// </summary>
    public static bool IsAuthenticated(int sessionId)
    {
        throw new NotImplementedException();
    }
}
