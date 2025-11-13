CREATE TABLE [dbo].[Invoice] (
    [INV_ID]         INT           IDENTITY (1, 1) NOT NULL,
    [INV_OrderID]    INT           NOT NULL,
    [INV_PaymentID]  INT           NULL,
    [INV_BuyerID]    INT           NOT NULL,
    [INV_DateIssued] DATETIME2 (7) DEFAULT (sysdatetime()) NOT NULL,
    [IsDeleted]      BIT           DEFAULT ((0)) NOT NULL,
    PRIMARY KEY CLUSTERED ([INV_ID] ASC),
    FOREIGN KEY ([INV_OrderID]) REFERENCES [dbo].[Order] ([O_ID]),
    FOREIGN KEY ([INV_PaymentID]) REFERENCES [dbo].[Payment] ([PM_ID]),
    FOREIGN KEY ([INV_BuyerID]) REFERENCES [dbo].[Buyer] ([B_ID])
);