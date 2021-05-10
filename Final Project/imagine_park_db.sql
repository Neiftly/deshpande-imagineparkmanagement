If Exists(Select 1 From master.dbo.sysdatabases Where NAME = 'imagine_park_db')
Begin
	Drop Database imagine_park_db
	print '' print '*** dropping database imagine_park_db ***'
End
Go

print '' print '*** creating database imagine_park_db ***'
Go
Create Database imagine_park_db
Go

print '' print '*** using database imagine_park_db ***'
Go
Use [imagine_park_db]
Go

print '' print '*** creating ZIP table ***'
GO
Create Table [dbo].[ZIP](
	[ZIPCode] 			[nvarchar](5) Not Null,
	[City]				[nvarchar](256) Not Null,
	[StateAbbreviation] [nvarchar](2) Not Null,
	[StateName] 		[nvarchar](60) Not Null,
	
	Constraint [pk_ZIPCode] Primary Key([ZIPCode] ASC)
	
)
GO

print '' print '+++ creating ZIP test data +++'
GO
Insert Into [dbo].[ZIP]
		([ZIPCode], [City], [StateAbbreviation], [StateName])
	Values
		('52235', 'Hills', 'IA', 'Iowa'),
		('52240', 'Iowa City', 'IA', 'Iowa'),
		('52241', 'Coralville', 'IA', 'Iowa'),
		('52242', 'Iowa City', 'IA', 'Iowa'),
		('52243', 'Iowa City', 'IA', 'Iowa'),
		('52244', 'Iowa City', 'IA', 'Iowa'),
		('52245', 'Iowa City', 'IA', 'Iowa'),
		('52246', 'Iowa City', 'IA', 'Iowa'),
		('52317', 'North Liberty', 'IA', 'Iowa'),
		('52319', 'Oakdale', 'IA', 'Iowa'),
		('52322', 'Oxford', 'IA', 'Iowa'),
		('52333', 'Solon', 'IA', 'Iowa'),
		('52338', 'Swisher', 'IA', 'Iowa'),
		('52340', 'Tiffin', 'IA', 'Iowa')
GO

print '' print '*** creating worker table ***'
GO
Create Table [dbo].[Worker](
	[WorkerID] 		[int] Identity(1000000, 1) 	Not Null,
	[LastName] 		[nvarchar](100) 			Not Null,
	[FirstName] 	[nvarchar](50) 				Not Null,
	[StreetAddress] [nvarchar](256) 			Not Null,
	[ZIPCode] 		[nvarchar](5) 				Not Null,
	[Phone] 		[nvarchar](15) 				Not Null,
	[Email] 		[nvarchar](100) 			Not Null,
	[PasswordHash] 	[nvarchar](100) 			Not Null 	Default 
		'9C9064C59F1FFA2E174EE754D2979BE80DD30DB552EC03E7E327E9B1A4BD594E',
	[Active] 		[bit]						Not Null 	Default 1,
	[StartDate] 	[datetime] 					Not Null 	Default current_timestamp,
	[EndDate] 		[datetime] 					Null,
	
	Constraint [pk_WorkerID] Primary Key([WorkerID] ASC),
	Constraint [fk_ZIPCode] Foreign Key([ZIPCode]) References [dbo].[ZIP]([ZIPCode]),
	Constraint [ak_Email] Unique([Email] ASC)
)
GO

print '' print '+++ creating Worker test data +++'
GO
Insert Into [dbo].[Worker]
		([LastName], [FirstName], [StreetAddress], [ZIPCode], [Phone], [Email])
	Values
		('Watkins', 'Sioned', '712 Chestnut St', '52235', '7245157405', 'sioned@imaginepark.com'),
		('Rollins', 'Brandi', '3578 Riverside Dr', '52246', '2489774767', 'brandi@imaginepark.com'),
		('Snider', 'Jevon', '117 E South St', '52240', '6628275255', 'jevon@imaginepark.com'),
		('Massey', 'Alex', '320 N Saint Louis St', '52322', '9187883888', 'alex@imaginepark.com'),
		('Grimes', 'Lucille', '7575 Kingsport Hwy', '52340', '4236394375', 'lucille@imaginepark.com'),
		('Odom', 'Devon', '126 Union St', '52340', '4136425398', 'devon@imaginepark.com'),
		('Paul', 'Carter', '3548 Grant Ave', '52319', '6145390690', 'carter@imaginepark.com'),
		('Connelly', 'Monica', '3262 Beechwood Dr', '52244', '3305349323', 'monica@imaginepark.com'),
		('Pacheco', 'Esha', '200 S Westphalia St #B B', '52338', '9895873455', 'esha@imaginepark.com'),
		('Braun', 'Thelma', '28 Constable Rd', '52243', '7326583868', 'thelma@imaginepark.com')
GO

print '' print '*** creating available rank table ***'
Go
Create Table [dbo].[AvailableRank](
	[AvailableID] [int] 		Not Null,
	[Description] [nvarchar](4) Not Null,
	
	Constraint [pk_AvailableID] Primary Key([AvailableID] ASC)
)
GO

