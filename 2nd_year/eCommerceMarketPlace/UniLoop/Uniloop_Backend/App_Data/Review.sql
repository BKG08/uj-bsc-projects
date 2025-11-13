----------------------REVIEW-----------------------
-- Reviews from buyers about sellers for specific order items
CREATE TABLE Review (
    R_ID INT IDENTITY(1,1) PRIMARY KEY,
    R_Reviewer INT NOT NULL,
    R_Seller INT NOT NULL,
    R_OrderItem INT NOT NULL,
    R_Rating INT NULL,
    CONSTRAINT CheckRating CHECK (R_Rating >= 1 AND R_Rating <= 5),
    R_Comment VARCHAR(500) NULL,
    R_DateReviewed DATE NULL,
    IsDeleted BIT NOT NULL DEFAULT 0,
    FOREIGN KEY (R_Reviewer) REFERENCES BUYER(B_ID),
    FOREIGN KEY (R_Seller) REFERENCES SELLER(S_ID),
    FOREIGN KEY (R_OrderItem) REFERENCES OrderItem(OI_ID)
);