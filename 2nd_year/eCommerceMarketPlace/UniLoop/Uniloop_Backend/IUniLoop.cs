using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Uniloop_Backend
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IUniLoop" in both code and config file together.
    [ServiceContract]
    public interface IUniLoop
    {

        // Function for the log in 
        [OperationContract]
        int Login(string Email, string Password);

        // Function for the signup
        [OperationContract]
        Boolean Signup(string title, string fName, string lName, string uEmail, string phone, string password);

        // function to get a user from a id
        [OperationContract]
        StandardUser GetUser(int id);

        // function to add StandardUser to the Buyer table
        [OperationContract]
        bool MakeBuyer(int userId);

        // function to check if the user is buyer when they clicked a product to add to a cart
        [OperationContract]
        bool IsBuyer(int user);

        // function to set the seller profile
        [OperationContract]
        bool SetSeller(int userID, string bio, string campus);

        // function to verify/get the seller
        [OperationContract]
        int VerifySeller(int sellerID);

        // function to get the Sellers
        [OperationContract]
        List<Seller> GetSellers();

        // function to get the Seller
        [OperationContract]
        Seller GetSeller(int sellerID);

        // Function to update Seller
        [OperationContract]
        bool EditSeller(int userID, string bio, string campus, string image);

        // function to set the admin in the admin table
        [OperationContract]
        bool SetAdmin(int adminID, string role);

        // function to set the admin in the admin table
        [OperationContract]
        int VerifyAdmin(int adminID);

        // function to get the admin from the admin table
        [OperationContract]
        Admin GetAdmin(int adminID); // Does not work)

        // function to add the admin directly to the admin table
        [OperationContract]
        bool RegisterAdmin(int adderID, string title, string FName, string LName, string SEmail, string Phone, string Password, string role);

        // function to get product in Products table
        [OperationContract]
        Product GetProduct(int ID);

        // function to get all active products in Products table
        [OperationContract]
        List<Product> GetProducts();

        // function to set a products in Products table by seller
        [OperationContract]
        bool SetProduct(int sellerID, int categoryID, string name, string description, int quantity, string img, decimal Sellerprice, decimal adminprice);

        // function to edit the product
        [OperationContract]
        bool EditProduct(int productId, int sellerID, int categoryID, string name, string description, int quantity, decimal sellerPrice, decimal? adminPrice, string imagePath = null, string imageDescription = null);

        // function to delete the product
        [OperationContract]
        bool DeleteProduct(int productId, int sellerID);

        // function to add a category of products
        [OperationContract]
        bool AddFilterCategory(string name, int? pcategory);

        // function to add a create a cart
        [OperationContract]
        bool MakeBuyerCart(int userID);

        // Get the image path
        [OperationContract]
        UniImage GetImg(int ID);


        // Get list of all sellers Item
        [OperationContract]
        List<Product> MyProductListing(int SellerID);


        // get the products with a filtered category by name
        [OperationContract]
        List<Product> MyProductFilter3(int FilterID);

        // get the products with a filtered category by price
        [OperationContract]
        List<Product> MyProductFilter2(decimal price);

        // get the products with a filtered category by price and name
        [OperationContract]
        List<Product> MyProductFilter1(int FilterID, decimal price);

        // get the products with a simller name
        [OperationContract]
        List<Product> SearchItem(string Name);

        // get the filter category
        [OperationContract]
        Category GetFilter(int filterID);

        // get the filter category list
        [OperationContract]
        List<Category> GetFilters();

        // get the number of distinct products
        [OperationContract]
        List<string> ProductGetDistinctProductNames();

        // function to get buyer a cart
        [OperationContract]
        ShoppingCart GetBuyerCart(int userID);

        // function to adds products to a cart
        [OperationContract]
        bool AddToCart(int Product, int ShoppingCartID, int Quantity);

        // function to removes products from a cart
        [OperationContract]
        bool RemoveFromCart(int ShoppingCartID, int productId);

        // function to check if a cart exists
        [OperationContract]
        bool isCart(int ShoppingCartID);


        // function to updates product's quantity in a cart
        [OperationContract]
        bool UpdateQuantity(int userId, int productId, int newQuantity);

       // function to gets cart
       [OperationContract]
        decimal GetCartTotal(int userId);

        [OperationContract]
        bool AddWishlist(int buyerId);

        [OperationContract]
        bool AddProductToWishlist(int buyerId, int productId);

        [OperationContract]
        bool RemoveProductFromWishlist(int buyerId, int productId);

        [OperationContract]
        bool isProductInCart(int userID, int prodID);

        [OperationContract]
        int GetShoppingCartID(int BuyerID);

        [OperationContract]
        bool AddOrder(int BuyerID);

        [OperationContract]
        List<ShoppingCartProduct> getShoppingCart(int CartID);
        
        [OperationContract]
        List<WishlistItem> GetWishlistItems(int wishlistID);

        [OperationContract]
        int GetWishList(int buyerID);

        [OperationContract]
        bool UpdateUserDetails(int userId, string firstName, string lastName, string email, string phoneNumber);

        [OperationContract]
        bool UpdateSellerDetails(int userId, string Bio, int Img, string Name, string Lastname, string email, string phone);

        [OperationContract]
        bool AddInvoice(int buyerId, int orderId, decimal totalAmount, string status, int? paymentId = null);

        [OperationContract]
        bool AddOrder(int buyerID, int pickUpLocationID, decimal deliveryFee);

    }
}