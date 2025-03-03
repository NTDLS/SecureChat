INSERT INTO Account
(
	Id,
	Username,
	DisplayName,
	PasswordHash,
	LastSeen,
	State,
	Status
)
SELECT
	@Id,
	@Username,
	@DisplayName,
	@PasswordHash,
	@LastSeen,
	'Offline',
	''

