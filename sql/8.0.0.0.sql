drop trigger no_update_contract;

update contract
	set version = '7.4.0.5'
	where version = '8.0.0.0';

create trigger no_update_contract
	before update
	on contract for each row
		signal sqlstate '45000'
			set message_text = 'cannot update contract';
