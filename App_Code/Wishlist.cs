using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebMatrix.Data;

/// <summary>
/// All methods for wishlists
/// </summary>
public class Wishlist
{
    public int wishlist_id { get; private set; }
    public int user_id { get; private set; }
    public string name { get; private set; }
    public string description { get; private set; }
    public string wishlist_link { get; private set; }

    /// <summary>
    /// Constructor with saving to database
    /// </summary>
    public Wishlist(int user_id, string name, string description, string wishlist_link = null, int wishlist_id = 0, bool save = false)
    {
        this.wishlist_id = wishlist_id;
        this.user_id = user_id;
        this.name = name;
        this.description = description;
        this.wishlist_link = wishlist_link;
        if(save) SaveInDb();
    }

    /// <summary>
    /// Database insertion fo wishlist
    /// </summary>
    private void SaveInDb()
    {
        string userLink = HttpContext.Current.Session["user_link"].ToString();
        if (wishlist_link == null)
        {
            wishlist_link = Guid.NewGuid().ToString();
        }
        Database db = Database.Open("test");
        string InstertQuery = "INSERT INTO wishlists (user_id, name, description, wishlist_link) VALUES (@0, @1, @2, @3)";
        db.Execute(InstertQuery, user_id, name, description, wishlist_link);
        InstertQuery = "INSERT INTO has (user_link, wishlist_link) VALUES (@0, @1)";
        db.Execute(InstertQuery, userLink, wishlist_link);
        wishlist_id = Convert.ToInt32(db.GetLastInsertId());
    }
}