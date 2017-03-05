using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Linq;
using WebMatrix.Data;

/// <summary>
/// All methods for items
/// </summary>
public class Item
{
    public int item_id { get; set; }
    public int wishlist_id { get; set; }
    public string name { get; set; }
    public string description { get; set; }
    public string buy_url { get; set; }
    public int visible { get; set; }
    public int priority { get; set; }

    public Item(int wishlist_id, string name, string description, int item_id = 0, string buy_url = null, int visible = 1, int priority = 1, bool save = false)
    {
        this.item_id = item_id;
        this.wishlist_id = wishlist_id;
        this.name = name;
        this.description = description;
        this.buy_url = buy_url;
        this.visible = visible;
        this.priority = priority;
        if (save) SaveInDb();
    }

    private void SaveInDb()
    {
        Database db = Database.Open("test");
        string InstertQuery = "INSERT INTO items (wishlist_id, name, description, buy_url, visible, priority) VALUES (@0, @1, @2, @3, @4, @5)";
        db.Execute(InstertQuery, wishlist_id, name, description, buy_url, visible, priority);
        item_id = Convert.ToInt32(db.GetLastInsertId());
    }
}