print '' print '+++ creating AvailableRank test data +++'
GO
Insert Into [dbo].[AvailableRank]
		([AvailableID], [Description])
	Values
		(0, 'None'),
		(1, 'AM'),
		(2, 'PM'),
		(3, 'Both')
GO

print '' print '*** creating availability table ***'
GO
Create Table [dbo].[Availability](
	[WorkerID] 	[int] Not Null,
	[Sunday] 	[int] Not Null Default 3,
	[Monday] 	[int] Not Null Default 3,
	[Tuesday] 	[int] Not Null Default 3,
	[Wednesday] [int] Not Null Default 3,
	[Thursday] 	[int] Not Null Default 3,
	[Friday] 	[int] Not Null Default 3,
	[Saturday] 	[int] Not Null Default 3,
	
	Constraint [fk_WorkerID_Availability] Foreign Key([WorkerID]) References [dbo].[Worker]([WorkerID]),
	Constraint [fk_Sunday] Foreign Key([Sunday]) References [dbo].[AvailableRank]([AvailableID]),
	Constraint [fk_Monday] Foreign Key([Monday]) References [dbo].[AvailableRank]([AvailableID]),
	Constraint [fk_Tuesday] Foreign Key([Tuesday]) References [dbo].[AvailableRank]([AvailableID]),
	Constraint [fk_Wednesday] Foreign Key([Wednesday]) References [dbo].[AvailableRank]([AvailableID]),
	Constraint [fk_Thursday] Foreign Key([Thursday]) References [dbo].[AvailableRank]([AvailableID]),
	Constraint [fk_Friday] Foreign Key([Friday]) References [dbo].[AvailableRank]([AvailableID]),
	Constraint [fk_Saturday] Foreign Key([Saturday]) References [dbo].[AvailableRank]([AvailableID])
)
GO

print '' print '+++ creating Availability Test Data +++'
Go
Insert Into [dbo].[Availability]
		([WorkerID], [Sunday], [Monday], [Tuesday], [Wednesday], [Thursday], [Friday], [Saturday])
	Values
		(1000000, '3', '3', '3', '3', '3', '3', '3'),
		(1000001, '0', '3', '3', '3', '3', '3', '0'),
		(1000002, '0', '3', '3', '3', '3', '3', '1'),
		(1000003, '3', '0', '0', '3', '3', '3', '3'),
		(1000004, '2', '2', '2', '2', '2', '0', '0'),
		(1000005, '2', '2', '2', '2', '2', '2', '2'),
		(1000006, '3', '2', '2', '2', '2', '2', '2'),
		(1000007, '1', '2', '2', '2', '2', '2', '2'),
		(1000008, '3', '2', '2', '2', '2', '2', '2'),
		(1000009, '2', '2', '2', '2', '2', '2', '2')
GO

print '' print '*** creating worker type table ***'
GO
Create Table [dbo].[WorkerType](
	[WorkerID] 	[int] 				Not Null,
	[Paid] 		[bit] 				Not Null Default 1,
	[Salary] 	[decimal](12, 2) 	Not Null,
	
	Constraint [pk_WorkerID_WorkerType] Primary Key ([WorkerID]),
	Constraint [fk_WorkerID_WorkerType] Foreign Key([WorkerID]) References [dbo].[Worker]([WorkerID])
)
GO

print '' print '+++ creating Worker Type test data +++'
GO
Insert Into [dbo].[WorkerType]
		([WorkerID], [Paid], [Salary])
	Values
		(1000000, 1, 88.00),
		(1000001, 1, 65.50),
		(1000002, 1, 74.00),
		(1000003, 0, 0.00),
		(1000004, 1, 20.00),
		(1000005, 0, 0.00),
		(1000006, 0, 0.00),
		(1000007, 0, 0.00),
		(1000008, 0, 0.00),
		(1000009, 1, 15.00)
GO

print '' print '*** creating role table ***'
GO
Create Table [dbo].[Role] (
	[RoleID] 		[nvarchar](25)	Not Null,
	[Description] 	[nvarchar](250) Null,
	
	Constraint [pk_RoleID] Primary Key([RoleID] ASC)
)
GO

print '' print '+++ creating Role test data +++'
GO
Insert Into [dbo].[Role]
		([RoleID], [Description])
	Values
		('Park Manager', 'Manages The Entire Park'),
		('Park Staff Admin', 'Manages The Staff'),
		('QuarterMaster', 'Manages Inventory'),
		('Maintenance Manager', 'Creates Maintenance Tasks'),
		('Intern', 'Does work as assigned'),
		('Volunteer', 'Unpaid Worker')
GO

print '' print '*** creating worker role table ***'
GO
Create Table [dbo].[WorkerRole] (
	[WorkerID] 		[int] 	 		Not Null,
	[RoleID]		[nvarchar](25) 	Not Null,
	
	Constraint [pk_WorkerID_RoleId] Primary Key ([WorkerID], [RoleID]),
	Constraint [fk_WorkerID_WorkerRole] Foreign Key([WorkerID]) References [dbo].[Worker]([WorkerID]),
	Constraint [fk_RoleID_WorkerRole] Foreign Key([RoleID]) References [dbo].[Role]([RoleID])
)
GO

