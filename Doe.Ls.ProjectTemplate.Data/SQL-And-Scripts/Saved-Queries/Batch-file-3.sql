DECLARE	@return_value int

EXEC	@return_value = [dbo].[MovePositionNumberToTargetDocNumber]
		@sourcePositionNumber = N'193697',
		@targetDocNumber = N'DOC16/201912'

SELECT	'Return Value' = @return_value

GO
