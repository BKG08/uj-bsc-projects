----------------------PAYMENT-----------------------
-- Payments linked to orders
CREATE TABLE Payment (
    PM_ID INT IDENTITY(1,1) PRIMARY KEY,
    PM_Order INT NOT NULL,
    PM_Amount DECIMAL(10,2) NOT NULL,
    PM_Provider VARCHAR(100) NOT NULL,
    PM_Status VARCHAR(10) NOT NULL,
    CONSTRAINT CheckPaymentStatus CHECK (PM_Status IN ('Pending', 'Completed', 'Failed', 'Refunded')),
    PM_DatePaid DATE NULL,
    PM_DateRefunded DATE NULL,
    IsDeleted BIT NOT NULL DEFAULT 0,
    FOREIGN KEY (PM_Order) REFERENCES [Order](O_ID)
);