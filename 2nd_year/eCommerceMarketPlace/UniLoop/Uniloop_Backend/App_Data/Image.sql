-- ---------------------- UniImage -----------------------
CREATE TABLE UniImage (
    IMG_ID INT IDENTITY(1,1) PRIMARY KEY,
    IMG_Description VARCHAR(100) NULL DEFAULT 'image of something',
    IMG_Path VARCHAR(MAX) NOT NULL,
    IMG_Size DECIMAL(10,2) NOT NULL CHECK (IMG_Size > 0),  -- size in KB, must be positive --
    IMG_TimeStamp DATETIME2 DEFAULT SYSDATETIME(),
    CONSTRAINT UQ_UniImage_Path UNIQUE (IMG_Path)          -- ensure unique file path --
);