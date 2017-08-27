select Time AS 'Time',
    sum(
        (
            if(isnull(AccountIn),0,1)
            + if(isnull(AccountOut),0,-1)
        ) * Amount * Value
    ) AS 'In and Out',
    
    sum(
        (
            if(isnull(AccountIn),0,1) 
            * if(isnull(AccountOut),0,1)
        ) * Amount * Value
    ) AS 'Transfer'
    from Detailed
    group by Time
union all
select 'Total' AS 'Time',
    sum(
        (
            if(isnull(AccountIn),0,1)
            + if(isnull(AccountOut),0,-1)
        ) * Amount * Value
    ) AS 'In and Out',
    sum(
        (
            if(isnull(AccountIn),0,1) 
            * if(isnull(AccountOut),0,1)
        ) * Amount * Value
    ) AS 'Transfer'
    from Detailed