print '' print '+++ creating WorkerRole test data +++'
GO
Insert Into [dbo].[WorkerRole]
		([WorkerID], [RoleID])
	Values
		(1000000, 'Park Manager'),
		(1000000, 'Park Staff Admin'),
		(1000000, 'QuarterMaster'),
		(1000000, 'Maintenance Manager'),
		(1000001, 'Park Staff Admin'),
		(1000002, 'QuarterMaster'),
		(1000003, 'Volunteer'),
		(1000004, 'Maintenance Manager'),
		(1000005, 'Volunteer'),
		(1000006, 'Volunteer'),
		(1000007, 'Volunteer'),
		(1000008, 'Volunteer'),
		(1000009, 'Intern')
GO

print '' print '*** creating WorkerAuditTrail table ***'
GO
Create Table [dbo].[WorkerAuditTrail] (
	[WorkerID] [int] Not Null,
    [LastName] [nvarchar](100) Not Null,
    [FirstName] [nvarchar](50) Not Null,
    [StreetAddress] [nvarchar](256) Not Null,
    [ZIPCode] [nvarchar](5) Not Null,
    [Phone] [nvarchar](15),
    [Email] [nvarchar](100),
    [StartDate] [datetime] Not Null Default current_timestamp,
    [EndDate] [datetime],
    [ChangeType] [nvarchar](6) Not Null,
    [ChangeDateTime] [datetime] Not Null,
    [AdminID] [nvarchar](256) Not Null,
)

print '' print '*** creating Project table ***'
GO
Create Table [dbo].[Project] (
	[ProjectID] 		[int] Identity(1000000, 1) 	Not Null,
    [WorkerID] 			[int] 						Null,
    [Paid] 				[bit] 						Not Null Default 1,
    [TaskListFilename] 	[nvarchar](256) 			Null,
    [StartDate] 		[datetime] 					Not Null Default current_timestamp,
    [Deadline] 			[datetime] 					Null,
    [EndDate] 			[datetime] 					Null,
    [HoursWorked] 		[int] 						Not Null Default 0,
    
	Constraint [pk_ProjectID] Primary Key ([ProjectID]),
    Constraint [fk_WorkerID_Project] Foreign Key ([WorkerID]) References [dbo].[Worker]([WorkerID])
)
GO

print '' print '+++ creating Project test data +++'
GO
Insert Into [dbo].[Project]
		([WorkerID], [Paid], [TaskListFilename], [StartDate], [Deadline], [EndDate], [HoursWorked])
	Values
		(1000004, 1, Null, '2019-08-26 00:00:00', '1/1/1753 12:00:00 AM', '2020-01-15 00:00:00', 134),
		(1000004, 1, Null, '2019-10-10 00:00:00', '1/1/1753 12:00:00 AM', '2020-02-01 11:59:59', 43),
		(1000003, 0, Null, '2020-11-28 00:00:00', '2020-12-07 00:00:00', '1/1/1753 12:00:00 AM', Default),
		(1000002, 1, Null, '2020-04-04 00:00:00', '1/1/1753 12:00:00 AM', '1/1/1753 12:00:00 AM', Default),
		(1000009, 1, Null, '2020-05-01 00:00:00', '1/1/1753 12:00:00 AM', '1/1/1753 12:00:00 AM', Default),
		(1000005, 0, Null, Default, DateAdd(day, 7, current_timestamp), '1/1/1753 12:00:00 AM', Default),
		(1000006, 0, Null, Default, DateAdd(day, 7, current_timestamp), '1/1/1753 12:00:00 AM', Default),
		(1000007, 0, Null, Default, DateAdd(day, 7, current_timestamp), '1/1/1753 12:00:00 AM', Default),
		(1000008, 0, Null, Default, DateAdd(day, 7, current_timestamp), '1/1/1753 12:00:00 AM', Default),
		(1000003, 0, Null, Default, DateAdd(day, 7, current_timestamp), '1/1/1753 12:00:00 AM', Default),
		(1000009, 1, Null, Default, DateAdd(day, 15, current_timestamp), '1/1/1753 12:00:00 AM', Default),
		(1000002, 1, Null, '2020-01-01 00:00:00', '1/1/1753 12:00:00 AM', '1/1/1753 12:00:00 AM', Default)
GO

print '' print '*** creating ProjectName table ***'
GO
Create Table [dbo].[ProjectName] (
	[ProjectName] 			[nvarchar](256) Not Null,
	[ProjectDescription] 	[nvarchar](256) Not Null,
	
	Constraint [pk_ProjectName] Primary Key ([ProjectName])
)
GO

