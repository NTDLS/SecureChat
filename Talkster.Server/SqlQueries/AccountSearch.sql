SELECT
    A.Id,
    A.DisplayName,
    A.State,
    A.LastSeen,
    CASE 
        WHEN Coalesce(CS.SourceAccountId, CT.TargetAccountId) IS NOT NULL THEN 1 
        ELSE 0 
    END AS IsExitingContact
FROM
    Account AS A
LEFT OUTER JOIN Contact AS CS
    ON CS.SourceAccountId = A.Id
    AND CS.TargetAccountId = @AccountId
LEFT OUTER JOIN Contact AS CT
    ON CT.TargetAccountId = A.Id
    AND CT.SourceAccountId = @AccountId
WHERE
	DisplayName LIKE '%' || @DisplayName || '%'

	