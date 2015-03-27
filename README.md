# SimpleTwitter app

UI AngularJS and Bootstrap
RESTful service WebApi (asyncronous)

WebApi sends write Commands to WCF Service, which is a standalone console app (can be hosted on a separate server). 

WCF Service using NServiceBus (with underliying MSMQ) sends commands to Write Side.

Write Side is a console app (so can be hosted on another separate server), processes commands and publishes corresponding events through NServiceBus
write side persists data in memory using  Fake DbSet, which can be easily replaced with EF implementation

Read Side is another console app subscribed on events published by Write Side and updates Read Models (Redis storage is used)

in real production console applications better to convert into IIS hosted web service (WCF app) 
and windows services (write side and read side apps)

used patterns: CQRS pattern without Event sourcing, Repository + GenericRepository, Unit of Work
used technologies: NServiceBus (sender-> publisher -> subscriber), Redis (with StackExchange client), WCF, WebAPi, AngularJS 
