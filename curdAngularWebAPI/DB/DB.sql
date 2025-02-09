

CREATE PROC [dbo].[usp_ProductItem]
(
  @Mode           INT = 0,
  @ProductItemID  INT = 0,
  @ProductName    VARCHAR(50) = '',
  @Price          INT = 0,
  @Rating         INT = 0,
  @Description    VARCHAR(300) = '',
  @Status         BIT = 0
)
AS
BEGIN
   SET NOCOUNT ON;
   IF(@Mode=1)  --INSERT RECORD
   BEGIN
     INSERT INTO ProductItems(ProductName,Price,Rating,Description,Status,CreateDate)
	 VALUES(@ProductName, @Price, @Rating, @Description, @Status, GETDATE())

	 SELECT 1 as APIStatus ,  @ProductName + ' inserted successfully.' AS Message;
   END

   IF(@Mode=2) --GET LIST
   BEGIN
    SELECT * FROM ProductItems
   END

   IF(@Mode=3)
   BEGIN
    SELECT * FROM ProductItems WHERE ProductItemID = @ProductItemID
   END

   IF (@Mode = 4)
    BEGIN
        UPDATE ProductItems
        SET 
            ProductName = @ProductName,
            Price = @Price,
            Rating = @Rating,
            Description = @Description,
            Status = @Status
        WHERE ProductItemID = @ProductItemID;
       
        SELECT 'Product updated successfully.' AS Message, 1 AS APIStatus;
  END

  IF(@Mode=5)
  BEGIN
   DELETE FROM ProductItems WHERE ProductItemID = @ProductItemID;

    SELECT 'Record deleted successfully.' AS Message, 1 AS APIStatus;
  END
END