SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
CREATE VIEW [dbo].[udv_userRoles]
AS
SELECT        dbo.SysUser.UserId, dbo.SysUser.Email, dbo.SysUser.FirstName, dbo.SysUser.LastName, dbo.SysUser.PrimaryPhone, dbo.SysUser.Note, dbo.SysUser.Active, 
                         dbo.SysRole.RoleId, dbo.SysRole.RoleTitle, dbo.SysUserRole.StructureId, dbo.SysUserRole.OrgLevelId, dbo.OrgLevel.OrgLevelName, 
                         dbo.SysUserRole.ActiveFrom, dbo.SysUserRole.ActiveTo, dbo.SysRole.RoleApiName, dbo.OrgLevel.OrgLevelTitle
FROM            dbo.SysUser INNER JOIN
                         dbo.SysUserRole ON dbo.SysUser.UserId = dbo.SysUserRole.UserId INNER JOIN
                         dbo.SysRole ON dbo.SysUserRole.RoleId = dbo.SysRole.RoleId INNER JOIN
                         dbo.OrgLevel ON dbo.SysUserRole.OrgLevelId = dbo.OrgLevel.OrgLevelId
GO
EXEC sp_addextendedproperty N'MS_DiagramPane1', N'[0E232FF0-B466-11cf-A24F-00AA00A3EFFF, 1.00]
Begin DesignProperties = 
   Begin PaneConfigurations = 
      Begin PaneConfiguration = 0
         NumPanes = 4
         Configuration = "(H (1[41] 4[35] 2[5] 3) )"
      End
      Begin PaneConfiguration = 1
         NumPanes = 3
         Configuration = "(H (1 [50] 4 [25] 3))"
      End
      Begin PaneConfiguration = 2
         NumPanes = 3
         Configuration = "(H (1 [50] 2 [25] 3))"
      End
      Begin PaneConfiguration = 3
         NumPanes = 3
         Configuration = "(H (4 [30] 2 [40] 3))"
      End
      Begin PaneConfiguration = 4
         NumPanes = 2
         Configuration = "(H (1 [56] 3))"
      End
      Begin PaneConfiguration = 5
         NumPanes = 2
         Configuration = "(H (2 [66] 3))"
      End
      Begin PaneConfiguration = 6
         NumPanes = 2
         Configuration = "(H (4 [50] 3))"
      End
      Begin PaneConfiguration = 7
         NumPanes = 1
         Configuration = "(V (3))"
      End
      Begin PaneConfiguration = 8
         NumPanes = 3
         Configuration = "(H (1[56] 4[18] 2) )"
      End
      Begin PaneConfiguration = 9
         NumPanes = 2
         Configuration = "(H (1 [75] 4))"
      End
      Begin PaneConfiguration = 10
         NumPanes = 2
         Configuration = "(H (1[66] 2) )"
      End
      Begin PaneConfiguration = 11
         NumPanes = 2
         Configuration = "(H (4 [60] 2))"
      End
      Begin PaneConfiguration = 12
         NumPanes = 1
         Configuration = "(H (1) )"
      End
      Begin PaneConfiguration = 13
         NumPanes = 1
         Configuration = "(V (4))"
      End
      Begin PaneConfiguration = 14
         NumPanes = 1
         Configuration = "(V (2))"
      End
      ActivePaneConfig = 0
   End
   Begin DiagramPane = 
      Begin Origin = 
         Top = 0
         Left = 0
      End
      Begin Tables = 
         Begin Table = "SysUser"
            Begin Extent = 
               Top = 6
               Left = 38
               Bottom = 135
               Right = 220
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "SysUserRole"
            Begin Extent = 
               Top = 6
               Left = 258
               Bottom = 135
               Right = 440
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "SysRole"
            Begin Extent = 
               Top = 138
               Left = 38
               Bottom = 342
               Right = 220
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "OrgLevel"
            Begin Extent = 
               Top = 162
               Left = 716
               Bottom = 291
               Right = 886
            End
            DisplayFlags = 280
            TopColumn = 0
         End
      End
   End
   Begin SQLPane = 
   End
   Begin DataPane = 
      Begin ParameterDefaults = ""
      End
      Begin ColumnWidths = 9
         Width = 284
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
      End
   End
   Begin CriteriaPane = 
      Begin ColumnWidths = 11
         Column = 4920
         Alias = 900
         Table = 1170
         Output = 720
         Append = 1400
         NewValue = 1170
         SortType = 1350
         SortOrder = 1410
         GroupBy = 1350
         Filter = 1350
         Or = 1350
         Or = 1350
         Or = 1350
      End', 'SCHEMA', N'dbo', 'VIEW', N'udv_userRoles', NULL, NULL
GO
EXEC sp_addextendedproperty N'MS_DiagramPane2', N'
   End
End
', 'SCHEMA', N'dbo', 'VIEW', N'udv_userRoles', NULL, NULL
GO
DECLARE @xp int
SELECT @xp=2
EXEC sp_addextendedproperty N'MS_DiagramPaneCount', @xp, 'SCHEMA', N'dbo', 'VIEW', N'udv_userRoles', NULL, NULL
GO
