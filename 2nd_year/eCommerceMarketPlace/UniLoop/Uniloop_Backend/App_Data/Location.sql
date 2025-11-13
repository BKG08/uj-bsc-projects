----------------------PICKUPLOCATION-----------------------
-- Predefined static list of campus pickup locations
-- No soft delete flag included, assumed static
CREATE TABLE PickUpLocation (
    PL_ID INT IDENTITY(1,1) PRIMARY KEY,
    PL_CampusName VARCHAR(100) NOT NULL,
    PL_CampusArea VARCHAR(100) NOT NULL,
    PL_CampusPickUpPoint VARCHAR(100) NULL,
    PL_Notes VARCHAR(100) NULL
);

----------------------FAVOURITEBUYERLOCATION-----------------------
-- Buyers' saved favourite pickup locations linked to BUYER and PickUpLocation
CREATE TABLE FavouriteBuyerLocation (
    FBL_ID INT IDENTITY(1,1) PRIMARY KEY,
    FBL_BuyerID INT NOT NULL,
    FBL_PickUpLocationID INT NOT NULL,
    IsDeleted BIT NOT NULL DEFAULT 0,
    FOREIGN KEY (FBL_BuyerID) REFERENCES BUYER(B_ID),
    FOREIGN KEY (FBL_PickUpLocationID) REFERENCES PickUpLocation(PL_ID)
);