DELETE FROM
	Contact
WHERE
	(SourceAccountId = @SourceAccountId AND TargetAccountId = @TargetAccountId)
	OR (SourceAccountId = @TargetAccountId AND TargetAccountId = @SourceAccountId)
