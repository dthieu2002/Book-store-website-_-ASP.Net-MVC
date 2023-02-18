using BTL_ASPDotNet2022.Data;
using System.Text.Json.Serialization;

namespace BTL_ASPDotNet2022.Models
{
    public class Cart
    {
        //=== Properties
        public List<Cart_Item> Items { get; set; }

        //=== Method
        public Cart()
        {
            Items = new List<Cart_Item>();
        }
        public void AddItem(Cart_Item item)
        {
            foreach(Cart_Item tmp in this.Items)
            {
                if(tmp.Book.Id == item.Book.Id)
                {
                    tmp.Quantity += item.Quantity;
                    return;
                }
            }

            // If not exists , add item into cart
            this.Items.Add(item);
        }

        public void RemoveItem(int BookId)
        {
            foreach (Cart_Item tmp in this.Items)
            {
                if (tmp.Book.Id == BookId)
                {
                    this.Items.Remove(tmp);
                    return;
                }
            }

            throw new NullReferenceException();
        }


    }

    public class Cart_Item
    {
        //=== Properties
        public Book Book { get; set; }
        public int Quantity { get; set; }

        //=== Method
        public Cart_Item(Book Book, int Quantity)
        {
            this.Book = Book;
            this.Quantity = Quantity;
        }

    }
}
