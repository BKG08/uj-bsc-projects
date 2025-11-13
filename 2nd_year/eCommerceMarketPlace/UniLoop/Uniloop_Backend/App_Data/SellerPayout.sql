----------------------SELLERPAYOUT-----------------------
-- Seller payouts after orders, linked to OrderItem and SELLER
CREATE TABLE SellerPayout (
    SP_ID INT IDENTITY(1,1) PRIMARY KEY,
    SP_OrderItem INT NOT NULL,
    SP_Order INT NOT NULL,
    SP_Seller INT NOT NULL,
    SP_Amount DECIMAL(10,2) NOT NULL, -- amount before deductions
    SP_Commision DECIMAL(10,2) NOT NULL, -- 10% of the product price
    SP_NetAmount DECIMAL(10,2) NOT NULL, -- amount after deductions
    SP_Status VARCHAR(10) NOT NULL,
    CONSTRAINT CheckPayoutStatus CHECK (SP_Status IN ('pending', 'paid', 'refunded')), -- refunded in the case of product or delivery issues
    SP_DatePaid DATE NOT NULL,
    SP_DateRefunded DATE NULL,
    IsDeleted BIT NOT NULL DEFAULT 0,
    FOREIGN KEY (SP_OrderItem, SP_Order) REFERENCES OrderItem(OI_ID, OI_Order),
    FOREIGN KEY (SP_Seller) REFERENCES SELLER(S_ID)
);