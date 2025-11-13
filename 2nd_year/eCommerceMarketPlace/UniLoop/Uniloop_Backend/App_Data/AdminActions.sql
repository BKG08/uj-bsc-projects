----------------------ADMINACTIONS-----------------------
-- Logs admin actions for audit, linked to Admin accounts
CREATE TABLE AdminActions (
    AA_ID INT IDENTITY(1,1) PRIMARY KEY,
    AA_Admin INT NOT NULL,
    AA_Target VARCHAR(10) NOT NULL,
    CONSTRAINT CheckTarget CHECK (AA_Target IN ('Seller', 'Buyer', 'Product', 'Review')),
    AA_ActionType VARCHAR(100) NOT NULL,
    CONSTRAINT CheckAction CHECK (AA_ActionType IN (
        'User Management', 
        'Product & Listing Control', 
        'Order & Transaction Oversight', 
        'Complaint & Dispute Handling',  
        'Platform Settings & Content', 
        'Analytics & Reports'
    )),
    AA_Notes VARCHAR(100) NULL,
    DatePerformed DATE NOT NULL,
    IsDeleted BIT NOT NULL DEFAULT 0,
    FOREIGN KEY (AA_Admin) REFERENCES Admin(A_ID)
);