/**********************\
 *                    *
 *     PRE-DEPLOY     *
 *                    *
\**********************/

create trigger no_update_contract
	before update
	on contract for each row
		signal sqlstate '45000'
			set message_text = 'cannot update contract';

create trigger no_delete_contract
	before delete
	on contract for each row
		signal sqlstate '45000'
			set message_text = 'cannot delete contract';

create trigger no_update_terms
	before update
	on terms for each row
		signal sqlstate '45000'
			set message_text = 'cannot update terms';

create trigger no_delete_terms
	before delete
	on terms for each row
		signal sqlstate '45000'
			set message_text = 'cannot delete terms';

alter table terms
	add unique key UK_Terms (Contract_ID, Language);

insert into contract (beginDate, version)
	values (now(), '6.0.1.0');

select @contract_id := id
	from contract
	order by id desc
	limit 1;

set @PT = replace(replace('
{
	"Text": "O sistema Don\'t fly Money, abaixo constando como Sistema, e a pessoa que registrou-se no Sistema, abaixo constando como Pessoa, concordam com os termos descritos.",
	"Items": [
		{
			"Text": "O Sistema não possui quaisquer vínculos com instituições financeiras:",
			"Items": [
				{ "Text": "Dados apresentados no Sistema são inseridos apenas pela Pessoa;" },
				{ "Text": "Movimentações feitas em Sistema não alteram dados em contas bancárias ou quaisquer outros meios externos;" },
				{ "Text": "Movimentações em meios externos não afetam os dados do Sistema, sendo estes apenas alteráveis pela Pessoa via Sistema." }
			]
		},
		{ "Text": "Dados apresentados em Sistema e decisões tomadas com base nestes dados são de total responsabilidade da Pessoa;" },
		{
			"Text": "Os meios de comunicação do Sistema com a Pessoa são:",
			"Items": [
				{ "Text": "E-mail cadastrado pela Pessoa ao se registrar em Sistema, cujo contato é feito somente pelos endereços {{noreply_address}} - apenas para alertas automatizados - e {{support_address}} - para suporte;" },
				{ "Text": "Sistema web, registrado no domínio {{site_address}};" },
				{ "Text": "Aplicativo para dispositivos android {{android_address}}, instalação via Play Store." }
			]
		},
		{
			"Text": "O código fonte do Sistema - instruções para computador que regem o funcionamento do Sistema - está em um endereço web:",
			"Items": [
				{ "Text": "O endereço é público: {{github_address}};" },
				{ "Text": "O código fonte é ABERTO, podendo QUALQUER pessoa com acesso não-censurado a internet visualizá-lo e sugerir edições, usando uma conta do github." }
			]
		},
		{ "Text": "Cookies são usados para controlar seu acesso a informações e a segurança dos formulários deste site;" },
		{ "Text": "A Pessoa se compromete a manter o e-mail cadastrado atualizado;" },
		{ "Text": "Quaisquer atos que violem leis feitos por intermédio do Sistema são responsabilidade da Pessoa;" },
		{
			"Text": "Privacidade: dados inseridos no Sistema pela Pessoa são usados em:",
			"Items": [
				{
					"Text": "relatórios pessoais, que podem ser vistos apenas pela Pessoa, no Site e no Aplicativo de celular:",
					"Items": [
						{ "Text": "O acesso a esses relatórios é controlado por login e senha, que a Pessoa é responsável por manter em segredo." }
					]
				},
				{
					"Text": "para comunicação:",
					"Items": [
						{ "Text": "sobre segurança;" },
						{ "Text": "notificações autorizadas." }
					]
				},
				{ "Text": "pela força da lei." }
			]
		},
		{
			"Text": "Quando a Pessoa abandona o Sistema por 90 dias, seus dados podem ser removidos:",
			"Items": [
				{ "Text": "Para evitar isso, a Pessoa deve interagir com o Sistema (estando logada) pelo site e/ou pelo aplicativo;" },
				{ "Text": "Agendamentos sendo rodados não contam como interações válidas para esta regra;" },
				{ "Text": "Haverá dois avisos, um em torno de 30 dias e outro em torno de 60 dias após a última interação válida, enviados ao e-mail cadastrado no Sistema pela Pessoa;" },
				{ "Text": "Passados 90 dias sem interações válidas descritas acima, os dados da Pessoa podem ser removidos permanentemente do Sistema." }
			]
		},
		{
			"Text": "Quando os dados são removidos do Sistema, não podem ser automaticamente recuperados. Dado que talvez a Pessoa queira manter os dados para si, mesmo sem usar o Sistema, os dados serão exportados pelo Sistema para um arquivo antes da remoção:",
			"Items": [
				{ "Text": "a Pessoa pode requisitar o arquivo enviando um e-mail para {{support_address}}, usando o mesmo e-mail cadastrado no Sistema;" },
				{ "Text": "a Pessoa pode requisitar a exclusão permanente desse arquivo enviando um e-mail para {{support_address}}, usando o mesmo e-mail cadastrado no Sistema;" },
				{ "Text": "se a Pessoa requisitar o arquivo e/ou sua exclusão permanente, isso será feito assim que possível após ser vista a requisição;" },
				{ "Text": "a criação do arquivo não é uma garantia de sua existência para sempre: o arquivo pode ser apagado a qualquer momento após a remoção dos dados do Sistema, sem consulta prévia a Pessoa." },
			]
		},
		{
			"Text": "Estes termos podem ser alterados a qualquer momento:",
			"Items": [
				{ "Text": "Em caso de alteração, a Pessoa precisa aceitar os novos termos para continuar usando o Sistema;" },
				{ "Text": "Passados 90 dias, caso não aceite os novos termos, os dados da Pessoa podem ser removidos permanentemente do Sistema, sob as mesmas condições da falta de interações descrita acima nesse contrato." }
			]
		}
	]
}
', '\n', ''), '\t', '');

set @EN = replace(replace('
{
	"Text": "The Don\'t fly Money system, below referred as System, and the person who is registered at the System, below referred as Person, agree with the described terms.",
	"Items": [
		{
			"Text": "The System has no links with any financial institutions:",
			"Items": [
				{ "Text": "System data is only inserted by the Person;" },
				{ "Text": "Moves made at the System do not change bank account data or any other external resources;" },
				{ "Text": "Moves at external resources do not change System data, these being only editable by the Person at the System." }
			]
		},
		{ "Text": "System data and decisions made based on this data are sole responsibility of the Person;" },
		{
			"Text": "Communication vehicles from System to the Person are:",
			"Items": [
				{ "Text": "E-mail informed by the Person on the registration at the System, which is only contacted by the addresses {{noreply_address}} - just for automated alerts - and {{support_address}} - for support;" },
				{ "Text": "Web system, registered at site {{site_address}};" },
				{ "Text": "Android app {{android_address}}, installed by Play Store." }
			]
		},
		{
			"Text": "The System source code - instructions to computer which rule the System - is hosted at a web address:",
			"Items": [
				{ "Text": "The address is public: {{github_address}};" },
				{ "Text": "The source code is OPEN and ANY person using uncensored internet access can see it and suggest changes, using a github login." }
			]
		},
		{ "Text": "Cookies are used to control logon and security of forms of this site;" },
		{ "Text": "The Person agrees to keep the registered e-mail up to date;" },
		{ "Text": "Any law violations done through the System are sole responsibility of the Person;" },
		{
			"Text": "Privacy: data inputted into the System by the Person is used:",
			"Items": [
				{
					"Text": "at personal reports, that can be seen just by the Person, at the Site and the Mobile App:",
					"Items": [
						{ "Text": "the access to this reports is controlled by login and password, which the Person is sole responsible to keep secret." }
					]
				},
				{
					"Text": "to communicate:",
					"Items": [
						{ "Text": "about security;" },
						{ "Text": "authorized notifications." }
					]
				},
				{ "Text": "by the force of law." }
			]
		},
		{
			"Text": "When the Person abandons the System for 90 days, their data can be removed:",
			"Items": [
				{ "Text": "To avoid this, the Person should interact with the System (logged in) by website and/or android app;" },
				{ "Text": "Running schedules are not valid interactions for this rule;" },
				{ "Text": "There will be two warnings, one around 30 days and other around 60 days after last valid interaction, sent to registered e-mail at the System by the Person;" },
				{ "Text": "Passed 90 days without above described interactions the Person\'s data can be permanently removed from the System." }
			]
		},
		{
			"Text": "When the data is removed from the System, this cannot be automatically restored. Given maybe the Person wants to keep the data to itself even without using the System, the data will be exported by the System to a file before removal:",
			"Items": [
				{ "Text": "the Person can request the file sending e-mail to {{support_address}} using same e-mail registered at the System;" },
				{ "Text": "the Person can request the permanent deletion of the file sending e-mail to {{support_address}} using same e-mail registered at the System;" },
				{ "Text": "if the Person request the data file and/or its permanent deletion it will be made as soon possible after the request is seen;" },
				{ "Text": "the creation of the file is not a guarantee of its existence forever: the file can be deleted any time after System data removal, without prior check with the Person." },
			]
		},
		{
			"Text": "These terms can be changed any time in the future:",
			"Items": [
				{ "Text": "If the terms change, the Person need to accept the new terms to keep using the System;" },
				{ "Text": "Passed 90 days, if the new terms are not accepted, the Person\'s data can be permanently removed from the System, under the same conditions of lack of interaction describe before in this terms." }
			]
		}
	]
}
', '\n', ''), '\t', '');

insert into terms
		(Json, Language, Contract_ID)
	values
		(@PT, 'pt-BR', @contract_id),
		(@EN, 'en-US', @contract_id);

create table control (
	ID bigint auto_increment not null,
	Creation datetime not null,
	Active bit not null,
	IsAdm bit not null,
	IsRobot bit not null,
	WrongLogin int not null,
	RemovalWarningSent int not null,
	RobotCheck datetime not null,
	TempUser_ID int not null,
	LastAccess datetime null,
	ProcessingDeletion bit not null,
	primary key (ID)
);

alter table user
	modify column Email varchar(320) not null;

create table `purge` (
	ID bigint auto_increment,
	Email varchar(320) not null,
	When_ datetime not null,
	Why int not null,
	S3 varchar(500) null,
	Password varchar(60) not null,
	TFA varchar(32) null,
	primary key (ID)
);

create trigger no_update_purge
	before update
	on `purge` for each row
		signal sqlstate '45000'
			set message_text = 'cannot update purge';

create trigger no_delete_purge
	before delete
	on `purge` for each row
		signal sqlstate '45000'
			set message_text = 'cannot delete purge';

/**********************\
 *                    *
 *     POS-DEPLOY     *
 *                    *
\**********************/

insert into control (
	TempUser_ID, Creation, Active, IsAdm, WrongLogin,
	IsRobot, RemovalWarningSent, ProcessingDeletion, RobotCheck, LastAccess
)
	select ID, Creation, Active, IsAdm, WrongLogin,
			0, 0, 0, NOW(), NOW()
		from user;

alter table user
	add Control_ID bigint,
	add foreign key (Control_ID)
		references control (ID);

set sql_safe_updates = 0;
update user u
		inner join control c
			on u.ID = c.TempUser_ID
	set u.Control_ID = c.ID;
set sql_safe_updates = 1;

set foreign_key_checks = 0;
alter table user
	modify Control_ID bigint not null;
set foreign_key_checks = 1;

alter table user
	drop Creation,
	drop Active,
	drop IsAdm,
	drop WrongLogin;

alter table control
	drop TempUser_ID;

/**********************\
 *                    *
 *   EMERGENCY  FIX   *
 *                    *
\**********************/

alter table `purge`
	modify column Why smallint not null;

/* CANNOT USE varchar(320) */
alter table user
	add column Username varchar(64),
	add column Domain varchar(255);

set sql_safe_updates = 0;
update user
	set Username = SUBSTRING_INDEX(Email, '@', 1),
		Domain   = SUBSTRING_INDEX(Email, '@', -1);
set sql_safe_updates = 1;

alter table user
	modify column Username varchar(64) not null,
	modify column Domain varchar(255) not null,
	add unique UK_Email (Username, Domain),
	drop column Email;

alter table user
	add unique UK_Control (Control_ID);