print '' print '+++ creating ProjectName test data +++'
GO
Insert Into [dbo].[ProjectName]
		([ProjectName], [ProjectDescription])
	Values
		('Repair Tool Shed', 'Find and fix any damage to the tool shed and then clean it.'),
		('Replace Trail Signs', 'Find and Replace damaged or missing Trail Signs'),
		('Amphitheater Lawnmowing', 'Using the Lawnmower Cut the grass around the amphitheater'),
		('Paint the Tool Shed', 'Repaint the Tool Shed to an approved color(See Park Manager)'),
		('Check Trails for repairs', 'Walk trails and report any damage'),
		('Clean Northeast Trail', 'Clean the northeast trail if damage or debris is reported'),
		('Clean Northwest Trail', 'Clean the northwest trail if damage or debris is reported'),
		('Check Bathrooms', 'Check bathrooms for resupply and cleaning, if neccessary resolve issues'),
		('Clean Guest Center', 'Make the Guest Center both presentable and livable'),
		('Replace Lightbulbs', 'Check all buildings with electricity for dead bulbs, if found replace with new ones'),
		('Vehicle Maintenance', 'See what kind of maintenance is required. Either fix it or take into town to the auto shop'),
		('Broken Tools Project', 'Tools assigned to this project are broken and need replacing or fixing')
GO		

print '' print '*** creating ProjectDescription table ***'
GO
Create Table [dbo].[ProjectDescription] (
	[ProjectID] 	[int] 			Not Null,
	[ProjectName] 	[nvarchar](256) Not Null,
	
	Constraint [pk_ProjectID_ProjectName] Primary Key ([ProjectID], [ProjectName]),
	Constraint [fk_ProjectID_ProjectDescription] Foreign Key ([ProjectID]) References [dbo].[Project]([ProjectID]),
	Constraint [fk_ProjectName_ProjectDescription] Foreign Key ([ProjectName]) References [dbo].[ProjectName]([ProjectName])
)
GO

print '' print '+++ creating ProjectDescription test data +++'
GO
Insert Into [dbo].[ProjectDescription]
		([ProjectID], [ProjectName])
	Values
		(1000000, 'Repair Tool Shed'),
		(1000001, 'Replace Trail Signs'),
		(1000002, 'Amphitheater Lawnmowing'),
		(1000003, 'Paint the Tool Shed'),
		(1000004, 'Check Trails for repairs'),
		(1000005, 'Clean Northeast Trail'),
		(1000006, 'Clean Northwest Trail'),
		(1000007, 'Check Bathrooms'),
		(1000008, 'Clean Guest Center'),
		(1000009, 'Replace Lightbulbs'),
		(1000010, 'Vehicle Maintenance'),
		(1000011, 'Broken Tools Project')
GO
		

print '' print '*** creating project audit trail table ***'
GO
Create Table [dbo].[ProjectAuditTrail] (
	[ProjectID] 		[int] Identity(1000000, 1) 	Not Null,
    [WorkerID] 			[int] 						Null,
    [Paid] 				[bit] 						Not Null Default 1,
    [TaskListFilename] 	[nvarchar](256) 			Null,
    [StartDate] 		[datetime] 					Not Null Default current_timestamp,
    [Deadline] 			[datetime] 					Null,
    [EndDate] 			[datetime] 					Null,
    [HoursWorked] 		[int] 						Null,
	[ChangeType] 		[nvarchar](6) 				Not Null,
	[ChangeDateTime] 	[datetime] 					Not Null Default current_timestamp,
	[AdminID] 			[nvarchar](256) 			Not Null
)
GO

print '' print '*** creating tool table ***'
GO
Create Table [dbo].[Tool] (
	[ToolID] 			[int] Identity(1000000, 1) 	Not Null,
	[ToolDescription] 	[nvarchar](256) 			Not Null,
	
	Constraint [pk_ToolID] Primary Key ([ToolID])
)
GO

print '' print '+++ creating tool test data +++'
GO
Insert Into [dbo].[Tool]
		([ToolDescription])
	Values
		('Mattock 1'),
		('Mattock 2'),
		('Mattock 3'),
		('Mattock 4'),
		('Mattock 5'),
		('Manual Weed Whacker 1'),
		('Manual Weed Whacker 2'),
		('Manual Weed Whacker 3'),
		('Manual Weed Whacker 4'),
		('Manual Weed Whacker 5'),
		('Lawnmower 1'),
		('Lawnmower 2'),
		('Truck 1'),
		('Truck 2'),
		('Van 1'),
		('Axe 1'),
		('Axe 2'),
		('Axe 3'),
		('HandSaw 1'),
		('HandSaw 2'),
		('HandSaw 3'),
		('Tamper 1'),
		('Tamper 2'),
		('Painting Kit 1'),
		('Painting Kit 2'),
		('Bathroom Cleaning Kit 1'),
		('Bathroom Cleaning Kit 2'),
		('Lightbulb Replacement Kit'),
		('General Repair Kit 1'),
		('General Repair Kit 2'),
		('General Repair Kit 3'),
		('General Repair Kit 4'),
		('Northwest Trail Status Notebook'),
		('Northeast Trail Status Notebook')
GO
		

