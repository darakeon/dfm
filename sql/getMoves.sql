Select m.ID, m.Description, c.Name Category, ai.Name AccountIn, ao.Name AccountOut, m.Date, d.Description, d.Amount, d.Value
    from move m
        inner join detail d
            on m.id = d.move_id
        inner join category c
            on m.category_id = c.id            
        left join month mi
                on m.in_id = mi.id
            left join year yi
                on mi.year_id = yi.id
            left join account ai
                on yi.account_id = ai.id
        left join month mo
                on m.out_id = mo.id
            left join year yo
                on mo.year_id = yo.id
            left join account ao
                on yo.account_id = ao.id
    where ao.user_id = 18
        or ai.user_id = 18