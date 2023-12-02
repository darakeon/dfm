/**********************\
 *                    *
 *     PRE-DEPLOY     *
 *                    *
\**********************/

alter table control
	add MiscDna int null;

/**********************\
 *                    *
 *     POS-DEPLOY     *
 *                    *
\**********************/

set sql_safe_updates = 0;
update control
	set MiscDna = 1
		+ round(rand(), 0) * pow(2,0)
		+ round(rand(), 0) * pow(2,1)
		+ round(rand(), 0) * pow(2,2)
		+ round(rand(), 0) * pow(2,3)
		+ round(rand(), 0) * pow(2,4)
		+ round(rand(), 0) * pow(2,5)
		+ round(rand(), 0) * pow(2,6)
		+ round(rand(), 0) * pow(2,7)
		+ round(rand(), 0) * pow(2,8)
	where MiscDna is null;
set sql_safe_updates = 1;

alter table control
	modify MiscDna int not null;