print '' print '*** creating Tool Project table ***'
GO
Create Table [dbo].[ProjectTool] (
	[ProjectID] [int] Not Null,
	[ToolID] 	[int] Not Null,
	
	Constraint [pk_ToolID_ProjectID] Primary Key ([ToolID], [ProjectID]),
	Constraint [fk_ProjectID_ProjectTool] Foreign Key ([ProjectID]) References [dbo].[Project]([ProjectID]),
	Constraint [fk_ToolID_ProjectTool] Foreign Key ([ToolID]) References [dbo].[Tool]([ToolID])
	
)
GO

print '' print '+++ creating Tool Project test data +++'
GO
Insert Into [dbo].[ProjectTool]
		([ProjectID], [ToolID])
	Values
		(1000002, 1000010),
		(1000002, 1000007),
		(1000003, 1000023),
		(1000004, 1000032),
		(1000004, 1000033),
		(1000005, 1000028),
		(1000005, 1000018),
		(1000005, 1000005),
		(1000005, 1000000),
		(1000005, 1000021),
		(1000005, 1000015),
		(1000006, 1000019),
		(1000006, 1000006),
		(1000006, 1000001),
		(1000006, 1000022),
		(1000006, 1000016),
		(1000007, 1000025),
		(1000008, 1000026),
		(1000008, 1000029),
		(1000009, 1000027),
		(1000010, 1000011),
		(1000010, 1000012),
		(1000010, 1000014),
		(1000011, 1000008),
		(1000011, 1000017)
GO

print '' print '*** creating sp_authenticate_user ***'
GO
Create Procedure [dbo].[sp_authenticate_user]
	(
	@Email			[nvarchar](100),
	@PasswordHash	[nvarchar](100)
	)
AS
	Begin
		Select 	Count(Email)
		From 	Worker
		Where 	Email = @Email
		And 	PasswordHash = @PasswordHash
		And 	Active = 1
	End
GO

print '' print '*** creating sp_update_passwordhash ***'
GO
Create Procedure [dbo].[sp_update_passwordhash]
	(
	@Email				[nvarchar](100),
	@OldPasswordHash	[nvarchar](100),
	@NewPasswordHash	[nvarchar](100)
	)
AS
	Begin
		Update 	Worker
			Set 	PasswordHash = @NewPasswordHash
			Where 	Email = @Email
			And 	PasswordHash = @OldPasswordHash
		Return 	@@RowCount /* Operation SuperGlobal  System should do automatically */
	End
GO

print '' print '*** creating sp_select_user_by_email ***'
GO
Create Procedure [dbo].[sp_select_user_by_email]
	(
	@Email			[nvarchar](100)
	)
AS
	Begin
		Select 	WorkerId, Email, LastName, FirstName, StreetAddress, ZIPCode, Phone, Active
		From 	Worker
		Where 	Email = @Email
	End
GO

print '' print '*** creating sp_select_roles_by_workerId ***'
GO
Create Procedure [dbo].[sp_select_roles_by_workerId]
	(
	@WorkerID		[int]
	)
AS
	Begin
		Select 	RoleID
		From 	WorkerRole
		Where 	WorkerID = @WorkerID
	End
GO

print '' print '*** creating sp_update_employee_profile_data ***'
GO
Create Procedure [dbo].[sp_update_worker_profile_data]
	(
	@WorkerID			[int],
	@NewEmail			[nvarchar](100),
	@NewFirstName		[nvarchar](50),
	@NewLastName		[nvarchar](100),
	@NewPhone			[nvarchar](15),
	@NewStreetAddress 	[nvarchar](256),
	@NewZIPCode			[nvarchar](5),
	
	@OldEmail			[nvarchar](100),
	@OldFirstName		[nvarchar](50),
	@OldLastName		[nvarchar](100),
	@OldPhone			[nvarchar](15),
	@OldStreetAddress	[nvarchar](256),
	@OldZIPCode			[nvarchar](5)
	
	)
AS
	Begin
		Update  Worker
			Set 	Email = @NewEmail,
					FirstName = @NewFirstName,
					LastName = @NewLastName,
					Phone = @NewPhone,
					StreetAddress = @NewStreetAddress,
					ZIPCode = @NewZIPCode
			Where 	WorkerID = @WorkerID
			And 	Email = @OldEmail
			And		FirstName = @OldFirstName
			And		LastName = @OldLastName
			And		Phone = @OldPhone
			And		StreetAddress = @OldStreetAddress
			And		ZIPCode = @OldZIPCode
		Return 	@@RowCount
	End
GO

print '' print '*** creating sp_update_availability ***'
GO
Create Procedure [dbo].[sp_update_worker_availability]
	(
		@WorkerID		[int],
		@NewSunday		[int],
		@NewMonday		[int],
		@NewTuesday		[int],
		@NewWednesday	[int],
		@NewThursday	[int],
		@NewFriday		[int],
		@NewSaturday	[int],
		
		@OldSunday		[int],
		@OldMonday		[int],
		@OldTuesday		[int],
		@OldWednesday	[int],
		@OldThursday	[int],
		@OldFriday		[int],
		@OldSaturday	[int]
	)
