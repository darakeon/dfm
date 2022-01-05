insert into contract (beginDate, version)
	values (now(), '8.0.0.0');

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
			"Text": "Quando os dados são removidos do Sistema, não podem ser automaticamente recuperados. Dado que talvez a Pessoa queira manter os dados para si, mesmo sem usar o Sistema, as movimentações serão exportadas pelo Sistema para um arquivo antes da remoção:",
			"Items": [
				{ "Text": "a Pessoa pode requisitar o arquivo enviando um e-mail para {{support_address}}, usando o mesmo e-mail cadastrado no Sistema;" },
				{ "Text": "a Pessoa pode requisitar a exclusão permanente desse arquivo enviando um e-mail para {{support_address}}, usando o mesmo e-mail cadastrado no Sistema;" },
				{ "Text": "se a Pessoa requisitar o arquivo e/ou sua exclusão permanente, isso será feito assim que possível após ser vista a requisição;" },
				{ "Text": "a criação do arquivo não é uma garantia de sua existência para sempre: o arquivo pode ser apagado a qualquer momento após a remoção dos dados do Sistema, sem consulta prévia a Pessoa." },
			]
		},
		{
			"Text": "Existe um menu dentro das configurações no site para a Pessoa poder excluir seus dados do Sistema:",
			"Items": [
				{ "Text": "Os dados não são excluídos instantaneamente;" },
				{ "Text": "Nesse caso - a exclusão ser uma requisição da Pessoa - as movimentações não são gravadas em um arquivo;" },
				{ "Text": "Após a remoção, não existe uma forma de a Pessoa recuperar os dados via sistema;" },
				{ "Text": "É pedida a senha da Pessoa para registrar o pedido de exclusão." }
			]
		},
		{
			"Text": "Quando os dados são removidos do Sistema, o endereço de e-mail registrado é guardado para caso seja necessário consultar motivo e/ou data da remoção dos dados;",
		},
		{
			"Text": "O Sistema, considerando Site e Aplicativo de celular, é mantido como aqui apresentado, e pode ficar indisponível. Não há garantias de o Sistema estar online, ou de permanecer online, não cabendo ônus a quem o mantém em caso de indisponibilidade, seja esta temporária ou permanente;",
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
			"Text": "When the data is removed from the System, this cannot be automatically restored. Given maybe the Person wants to keep the data to itself even without using the System, the moves will be exported by the System to a file before removal:",
			"Items": [
				{ "Text": "the Person can request the file sending e-mail to {{support_address}} using same e-mail registered at the System;" },
				{ "Text": "the Person can request the permanent deletion of the file sending e-mail to {{support_address}} using same e-mail registered at the System;" },
				{ "Text": "if the Person request the data file and/or its permanent deletion it will be made as soon possible after the request is seen;" },
				{ "Text": "the creation of the file is not a guarantee of its existence forever: the file can be deleted any time after System data removal, without prior check with the Person." },
			]
		},
		{
			"Text": "There is menu inside system settings at the website to the Person delete their data from the System:",
			"Items": [
				{ "Text": "The data is not removed instantly;" },
				{ "Text": "In this case - the Person asking the data deletion - the moves are not recorded in a file;" },
				{ "Text": "After the removal there is no way to the Person to recover the data through the System;" },
				{ "Text": "The Person\'s password is asked to register the data deletion request." }
			]
		},
		{
			"Text": "When the data is removed from the System, the registered e-mail address is kept in case the information of reason and/or date of data removal is needed;",
		},
		{
			"Text": "The System (considering website and mobile app) is kept as here presented and can be unavailable. There are no guarantees of the System being online or to stay online, and there will be no onus for the System maintainer in case of unavailability, this one being temporary or permanent;",
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

alter table terms
	modify column Json varchar(6000) not null;

insert into terms
		(Json, Language, Contract_ID)
	values
		(@PT, 'pt-BR', @contract_id),
        (@EN, 'en-US', @contract_id);

-- migrations

create table migrations (
	id int not null auto_increment primary key,
	name varchar(15) not null unique,
	executed datetime not null default(now())
);

insert into migrations (name, executed) values ('0.0.0.0', '2011-07-13 16:28:54');
insert into migrations (name, executed) values ('1.0.0.0', '2011-09-22 01:43:09');
insert into migrations (name, executed) values ('1.0.0.2', '2011-10-01 20:24:54');
insert into migrations (name, executed) values ('1.0.0.3', '2011-10-15 00:24:29');
insert into migrations (name, executed) values ('1.0.0.4', '2011-11-11 21:08:49');
insert into migrations (name, executed) values ('1.0.2.0', '2012-03-17 00:52:35');
insert into migrations (name, executed) values ('1.0.2.3', '2013-03-07 21:03:36');
insert into migrations (name, executed) values ('1.0.3.0', '2013-08-22 04:03:24');
insert into migrations (name, executed) values ('2.0.0.0', '2014-05-31 06:46:45');
insert into migrations (name, executed) values ('2.0.0.3', '2014-11-26 07:42:38');
insert into migrations (name, executed) values ('2.0.1.0', '2014-12-25 21:34:18');
insert into migrations (name, executed) values ('2.0.1.1', '2015-01-03 12:33:44');
insert into migrations (name, executed) values ('2.0.4.0', '2016-07-18 23:00:31');
insert into migrations (name, executed) values ('2.1.0.0', '2017-01-13 04:01:11');
insert into migrations (name, executed) values ('2.1.2.0', '2017-01-15 17:21:31');
insert into migrations (name, executed) values ('3.0.0.0', '2017-04-03 02:06:44');
insert into migrations (name, executed) values ('4.0.0.0', '2017-09-05 02:23:17');
insert into migrations (name, executed) values ('4.1.2.0', '2018-03-03 17:31:44');
insert into migrations (name, executed) values ('4.1.2.2', '2018-11-24 22:18:40');
insert into migrations (name, executed) values ('4.1.2.8', '2019-03-04 21:07:47');
insert into migrations (name, executed) values ('4.1.3.0', '2019-08-25 04:25:42');
insert into migrations (name, executed) values ('4.1.3.2', '2019-09-05 00:21:53');
insert into migrations (name, executed) values ('4.1.3.3', '2019-09-08 03:40:20');
insert into migrations (name, executed) values ('4.1.3.4', '2019-09-11 17:53:11');
insert into migrations (name, executed) values ('4.1.3.6', '2019-09-25 23:43:30');
insert into migrations (name, executed) values ('4.1.3.7', '2019-11-16 12:31:35');
insert into migrations (name, executed) values ('4.1.3.9', '2019-12-08 22:23:14');
insert into migrations (name, executed) values ('4.1.4.5', '2020-06-09 18:51:32');
insert into migrations (name, executed) values ('4.4.0.0', '2020-10-17 01:22:26');
insert into migrations (name, executed) values ('4.5.0.1', '2020-11-28 03:00:13');
insert into migrations (name, executed) values ('6.1.0.0', '2021-07-16 01:25:41');
insert into migrations (name, executed) values ('7.4.0.0', '2021-09-11 15:05:50');

insert into migrations (name) values ('7.4.0.5');
