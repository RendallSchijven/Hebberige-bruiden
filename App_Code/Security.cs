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

        //generate UNIQUE string
        string publicLink = Guid.NewGuid().ToString();

        //test for existing email
        string GetQuery = "SELECT email FROM users WHERE email=@0";
        if (db.QuerySingle(GetQuery, email) != null) return "Email already set";

        string InsertQuery = "INSERT INTO users (email, password, public_link) VALUES(@0, @1, @2); ";
        db.Execute(InsertQuery, email, hashPass, publicLink);
        return "succes";
    }

    /// <summary>
    /// Login to blyat
    /// </summary>
    public static string Login(string email, string password)
    {
        var db = Database.Open("test");
        string GetQuery = "SELECT user_id, email, password, public_link FROM users WHERE email=@0";
        var result = db.QuerySingle(GetQuery, email);
        if (result == null) return "No account associated with this email";
        if (!BCrypt.Net.BCrypt.Verify(password, result["password"])) return "Password is incorrect";
        HttpContext.Current.Session["user_id"] = result["user_id"];
        HttpContext.Current.Session["email"] = result["email"];
        HttpContext.Current.Session["public_link"] = result["public_link"];
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
}