AS
	Begin
		Update Availability
			Set		Sunday = @NewSunday,
					Monday = @NewMonday,
					Tuesday = @NewTuesday,
					Wednesday = @NewWednesday,
					Thursday = @NewThursday,
					Friday = @NewFriday,
					Saturday = @NewSaturday
			Where 	WorkerID = @WorkerID
			And 	Sunday = @OldSunday
			And		Monday = @OldMonday
			And		Tuesday = @OldTuesday
			And		Wednesday = @OldWednesday
			And		Thursday = @OldThursday
			And		Friday = @OldFriday
			And		Saturday = @OldSaturday
		Return @@RowCount
	End
GO

print '' print '*** creating sp_add_worker_availability ***'
GO
Create Procedure [dbo].[sp_add_worker_availability]
	(
		@WorkerID	[int],
		@Sunday		[int],
		@Monday		[int],
		@Tuesday	[int],
		@Wednesday	[int],
		@Thursday	[int],
		@Friday		[int],
		@Saturday	[int]
	)
AS
	Begin
		Insert Into [dbo].[Availability]
			([WorkerID], [Sunday], [Monday], [Tuesday], [Wednesday], [Thursday], [Friday], [Saturday])
		Values
			(@WorkerID, @Sunday, @Monday, @Tuesday, @Wednesday, @Thursday, @Friday, @Saturday)
		Return @@RowCount
	End
GO

print '' print '*** creating sp_select_all_availability ***'
GO
Create Procedure [dbo].[sp_select_all_availability]
AS
	Begin
		Select 	WorkerID,
				Sunday,
				Monday,
				Tuesday,
				Wednesday,
				Thursday,
				Friday,
				Saturday
		From 	Availability
		Order By WorkerID ASC
	End
GO

print '' print '*** creating sp_select_availability_by_id ***'
GO
Create Procedure [dbo].[sp_select_availability_by_id]
	(
		@WorkerID [int]
	)
AS
	Begin
		Select 	Sunday,
				Monday,
				Tuesday,
				Wednesday,
				Thursday,
				Friday,
				Saturday
		From 	Availability
		Where 	WorkerID = @WorkerID
		Order By WorkerID ASC 
	End
GO

print '' print '*** creating sp_insert_new_user ***'
GO
Create Procedure [dbo].[sp_insert_new_user]
	(
	@Email			[nvarchar](100),
	@FirstName		[nvarchar](50),
	@LastName		[nvarchar](100),
	@Phone			[nvarchar](15),
	@StreetAddress	[nvarchar](256),
	@ZIPCode		[nvarchar](5)
	)
AS
	Begin
		Insert Into [dbo].[Worker]
			([Email], [FirstName], [LastName], [Phone], [StreetAddress], [ZIPCode])
		Values
			(@Email, @FirstName, @LastName, @Phone, @StreetAddress, @ZIPCode)
		Select Scope_Identity()
	End
GO

print '' print '*** creating sp_select_all_employees ***'
GO
Create Procedure [dbo].[sp_select_all_workers]
AS
	Begin
		Select 		WorkerID, Email, FirstName, LastName, Phone, StreetAddress, ZIPCode, Active, StartDate, EndDate
		From 		Worker
		Order By 	LastName ASC
	End
GO

print '' print '*** creating sp_select_workers_by_active ***'
GO
Create Procedure [dbo].[sp_select_workers_by_active]
	(
	@Active 		[bit]
	)
AS
	Begin
		Select 		WorkerID, Email, FirstName, LastName, Phone, StreetAddress, ZIPCode, Active, StartDate, EndDate
		From 		Worker
		Where		Active = @Active
		Order By 	LastName ASC
	End
GO

print '' print '*** creating sp_select_worker_by_id ***'
GO
Create Procedure [dbo].[sp_select_worker_by_id]
	(
	@WorkerID		[int]
	)
AS
	Begin
		Select 		WorkerID, Email, FirstName, LastName, Phone, StreetAddress, ZIPCode, Active, StartDate, EndDate
		From 		Worker
		Where 		WorkerID = @WorkerID
	End
GO

print '' print '*** creating sp_reset_passwordhash ***'
GO
Create Procedure [dbo].[sp_reset_passwordhash]
	(
	@Email				[nvarchar](100)
	)
AS
	Begin
		Update 	Worker
			Set 	PasswordHash = 
				'9C9064C59F1FFA2E174EE754D2979BE80DD30DB552EC03E7E327E9B1A4BD594E'
			Where 	Email = @Email
		Return 	@@RowCount 
	End
GO

print '' print '*** creating sp_safely_deactivate_worker ***'
GO
Create Procedure [dbo].[sp_safely_deactivate_worker]
	(
	@WorkerID				[int]
	)
AS
	Begin
		Declare @Admins int;
		Select @Admins = Count(RoleId)
		From WorkerRole
		Where RoleID = 'Park Staff Admin'
		And WorkerRole.WorkerID = @WorkerID;
		
		IF @Admins = 1
			Begin
				Return 0
			End
		ELSE
			Begin
				Update 	Worker
					Set 	Active = 0,
							EndDate = current_timestamp
					Where 	Worker.WorkerID = @WorkerID
				Return 	@@RowCount 
			End
	End
GO

