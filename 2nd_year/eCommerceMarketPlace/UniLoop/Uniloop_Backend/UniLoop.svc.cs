using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Security.Cryptography;
using System.ServiceModel;
using System.Text;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Net.Mail;
using System.Threading.Tasks;

namespace Uniloop_Backend
{
    public class UniLoop : IUniLoop
    {

        // Connect the database
        private UniLoopDataContext db = new UniLoopDataContext();


        // this is the log in function
        public int Login(string Email, string Password)
        {
            Password = HashPassword(Password);
            var user = (from u in db.StandardUsers
                        where u.U_UniEmail == Email && u.U_Password == Password && u.IsDeleted == false
                        select u).FirstOrDefault();

            if (user != null)
            {
                return user.U_ID;
            }
            else
            {
                return 0;
            }
        }

        // this is the signup funtion that registers user to the signup page
        public Boolean Signup(string title, string fName, string lName, string uEmail, string phone, string password)
        {
            var user = (from u in db.StandardUsers
                        where u.U_UniEmail == uEmail && u.IsDeleted == false
                        select u).FirstOrDefault();

            if (user == null)
            {
                StandardUser newUser = new StandardUser
                {
                    U_Title = title,
                    U_FName = fName,
                    U_LName = lName,
                    U_UniEmail = uEmail,
                    U_Phone = phone,
                    U_Password = HashPassword(password),
                    U_DateCreated = DateTime.Now,
                    U_Status = "activated"
                };
                // add to the database
                 db.StandardUsers.InsertOnSubmit(newUser);
                //Save changes to DB
                db.SubmitChanges();

                bool isBuyer = MakeBuyer(newUser.U_ID);
                if (isBuyer)
                {
                    return true;
                }
                return true;
            }
            else
            {
                return false;
            }

        }

        // Function is the screcy function that is responsible to hash the password
        public static string HashPassword(string password)
        {
            SHA1 algorithm = SHA1.Create();
            byte[] byteArray = null;
            byteArray = algorithm.ComputeHash(Encoding.Default.GetBytes(password));
            string hashedPassword = "";
            for (int i = 0; i < byteArray.Length; i++)
            {
                hashedPassword += byteArray[i].ToString("x2");
            }
            return hashedPassword;
        }

        // function to get a user from a id
        public StandardUser GetUser(int id)
        {
            var user = db.StandardUsers.FirstOrDefault(u => u.U_ID == id && !u.IsDeleted);
            if (user == null) return null;

            return new StandardUser
            {
                U_ID = user.U_ID,
                U_Title = user.U_Title,
                U_FName = user.U_FName,
                U_LName = user.U_LName,
                U_UniEmail = user.U_UniEmail,
                U_Phone = user.U_Phone,
                U_Status = user.U_Status,
                U_DateCreated = user.U_DateCreated
            };
        }

        // function to add StandardUser to the Buyer table
        public bool MakeBuyer(int userID)
        {
            var buyer = (from b in db.Buyers
                          where b.B_ID == userID && b.IsDeleted == false
                          select b).FirstOrDefault();

            if (buyer == null)
            {
                Buyer newBuyer = new Buyer()
                {
                    B_ID = userID,
                    IsDeleted = false
                };

                db.Buyers.InsertOnSubmit(newBuyer);
                db.SubmitChanges();

                //allocate empty cart and wishlist
                bool cartAdded = MakeBuyerCart(buyer.B_ID);
                bool wishListAdded = AddWishlist(buyer.B_ID);
                if (cartAdded == true && wishListAdded == true)
                {
                    db.SubmitChanges();
                }
                
                return true;
            }
            else
            {
                return false;
            }


        }

