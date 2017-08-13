Use dontflymon;

    alter table Detail 
        add column FutureMove_ID INTEGER;

    alter table Schedule 
        add column ShowInstallment TINYINT(1);

    create table FutureMove (
        ID INTEGER NOT NULL AUTO_INCREMENT,
       Description VARCHAR(50) not null,
       Date DATETIME not null,
       Nature INTEGER not null,
       Schedule_ID INTEGER,
       Category_ID INTEGER not null,
       In_ID INTEGER,
       Out_ID INTEGER,
       primary key (ID)
    );

    alter table Account 
        add index (User_ID), 
        add constraint FKBE1051AF63517727 
        foreign key (User_ID) 
        references User (ID);

    alter table Category 
        add index (User_ID), 
        add constraint FK6482F2463517727 
        foreign key (User_ID) 
        references User (ID);

    alter table Detail 
        add index (Move_ID), 
        add constraint FK321CBF8F848A59CB 
        foreign key (Move_ID) 
        references Move (ID);

    alter table Detail 
        add index (FutureMove_ID), 
        add constraint FK321CBF8F90E5CDC4 
        foreign key (FutureMove_ID) 
        references FutureMove (ID);

    alter table Month 
        add index (Year_ID), 
        add constraint FKA1324E9547E6F255 
        foreign key (Year_ID) 
        references Year (ID);

    alter table Schedule 
        add index (User_ID), 
        add constraint FK810B15B63517727 
        foreign key (User_ID) 
        references User (ID);

    alter table Security 
        add index (User_ID), 
        add constraint FKAAE052DE63517727 
        foreign key (User_ID) 
        references User (ID);

    alter table Summary 
        add index (Month_ID), 
        add constraint FKB6AB21C8D77739DC 
        foreign key (Month_ID) 
        references Month (ID);

    alter table Summary 
        add index (Year_ID), 
        add constraint FKB6AB21C847E6F255 
        foreign key (Year_ID) 
        references Year (ID);

    alter table Summary 
        add index (Category_ID), 
        add constraint FKB6AB21C844FCFB39 
        foreign key (Category_ID) 
        references Category (ID);

    alter table Year 
        add index (Account_ID), 
        add constraint FKE6D5EF1CC57BEA12 
        foreign key (Account_ID) 
        references Account (ID);

    alter table FutureMove 
        add index (Schedule_ID), 
        add constraint FKFAF045B96C85B666 
        foreign key (Schedule_ID) 
        references Schedule (ID);

    alter table FutureMove 
        add index (Category_ID), 
        add constraint FKFAF045B944FCFB39 
        foreign key (Category_ID) 
        references Category (ID);

    alter table FutureMove 
        add index (In_ID), 
        add constraint FKFAF045B9707068F 
        foreign key (In_ID) 
        references Account (ID);

    alter table FutureMove 
        add index (Out_ID), 
        add constraint FKFAF045B961325821 
        foreign key (Out_ID) 
        references Account (ID);

    alter table Move 
        add index (Schedule_ID), 
        add constraint FK912AEC5F6C85B666 
        foreign key (Schedule_ID) 
        references Schedule (ID);

    alter table Move 
        add index (Category_ID), 
        add constraint FK912AEC5F44FCFB39 
        foreign key (Category_ID) 
        references Category (ID);

    alter table Move 
        add index (In_ID), 
        add constraint FK912AEC5FCF923452 
        foreign key (In_ID) 
        references Month (ID);

    alter table Move 
        add index (Out_ID), 
        add constraint FK912AEC5FA9A76AFC 
        foreign key (Out_ID) 
        references Month (ID);

	alter table Schedule
		drop column next;