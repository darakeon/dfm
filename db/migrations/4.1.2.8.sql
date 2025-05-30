CREATE UNIQUE INDEX UK_Month_Time_Year
	on month(time, year_id);

CREATE UNIQUE INDEX UK_Year_Time_Account
	on year(time, account_id);
