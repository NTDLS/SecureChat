SELECT
	A.Id,
	A.DisplayName,
	Ac.IsAccepted,
	A.LastSeen,
	A.State,
	A.ProfileJson,
	1 as RequestedByMe
FROM
	Contact as Ac
INNER JOIN Account as A
	ON A.Id = Ac.TargetAccountId	
WHERE
	Ac.SourceAccountId = @AccountId
	AND Ac.TargetAccountId = @QueryAccountId
	AND Ac.IsAccepted = 1

UNION

SELECT
	A.Id,
	A.DisplayName,
	Ac.IsAccepted,
	A.LastSeen,
	A.State,
	A.ProfileJson,
	0 as RequestedByMe
FROM
	Contact as Ac
INNER JOIN Account as A
	ON A.Id = Ac.SourceAccountId
WHERE
	Ac.TargetAccountId = @AccountId
	AND Ac.SourceAccountId = @QueryAccountId
	AND Ac.IsAccepted = 1
