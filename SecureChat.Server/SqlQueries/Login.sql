SELECT
	Id
FROM
	Account
WHERE
	Username = @Username
	AND PasswordHash = @PasswordHash
