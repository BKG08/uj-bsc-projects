----------------------SHOPPINGCART-----------------------
-- Shopping cart linked to BUYER
CREATE TABLE ShoppingCart (
    SC_ID INT IDENTITY(1,1) PRIMARY KEY,
    SC_Buyer INT NOT NULL,
    SC_DateCreated DATE NOT NULL,
    SC_DateUpdated DATE NULL,
    IsDeleted BIT NOT NULL DEFAULT 0,
    FOREIGN KEY (SC_Buyer) REFERENCES BUYER(B_ID)
);

----------------------SHOPPINGCARTPRODUCT-----------------------
-- Products in shopping carts, with quantities and dates
CREATE TABLE ShoppingCartProduct (
    SCP_CartID INT NOT NULL,
    SCP_ProductID INT NOT NULL,
    SCP_Quantity INT NOT NULL,
    SCP_DateAdded DATE NOT NULL,
    SCP_DateRemoved DATE NULL,
    IsDeleted BIT NOT NULL DEFAULT 0,
    PRIMARY KEY (SCP_CartID, SCP_ProductID),
    FOREIGN KEY (SCP_CartID) REFERENCES ShoppingCart(SC_ID),
    FOREIGN KEY (SCP_ProductID) REFERENCES Product(P_ID)
);