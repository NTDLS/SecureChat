SELECT
	Id,
	Username,
	DisplayName,
	ProfileJson
FROM
	Account
WHERE
	Username = @Username
	AND PasswordHash = @PasswordHash
