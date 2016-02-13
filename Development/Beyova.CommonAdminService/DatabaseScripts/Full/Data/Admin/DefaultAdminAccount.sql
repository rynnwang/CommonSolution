
SET IDENTITY_INSERT [dbo].[AdminRole] ON 

GO
INSERT [dbo].[AdminRole] ([RowId], [Key], [Name], [ParentKey], [Description], [CreatedStamp], [LastUpdatedStamp], [CreatedBy], [LastUpdatedBy], [State]) 
VALUES (1, N'82a81193-971e-44df-9482-3c389aac6fde', N'Administrator', NULL, N'Built-in administrator role', CAST(N'2016-02-13 05:20:21.703' AS DateTime), CAST(N'2016-02-13 05:20:21.703' AS DateTime), N'ee36cc51-0ed1-43cb-906a-eaa7be267a19', N'ee36cc51-0ed1-43cb-906a-eaa7be267a19', 0)
GO
SET IDENTITY_INSERT [dbo].[AdminRole] OFF
GO
SET IDENTITY_INSERT [dbo].[AdminUserInfo] ON 

GO
INSERT [dbo].[AdminUserInfo] ([RowId], [Key], [LoginName], [Password], [Name], [Email], [ThirdPartyId], [PasswordResetToken], [PasswordResetExpiredStamp], [CreatedStamp], [LastUpdatedStamp], [CreatedBy], [LastUpdatedBy], [State]) 
VALUES (1, N'ee36cc51-0ed1-43cb-906a-eaa7be267a19', N'administrator', N'D033E22AE348AEB5660FC2140AEC35850C4DA997', N'Administrator', NULL, NULL, NULL, NULL, CAST(N'2016-02-13 05:17:36.543' AS DateTime), CAST(N'2016-02-13 05:17:36.543' AS DateTime), N'ee36cc51-0ed1-43cb-906a-eaa7be267a19', N'ee36cc51-0ed1-43cb-906a-eaa7be267a19', 0)
GO
SET IDENTITY_INSERT [dbo].[AdminUserInfo] OFF
GO
SET IDENTITY_INSERT [dbo].[AdminUserRoleBinding] ON 

GO
INSERT [dbo].[AdminUserRoleBinding] ([RowId], [Key], [UserKey], [RoleKey], [CreatedStamp], [LastUpdatedStamp], [CreatedBy], [LastUpdatedBy], [State]) 
VALUES (1, N'b3c5bf8d-32f5-4084-b9f9-54fe869c18c8', N'ee36cc51-0ed1-43cb-906a-eaa7be267a19', N'82a81193-971e-44df-9482-3c389aac6fde', CAST(N'2016-02-13 05:21:22.853' AS DateTime), CAST(N'2016-02-13 05:21:22.853' AS DateTime), N'ee36cc51-0ed1-43cb-906a-eaa7be267a19', N'ee36cc51-0ed1-43cb-906a-eaa7be267a19', 0)
GO
SET IDENTITY_INSERT [dbo].[AdminUserRoleBinding] OFF
GO

