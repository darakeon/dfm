alter table move
	drop foreign key FK_Move_In,
	drop foreign key FK_Move_Out,
	change column In_ID In_ID_old bigint null,
	change column Out_ID Out_ID_old bigint null;

alter table move
	add column In_ID bigint null,
    add constraint FK_Move_In
		foreign key (In_ID)
        references account(ID),
	add column Out_ID bigint null,
    add constraint FK_Move_Out
		foreign key (Out_ID)
        references account(ID);

update move x
	inner join month m
		on x.in_id_old = m.id
	inner join year y
		on m.year_id = y.id
	set x.in_id = y.account_ID
    where x.id <> 0;

update move x
	inner join month m
		on x.out_id_old = m.id
	inner join year y
		on m.year_id = y.id
	set x.out_id = y.account_ID
    where x.id <> 0;

alter table move
	add column Day smallint not null,
	add column month smallint not null,
	add column year smallint not null,
	modify column Date datetime null;

update move
	set Day = day(Date),
		month = month(Date),
		year = year(Date)
	where id <> 0;

alter table summary
	drop foreign key FK_Summary_Month,
	drop foreign key FK_Summary_Year;

alter table summary
	add column Account_ID bigint null,
    add constraint FK_Summary_Account
		foreign key (Account_ID)
        references account(ID),
	add column Time int not null;

update summary s
	inner join month m
		on s.month_id = m.id
	inner join year y
		on m.year_id = y.id
	set s.account_id = y.account_ID,
		s.time = y.time * 100 + m.time
	where s.id <> 0;

update summary s
	inner join year y
		on s.year_id = y.id
	set s.account_id = y.account_ID,
		s.time = y.time
	where s.id <> 0;

alter table summary
	modify column Account_ID bigint not null;

alter table summary
	add constraint UK_Summary
		unique key (Account_ID, Nature, Time, Category_ID);

update contract
	set version = '3.0.0.0'
    where version = '003000000000';
    
update contract
	set version = '4.0.0.0'
    where version = '004000000000';

insert into contract
	(BeginDate, Version)
    values
    (now(), '4.1.3.6');

insert into terms
	(Language, Contract_ID, Json)
	select 'en-US', id, '{"Text": "The Don''t fly Money system, below referred as System, and the person who is registered at the system, below referred as User, agree with the described terms.","Items": [{"Text": "The System has no links with any financial institutions.","Items": [{ "Text": "System data is only inserted by the User;" },{ "Text": "Moves made at the System do not change bank account data or any other external resource;" },{ "Text": "Moves at external resource do not change System data, these being only editable by the User by the System;" }]},{ "Text": "System data and decisions made based on this data are sole responsibility of the User." },{"Text": "Communication vehicles from System to User are:","Items": [{ "Text": "E-mail informed by User on the registration at the System, which is only contacted by the addresses {{noreply_address}} - just for automatized alerts - and {{support_address}} - for support;" },{ "Text": "Web system, registered at site {{site_address}};" },{ "Text": "Android app {{android_address}}, installed by Play Store." }]},{"Text": "The System source code - instructions to computer which rule the System - is hosted at a web address.","Items": [{ "Text": "The address is public: {{github_address}}" },{ "Text": "The source code is OPEN and ANY person using uncensored internet access can see it and suggest changes, using a github login." }]},{ "Text": "Cookies are used to control logon and security of forms of this site" },{ "Text": "The User agrees to keep the registered e-mail updated." },{ "Text": "Any law violations done through the System are sole responsibility of the User." },{"Text": "These terms can be changed any time in the future.","Items": [{ "Text": "If the terms change, the user need to accept the new terms to keep using the system;" },{ "Text": "Passed 90 days, if the new terms are not accepted, User data can be permanently removed from the System." }]}]}'
		from contract
		where version = '4.1.3.6'
	union all
	select 'pt-BR', id, '{"Text": "O sistema Don''t fly Money, abaixo constando como Sistema, e a pessoa que registrou-se no sistema, abaixo constando como Usufrente, concordam com os termos descritos.","Items": [{"Text": "O Sistema não possui quaisquer vínculos com instituições financeiras.","Items": [{ "Text": "Dados apresentados no Sistema são inseridos apenas por Usufrente;" },{ "Text": "Movimentações feitas em Sistema não alteram dados em contas bancárias ou quaisquer outros meios externos;" },{ "Text": "Movimentações em meios externos não afetam os dados de Sistema, sendo estes apenas alteráveis por Usufrente via Sistema;" }]},{ "Text": "Dados apresentados em Sistema e decisões tomadas com base nestes dados são de total responsabilidade de Usufrente." },{"Text": "Os meios de comunicação de Sistema com Usufrente são:","Items": [{ "Text": "E-mail cadastrado por Usufrente ao se registrar em Sistema, cujo contato é feito somente pelos endereços {{noreply_address}} - apenas para alertas automatizados - e {{support_address}} - para suporte;" },{ "Text": "Sistema web, registrado no domínio {{site_address}};" },{ "Text": "Aplicativo para dispositivos android {{android_address}}, instalação via Play Store." }]},{"Text": "O código fonte de Sistema - instruções para computador que regem o funcionamento de Sistema - está em um endereço web.","Items": [{ "Text": "O endereço é público: {{github_address}}" },{ "Text": "O código fonte é ABERTO, podendo QUALQUER pessoa com acesso não-censurado a internet visualizá-lo e sugerir edições, usando uma conta do github." }]},{ "Text": "Cookies são usados para controlar seu acesso a informações e a segurança dos formulários deste site" },{ "Text": "Usufruente se compromete a manter o e-mail cadastrado atualizado." },{ "Text": "Quaisquer atos que violem leis feitos por intermédio de Sistema são responsabilidade de Usufrente." },{"Text": "Estes termos podem ser alterados a qualquer momento.","Items": [{ "Text": "Em caso de alteração, Usufrente precisa aceitar os novos termos para continuar usando o sistema;" },{ "Text": "Passados 90 dias, caso não aceite os novos termos, dados de Usufrente podem ser removidos permanentemente de Sistema." }]}]}'
		from contract
		where version = '4.1.3.6';

alter table schedule
	add column Day smallint not null,
	add column month smallint not null,
	add column year smallint not null,
	modify column Date datetime null;

update schedule
	set Day = day(Date),
		month = month(Date),
		year = year(Date)
	where id <> 0;
