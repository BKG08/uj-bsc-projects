----------------------Buyer-----------------------
-- Buyer profile linked 1-to-1 to StandardUser (B_ID FK)
-- Stores optional favourite location
CREATE TABLE Buyer (
    B_ID INT NOT NULL PRIMARY KEY,
    B_FavLocation VARCHAR(100) NULL,
    IsDeleted BIT NOT NULL DEFAULT 0,
    FOREIGN KEY (B_ID) REFERENCES StandardUser(U_ID)
);
