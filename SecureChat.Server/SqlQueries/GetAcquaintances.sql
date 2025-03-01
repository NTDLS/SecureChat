SELECT
	A.Id,
	A.DisplayName,
	Ac.IsAccepted,
	A.LastSeen
FROM
	Acquaintance as Ac
INNER JOIN Account as A
	ON A.Id = Ac.SourceAccountId
WHERE
	Ac.TargetAccountId = @AccountId
	
UNION
	
SELECT
	A.Id,
	A.DisplayName,
	Ac.IsAccepted,
	A.LastSeen
FROM
	Acquaintance as Ac
INNER JOIN Account as A
	ON A.Id = Ac.TargetAccountId	
WHERE
	Ac.SourceAccountId = @AccountId
