UPDATE
	Account
SET
	State = @State,
	Status = @Status,
	LastSeen = @LastSeen
WHERE
	Id = @AccountId
