insert into migrations (name) values ('13.0.0.0');

-- contract begin
	insert into contract (beginDate, version)
		values (now(), '13.0.0.0');

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
			{
				"Text": "A funcionalidade de Moeda permite à Pessoa colocar valores diferentes para uma Movimentação de Transferência entre Contas no Sistema:",
				"Items": [
					{ "Text": "O Sistema não calcula qualquer conversão entre moedas por si mesmo;" },
					{ "Text": "Os valores para cada uma das duas moedas precisam ser inseridos pela Pessoa." }
				]
			},
			{
				"Text": "A funcionalidade de Agendamento permite agendar Movimentações para serem criadas no futuro. Existe uma automação executando a cada 15 minutos para inserir as Movimentações de agendamentos pendentes. Se a Pessoa agendar uma Movimentação para uma data passada, esta será criada na próxima vez que esta automação executar.",
				"New": true
			},
			{ 
				"Text": "Dados apresentados em Sistema e decisões tomadas com base nestes dados são de total responsabilidade da Pessoa:",
				"Items": [
					{ "Text": "Este contrato é firmado apenas com a Pessoa, não com qualquer indivíduo mesmo que esteja relacionado à Pessoa;" },
					{ "Text": "Se um indivíduo relacionado à Pessoa se registrar no Sistema, isso significa outra relação Pessoa x Sistema, não afetando outras relações de outras pessoas com o Sistema." }
				]
			},
			{
				"Text": "Os meios de comunicação do Sistema com a Pessoa são:",
				"Items": [
					{ "Text": "E-mail cadastrado pela Pessoa ao se registrar em Sistema, cujo contato é feito somente pelos endereços no-reply@dontflymoney.com - apenas para alertas automatizados - e dfm@dontflymoney.com - para suporte;" },
					{ "Text": "Sistema web, disponível em https://dontflymoney.com ;" },
					{ "Text": "Aplicativo para Dispositivos Android \'Don\'t fly Money\', que pode ser instalado via Play Store no endereço https://play.google.com/store/apps/details?id=com.dontflymoney.view ." }
				]
			},
			{
				"Text": "O código fonte do Sistema - instruções para computador que regem o funcionamento do Sistema - está em um endereço web:",
				"Items": [
					{ "Text": "O endereço é público: https://github.com/darakeon/dfm ;" },
					{ "Text": "O código fonte é ABERTO, podendo QUALQUER pessoa com acesso não-censurado a internet visualizá-lo e sugerir edições, usando uma conta do github." }
				]
			},
			{ "Text": "Cookies são usados para controlar seu acesso a informações e a segurança dos formulários deste Site;" },
			{
				"Text": "Após colocar seu e-mail e senha e se logar no Site ou no Aplicativo Android, seu login é mantido e seus dados disponíveis no dispositivo sem data de expiração definida:",
				"Items": [
					{ "Text": "O Site possui um botão no topo a direita para desfazer o login, removendo desta forma o acesso aos dados;" },
					{ "Text": "O Aplicativo Android possui o mesmo botão, no topo a esquerda na tela inicial e abaixo a direita nas outras telas, assim é possível desfazer o login, removendo desta forma o acesso aos dados;" },
					{ "Text": "O Site, na tela principal logada, possui um menu \'Logins\' no topo a direita onde é possível ver todos os logins ativos e desativá-los." }
				]
			},
			{ "Text": "A Pessoa se compromete a manter o e-mail cadastrado atualizado;" },
			{ "Text": "Quaisquer atos que violem leis feitos por intermédio do Sistema são responsabilidade da Pessoa;" },
			{
				"Text": "Privacidade: dados inseridos no Sistema pela Pessoa são usados:",
				"Items": [
					{
						"Text": "em relatórios pessoais, que podem ser vistos apenas pela Pessoa, no Site e no Aplicativo Android:",
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
				"Text": "A senha escolhida pela Pessoa no Sistema é registrada usando um protocolo de segurança:",
				"Items": [
					{ "Text": "ao invés de registrar o texto como foi digitado, é registrado um outro texto, gerado a partir do que foi digitado pela Pessoa;" },
					{ "Text": "quando a Pessoa digita novamente a senha no Sistema, a senha digitada naquele momento e o texto gerado são comparados para confirmar que a senha está correta;" },
					{ "Text": "não é possível saber qual a senha original tendo apenas o texto gerado, apenas a comparação entre a senha e o texto gerado é possível." }
				]
			},
			{
				"Text": "Quando a Pessoa se registra no Sistema existe um período de 7 dias para confirmar o endereço de e-mail:",
				"Items": [
					{
						"Text": "Uma mensagem é enviada ao endereço de e-mail informado quando a Pessoa se cadastra:",
						"Items": [
							{ "Text": "Ela contém um link - que a Pessoa pode clicar - e um token - que pode ser usado no Site seguindo as instruções descritas na mensagem de e-mail;" },
							{ "Text": "Qualquer dos dois métodos pode ser usado para confirmar o endereço de e-mail;" },
							{ "Text": "Ambos expiram em 3 dias;" },
							{ "Text": "Você pode enviar uma nova confirmação clicando no aviso que começa a aparecer no Site após dois dias de cadastro." }
						]
					},
					{ "Text": "Se o período de confirmação terminar, a Pessoa não pode mais usar o Sistema;" },
					{ "Text": "Cada vez que a Pessoa tenta se conectar depois do período terminado, uma nova mensagem de confirmação é enviada para o endereço de e-mail;" },
					{ "Text": "A funcionalidade de movimentações agendadas não está ativa enquanto o e-mail não for confirmado." }
				]
			},
			{
				"Text": "Quando a Pessoa abandona o Sistema por 90 dias, seus dados podem ser removidos:",
				"Items": [
					{ "Text": "Para evitar isso, a Pessoa deve interagir com o Sistema (estando logada) pelo Site e/ou pelo Aplicativo Android;" },
					{ "Text": "Agendamentos sendo rodados não contam como interações válidas para esta regra;" },
					{ "Text": "Haverá dois avisos, um em torno de 30 dias e outro em torno de 60 dias após a última interação válida, enviados ao e-mail cadastrado no Sistema pela Pessoa;" },
					{ "Text": "Passados 90 dias sem interações válidas descritas acima, o acesso direto e a manipulação via Sistema dos dados da Pessoa podem ser removidos." }
				]
			},
			{
				"Text": "Quando os dados são removidos do Sistema, não podem ser automaticamente recuperados. Dado que talvez a Pessoa queira manter os dados para si, mesmo sem usar o Sistema, as movimentações serão exportadas pelo Sistema para um arquivo antes da remoção:",
				"Items": [
					{ "Text": "a Pessoa pode requisitar o arquivo através do menu \'Dados Removidos\', usando os últimos e-mail e senha cadastrados no Sistema antes da exclusão;" },
					{ "Text": "uma vez requisitado, o arquivo será enviado para o endereço de e-mail originalmente registrado no Sistema, o mesmo preenchido na tela \'Dados Removidos\';" },
					{ "Text": "na mensagem de e-mail enviada com o arquivo, haverá um link para requisitar a exclusão permanente do arquivo - o link é válido por 7 dias;" },
					{ "Text": "a criação do arquivo não é uma garantia de sua existência para sempre: o arquivo pode ser apagado a qualquer momento após a remoção dos dados do Sistema, sem consulta prévia a Pessoa." }
				]
			},
			{
				"Text": "Existe um menu dentro das configurações no Site e um link dentro das configurações do Aplicativo Android onde a Pessoa pode excluir seus dados do Sistema:",
				"Items": [
					{ "Text": "Os dados não são excluídos instantaneamente;" },
					{ "Text": "Nesse caso - a exclusão ser uma requisição da Pessoa - as movimentações não são gravadas em um arquivo;" },
					{ "Text": "Após a remoção, não existe uma forma de a Pessoa recuperar os dados via sistema;" },
					{ "Text": "É pedida a senha da Pessoa para registrar o pedido de exclusão." }
				]
			},
			{ "Text": "Quando os dados são removidos do Sistema, o endereço de e-mail registrado é guardado usando o mesmo processo já usado para guardar a senha (descrito acima nestes termos);" },
			{ "Text": "O Sistema, considerando Site e Aplicativo Android, é mantido como aqui apresentado, e pode ficar indisponível. Não há garantias de o Sistema estar online, ou de permanecer online, não cabendo ônus a quem o mantém em caso de indisponibilidade, seja esta temporária ou permanente;" },
			{
				"Text": "As imagens do Sistema possuem seus direitos resguardados:",
				"Items": [
					{
						"Text": "A imagem logo do Site - porquinho - e a imagem de segurança tiveram sua autoria registrada na Biblioteca Nacional Brasileira;",
						"Items": [
							{ "Text": "Seu uso só pode ser feito mediante autorização através do e-mail dfm@dontflymoney.com ." }
						]
					},
					{ "Text": "O robô Android é modificado a partir de trabalhos criados e compartilhados pela Google e usado de acordo com os termos descritos na Licença de atribuição Creative Commons 3.0." }
				]
			},
			{
				"Text": "Estes termos podem ser alterados a qualquer momento:",
				"Items": [
					{ "Text": "Em caso de alteração, a Pessoa precisa aceitar os novos termos para continuar usando o Sistema;" },
					{ "Text": "Passados 90 dias, caso não aceite os novos termos, os dados da Pessoa podem ser removidos do Sistema, sob as mesmas condições da falta de interações descrita acima nesse contrato." }
				]
			},
			{
				"Text": "Estes termos não serão invalidados por:",
				"Items": [
					{ "Text": "Uma cláusula ser invalidada;" },
					{ "Text": "Qualquer exceção a qualquer caso que ocorra." }
				]
			},
			{
				"Text": "Estes termos podem ser encontrados a qualquer momento em que o Site esteja disponível:",
				"Items": [
					{ "Text": "Há um link no rodapé de todas as páginas do Site;" },
					{ "Text": "Há links nas telas de login e configurações do Aplicativo Android." }
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
			{
				"Text": "Currency feature allow the Person to put different values for a Move Transfer between Accounts in the System:",
				"Items": [
					{ "Text": "The System does not calculate any currency conversion by itself;" },
					{ "Text": "The values of both currency need to be input by the Person." }
				]
			},
			{
				"Text": "Schedule feature allow to schedule Moves to be created at the future. There is an automation running every 15 minutes to insert the Moves for the pending Schedules. If the Person schedule a Move for a past date, it will be created in the next time this automation runs.",
				"New": true
			},
			{ 
				"Text": "System data and decisions made based on this data are sole responsibility of the Person:",
				"Items": [
					{ "Text": "This contract is only agreed with the Person, not with anyone not registered into the System even though related to the Person;" },
					{ "Text": "If someone related to the Person register at the System, this is another relation Person x System, not affecting other relations to other people with the System." }
				]
			},
			{
				"Text": "Communication vehicles from System to the Person are:",
				"Items": [
					{ "Text": "E-mail informed by the Person on the registration at the System, which is only contacted by the addresses no-reply@dontflymoney.com - just for automated alerts - and dfm@dontflymoney.com - for support;" },
					{ "Text": "Website, available at the address https://dontflymoney.com ;" },
					{ "Text": "Android Mobile App \'Don\'t fly Money\', that can be installed via Play Store at the address https://play.google.com/store/apps/details?id=com.dontflymoney.view ." }
				]
			},
			{
				"Text": "The System source code - instructions to computer which rule the System - is hosted at a web address:",
				"Items": [
					{ "Text": "The address is public: https://github.com/darakeon/dfm ;" },
					{ "Text": "The source code is OPEN and ANY person using uncensored internet access can see it and suggest changes, using a github login." }
				]
			},
			{ "Text": "Cookies are used to control logon and security of forms of this Website;" },
			{
				"Text": "After you enter your credentials at the Website or Android App, it keeps you logged in and your data available at that device with no defined expiration date:",
				"Items": [
					{ "Text": "The Website has a button at the top right corner so one can remove the login, removing this way the access to its data;" },
					{ "Text": "The Android App has the same button, at top left corner in the app start screen, at bottom right corner at other screens, so one can remove the login, removing this way the access to its data;" },
					{ "Text": "The Website, in the logged in main screen, has a menu \'Logins\' at the top right corner where you can see all active logins and delete them." }
				]
			},
			{ "Text": "The Person agrees to keep the registered e-mail up to date;" },
			{ "Text": "Any law violations done through the System are sole responsibility of the Person;" },
			{
				"Text": "Privacy: data inputted into the System by the Person is used:",
				"Items": [
					{
						"Text": "at personal reports, that can be seen just by the Person, at the Website and the Android App:",
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
				"Text": "The password chosen by the Person at the System is registered using a security protocol:",
				"Items": [
					{ "Text": "instead of registering the text as typed, it is registered another text, generated from what was typed by the Person;" },
					{ "Text": "when the Person types the password again in the System, the typed password at that moment and the generated text are compared to confirm that the password is correct;" },
					{ "Text": "it is not possible to know which is the original password having only the generated text, only the comparison between the password and the text is possible." }
				]
			},
			{
				"Text": "Once the Person register at the System there is a period of 7 days to confirm the email address:",
				"Items": [
					{
						"Text": "A message is sent to the email address informed while registering:",
						"Items": [
							{ "Text": "It contains both a link - which the Person can click - and a token - that can be used at the Website following the email message instructions;" },
							{ "Text": "Any of the two methods can be used to confirm the Person email address;" },
							{ "Text": "Both methods expire in 3 days;" },
							{ "Text": "You can send a new confirmation message by clicking a warning that appears in the System after 2 days of registration." }
						]
					},
					{ "Text": "If the period to confirm ends the Person cannot use the System anymore;" },
					{ "Text": "Each time the Person try to login after the period a new email message is sent to confirm the email address;" },
					{ "Text": "Scheduled moves feature will not be active while the email is not confirmed." }
				]
			},
			{
				"Text": "When the Person abandons the System for 90 days, their data can be removed:",
				"Items": [
					{ "Text": "To avoid this, the Person should interact with the System (logged in) by Website and/or Android App;" },
					{ "Text": "Running schedules are not valid interactions for this rule;" },
					{ "Text": "There will be two warnings, one around 30 days and other around 60 days after last valid interaction, sent to registered e-mail at the System by the Person;" },
					{ 
						"Text": "Passed 90 days without above described interactions the Person\'s data can be removed from direct access and manipulation through the System."
					}
				]
			},
			{
				"Text": "When the data is removed from the System, this cannot be automatically restored. Given maybe the Person wants to keep the data to itself even without using the System, the moves will be exported by the System to a file before removal:",
				"Items": [
					{ "Text": "the Person can request the file through the \'Deleted Data\' menu, using same e-mail and password last registered at the System before the deletion;" },
					{ "Text": "once requested, the file will be send to the email address originally registered at the System, same filled in the \'Deleted Data\' page;" },
					{ "Text": "in the email message sent with the file, there will be a link to request the permanent deletion of the file - this link is valid for 7 days;" },
					{ "Text": "the creation of the file is not a guarantee of its existence forever: the file can be deleted any time after System data removal, without prior check with the Person." }
				]
			},
			{
				"Text": "There is menu inside system settings at the Website and a link inside Android App settings where the Person can delete their data from the System:",
				"Items": [
					{ "Text": "The data is not removed instantly;" },
					{ "Text": "In this case - the Person asking the data deletion - the moves are not recorded in a file;" },
					{ "Text": "After the removal there is no way to the Person to recover the data through the System;" },
					{ "Text": "The Person\'s password is asked to register the data deletion request." }
				]
			},
			{ "Text": "When the data is removed from the System, the registered e-mail address is kept using same process used to register the password (described above in this terms);" },
			{ "Text": "The System (considering Website and Android App) is kept as here presented and can be unavailable. There are no guarantees of the System being online or to stay online, and there will be no onus for the System maintainer in case of unavailability, this one being temporary or permanent;" },
			{
				"Text": "System images have the rights protected as follows:",
				"Items": [
					{
						"Text": "The logo image - the pig - and the safety image had their authorship registered at Biblioteca Nacional (National Library) in Brazil;",
						"Items": [
							{ "Text": "They only can be used with authorization via the email dfm@dontflymoney.com ." }
						]
					},
					{ "Text": "The Android robot is reproduced or modified from work created and shared by Google and used according to terms described in the Creative Commons 3.0 Attribution License." }
				]
			},
			{
				"Text": "These terms can be changed any time in the future:",
				"Items": [
					{ "Text": "If the terms change, the Person need to accept the new terms to keep using the System;" },
					{ "Text": "Passed 90 days, if the new terms are not accepted, the Person\'s data can be removed from the System, under the same conditions of lack of interaction describe before in this terms." }
				]
			},
			{
				"Text": "These terms are not invalidated by:",
				"Items": [
					{ "Text": "One clause being invalid;" },
					{ "Text": "Any exception made in any case." }
				]
			},
			{
				"Text": "These terms can be found whenever the Website is available:",
				"Items": [
					{ "Text": "There is a link at the footer of all Website pages;" },
					{ "Text": "There are links inside the Android App in login and settings screens." }
				]
			}
		]
	}
	', '\n', ''), '\t', '');

	select @len := max(len) from (
			select max(length(Json)) as len
				from terms
		union
			select length(@PT)
		union
			select length(@EN)
	) as lens;

	set @alter_query = concat('
	alter table terms
		modify column Json varchar(', @len, ') not null;
	');
	prepare alter_query from @alter_query;
	execute alter_query;
	deallocate prepare alter_query;

	insert into terms
			(Json, Language, Contract_ID)
		values
			(@PT, 'pt-BR', @contract_id),
			(@EN, 'en-US', @contract_id);
-- contract end


create table archive (
	ID BIGINT NOT NULL AUTO_INCREMENT,
	ExternalId LONGBLOB NOT NULL,
	Filename VARCHAR(256) NOT NULL,
	Status SMALLINT NOT NULL,
	Uploaded DATETIME NOT NULL,
	User_ID BIGINT NOT NULL,

	CONSTRAINT PK_Archive
		PRIMARY KEY (ID),

	CONSTRAINT UK_Archive
		UNIQUE(ExternalId(16)),

	CONSTRAINT FK_Archive_User
		FOREIGN KEY (User_ID)
		REFERENCES user (ID)
);

create table line (
	ID BIGINT NOT NULL AUTO_INCREMENT,
	Position SMALLINT NOT NULL,
	Description VARCHAR(50) NOT NULL,
	Date DATE NOT NULL,
	Category VARCHAR(20) NOT NULL,
	Nature SMALLINT NULL,
	In_ VARCHAR(20) NOT NULL,
	Out_ VARCHAR(20) NOT NULL,
	ValueCents INT NOT NULL,
	ConversionCents INT NULL,
	Scheduled DATETIME NOT NULL,
	Status SMALLINT NOT NULL,

	Archive_ID BIGINT NOT NULL,

	CONSTRAINT PK_Line
		PRIMARY KEY (ID),

	CONSTRAINT UK_Line
		UNIQUE(Position, Archive_ID),

	CONSTRAINT FK_Line_Archive
		FOREIGN KEY (Archive_ID)
		REFERENCES archive (ID)
);

ALTER TABLE detail
	ADD Line_ID BIGINT NULL,
	ADD CONSTRAINT FK_Detail_Line
		FOREIGN KEY (Line_ID)
		REFERENCES line (ID);