print '' print '*** creating sp_reactivate_worker ***'
GO
Create Procedure [dbo].[sp_reactivate_worker]
	(
	@WorkerID				[int]
	)
AS
	Begin
		Update 	Worker
			Set 	Active = 1,
					StartDate = current_timestamp,
					EndDate = Null
			Where 	WorkerID = @WorkerID
		Return 	@@RowCount 
	End
GO

print '' print '*** creating sp_add_workerrole ***'
GO
Create Procedure [dbo].[sp_add_workerrole]
	(
		@WorkerID 	[int],
		@RoleID 		[nvarchar](25)
	)
AS
	Begin
		Insert Into WorkerRole
			([WorkerID], [RoleID])
		Values
			(@WorkerID, @RoleID)
		Return @@RowCount
	End
GO

print '' print '*** creating sp_select_all_roles ***'
GO
Create Procedure [dbo].[sp_select_all_roles]
AS
	Begin 
		Select 	RoleID
		From 	Role
		Order By RoleID ASC
	End
GO

print '' print '*** creating sp_safely_remove_workerrole ***'
GO
Create Procedure [dbo].[sp_safely_remove_workerrole]
	(
	@WorkerID 	[int],
	@RoleID 	[nvarchar](25)
	)
AS
	Begin
		Declare @Admins int;
		Select @Admins = Count(RoleID)
		From WorkerRole
		Where RoleID = 'Park Staff Admin';
		
		IF @RoleId = 'Park Staff Admin' AND @Admins = 1
			Begin
				Return 0
			End
		ELSE
			Begin
				Delete From WorkerRole
					Where WorkerID = @WorkerID
						And RoleID = @RoleID
				Return @@RowCount
			End
	End
GO
	
print '' print '*** creating sp_add_project_name ***'
GO
Create Procedure [dbo].[sp_add_project_name]
	(
		@ProjectName		[nvarchar](256),
		@ProjectDescription	[nvarchar](256)
	)
AS
	BEGIN
		Insert Into ProjectName
			([ProjectName], [ProjectDescription])
		Values
			(@ProjectName, @ProjectDescription)
		Return @@RowCount
	End
GO

print '' print '*** creating sp_select_all_project_names ***'
GO
Create Procedure sp_select_all_project_names
AS
	Begin
		Select ProjectName, ProjectDescription
		From ProjectName
		Order By ProjectName
	End
GO

print '' print '*** creating sp_add_tool ***'
GO
Create Procedure [dbo].[sp_add_tool]
	(
		@ToolDescription [nvarchar](256)
	)
AS
	BEGIN
		Insert Into Tool
			([ToolDescription])
		Values
			(@ToolDescription)
		Select Scope_Identity()
	End
GO

print '' print '*** creating sp_select_all_tools ***'
GO
Create Procedure [dbo].[sp_select_all_tools]
AS
	Begin
		Select Tool.ToolID, Tool.ToolDescription /*, ProjectTool.ProjectID */
		From Tool /*Join ProjectTool
			on Tool.ToolID = ProjectTool.ToolID */
		Order By Tool.ToolID ASC
	End
GO

print '' print '*** creating sp_select_tool_by_id ***'
GO
Create Procedure [dbo].[sp_select_tool_by_id]
	(
		@ToolID [int]
	)
AS
	Begin
		Select ToolDescription
		From Tool
		Where ToolID = @ToolID
		Order By ToolID ASC
	End
GO

print '' print '*** creating sp_select_tool_by_projectid ***'
GO
Create Procedure [dbo].[sp_select_tool_by_projectid]
	(
		@ProjectID [int]
	)
AS
	Begin
		Select ToolID
		From ProjectTool
		Where ProjectID = @ProjectID
		Order By ToolID ASC
	End
GO

print '' print '*** creating sp_finish_project ***'
GO
Create Procedure [dbo].[sp_finish_project]
	(
		@ProjectID	[int],
		@StartDate 	[datetime],
		@EndDate 	[datetime]
	)
AS
	BEGIN
		Update Project
			Set 	EndDate = @EndDate,
					HoursWorked = DATEDIFF(hh, @StartDate, @EndDate)
			Where 	ProjectID = @ProjectID
			And		StartDate = @StartDate
		Return @@RowCount
	End
GO

print '' print '*** creating sp_add_project ***'
GO
Create Procedure [dbo].[sp_add_project]
	(
		@WorkerID			[int],
		@Paid				[bit],
		@TaskListFilename	[nvarchar](256),
		@StartDate			[datetime],
		@Deadline			[datetime],
		@EndDate			[datetime],
		@HoursWorked		[int],
		@ProjectName		[nvarchar](256)
	)
AS
	Begin
		Declare @Scope_Identity int;
		Insert Into [dbo].[Project]
			([WorkerID], [Paid], [TaskListFilename], [StartDate], [Deadline], [EndDate], [HoursWorked])
		Values
			(@WorkerID, @Paid, @TaskListFilename, @StartDate, @Deadline, @EndDate, @HoursWorked)
		Set @Scope_Identity = (Select Scope_Identity())
		Insert Into [dbo].[ProjectDescription]
			([ProjectID], [ProjectName])
		Values
			(@Scope_Identity, @ProjectName)
		Return @Scope_Identity
	End
