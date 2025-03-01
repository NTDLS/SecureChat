SELECT
	Id,
	Username,
	DisplayName
FROM
	Account
WHERE
	Username = @Username
	AND PasswordHash = @PasswordHash
