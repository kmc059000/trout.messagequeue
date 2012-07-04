
create table EmailQueueItems
(
	ID int PRIMARY KEY identity(1,1) not null,
	[To] nvarchar(1000) default('') not null,
	[Cc] nvarchar(1000) default('')  not null,
	[Bcc] nvarchar(1000) default('')  not null,
	Subject nvarchar(1000) default('') not null,
	Body nvarchar(max) default('') not null,
	IsSent bit not null,
	IsFailed bit not null,
	NumberTries tinyint not null,
	CreateDate datetime not null,
	LastTryDate datetime null,
	SendDate datetime null,
	IsBodyHtml bit not null,
	AttachmentCount tinyint not null
)

