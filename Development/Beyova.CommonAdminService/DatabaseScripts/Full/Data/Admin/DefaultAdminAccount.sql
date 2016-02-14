SET IDENTITY_INSERT [dbo].[AdminPermission] ON 

GO
INSERT [dbo].[AdminPermission] ([RowId], [Key], [Name], [Identifier], [Description], [CreatedStamp], [LastUpdatedStamp], [CreatedBy], [LastUpdatedBy], [State]) 
VALUES (1, N'177a042e-e968-4901-8e87-aa8e1f282d07', N'Administration', N'Administration', N'People who has this permission can access admin management.', CAST(N'2016-02-14 05:25:42.803' AS DateTime), CAST(N'2016-02-14 05:25:42.803' AS DateTime), N'ee36cc51-0ed1-43cb-906a-eaa7be267a19', N'ee36cc51-0ed1-43cb-906a-eaa7be267a19', 4)
GO
INSERT [dbo].[AdminPermission] ([RowId], [Key], [Name], [Identifier], [Description], [CreatedStamp], [LastUpdatedStamp], [CreatedBy], [LastUpdatedBy], [State]) 
VALUES (2, N'1956351a-edac-49c0-a59e-728076ee2a91', N'Admin User Operation', N'CreateOrUpdateAdminUser', N'People who has permission to create or update user', CAST(N'2016-02-14 05:25:42.803' AS DateTime), CAST(N'2016-02-14 05:25:42.803' AS DateTime), N'ee36cc51-0ed1-43cb-906a-eaa7be267a19', N'ee36cc51-0ed1-43cb-906a-eaa7be267a19', 4)
GO
INSERT [dbo].[AdminPermission] ([RowId], [Key], [Name], [Identifier], [Description], [CreatedStamp], [LastUpdatedStamp], [CreatedBy], [LastUpdatedBy], [State]) 
VALUES (3, N'1524fc3c-f810-430b-b1d9-20d4f5dc6b70', N'Admin Permission Operation', N'CreateOrUpdateAdminPermission', N'People who has permission on role, permission, binding operation', CAST(N'2016-02-14 05:25:42.803' AS DateTime), CAST(N'2016-02-14 05:25:42.803' AS DateTime), N'ee36cc51-0ed1-43cb-906a-eaa7be267a19', N'ee36cc51-0ed1-43cb-906a-eaa7be267a19', 4)
GO
SET IDENTITY_INSERT [dbo].[AdminPermission] OFF
GO
SET IDENTITY_INSERT [dbo].[AdminRole] ON 

GO
INSERT [dbo].[AdminRole] ([RowId], [Key], [Name], [ParentKey], [Description], [CreatedStamp], [LastUpdatedStamp], [CreatedBy], [LastUpdatedBy], [State]) 
VALUES (1, N'82a81193-971e-44df-9482-3c389aac6fde', N'Administrator', NULL, N'Built-in administrator role', CAST(N'2016-02-13 05:20:21.703' AS DateTime), CAST(N'2016-02-13 05:20:21.703' AS DateTime), N'ee36cc51-0ed1-43cb-906a-eaa7be267a19', N'ee36cc51-0ed1-43cb-906a-eaa7be267a19', 4)
GO
SET IDENTITY_INSERT [dbo].[AdminRole] OFF
GO
SET IDENTITY_INSERT [dbo].[AdminRolePermissionBinding] ON 

GO
INSERT [dbo].[AdminRolePermissionBinding] ([RowId], [Key], [RoleKey], [PermissionKey], [CreatedStamp], [LastUpdatedStamp], [CreatedBy], [LastUpdatedBy], [State]) 
VALUES (1, N'eaa9fe76-dce5-4fc0-98ea-44d3190637a3', N'82a81193-971e-44df-9482-3c389aac6fde', N'177a042e-e968-4901-8e87-aa8e1f282d07', CAST(N'2016-02-14 05:25:42.803' AS DateTime), CAST(N'2016-02-14 05:25:42.803' AS DateTime), N'ee36cc51-0ed1-43cb-906a-eaa7be267a19', N'ee36cc51-0ed1-43cb-906a-eaa7be267a19', 4)
GO
INSERT [dbo].[AdminRolePermissionBinding] ([RowId], [Key], [RoleKey], [PermissionKey], [CreatedStamp], [LastUpdatedStamp], [CreatedBy], [LastUpdatedBy], [State]) 
VALUES (2, N'81e4b5f7-d84b-440d-bc5d-b53fee82285e', N'82a81193-971e-44df-9482-3c389aac6fde', N'1956351a-edac-49c0-a59e-728076ee2a91', CAST(N'2016-02-14 05:25:42.803' AS DateTime), CAST(N'2016-02-14 05:25:42.803' AS DateTime), N'ee36cc51-0ed1-43cb-906a-eaa7be267a19', N'ee36cc51-0ed1-43cb-906a-eaa7be267a19', 4)
GO
INSERT [dbo].[AdminRolePermissionBinding] ([RowId], [Key], [RoleKey], [PermissionKey], [CreatedStamp], [LastUpdatedStamp], [CreatedBy], [LastUpdatedBy], [State]) 
VALUES (3, N'1a47241d-8788-4973-bdce-d7bf94c4cbbf', N'82a81193-971e-44df-9482-3c389aac6fde', N'1524fc3c-f810-430b-b1d9-20d4f5dc6b70', CAST(N'2016-02-14 05:25:42.803' AS DateTime), CAST(N'2016-02-14 05:25:42.803' AS DateTime), N'ee36cc51-0ed1-43cb-906a-eaa7be267a19', N'ee36cc51-0ed1-43cb-906a-eaa7be267a19', 0)
GO
SET IDENTITY_INSERT [dbo].[AdminRolePermissionBinding] OFF
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
