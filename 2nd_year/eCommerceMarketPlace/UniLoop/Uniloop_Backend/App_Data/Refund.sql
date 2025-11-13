----------------------REFUND-----------------------
-- Refunds linked to payments and orders
CREATE TABLE Refund (
    R_ID INT IDENTITY(1,1) PRIMARY KEY,
    R_PaymentID INT NOT NULL,                  -- which payment is being refunded
    R_OrderID INT NOT NULL,                    -- which order the refund relates to
    R_Amount DECIMAL(10,2) NOT NULL,           -- how much was refunded
    R_Reason VARCHAR(255) NULL,                -- optional reason for refund
    R_Method VARCHAR(7) NOT NULL,             -- refund method
    CONSTRAINT CheckRefundMethod CHECK (R_Method IN ('bank', 'credit')), 
    R_Status VARCHAR(10) NOT NULL,             -- refund state
    CONSTRAINT CheckRefundStatus CHECK (R_Status IN ('requested', 'approved', 'processed', 'declined')),
    R_DateRequested DATE NOT NULL,             -- when refund was initiated
    R_DateProcessed DATE NULL,                 -- when refund was completed
    IsDeleted BIT NOT NULL DEFAULT 0,         
    FOREIGN KEY (R_PaymentID) REFERENCES Payment(PM_ID),
    FOREIGN KEY (R_OrderID) REFERENCES [Order](O_ID)
);
