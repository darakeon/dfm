alter table year
	drop foreign key FK_Year_Account;

use dfm_test;

delimiter //

create procedure cleanup()
begin
	set sql_safe_updates = 0;

	delete from detail;
	delete from move;
	delete from schedule;
	delete from summary;
	delete from account;
	delete from category;

	delete from acceptance;
	delete from terms;
	delete from contract;
	delete from security;
	delete from ticket;
	delete from user;
	delete from config;

	set sql_safe_updates = 1;

	insert into contract
		(beginDate, version)
		values
		(now(), 'test');

	insert into terms
		(contract_ID, language, json)
		select id, 'en-US', '{ \"Text\": \"en-US\" }'
			from contract
		union all
		select id, 'pt-BR', '{ \"Text\": \"pt-BR\" }'
			from contract;
end//

delimiter ;
