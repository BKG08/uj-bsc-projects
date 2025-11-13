----------------------Seller-----------------------
-- Seller profile linked 1-to-1 to STANDARDUSER (S_ID FK)
-- Contains biography and rating info
CREATE TABLE Seller (
    S_ID INT NOT NULL PRIMARY KEY,
    S_Bio VARCHAR(MAX) NULL,
    S_Campus VARCHAR(100) NULL,
	CONSTRAINT CheckSellerCampus CHECK (S_Campus IN ('DFC', 'APK', 'APB', 'SWC', 'JBS')),
	S_ProfilePic INT NULL,
    S_RatingAvg DECIMAL(3,2) NOT NULL DEFAULT 0.00,
    S_RatingCount INT NOT NULL DEFAULT 0,
    IsDeleted BIT NOT NULL DEFAULT 0,
    FOREIGN KEY (S_ProfilePic) REFERENCES UniImage(IMG_ID),
    FOREIGN KEY (S_ID) REFERENCES StandardUser(U_ID),
);