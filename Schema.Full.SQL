/* ================================================================   
			v1.0: The outline for an ERP schema   
   ================================================================ */

/* .................. Dropping existing schema .................. */

/* Dependency Level 7 - Details/Transaction Details */
drop table if exists PAYMENT_LINE;
drop table if exists ITEM_PICKING_LINE;
drop table if exists ITEM_INV_LINE;

/* Dependency Level 6 - Tables */
drop table if exists PRODUCTION_PART;
drop table if exists CUSTOMER_PLACE;
drop table if exists SUPPLIER_PLACE;
drop table if exists BANK_ACCOUNT;

drop table if exists INVOICE_LINE;
drop table if exists ITEM_ORDER_LINE;

/* Dependency Level 5 - Tables */
drop table if exists INVOICE;
drop table if exists PAYMENT;
drop table if exists ITEM_PICKING;
drop table if exists ITEM_ORDER;

/* Dependency Level 4 - Tables */
drop table if exists PRODUCTION;
drop table if exists ITEM_INV;
drop table if exists CUSTOMER;
drop table if exists SUPPLIER;
drop table if exists BANK;

/* Dependency Level 3 - Tables */
drop table if exists ITEM_PKG;
drop table if exists ITEM_SPEC;

/* Dependency Level 2 - Tables */
drop table if exists ITEM;
drop table if exists CITY;

/* Dependency Level 1 - Tables */
drop table if exists REGION;

/* Dependency Level 0 - Tables */
drop table if exists PRODUCTION_PHASE;
drop table if exists PRODUCTION_MACHINE;
drop table if exists ITEM_CATEGORY;
drop table if exists MEASUREMENT_UNIT;
drop table if exists SPEC_TYPE;
drop table if exists STORE_POS;
drop table if exists STORE;
drop table if exists COUNTRY;
drop table if exists PAYMENT_TYPE;
drop table if exists CUSTOMER_CATEGORY;
drop table if exists SUPPLIER_CATEGORY;
drop table if exists SHIPMENT_COMPANY;



/* .................. Creating the schema .................. */

/* Dependency Level 1 - Tables */
create table STORE
(
    CID int not null,
    constraint PK_STORE primary key (CID)
);
go

create table STORE_POS
(
    CID       int not null,
    STORE_CID int not null,
    constraint PK_STORE_POS primary key (CID),
    constraint FK_STORE_POS_STORE foreign key (STORE_CID) references STORE (CID)
);
go

create table PRODUCTION_PHASE
(
    CID int not null,
    constraint PK_PRODUCTION_PHASE primary key (CID)
);
go

create table PRODUCTION_MACHINE
(
    CID int not null,
    constraint PK_PRODUCTION_MACHINE primary key (CID)
);
go

create table MEASUREMENT_UNIT
(
    CID int not null,
    constraint PK_MEASUREMENT_UNIT primary key (CID)
);
go

create table ITEM_CATEGORY
(
    CID                 int not null,
    LEVEL               int not null default 0,
    PARENT_CATEGORY_CID int null,
    constraint PK_ITEM_CATEGORY primary key (CID),
    constraint SK_ITEM_CATEGORY_01 foreign key (PARENT_CATEGORY_CID) references ITEM_CATEGORY (CID)
);
go

create table SPEC_TYPE
(
    CID int not null,
    constraint PK_SPEC_TYPE primary key (CID)
);
go

create table COUNTRY
(
    CID int not null,
    constraint PK_COUNTRY primary key (CID)
);
go

create table PAYMENT_TYPE
(
    CID int not null,
    constraint PK_PAYMENT_TYPE primary key (CID)
);
go

create table CUSTOMER_CATEGORY
(
    CID                 int not null,
    LEVEL               int not null default 0,
    PARENT_CATEGORY_CID int null,
    constraint PK_CUSTOMER_CATEGORY primary key (CID),
    constraint SK_CUSTOMER_CATEGORY_01 foreign key (PARENT_CATEGORY_CID) references CUSTOMER_CATEGORY (CID)
);
go

create table SUPPLIER_CATEGORY
(
    CID                 int not null,
    LEVEL               int not null default 0,
    PARENT_CATEGORY_CID int null,
    constraint PK_SUPPLIER_CATEGORY primary key (CID),
    constraint SK_SUPPLIER_CATEGORY_01 foreign key (PARENT_CATEGORY_CID) references SUPPLIER_CATEGORY (CID)
);
go

