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
        string hashPass = HashPassword(password);
        var db = Database.Open("test");

        //test for existing email
        string GetQuery = "SELECT email FROM users WHERE email=@0";
        if (db.QuerySingle(GetQuery, email) != null) return "Email already set";

        //create user
        string InsertQuery = "INSERT INTO users (email, password) VALUES(@0, @1); ";
        db.Execute(InsertQuery, email, hashPass);
        Security.Login(email, password);
        return "";
    }

    /// <summary>
    /// Login to blyat
    /// </summary>
    public static string Login(string email, string password)
    {
        //open database
        var db = Database.Open("test");

        //expect single result
        string GetQuery = "SELECT user_id, email, password FROM users WHERE email=@0";
        var result = db.QuerySingle(GetQuery, email);

        //check for existing email
        if (result == null) return "No account associated with this email";

        //check for correct password
        if (!BCrypt.Net.BCrypt.Verify(password, result["password"])) return "Password is incorrect";

        //Set sessions
        HttpContext.Current.Session["user_id"] = result["user_id"];
        HttpContext.Current.Session["email"] = result["email"];
        HttpContext.Current.Response.Redirect("Default");
        return "";
    }

    /// <summary>
    /// Logout from blyat
    /// </summary>
    public static void Logout()
    {
        HttpContext.Current.Session.Clear();
        HttpContext.Current.Session.Abandon();
        HttpContext.Current.Response.Redirect("Default");
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
    public static bool IsAuthenticated()
    {
        if (HttpContext.Current.Session["user_id"] == null) return false;
        return true;
    }

    public static string UniqueID()
    {
        return Guid.NewGuid().ToString();
    }
}
