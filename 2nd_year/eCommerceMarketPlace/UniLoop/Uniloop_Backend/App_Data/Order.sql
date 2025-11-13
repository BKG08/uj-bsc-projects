----------------------ORDER-----------------------
-- Customer orders linked to BUYER and PickUpLocation
CREATE TABLE [Order] (
    O_ID INT IDENTITY(1,1) PRIMARY KEY,
    O_Buyer INT NOT NULL,
    O_PickUpLocationID INT NOT NULL,
    O_DeliveryFee DECIMAL(10,2) NOT NULL,
    O_VAT DECIMAL(10,2) NOT NULL DEFAULT 0.00, -- not yet applicable to UniLoop as it is not yet a regsitered VAT vendor
    O_SubTotal DECIMAL(10,2) NOT NULL, -- amount before VAT and delivery fee
    O_TotalPrice DECIMAL(10,2) NOT NULL,-- amount to be paid by buyer (inclusive of everything)
    O_Status VARCHAR(10) NOT NULL,
    CONSTRAINT CheckOrderStatus CHECK (O_Status IN ('paid', 'shipped', 'delivered', 'cancelled', 'returned')),
    O_DatePlaced DATE NOT NULL,
    O_DatePaid DATE NULL,
    IsDeleted BIT NOT NULL DEFAULT 0,
    FOREIGN KEY (O_Buyer) REFERENCES BUYER(B_ID),
    FOREIGN KEY (O_PickUpLocationID) REFERENCES PickUpLocation(PL_ID)
);

----------------------ORDERITEM-----------------------
-- Items within orders linked to PRODUCT, SELLER and ORDER
CREATE TABLE OrderItem (
    OI_ID INT NOT NULL,      
    OI_Order INT NOT NULL,                                  
    OI_Seller INT NOT NULL,                     
    OI_Quantity INT NOT NULL,
    OI_UnitPrice DECIMAL(10,2) NOT NULL,
    IsDeleted BIT NOT NULL DEFAULT 0,
    FOREIGN KEY (OI_Order) REFERENCES [Order](O_ID),
    FOREIGN KEY (OI_ID) REFERENCES Product(P_ID),
    FOREIGN KEY (OI_Seller) REFERENCES SELLER(S_ID),
    PRIMARY KEY (OI_ID, OI_Order)
);