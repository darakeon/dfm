insert into contract
	(BeginDate, Version)
	values
	(now(), '4.5.0.1');

insert into terms
	(Language, Contract_ID, Json)
	select 'en-US', id, '
{
	"Text": "The Don''t fly Money system, below referred as System, and the person who is registered at the system, below referred as User, agree with the described terms.",
	"Items": [
		{
			"Text": "The System has no links with any financial institutions.",
			"Items": [
				{ "Text": "System data is only inserted by the User;" },
				{ "Text": "Moves made at the System do not change bank account data or any other external resource;" },
				{ "Text": "Moves at external resource do not change System data, these being only editable by the User by the System;" }
			]
		},
		{ "Text": "System data and decisions made based on this data are sole responsibility of the User." },
		{
			"Text": "Communication vehicles from System to User are:",
			"Items": [
				{ "Text": "E-mail informed by User on the registration at the System, which is only contacted by the addresses {{noreply_address}} - just for automatized alerts - and {{support_address}} - for support;" },
				{ "Text": "Web system, registered at site {{site_address}};" },
				{ "Text": "Android app {{android_address}}, installed by Play Store." }
			]
		},
		{
			"Text": "The System source code - instructions to computer which rule the System - is hosted at a web address.",
			"Items": [
				{ "Text": "The address is public: {{github_address}}" },
				{ "Text": "The source code is OPEN and ANY person using uncensored internet access can see it and suggest changes, using a github login." }
			]
		},
		{ "Text": "Cookies are used to control logon and security of forms of this site." },
		{ "Text": "The User agrees to keep the registered e-mail updated." },
		{ "Text": "Any law violations done through the System are sole responsibility of the User." },
		{
			"Text": "Privacy: data inputed on the System by User is used on personal reports, that can be seen on the Site or the Mobile App, just by the User.",
			"Items": [
				{ "Text": "The access to this reports is controlled by login and password, which User is responsible to keep in secret;" },
				{ "Text": "The other way that this data can be accessed is by the force of law." }
			]
		},
		{
			"Text": "These terms can be changed any time in the future.",
			"Items": [
				{ "Text": "If the terms change, the user need to accept the new terms to keep using the system;" },
				{ "Text": "Passed 90 days, if the new terms are not accepted, User data can be permanently removed from the System." }
			]
		}
	]
}
'
		from contract
		where version = '4.5.0.1'
	union all
	select 'pt-BR', id, '
{
	"Text": "O sistema Don''t fly Money, abaixo constando como Sistema, e a pessoa que registrou-se no sistema, abaixo constando como Pessoa, concordam com os termos descritos.",
	"Items": [
		{
			"Text": "O Sistema não possui quaisquer vínculos com instituições financeiras.",
			"Items": [
				{ "Text": "Dados apresentados no Sistema são inseridos apenas pela Pessoa;" },
				{ "Text": "Movimentações feitas em Sistema não alteram dados em contas bancárias ou quaisquer outros meios externos;" },
				{ "Text": "Movimentações em meios externos não afetam os dados de Sistema, sendo estes apenas alteráveis pela Pessoa via Sistema;" }
			]
		},
		{ "Text": "Dados apresentados em Sistema e decisões tomadas com base nestes dados são de total responsabilidade da Pessoa." },
		{
			"Text": "Os meios de comunicação de Sistema com a Pessoa são:",
			"Items": [
				{ "Text": "E-mail cadastrado pela Pessoa ao se registrar em Sistema, cujo contato é feito somente pelos endereços {{noreply_address}} - apenas para alertas automatizados - e {{support_address}} - para suporte;" },
				{ "Text": "Sistema web, registrado no domínio {{site_address}};" },
				{ "Text": "Aplicativo para dispositivos android {{android_address}}, instalação via Play Store." }
			]
		},
		{
			"Text": "O código fonte de Sistema - instruções para computador que regem o funcionamento de Sistema - está em um endereço web.",
			"Items": [
				{ "Text": "O endereço é público: {{github_address}}" },
				{ "Text": "O código fonte é ABERTO, podendo QUALQUER pessoa com acesso não-censurado a internet visualizá-lo e sugerir edições, usando uma conta do github." }
			]
		},
		{ "Text": "Cookies são usados para controlar seu acesso a informações e a segurança dos formulários deste site." },
		{ "Text": "A Pessoa se compromete a manter o e-mail cadastrado atualizado." },
		{ "Text": "Quaisquer atos que violem leis feitos por intermédio de Sistema são responsabilidade da Pessoa." },
		{
			"Text": "Privacidade: dados inseridos no Sistema pela Pessoa são usados em relatórios pessoais, que podem ser vistos no Site ou no Aplicativo de celular, apenas pela Pessoa.",
			"Items": [
				{ "Text": "O acesso a esses relatórios é controlado por login e senha, que a Pessoa é responsável por manter em segredo;" },
				{ "Text": "A outra forma que estas informações podem ser acessadas é pela força da lei." }
			]
		},
		{
			"Text": "Estes termos podem ser alterados a qualquer momento.",
			"Items": [
				{ "Text": "Em caso de alteração, a Pessoa precisa aceitar os novos termos para continuar usando o sistema;" },
				{ "Text": "Passados 90 dias, caso não aceite os novos termos, dados da Pessoa podem ser removidos permanentemente de Sistema." }
			]
		}
	]
}
'
		from contract
		where version = '4.5.0.1';
