INSERT INTO Contact
(
	SourceAccountId,
	TargetAccountId,
	ContactHash,
	IsAccepted
)
SELECT
	@SourceAccountId,
	@TargetAccountId,
	@ContactHash,
	0
