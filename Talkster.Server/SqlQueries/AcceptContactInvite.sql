UPDATE
	Contact
SET
	IsAccepted = 1
WHERE
	SourceAccountId = @SourceAccountId
	AND TargetAccountId = @TargetAccountId
