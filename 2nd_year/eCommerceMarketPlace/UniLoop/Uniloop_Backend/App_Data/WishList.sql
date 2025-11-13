----------------------WISHLIST-----------------------
-- Wishlist linked to BUYER
CREATE TABLE Wishlist (
    W_ID INT IDENTITY(1,1) PRIMARY KEY,
    W_Buyer INT NOT NULL,
    W_DateCreated DATE NOT NULL,
    IsDeleted BIT NOT NULL DEFAULT 0,
    FOREIGN KEY (W_Buyer) REFERENCES BUYER(B_ID)
);

----------------------WISHLISTITEM-----------------------
-- Items in wishlists with dates
CREATE TABLE WishlistItem (
    WI_ID INT NOT NULL,
    WI_List INT NOT NULL,
    WI_DateAdded DATE NOT NULL,
    WI_DateRemoved DATE NULL,
    IsDeleted BIT NOT NULL DEFAULT 0,
    FOREIGN KEY (WI_List) REFERENCES Wishlist(W_ID),
    FOREIGN KEY (WI_ID) REFERENCES Product(P_ID),
    PRIMARY KEY (WI_ID, WI_List)
);