GO

print '' print '*** creating sp_update_project ***'
GO
Create Procedure [dbo].[sp_update_project]
	(
		@ProjectID				[int],
		@OldWorkerID			[int],
		@OldPaid				[bit],
		@OldTaskListFilename	[nvarchar](256),
		@OldStartDate			[datetime],
		@OldDeadline			[datetime],
		@OldEndDate				[datetime],
		@OldHoursWorked			[int],

		
		@NewWorkerID			[int],
		@NewPaid				[bit],
		@NewTaskListFilename	[nvarchar](256),
		@NewStartDate			[datetime],
		@NewDeadline			[datetime],
		@NewEndDate				[datetime],
		@NewHoursWorked			[int]
	)
AS
	Begin
		Update Project
			Set 	WorkerID = @NewWorkerID,
					Paid = @NewPaid,
					TaskListFilename = @NewTaskListFilename,
					StartDate = @NewStartDate,
					Deadline = @NewDeadline,
					EndDate = @NewEndDate, 
					HoursWorked = @NewHoursWorked
			Where 	ProjectID = @ProjectID
			And		WorkerID = @OldWorkerID
			And		Paid = @OldPaid
			And		TaskListFilename = @OldTaskListFilename
			And		StartDate = @OldStartDate
			And		Deadline = @OldDeadline
			And		EndDate = @OldEndDate
			And		EndDate = @OldEndDate
			And		HoursWorked = @OldHoursWorked
		Return @@RowCount		
	End
GO

print '' print '*** creating sp_select_all_projects ***'
GO
Create Procedure [dbo].[sp_select_all_projects]
AS
	Begin
		Select 	Project.ProjectID,
				Project.WorkerID,
				Project.Paid,
				Project.TaskListFilename,
				Project.StartDate,
				Project.Deadline,
				Project.EndDate,
				Project.HoursWorked,
				ProjectDescription.ProjectName,
				ProjectName.ProjectDescription
		From 	Project Inner Join ProjectDescription
				On Project.ProjectID = ProjectDescription.ProjectID
				Inner Join ProjectName 
				On ProjectName.ProjectName = ProjectDescription.ProjectName
		Order By Project.ProjectID ASC
	End
GO


print '' print '*** creating sp_select_project_by_id ***'
GO
Create Procedure [dbo].[sp_select_project_by_id]
	(
		@ProjectID [int]
	)
AS
	Begin
		Select 	Project.ProjectID,
				Project.WorkerID,
				Project.Paid,
				Project.TaskListFilename,
				Project.StartDate,
				Project.Deadline,
				Project.EndDate,
				Project.HoursWorked,
				ProjectDescription.ProjectName,
				ProjectName.ProjectDescription
		From 	Project Inner Join ProjectDescription
				On Project.ProjectID = ProjectDescription.ProjectID
				Inner Join ProjectName 
				On ProjectName.ProjectName = ProjectDescription.ProjectName
		Where 	Project.ProjectID = @ProjectID
		Order By Project.ProjectID ASC
	End
GO

print '' print '*** creating sp_select_project_by_worker_id ***'
GO
Create Procedure [dbo].[sp_select_project_by_worker_id]
	(
		@WorkerID [int]
	)
AS
	Begin
		Select 	Project.ProjectID,
				Project.WorkerID,
				Project.Paid,
				Project.TaskListFilename,
				Project.StartDate,
				Project.Deadline,
				Project.EndDate,
				Project.HoursWorked,
				ProjectDescription.ProjectName
				ProjectName.ProjectDescription
		From 	Project Inner Join ProjectDescription
				On Project.ProjectID = ProjectDescription.ProjectID
				Inner Join ProjectName 
				On ProjectName.ProjectName = ProjectDescription.ProjectName
		Where 	Project.WorkerID = @WorkerID
		Order By Project.ProjectID ASC
	End
GO

print '' print '*** creating sp_insert_tool_into_project ***'
GO
CREATE PROCEDURE [dbo].[sp_insert_tool_into_project]
	(
		@ToolID		[int],
		@ProjectID	[int]
	)
AS
	Begin
		Insert Into ProjectTool
				([ProjectID], [ToolID])
			Values
				(@ProjectID, @ToolID)
			Return @@RowCount
	End
GO

print '' print '*** creating sp_remove_tool_from_project ***'
GO
CREATE PROCEDURE [dbo].[sp_remove_tool_from_project]
	(
		@ProjectID	[int], 
		@ToolID		[int]
	)
AS
	Begin
		Delete From ProjectTool
		Where ProjectID = @ProjectID
		And ToolID = @ToolID
		Return @@RowCount
	End
GO

print '' print '*** creating sp_select_projectid_by_toolid ***'
GO
CREATE PROCEDURE [dbo].[sp_get_projectid_by_toolid]
	(
		@ToolID [int]
	)
AS
	Begin
		Select ProjectID
		From ProjectTool
		Where ToolID = @ToolID
	End
GO












