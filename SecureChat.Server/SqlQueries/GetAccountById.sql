SELECT
	Id,
	Username,
	DisplayName,
	LastSeen,
	State,
	ProfileJson
FROM
	Account
WHERE
	Id = @Id
