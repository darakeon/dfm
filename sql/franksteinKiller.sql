use dfm;

delete from summary
    where ((nature = 0 and month_id is null)
        or (nature = 1 and year_id is null))
        and id > 0;

delete from detail
    where move_id in (
            select id
                from move
                where out_id is null
                    and in_id is null
        ) and id > 0;

delete from move
    where id not in (
            select move_id
                from detail
                where move_id is not null
        ) and id > 0;

delete from schedule
    where id not in (
            select schedule_id
                from move
                where schedule_id is not null
        ) and id > 0;


   

delete from summary
    where month_id not in (
        select out_id from move
            where out_id is not null
        union all
        select in_id from move
            where in_id is not null
    ) and month_id is not null
      and id > 0;

delete from month
    where id not in (
        select out_id from move
            where out_id is not null
        union all
        select in_id from move
            where in_id is not null
    ) and id > 0;
    
delete from summary
    where year_id not in (
        select year_id from month
    ) and year_id is not null
      and id > 0;

delete from year
    where id not in (
        select year_id from month
    ) and id > 0;