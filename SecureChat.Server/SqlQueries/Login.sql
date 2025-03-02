SELECT
	Id,
	Username,
	DisplayName,
	Status
FROM
	Account
WHERE
	Username = @Username
	AND PasswordHash = @PasswordHash