        // function to check if the user is buyer when they clicked a product to add to a cart
        public bool IsBuyer(int user)
        {
            var buyer = (from b in db.Buyers
                         where b.B_ID == user && b.IsDeleted == false
                         select b).FirstOrDefault();

            if (buyer != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        //function for adding the images to the image table
        public bool AddImg(string name, string Description)
        {
            var exist = (from img in db.UniImages
                         where img.IMG_Path == name && img.IMG_Description == Description && img.IsDeleted == false
                         select img).FirstOrDefault();

            if (exist == null)
            {
                UniImage img = new UniImage()
                {
                    IMG_Description = Description,
                    IMG_Path = name,
                    IMG_TimeStamp = DateTime.Today
                };
                db.UniImages.InsertOnSubmit(img);
                db.SubmitChanges();

                return true;
            }
            else
            {
                return false;
            }

        }

        // Create a function that give the image integer 
        public int VerifyImg(string path)
        {
            var image = (from img in db.UniImages
                           where img.IMG_Path == path && img.IsDeleted == false
                           select img).FirstOrDefault();

            if (image.IMG_ID >= 1)
            {
                return image.IMG_ID;
            }
            else
            {
                return 0;
            }
        }

        // function to set the seller profile 
        public bool SetSeller(int userID, string bio, string campus)
        {
            var user = GetUser(userID);

            if (user != null)
            {
                // declare the seller variable
                Seller seller = new Seller()
                {
                    S_ID = user.U_ID,
                    S_Bio = bio,
                    S_Campus = campus,
                    IsDeleted = false
                };
                db.Sellers.InsertOnSubmit(seller);
                db.SubmitChanges();
                return true;
            }
            else
            {
                return false;

            }
        }

        // function to verify/get the seller
        public int VerifySeller(int sellerID)
        {
            var seller = (from s in db.Sellers
                          where s.S_ID == sellerID && s.IsDeleted == false
                          select s).FirstOrDefault();

            if (seller != null)
            {
                return seller.S_ID;
            }
            else
            {
                return 0;
            }
        }
       

        // function to set the admin in the admin table
        public bool SetAdmin(int adminID, string role)
        {
            var user = (from u in db.StandardUsers
                        where u.U_ID == adminID && u.IsDeleted == false
                        select u).FirstOrDefault();

            if (user != null)
            {
                // declare the admin variable
                Admin admin = new Admin()
                {
                    A_ID = adminID,
                    A_Role = role,
                    IsDeleted = false
                };
                db.Admins.InsertOnSubmit(admin);
                db.SubmitChanges();
                return true;
            }
            else
            {
                return false;
            }

        }

        // function to get the admin in the admin table
        public int VerifyAdmin(int adminID)
        {
            var admin = (from a in db.Admins
                         where a.A_ID == adminID && a.IsDeleted == false
                         select a).FirstOrDefault();

            // if the admin is not null return it
            if (admin != null)
            {
                return admin.A_ID;
            }
            else
            {
                return 0;
            }
        }

        // function to get the admin from the admin table
        public Admin GetAdmin(int adminID)
        {
            var admin = (from a in db.Admins
                         where a.A_ID == adminID && a.IsDeleted == false
                         select a).FirstOrDefault();

            if (admin != null)
            {
                return new Admin
                {
                    A_ID = admin.A_ID,
                    A_Role = admin.A_Role
                };

            }
            else
            {
                return null;
            }
        }

        // Function to register a new admin by an existing admin
        public bool RegisterAdmin(int adderID, string title, string FName, string LName, string SEmail, string Phone, string Password, string role)
        {
            var setterAdmin = GetAdmin(adderID);
            bool adminSet = false;

            if (setterAdmin != null)
            {
                bool signedUP = Signup(title, FName, LName, SEmail, Phone, Password);
                if (signedUP == true)
                {
                    var user = (from u in db.StandardUsers
                                where u.U_UniEmail == SEmail && u.IsDeleted == false
                                select u).FirstOrDefault();

                    if (user != null)
                    {
                        adminSet = SetAdmin(user.U_ID, role);
                    }
                }

            }
            return adminSet;
        }

        // Funtion to get the Seller details 
        public Seller GetSeller(int sellerID)
        {
            var seller = (from a in db.Sellers
                          where a.S_ID == sellerID && a.IsDeleted == false
                          select a).FirstOrDefault();

            if (seller != null)
            {
                return  new Seller
                    {
                        S_ID = seller.S_ID,
                        S_Bio = seller.S_Bio,
                        S_Campus = seller.S_Campus,
                        S_ProfilePic = seller.S_ProfilePic,
                        S_RatingAvg = seller.S_RatingAvg,
                        S_RatingCount = seller.S_RatingCount
                    };
            }
            else
            {
                return null;
            }
        }

        // function to get product in Products table
        public Product GetProduct(int ID)
        {
            var product = (from p in db.Products
                           where p.P_ID == ID && p.IsDeleted == false
                           select p).FirstOrDefault();

            if (product == null)
            {
                return null;
            }

            return new Product
            {
                P_ID = product.P_ID,
                P_SellerID = product.P_SellerID,
                P_CategoryID = product.P_CategoryID,
                P_Name = product.P_Name,
                P_Description = product.P_Description,
                P_QuantityAvailable = product.P_QuantityAvailable,
                P_BuyerSuggestedPrice = product.P_BuyerSuggestedPrice,
                P_AdminApprovedPrice = product.P_AdminApprovedPrice,
                P_Image = product.P_Image,
                P_Status = product.P_Status,
                P_DateCreated = product.P_DateCreated,
                P_DateUpdated = product.P_DateUpdated,
                IsDeleted = product.IsDeleted
            };
        }

        // function to get all active products in Products table
        public List<Product> GetProducts()
        {
            return db.Products.
                      Where(p => !p.IsDeleted && p.P_Status == "approved")
                      .Select(p => new Product
                      {
                          P_ID = p.P_ID,
                          P_SellerID = p.P_SellerID,
                          P_CategoryID = p.P_CategoryID,
                          P_Name = p.P_Name,
                          P_Description = p.P_Description,
                          P_QuantityAvailable = p.P_QuantityAvailable,
                          P_BuyerSuggestedPrice = p.P_BuyerSuggestedPrice,
                          P_AdminApprovedPrice = p.P_AdminApprovedPrice,
                          P_Image = p.P_Image,
                          P_Status = p.P_Status,
                          P_DateCreated = p.P_DateCreated,
                          P_DateUpdated = p.P_DateUpdated,
                          IsDeleted = p.IsDeleted
                      })
                      .ToList();
        }

        // Function to get all the products from the seller
        public List<Product> MyProductListing(int SellerID)
        {
            return db.Products
                      .Where(p => !p.IsDeleted && p.P_SellerID == SellerID)
                      .Select(p => new Product
                      {
                          P_ID = p.P_ID,
                          P_SellerID = p.P_SellerID,
                          P_CategoryID = p.P_CategoryID,
                          P_Name = p.P_Name,
                          P_Description = p.P_Description,
                          P_QuantityAvailable = p.P_QuantityAvailable,
                          P_BuyerSuggestedPrice = p.P_BuyerSuggestedPrice,
                          P_AdminApprovedPrice = p.P_AdminApprovedPrice,
                          P_Image = p.P_Image,
                          P_Status = p.P_Status,
                          P_DateCreated = p.P_DateCreated,
                          P_DateUpdated = p.P_DateUpdated,
                          IsDeleted = p.IsDeleted
                      })
                      .ToList();
        }

        // function to get product by name and price
        public List<Product> MyProductFilter1(int FilterID, decimal price)
        {
            return db.Products
                      .Where(p => !p.IsDeleted && p.P_CategoryID == FilterID && p.P_BuyerSuggestedPrice < price && p.P_Status == "approved")
                      .Select(p => new Product
                      {
                          P_ID = p.P_ID,
                          P_SellerID = p.P_SellerID,
                          P_CategoryID = p.P_CategoryID,
                          P_Name = p.P_Name,
                          P_Description = p.P_Description,
                          P_QuantityAvailable = p.P_QuantityAvailable,
                          P_BuyerSuggestedPrice = p.P_BuyerSuggestedPrice,
                          P_AdminApprovedPrice = p.P_AdminApprovedPrice,
                          P_Image = p.P_Image,
                          P_Status = p.P_Status,
                          P_DateCreated = p.P_DateCreated,
                          P_DateUpdated = p.P_DateUpdated,
                          IsDeleted = p.IsDeleted
                      })
                      .ToList();

        }
        // get the products with a filtered by price
        public List<Product> MyProductFilter2(decimal price)
        {
            return db.Products
                     .Where(p => !p.IsDeleted && p.P_BuyerSuggestedPrice < price && p.P_Status == "approved")
                     .OrderByDescending(p => p.P_BuyerSuggestedPrice)
                     .Select(p => new Product
                     {
                         P_ID = p.P_ID,
                         P_SellerID = p.P_SellerID,
                         P_CategoryID = p.P_CategoryID,
                         P_Name = p.P_Name,
                         P_Description = p.P_Description,
                         P_QuantityAvailable = p.P_QuantityAvailable,
                         P_BuyerSuggestedPrice = p.P_BuyerSuggestedPrice,
                         P_AdminApprovedPrice = p.P_AdminApprovedPrice,
                         P_Image = p.P_Image,
                         P_Status = p.P_Status,
                         P_DateCreated = p.P_DateCreated,
                         P_DateUpdated = p.P_DateUpdated,
                         IsDeleted = p.IsDeleted
                     })
                     .ToList();
        }

        // get the products with by name
        public List<Product> MyProductFilter3(int FilterID)
        {
            return db.Products
                      .Where(p => !p.IsDeleted && p.P_CategoryID == FilterID && p.P_Status == "approved")
                      .Select(p => new Product
                      {
                          P_ID = p.P_ID,
                          P_SellerID = p.P_SellerID,
                          P_CategoryID = p.P_CategoryID,
                          P_Name = p.P_Name,
                          P_Description = p.P_Description,
                          P_QuantityAvailable = p.P_QuantityAvailable,
                          P_BuyerSuggestedPrice = p.P_BuyerSuggestedPrice,
                          P_AdminApprovedPrice = p.P_AdminApprovedPrice,
                          P_Image = p.P_Image,
                          P_Status = p.P_Status,
                          P_DateCreated = p.P_DateCreated,
                          P_DateUpdated = p.P_DateUpdated,
                          IsDeleted = p.IsDeleted
                      })
                      .ToList();
        }

        // Function to add the products
        public bool SetProduct(int sellerID, int categoryID, string name, string description, int quantity, string img, decimal Sellerprice, decimal adminprice)
        {
            // check if such a seller exists
            var seller = (from s in db.Sellers
                          where s.IsDeleted == false && s.S_ID == sellerID
                          select s).FirstOrDefault();

            if (seller != null)
            {
                // add the image of the product
                AddImg(img, description);
                // get the image
                var image = (from i in db.UniImages
                              where i.IsDeleted == false && i.IMG_Path == img && i.IMG_Description == description
                                select i).FirstOrDefault();

                if (image != null)
                {
                    // add the product 
                    Product product = new Product
                    {
                        P_SellerID = sellerID,
                        P_CategoryID = categoryID,
                        P_Name = name,
                        P_Description = description,
                        P_QuantityAvailable = quantity,
                        P_Image = image.IMG_ID,
                        P_BuyerSuggestedPrice = Sellerprice,
                        P_AdminApprovedPrice = adminprice,
                        P_Status = "pending",
                        P_DateCreated = DateTime.Now,
                        P_DateUpdated = DateTime.Now
                    };
                    db.Products.InsertOnSubmit(product);
                    db.SubmitChanges();
           
                }
                return true;
            }
            else
            { 
                return false;
            }
        }

        //Function to edit the product

        public bool EditProduct(int productId, int sellerID, int categoryID, string name, string description, int quantity, decimal sellerPrice, decimal? adminPrice, string imagePath = null, string imageDescription = null)
        {
          

            var product = (from p in db.Products
                           where p.P_ID == productId && p.P_SellerID == sellerID 
                           select p).FirstOrDefault();

            if (product == null)
            {
                return false;
            }

            try
            {
                product.P_CategoryID = categoryID;
                product.P_Name = name;
                product.P_Description = description;

                product.P_QuantityAvailable = quantity;
                product.P_BuyerSuggestedPrice = sellerPrice;
                if (adminPrice != null)
                {
                    product.P_AdminApprovedPrice = adminPrice;
                }
                product.P_DateUpdated = DateTime.Now;

                // add image as new image
                if (imagePath != null && imageDescription != null)
                {
                    bool AddedImg = AddImg(imagePath, description);
                    if (!AddedImg)
                    {
                        product.P_Image = (from i in db.UniImages
                                           where i.IMG_Path == imagePath && i.IMG_Description == imageDescription
                                           select i.IMG_ID).FirstOrDefault(); ;
                    }
                }
                
                db.SubmitChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        // Function to delete a product
        public bool DeleteProduct(int productId, int sellerID)
        {
            var product = (from p in db.Products
                           where p.P_ID == productId && p.P_SellerID == sellerID && p.IsDeleted == false
                           select p).FirstOrDefault();

            // If no product is found, it can't be deleted.
            if (product == null)
            {
                return false;
            }

            try
            {
                product.IsDeleted = true;
                product.P_DateUpdated = DateTime.Now;
                db.SubmitChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        // Function to add a filter category
        public bool AddFilterCategory(string name, int? pcategory)  
        {
            // Check if a category with the same name already exists (not deleted)
            var existingCategory = db.Categories
                .FirstOrDefault(c => c.C_Name == name && c.IsDeleted == false);

            if (existingCategory != null)
            {
                return false; // category already exists
            }

            // Create new category
            Category newCategory = new Category()
            {
                C_Name = name,
                C_ParentCategoryID = pcategory, // can be null if no parent
                IsDeleted = false
            };

            db.Categories.InsertOnSubmit(newCategory);
            db.SubmitChanges();

            return true;
        }

        // Function for adding to the cart
        public bool MakeBuyerCart(int userID)
        {
            bool CanBuy = IsBuyer(userID);

            if (CanBuy)
            {
                ShoppingCart cart = new ShoppingCart
                {
                    SC_Buyer = userID,
                    SC_DateCreated = DateTime.Now,
                    SC_DateUpdated = DateTime.Now,
                    IsDeleted = false
                };
                db.ShoppingCarts.InsertOnSubmit(cart);
                db.SubmitChanges();
                return true;
            }
            else
            {
                return false;
            }
        }

        public List<Product> SearchItem(string Name)
        {
            return db.Products
                      .Where(p => !p.IsDeleted && (p.P_Name.Contains(Name) || p.P_Description.Contains(Name)) && p.P_Status == "Approved")
                      .Select(p => new Product
                      {
                          P_ID = p.P_ID,
                          P_SellerID = p.P_SellerID,
                          P_CategoryID = p.P_CategoryID,
                          P_Name = p.P_Name,
                          P_Description = p.P_Description,
                          P_QuantityAvailable = p.P_QuantityAvailable,
                          P_BuyerSuggestedPrice = p.P_BuyerSuggestedPrice,
                          P_AdminApprovedPrice = p.P_AdminApprovedPrice,
                          P_Image = p.P_Image,
                          P_Status = p.P_Status,
                          P_DateCreated = p.P_DateCreated,
                          P_DateUpdated = p.P_DateUpdated,
                          IsDeleted = p.IsDeleted
                      })
                      .ToList();

        }

        // Get all the the filter categories
        public List<Category> GetFilters()
        {
            return db.Categories
                .Where(C => !C.IsDeleted)
                .Select(C => new Category
                {
                    C_ID = C.C_ID,
                    C_Name = C.C_Name,
                    C_ParentCategoryID = C.C_ParentCategoryID,
                    IsDeleted = C.IsDeleted
                }).ToList();
        }

        // get the ShoppingCartProd
        public List<ShoppingCartProduct> getShoppingCart(int CartID)
        {
         
            return db.ShoppingCartProducts
                .Where(S => !S.IsDeleted && S.SCP_CartID == CartID)
                .Select(S => new ShoppingCartProduct
                {
                    SCP_CartID = S.SCP_CartID,
                    SCP_ProductID = S.SCP_ProductID,
                    SCP_DateAdded = S.SCP_DateAdded,
                    SCP_DateRemoved = S.SCP_DateRemoved,
                    SCP_Quantity = S.SCP_Quantity,
                    IsDeleted = S.IsDeleted
                }).ToList();

        }

        // get the filter category
        public Category GetFilter(int filterID)
        {
            var filter = (from F in db.Categories
                          where F.C_ID == filterID
                          select F).FirstOrDefault();

            return new Category
                {
                    C_ID = filter.C_ID,
                    C_Name = filter.C_Name,
                    C_ParentCategoryID = filter.C_ParentCategoryID,
                    IsDeleted = filter.IsDeleted
                };

        }

        // function to get the Sellers
        public List<Seller> GetSellers()
        {
            return db.Sellers
                .Where(C => !C.IsDeleted)
                .Select(C => new Seller
                {
                    S_ID = C.S_ID,
                    S_Bio= C.S_Bio,
                    S_Campus = C.S_Campus,
                    S_ProfilePic = C.S_ProfilePic,
                    S_RatingAvg = C.S_RatingAvg,
                    S_RatingCount = C.S_RatingCount
                }).ToList();

        }

        // Function for getting to the cart
        public ShoppingCart GetBuyerCart(int userID)
        {
            var cart = (from sc in db.ShoppingCarts
                        where sc.SC_Buyer == userID
                        select sc).FirstOrDefault();

            if (cart != null)
            {
                return new ShoppingCart
                {
                    SC_ID = cart.SC_ID,
                    SC_Buyer = cart.SC_Buyer,
                    SC_DateCreated = cart.SC_DateCreated,
                    SC_DateUpdated = cart.SC_DateUpdated
                };
            }
            else
            {
                return null;
            }

        }
        public List<string> ProductGetDistinctProductNames()
        {
            // Fetch unique product names from the Products table
            return db.Products
                    .Where(p=> p.P_Status.Contains("approved"))
                    .Select(p => p.P_Name)
                    .Distinct()
                    .ToList();  
        }

        // Function to add products to a product cart
        public bool AddToCart(int Product, int ShoppingCartID, int Quantity)
        {
            var cart = (from c in db.ShoppingCarts
                        where c.SC_ID == ShoppingCartID && c.IsDeleted == false
                        select c).FirstOrDefault();
            if (cart != null)
            {
                // check if a product already exits then add the quantity
                var cartitems = getShoppingCart(ShoppingCartID);
                foreach(ShoppingCartProduct p in cartitems)
                {
                    if (p.SCP_CartID == ShoppingCartID && p.SCP_ProductID == Product)
                    {
                        UpdateCartItemQuantity(cart.SC_Buyer,Product,p.SCP_Quantity + 1);
                        return true;
                    }
                }


                ShoppingCartProduct cartProduct = new ShoppingCartProduct()
                {
                    SCP_CartID = ShoppingCartID,
                    SCP_ProductID = Product,
                    SCP_Quantity = Quantity,
                    SCP_DateAdded = DateTime.Now,
                };
                db.ShoppingCartProducts.InsertOnSubmit(cartProduct);
                db.SubmitChanges();
                return true;
            }
            else
            {
                return false;
            }

        }

        // Function to get the integer of the shopping cart
        public int GetShoppingCartID(int BuyerID)
        {
            int CartId = (from C in db.ShoppingCarts
                          where C.SC_Buyer == BuyerID
                          select C.SC_ID).FirstOrDefault();

            return CartId;
        }

        // Function to remove a product in a cart
        public bool RemoveFromCart(int ShoppingCartID, int productId)
        {
            // Find the cart first
            var cart = (from c in db.ShoppingCarts
                        where c.SC_ID == ShoppingCartID && c.IsDeleted == false
                        select c).FirstOrDefault();

            if (cart != null)
            {
                // Find the product inside the cart
                var cartProduct = (from cp in db.ShoppingCartProducts
                                   where cp.SCP_CartID == ShoppingCartID
                                   && cp.SCP_ProductID == productId
                                   select cp).FirstOrDefault();

                if (cartProduct != null)
                {
                    cartProduct.IsDeleted = true;
                    db.SubmitChanges();
                    return true;
                }
                else
                {
                    // Product not found in the cart
                    return false;
                }
            }
            else
            {
                // Cart not found
                return false;
            }
        }

        // check if a cart exist
        public bool isCart(int ShoppingCartID)
        {
            var cart = (from C in db.ShoppingCarts
                        where C.SC_ID == ShoppingCartID
                        select C).FirstOrDefault();

            if(cart != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool UpdateQuantity(int userId, int productId, int newQuantity)
        {
            // Find the product in the cart
            var cartProduct = (from cp in db.ShoppingCartProducts
                               where cp.SCP_CartID == userId
                               // && cp.Product_ID == productId
                               select cp).FirstOrDefault();

            if (cartProduct != null)
            {
                cartProduct.SCP_Quantity = newQuantity;
                db.SubmitChanges();
                return true;
            }
            else
            {
                // Product not found in user's cart
                return false;
            }
        }

        public decimal GetCartTotal(int userId)
        {
            // Find the user's active cart
            var cart = (from c in db.ShoppingCarts
                        where c.SC_Buyer == userId && c.IsDeleted == false
                        select c).FirstOrDefault();

            if (cart != null)
            {
                // Sum up all product prices * quantities
                var total = (from cp in db.ShoppingCartProducts
                             join p in db.Products on cp.SCP_ProductID equals p.P_ID
                             where cp.SCP_CartID == cart.SC_ID
                             select cp.SCP_Quantity * p.P_BuyerSuggestedPrice).Sum();

                return total;
            }
            else
            {
                return 0; // no cart found
            }
        }

        public bool AddWishlist(int buyerId)
        {
            // Check if wishlist exists for this buyer
            var exists = (from w in db.Wishlists
                          where w.W_Buyer == buyerId && w.IsDeleted == false
                          select w).FirstOrDefault();

            if (exists == null)
            {
                // Create new wishlist
                var wishlist = new Wishlist
                {
                    W_Buyer = buyerId,
                    W_DateCreated = DateTime.Now,
                    IsDeleted = false
                };

                db.Wishlists.InsertOnSubmit(wishlist);
                db.SubmitChanges();
                return true;
            }
            else
            {
                return false; // already has one
            }
        }
        // get the wishlist ID
        public int GetWishList(int buyerID)
        {
            return  (from W in db.Wishlists
                    where W.W_Buyer == buyerID
                    select W.W_ID).FirstOrDefault();
            
        }
        public List<WishlistItem> GetWishlistItems(int wishlistID)
        {
            return db.WishlistItems
                .Where(W => W.WI_ID == wishlistID)
                .Select(W => new WishlistItem
                {
                    WI_ID = W.WI_ID,
                    WI_List = W.WI_List,
                    WI_DateAdded =W.WI_DateAdded,
                    WI_DateRemoved = W.WI_DateRemoved,
                    IsDeleted = W.IsDeleted
                }).ToList();
            
        }

        public bool AddProductToWishlist(int buyerId, int productId)
        {
            // Find the buyer's wishlist
            var wishlist = (from w in db.Wishlists
                            where w.W_Buyer == buyerId && w.IsDeleted == false
                            select w).FirstOrDefault();

            if (wishlist != null)
            {
                // Check if this product is already in the wishlist
                var existingItem = (from wi in db.WishlistItems
                                    where wi.WI_ID == wishlist.W_ID
                                    && wi.IsDeleted == false
                                    select wi).FirstOrDefault();

                if (existingItem == null)
                {
                    // Add new item to wishlist
                    var wishlistItem = new WishlistItem
                    {
                        WI_ID = productId,
                        WI_List = wishlist.W_ID,
                        WI_DateAdded = DateTime.Now,
                        IsDeleted = false
                    };

                    db.WishlistItems.InsertOnSubmit(wishlistItem);
                    db.SubmitChanges();
                    return true;
                }
                else
                {
                    return false; // product already exists in wishlist
                }
            }
            else
            {
                return false; // buyer does not have a wishlist
            }
        }

        public bool RemoveProductFromWishlist(int buyerId, int productId)
        {
            // Step 1: Find the buyer's wishlist
            var wishlist = (from w in db.Wishlists
                            where w.W_Buyer == buyerId && w.IsDeleted == false
                            select w).FirstOrDefault();

            if (wishlist != null)
            {
                // Step 2: Find the product in that wishlist
                var item = (from wi in db.WishlistItems
                            where wi.WI_ID == wishlist.W_ID
                            && wi.IsDeleted == false
                            select wi).FirstOrDefault();

                if (item != null)
                {
                    // Step 3: Soft delete 
                    item.IsDeleted = true;
                    item.WI_DateRemoved = DateTime.Now;
                    db.WishlistItems.InsertOnSubmit(item);
                    db.SubmitChanges();
                    return true;
                }
                else
                {
                    return false; // product not found in wishlist
                }
            }
            else
            {
                return false; // buyer has no wishlist
            }
        }

        //public static async Task<bool> SendUserMessageAsync(string userName, string userEmail, string userMessage)
        //{
        //    try
        //    {
        //        using (var client = new SmtpClient("smtp.office365.com", 587))
        //        {
        //            client.EnableSsl = true;

        //            // Your business email credentials
        //            client.Credentials = new NetworkCredential(
        //                "uni-loop@outlook.com", // your business email
        //                "SparkNodes"            // app password
        //            );

        //            var mail = new MailMessage
        //            {
        //                From = new MailAddress("uni-loop@outlook.com", "UniLoop Contact Form"),
        //                Subject = "New Message from Website Contact Form",
        //                Body = $"Name: {userName}\nEmail: {userEmail}\n\nMessage:\n{userMessage}",
        //                IsBodyHtml = false
        //            };

        //            mail.To.Add("uni-loop@outlook.com"); // send to your business inbox

        //            await client.SendMailAsync(mail); // asynchronous send
        //        }

        //        return true;
        //    }
        //    catch (Exception ex)
        //    {
        //        // Optional: log error to file or database
        //        Console.WriteLine("Error sending email: " + ex.Message);
        //        return false;
        //    }
        //}

        //protected async void btnSubmit_Click(object sender, EventArgs e)
        //{
        //    string name = txtName.Text;
        //    string email = txtEmail.Text;
        //    string message = txtMessage.Text;

        //    bool success = await EmailHelper.SendUserMessageAsync(name, email, message);

        //    if (success)
        //        lblStatus.Text = "Your message has been sent. Thank you!";
        //    else
        //        lblStatus.Text = "Failed to send your message. Please try again later.";
        //}

        public bool isProductInCart(int userID, int prodID)
        {

            // find buyer's cart
            var cart = (from c in db.ShoppingCarts
                        where c.SC_Buyer == userID && c.IsDeleted == false
                        select c).FirstOrDefault();

            if (cart == null)
                return false;

            // check if product is inside
            var record = (from s in db.ShoppingCartProducts
                          where s.SCP_CartID == cart.SC_ID
                             && s.SCP_ProductID == prodID
                             && s.IsDeleted == false
                          select s).FirstOrDefault();

            return record != null;
        }

        public bool UpdateCartItemQuantity(int BuyerID, int prodID, int quantity)
        {

            var cart = (from c in db.ShoppingCarts
                        where c.SC_Buyer == BuyerID && c.IsDeleted == false
                        select c).FirstOrDefault();

            if (cart == null)
                return false;

            var record = (from s in db.ShoppingCartProducts
                          where s.SCP_CartID == cart.SC_ID
                             && s.SCP_ProductID == prodID
                             && s.IsDeleted == false
                          select s).FirstOrDefault();

            if (record != null)
            {
                record.SCP_Quantity = quantity;
                record.SCP_DateAdded = DateTime.Now;
                db.SubmitChanges();
                return true;
            }

            return false; // product not found
        }
        // function to edit a user
        public bool UpdateUserDetails(int userId, string firstName, string lastName, string email, string phoneNumber)
        {
            try
            {

                var user = (from u in db.StandardUsers
                            where u.U_ID == userId
                            select u).FirstOrDefault();

                if (user == null)
                {
                    return false;
                }

                user.U_FName = firstName;
                user.U_LName = lastName;
                user.U_UniEmail = email;
                user.U_Phone = phoneNumber;

                db.SubmitChanges();

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        // update seller information
        public bool UpdateSellerDetails(int userId, string Bio, int Img, string Name, string Lastname, string email, string phone)
        {
            var seller = (from s in db.Sellers
                        where s.S_ID == userId
                        select s).FirstOrDefault();

            if (seller == null)
            {
                return false;
            }

            int imageToUse;
            if (db.UniImages.Any(u => u.IMG_ID == Img))
            {
                imageToUse = Img;
            }
            else
            {
                AddImg(Name,Bio);
                var image = (from img in db.UniImages
                             where img.IMG_Path == Name && img.IMG_Description == Bio && img.IsDeleted == false
                             select img).FirstOrDefault();

                if (image == null)
                {
                    return false;
                }

                imageToUse = image.IMG_ID;

            }

            try
            {
                seller.S_Bio = Bio;
                seller.S_ProfilePic = imageToUse;
                StandardUser user = GetUser(seller.S_ID);
                if (UpdateUserDetails(user.U_ID, user.U_FName, user.U_FName, user.U_UniEmail, user.U_Phone))
                {
                    db.SubmitChanges();
                } 
                else
                {
                    return false;
                }
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while updating seller details: {ex.Message}");
                return false;
            }
        }


        public bool EditSeller(int userID, string bio, string campus, string image)
        {
            try
            {
                var seller = (from s in db.Sellers
                              where s.S_ID == userID
                              select s).FirstOrDefault();

                if (seller == null)
                {
                    return false; // Seller not found
                }

                if (AddImg(image, bio))
                {
                    UniImage img = GetImg(seller.S_ID);
                    seller.S_ProfilePic = img.IMG_ID;
                }
                else
                {
                    return false;
                }

                // Update seller details
                seller.S_Bio = bio;
                seller.S_Campus = campus;

                db.SubmitChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }


        // a function to change the password
        public bool ChangePassword(string email, string newPassword)
        {
            try
            {
                string hashedPassword = HashPassword(newPassword);
                var user = (from S in db.StandardUsers
                            where S.U_UniEmail == email && S.IsDeleted == false
                            select S).FirstOrDefault();

                if (user != null)
                {
                    // Update the password property of the user object.
                    user.U_Password = hashedPassword;
                    db.SubmitChanges();

                    return true;
                }
                else
                {
                    return false;
                }

            }
            catch (Exception)
            {
                return false;
            }
        }

        // a function to add an order
        public bool AddOrder(int buyerID, int pickUpLocationID, decimal deliveryFee)
        {
            try
            {
                // Get the buyer's shopping cart
                var cart = (from c in db.ShoppingCarts
                            where c.SC_Buyer == buyerID && c.IsDeleted == false
                            select c).FirstOrDefault();

                if (cart == null)
                    return false; // no cart found

                // Get all active products in the cart
                var cartItems = (from scp in db.ShoppingCartProducts
                                 join p in db.Products on scp.SCP_ProductID equals p.P_ID
                                 where scp.SCP_CartID == cart.SC_ID && scp.IsDeleted == false && p.IsDeleted == false
                                 select new
                                 {
                                    Product = p,
                                    Quantity = scp.SCP_Quantity
                                 }).ToList();

                if (cartItems.Count == 0)
                    return false; // cart is empty

                // Calculate subtotal
                decimal subTotal = 0;
                foreach (var item in cartItems)
                {
                    if (item.Product.P_QuantityAvailable < item.Quantity)
                        return false; // insufficient stock

                    decimal price = item.Product.P_AdminApprovedPrice ?? item.Product.P_BuyerSuggestedPrice;
                    subTotal += price * item.Quantity;
                }

                // VAT and total
                decimal vat = 0; // not yet applicable
                decimal totalPrice = subTotal + deliveryFee + vat;

                // Create order
                var order = new Order
                {
                    O_Buyer = buyerID,
                    O_PickUpLocationID = pickUpLocationID,
                    O_SubTotal = subTotal,
                    O_DeliveryFee = deliveryFee,
                    O_VAT = vat,
                    O_TotalPrice = totalPrice,
                    O_Status = "paid", // or pending depending on workflow
                    O_DatePlaced = DateTime.Now,
                    IsDeleted = false
                };

                db.Orders.InsertOnSubmit(order);
                db.SubmitChanges(); // generates O_ID

                // Create order items and update stock
                foreach (var item in cartItems)
                {
                    decimal unitPrice = item.Product.P_AdminApprovedPrice ?? item.Product.P_BuyerSuggestedPrice;

                    var orderItem = new OrderItem
                    {
                        OI_ID = item.Product.P_ID,
                        OI_Order = order.O_ID,
                        OI_Seller = item.Product.P_SellerID,
                        OI_Quantity = item.Quantity,
                        OI_UnitPrice = unitPrice,
                        IsDeleted = false
                    };
                    db.OrderItems.InsertOnSubmit(orderItem);

                    // Decrease stock
                    item.Product.P_QuantityAvailable -= item.Quantity;
                }

                db.SubmitChanges();

                // Optionally, mark cart items as deleted
                foreach (var item in cartItems)
                {
                    var cartProduct = db.ShoppingCartProducts.First(scp => scp.SCP_CartID == cart.SC_ID && scp.SCP_ProductID == item.Product.P_ID);
                    cartProduct.IsDeleted = true;
                    cartProduct.SCP_DateRemoved = DateTime.Now;
                }
                db.SubmitChanges();

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }


        public bool AddInvoice(int buyerId, int orderId, decimal totalAmount, string status, int? paymentId = null)
        {
            try
            {
                // validate the order and buyer exist
                var order = (from o in db.Orders
                             where o.O_ID == orderId && o.IsDeleted == false
                             select o).FirstOrDefault();
                var buyer = (from b in db.Buyers 
                             where b.B_ID == buyerId && b.IsDeleted == false 
                             select b).FirstOrDefault( );

                if (order == null || buyer == null)
                    return false;

                // if paymentId is provided, validate it
                if (paymentId != null)
                {
                    var payment = (from p in db.Payments
                                   where p.PM_ID == paymentId && p.IsDeleted == false
                                   select p).FirstOrDefault();
                    if (payment == null)
                        return false;
                }

                // Create Invoice
                var invoice = new Invoice
                {
                    INV_OrderID = orderId,
                    INV_BuyerID = buyerId,
                    INV_PaymentID = paymentId,
                    INV_DateIssued = DateTime.Now,
                    IsDeleted = false
                };
                db.Invoices.InsertOnSubmit(invoice);
                db.SubmitChanges();

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        UniImage GetImg(int ID)
        {
            var image = (from img in db.UniImages
                         where img.IMG_ID ==  ID && img.IsDeleted == false
                         select img).FirstOrDefault();

            return image;
            
        }
      
    }

}