CREATE TABLE Product (
    P_ID INT IDENTITY(1,1) PRIMARY KEY,
    P_SellerID INT NOT NULL,
    P_CategoryID INT NOT NULL,
    P_Name VARCHAR(100) NOT NULL,
    P_Description VARCHAR(500) NOT NULL,
    P_QuantityAvailable INT NOT NULL,
    P_BuyerSuggestedPrice DECIMAL(10,2) NOT NULL,
    P_AdminApprovedPrice DECIMAL(10,2) NULL,
    P_Image INT NOT NULL,
    P_Status VARCHAR(10) NOT NULL,
    CONSTRAINT CheckProductStatus CHECK (P_Status IN ('draft', 'pending', 'approved', 'rejected')),
    P_DateCreated DATE NOT NULL,
    P_DateUpdated DATE NULL,
    IsDeleted BIT NOT NULL DEFAULT 0,
    FOREIGN KEY (P_SellerID) REFERENCES SELLER(S_ID),
    FOREIGN KEY (P_Image) REFERENCES UniImage(IMG_ID),
    FOREIGN KEY (P_CategoryID) REFERENCES Category(C_ID)
);