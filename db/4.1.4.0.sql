/* Non-destructive */

alter table move
	add CheckedIn bit not null default(0),
	add CheckedOut bit not null default(0);

set sql_safe_updates = 0;

update move
	set CheckedIn = Checked,
		CheckedOut = Checked;

set sql_safe_updates = 1;

/* Destructive */

alter table move
	drop Checked;