create table SHIPMENT_COMPANY
(
    CID int not null,
    constraint PK_SHIPMENT_COMPANY primary key (CID)
)


/* Dependency Level 2 - Archives */
create table REGION
(
    ID          int          not null identity,
    CODE        nvarchar(32) not null,
    COUNTRY_CID int          not null,
    constraint PK_REGION primary key (ID),
    constraint FK_REGIN_REF_COUNTRY foreign key (COUNTRY_CID) references COUNTRY (CID)
);
go


create table ITEM
(
    ID                   int          not null identity,
    CODE                 nvarchar(32) not null,
    MEASUREMENT_UNIT_CID int          not null,
    ITEM_CATEGORY_CID    int          not null,
    constraint PK_ITEM primary key (ID),
    constraint FK_ITEM_REF_MEASUREMENT_UNIT foreign key (MEASUREMENT_UNIT_CID) references MEASUREMENT_UNIT (CID),
    constraint FK_ITEM_REF_ITEM_CATEGORY foreign key (ITEM_CATEGORY_CID) references ITEM_CATEGORY (CID)
);
go



/* Dependency Level 3 - Details/Transactions */
create table CITY
(
    ID                   int not null identity,
    REGION_ID            int not null,
    SHIPMENT_COMPANY_CID int not null,
    constraint PK_CITY primary key (ID),
    constraint FK_CITY_REF_REGION foreign key (REGION_ID) references REGION (ID),
    constraint FK_CITY_REF_SHIPMENT_COMPANY foreign key (SHIPMENT_COMPANY_CID) references SHIPMENT_COMPANY (CID)
);
go

create table ITEM_PKG
(
    ID         int          not null identity,
    ITEM_ID    int          not null,
    BARCODE    nvarchar(32) not null,
    SERIAL_NUM bigint       not null,
    constraint PK_ITEM_PKG primary key (ID),
    constraint FK_ITEM_PKG_REF_ITEM foreign key (ITEM_ID) references ITEM (ID)
);
go

create table ITEM_SPEC
(
    ID            int not null identity,
    ITEM_ID       int not null,
    SPEC_TYPE_CID int,
    constraint PK_ITEM_SPEC primary key (ID),
    constraint FK_ITEM_SPEC_REF_ITEM foreign key (ITEM_ID) references ITEM (ID),
    constraint FK_ITEM_SPEC_REF_SPEC_TYPE foreign key (SPEC_TYPE_CID) references SPEC_TYPE (CID)
);
go





/* Dependency Level 4 - Details/Transactions */

create table ITEM_INV
(
    ID      int not null identity,
    ITEM_ID int not null,
    QTY     int not null default 0,
    constraint FK_ITEM_INV primary key (ID),
    constraint FK_ITEM_INV_REF_ITEM foreign key (ITEM_ID) references ITEM (ID)
);
go

create table PRODUCTION
(
    ID                   int not null identity,
    PRODUCTION_PHASE_CID int not null,
    ITEM_PKG_ID          int not null,
    constraint PK_PRODUCTION primary key (ID),
    constraint FK_PRODUCTION_REF_PRODUCTION_PHASE foreign key (PRODUCTION_PHASE_CID) references PRODUCTION_PHASE (CID),
    constraint FK_PRODUCTION_REF_ITEM_PKG foreign key (ITEM_PKG_ID) references ITEM_PKG (ID)
);
go

create table CUSTOMER
(
    ID                    int not null identity,
    CUSTOMER_CATEGORY_CID int not null,
    constraint PK_CUSTOMER primary key (ID),
    constraint FK_CUSTOMER_REF_CUSTOMER_CATEGORY foreign key (CUSTOMER_CATEGORY_CID) references CUSTOMER_CATEGORY (CID)
);
go

create table SUPPLIER
(
    ID                    int not null identity,
    SUPPLIER_CATEGORY_CID int not null,
    constraint PK_SUPPLIER primary key (ID),
    constraint FK_SUPPLIER_REF_SUPPLIER_CATEGORY foreign key (SUPPLIER_CATEGORY_CID) references SUPPLIER_CATEGORY (CID)
);
go

create table BANK
(
    ID          int not null identity,
    COUNTRY_CID int not null,
    constraint PK_BANK primary key (ID),
    constraint FK_BANK_REF_COUNTRY foreign key (COUNTRY_CID) references COUNTRY (CID)
);
go



