INSERT INTO Account
(
	Id,
	Username,
	DisplayName,
	PasswordHash,
	LastSeen,
	State,
	ProfileJson
)
SELECT
	@Id,
	@Username,
	@DisplayName,
	@PasswordHash,
	@LastSeen,
	'Offline',
	null