# SimpleTwitter app

This is a simple test application. 

The Test Assignment

Design and implement the API and server side modules for a simplified twitter-like social updates site:
●	The application contains users
●	Each user can post short text messages (140 chars)
●	Each user can follow other users, and get a feed of their latest updates. 
●	Each user can also get a global feed for all the users.
 
Implement an HTTP based (ReSTful in the loose sense of the term) API that exposes the following calls (no need for authentication, choose the format you like):
 
●	CreateUser [UserName]
●	PostMessage [UserId, MessageText]
●	Follow [FollowingUser, FollowedUser]
●	Unfollow [FollwingUser, UnfollowedUser]
●	GetFeed [ForUserId]
●	GetGlobalFeed

The design should be ready for heavy traffic web application, with many users and interactions. The design need to be ready for this situation. Please bear in mind the scalability of this system and try to find a good solution for a large scale application.

1. To achieve the main goal - messaging architecture is selected based on NServiceBus combined with CQRS approach without Event sourcing, as the ES is not needed in this app.
2. A simple UI is based on AngularJS and Bootstrap
3. RESTful service is a WebApi with asyncronous methods

The design explanation

WebApi communicates directly with ReadModels (Redis) through ReadModelFacade for quering data, for CRUD operations WebAPI communicates with WCFCommandService. It sends write Commands to WCF Service, which is a standalone console app (can be hosted on a separate server). 

WCF Service using NServiceBus (with underliying MSMQ) sends commands to Write Side.

Write Side is a console app (so can be hosted on another separate server), processes commands and publishes corresponding events through NServiceBus.
Write side persists data in memory using  Fake DbSet, which can be easily replaced with EF implementation (MSSQL Server or MYSQL )

Read Side is another console app subscribed on events published by Write Side and updates Read Models (Redis storage is used)

In production console applications better to convert into IIS hosted web service (WCF app) and windows services (write side and read side apps)

patterns used: CQRS pattern without Event sourcing, Repository + GenericRepository, Unit of Work
technologies used: NServiceBus (sender-> publisher -> subscriber), Redis (with StackExchange client), WCF, WebAPi