/* Dependency Level 5 - Transaction Headers */
create table ITEM_ORDER
(
    ID                int      not null identity,
    IS_CUSTOMER_ORDER int      not null default 0,
    CUSTOMER_ID       int      null,
    SUPPLIER_ID       int      null,
    ORDER_DATETIME    datetime not null,
    constraint PK_ITEM_ORDER primary key (ID),
    constraint FK_ITEM_ORDER_REF_CUSTOMER foreign key (CUSTOMER_ID) references CUSTOMER (ID),
    constraint FK_ITEM_ORDER_REF_SUPPLIER foreign key (SUPPLIER_ID) references SUPPLIER (ID)
);
go

create table ITEM_PICKING
(
    ID                     int      not null identity,
    ITEM_ID                int      not null,
    ITEM_ORDER_ID          int      not null,
    PICKING_START_DATETIME datetime not null,
    PICKING_END_DATETIME   datetime not null,
    constraint PK_ITEM_PICKING primary key (ID),
    constraint FK_ITEM_PICKING_REF_ITEM foreign key (ITEM_ID) references ITEM (ID),
    constraint FK_ITEM_PICKING_REF_ITEM_ORDER foreign key (ITEM_ORDER_ID) references ITEM_ORDER (ID)
);
go

create table PAYMENT
(
    ID                  int      not null identity,
    IS_CUSTOMER_PAYMENT int      not null default 0,
    CUSTOMER_ID         int      null,
    SUPPLIER_ID         int      null,
    ITEM_ORDER_ID       int      not null,
    PAYMENT_DATETIME    datetime not null,
    constraint PK_PAYMENT primary key (ID),
    constraint FK_PAYMENT_REF_CUSTOMER foreign key (CUSTOMER_ID) references CUSTOMER (ID),
    constraint FK_PAYMENT_REF_SUPPLIER foreign key (SUPPLIER_ID) references SUPPLIER (ID),
    constraint FK_PAYMENT_REF_ITEM_ORDER foreign key (ITEM_ORDER_ID) references ITEM_ORDER (ID)
);
go

create table INVOICE
(
    ID                  int not null identity,
    IS_CUSTOMER_INVOICE int not null default 0,
    CUSTOMER_ID         int null,
    SUPPLIER_ID         int null,
    ITEM_ORDER_ID       int not null,
    constraint PK_INVOICE primary key (ID),
    constraint FK_INVOICE_REF_CUSTOMER foreign key (CUSTOMER_ID) references CUSTOMER (ID),
    constraint FK_INVOICE_REF_SUPPLIER foreign key (SUPPLIER_ID) references SUPPLIER (ID),
    constraint FK_INVOICE_REF_ITEM_ORDER foreign key (ITEM_ORDER_ID) references ITEM_ORDER (ID)
);
go





/* Dependency Level 6 - Details/Transaction Details */
create table ITEM_ORDER_LINE
(
    ID            int   not null identity,
    ITEM_ORDER_ID int   not null,
    ITEM_ID       int   not null,
    QTY           float not null,
    constraint PK_ITEM_ORDER_LINE primary key (ID),
    constraint FK_ITEM_ORDER_LINE_REF_ITEM_ORDER foreign key (ITEM_ORDER_ID) references ITEM_ORDER (ID),
    constraint FK_ITEM_ORDER_LINE_REF_ITEM foreign key (ITEM_ID) references ITEM (ID)
);
go

create table INVOICE_LINE
(
    ID         int   not null identity,
    INVOICE_ID int   not null,
    ITEM_ID    int   not null,
    QTY        float not null,
    PRICE      float not null,
    LINE_TOTAL float not null,
    constraint PK_INVOICE_LINE primary key (ID),
    constraint FK_INVOICE_LINE_REF_INVOICE foreign key (INVOICE_ID) references INVOICE (ID),
    constraint FK_INVOICE_LINE_REF_ITEM foreign key (ITEM_ID) references ITEM (ID)
);
go

create table CUSTOMER_PLACE
(
    ID          int          not null identity,
    CUSTOMER_ID int          not null,
    ADDRESS     nvarchar(64) not null,
    CITY_ID     int          not null,
    constraint PK_CUSTOMER_PLACE primary key (ID),
    constraint FK_CUSTOMER_PLACE_REF_CUSTOMER foreign key (CUSTOMER_ID) references CUSTOMER (ID),
    constraint FK_CUSTOMER_PLACE_REF_CITY foreign key (CITY_ID) references CITY (ID)
);
go

