SELECT
	Id,
	DisplayName
FROM
	Account
WHERE
	Username = @Username
	AND PasswordHash = @PasswordHash
