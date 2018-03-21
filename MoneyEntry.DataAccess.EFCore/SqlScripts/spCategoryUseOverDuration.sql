CREATE PROC spCategoryUseOverDuration 
( 
  @Start DATE  
, @End DATE  
, @TypeId INT = 2 
, @PersonId INT  
, @Minimum MONEY 
) 
AS  
BEGIN 
    SELECT 
      t.CategoryID 
    , t.TypeID 
    , ty.Description AS TypeDesc 
    , c.Description AS CategoryDesc 
    , SUM(t.Amount) AS Amount 
    FROM teTransaction t 
        INNER JOIN tdCategory c ON c.CategoryID = t.CategoryID 
            AND t.CreatedDt BETWEEN @Start AND @End 
            AND t.PersonID = @PersonId 
            AND t.TypeID = ISNULL(@TypeId, t.TypeID) 
        INNER JOIN tdType ty ON ty.TypeID = t.TypeID 
    GROUP BY t.TypeID, ty.Description, t.CategoryID, c.Description 
    HAVING SUM(t.Amount) > @Minimum 
    ORDER BY SUM(t.Amount) DESC 
END