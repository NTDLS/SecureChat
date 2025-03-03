UPDATE
	Account
SET
	State = @State,
	LastSeen = @LastSeen
WHERE
	Id = @AccountId
