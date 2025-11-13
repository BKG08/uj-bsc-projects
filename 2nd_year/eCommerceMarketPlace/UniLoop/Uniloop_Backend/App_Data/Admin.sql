----------------------Admin-----------------------
-- Admin accounts linked 1-to-1 with STANDARDUSER (A_ID FK)
-- Stores admin role and soft delete flag
CREATE TABLE Admin (
    A_ID INT NOT NULL PRIMARY KEY, 
    A_Role VARCHAR(100) NOT NULL,
    CONSTRAINT CheckAdminRole CHECK (A_Role IN (
        'Super Manager', 'Product Manager', 'Order Manager', 
        'Customer Support Manager', 'Financial Manager')),
    IsDeleted BIT NOT NULL DEFAULT 0,
    FOREIGN KEY (A_ID) REFERENCES StandardUser(U_ID)
);