----------------------CATEGORY-----------------------
-- Product categories with optional parent category (hierarchical)
CREATE TABLE Category (
    C_ID INT IDENTITY(1,1) PRIMARY KEY,
    C_Name VARCHAR(100) NOT NULL,
    C_ParentCategoryID INT NULL,
    IsDeleted BIT NOT NULL DEFAULT 0,
    FOREIGN KEY (C_ParentCategoryID) REFERENCES Category(C_ID)
);