create table SUPPLIER_PLACE
(
    ID          int          not null identity,
    SUPPLIER_ID int          not null,
    ADDRESS     nvarchar(64) not null,
    CITY_ID     int          not null,
    constraint PK_SUPPLIER_PLACE primary key (ID),
    constraint FK_SUPPLIER_PLACE_REF_SUPPLIER foreign key (SUPPLIER_ID) references SUPPLIER (ID),
    constraint FK_SUPPLIER_PLACE_REF_CITY foreign key (CITY_ID) references CITY (ID)
);
go

create table BANK_ACCOUNT
(
    ID          int          not null identity,
    SUPPLIER_ID int          not null,
    IBAN        nvarchar(34) not null,
    BANK_ID     int          not null,
    constraint PK_BANK_ACCOUNT primary key (ID),
    constraint FK_BANK_ACCOUNT_REF_SUPPLIER foreign key (SUPPLIER_ID) references SUPPLIER (ID),
    constraint FK_BANK_ACCOUNT_REF_BANK foreign key (BANK_ID) references BANK (ID)
);
go

create table PRODUCTION_PART
(
    ID                     int not null identity,
    PRODUCTION_ID          int not null,
    NUM                    int not null,
    ITEM_ID                int not null,
    PRODUCTION_MACHINE_CID int not null,
    constraint FK_PRODUCTION_PART primary key (ID),
    constraint FK_PRODUCTION_PART_REF_PRODUCTION foreign key (PRODUCTION_ID) references PRODUCTION (ID),
    constraint FK_PRODUCTION_PART_REF_ITEM foreign key (ITEM_ID) references ITEM (ID),
    constraint FK_PRODUCTION_PART_REF_PRODUCTION_MACHINE foreign key (PRODUCTION_MACHINE_CID) references PRODUCTION_MACHINE (CID)
);
go




/* Dependency Level 7 - Details/Transaction Details */
create table PAYMENT_LINE
(
    ID               int   not null identity,
    PAYMENT_ID       int   not null,
    PAYMENT_TYPE_CID int   not null,
    AMOUNT           float not null default 0,
    BANK_ACCOUNT_ID  int   not null,
    constraint PK_PAYMENT_LINE primary key (ID),
    constraint FK_PAYMENT_LINE_REF_PAYMENT foreign key (PAYMENT_ID) references PAYMENT (ID),
    constraint FK_PAYMENT_LINE_REF_PAYMENT_TYPE foreign key (PAYMENT_TYPE_CID) references PAYMENT_TYPE (CID),
    constraint FK_PAYMENT_LINE_REF_BANK_ACCOUNT foreign key (BANK_ACCOUNT_ID) references BANK_ACCOUNT (ID)
);
go

create table ITEM_PICKING_LINE
(
    ID                 int   not null identity,
    ITEM_PICKING_ID    int   not null,
    ITEM_ORDER_LINE_ID int   not null,
    ITEM_PKG_ID        int   not null,
    STORE_POS_CID      int   null,
    QTY_IN_UNIT        float not null,
    constraint PK_ITEM_PICKING_LINE primary key (ID),
    constraint FK_ITEM_PICKING_LINE_REF_ITEM_PICKING foreign key (ITEM_PICKING_ID) references ITEM_PICKING (ID),
    constraint FK_ITEM_PICKING_LINE_REF_ITEM_ORDER_LINE foreign key (ITEM_ORDER_LINE_ID) references ITEM_ORDER_LINE (ID),
    constraint FK_ITEM_PICKING_LINE_REF_ITEM_PKG foreign key (ITEM_PKG_ID) references ITEM_PKG (ID),
    constraint FK_ITEM_PICKING_LINE_REF_STORE_POS foreign key (STORE_POS_CID) references STORE_POS (CID)
);
go

create table ITEM_INV_LINE
(
    ID            int   not null identity,
    ITEM_PKG_ID   int   not null,
    STORE_POS_CID int   not null,
    STORED_QTY    float not null,
    constraint PK_ITEM_INV_LINE primary key (ID),
    constraint FK_ITEM_INV_REF_ITEM_PKG foreign key (ITEM_PKG_ID) references ITEM_PKG (ID),
    constraint FK_ITEM_INV_LINE_REF_STORE_POS foreign key (STORE_POS_CID) references STORE_POS (CID)
);
go
