CREATE TABLE [dbo].[Order] (
    [OrderId] INT IDENTITY (1, 1) NOT NULL,
    [CustId]  INT NULL,
    [CarId]   INT NULL,
    PRIMARY KEY CLUSTERED ([OrderId] ASC),
	CONSTRAINT fk_orders_cust_id
FOREIGN KEY (CustId)
REFERENCES Customer (CustId)
ON DELETE CASCADE
ON UPDATE CASCADE,
CONSTRAINT fk_orders_inv_id
FOREIGN KEY (CarId)
REFERENCES Inventory (CarId)
ON DELETE CASCADE
ON UPDATE CASCADE
);

