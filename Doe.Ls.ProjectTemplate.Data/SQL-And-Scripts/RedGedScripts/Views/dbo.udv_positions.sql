
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
CREATE VIEW [dbo].[udv_positions]
AS
SELECT        TOP (100) PERCENT dbo.Position.PositionId, dbo.Position.PositionNumber AS [Position Number], dbo.Position.PositionTitle, dbo.Position.Description, 
                         dbo.Unit.UnitName AS Team, dbo.FunctionalArea.AreanName AS [Function Area], dbo.BusinessUnit.BUnitName AS [Business Unit], 
                         dbo.Directorate.DirectorateName AS Directorate, dbo.Executive.ExecutiveTitle AS Division, dbo.Position.PositionPath, dbo.Position.ReportToPositionId, 
                         dbo.Position.RolePositionDescriptionId, dbo.Position.UnitId, dbo.Position.PositionLevelId, dbo.Position.StatusId
FROM            dbo.BusinessUnit INNER JOIN
                         dbo.Position INNER JOIN
                         dbo.RolePositionDescription ON dbo.Position.RolePositionDescriptionId = dbo.RolePositionDescription.RolePositionDescId INNER JOIN
                         dbo.Unit ON dbo.Position.UnitId = dbo.Unit.UnitId ON dbo.BusinessUnit.BUnitId = dbo.Unit.BUnitId INNER JOIN
                         dbo.Executive INNER JOIN
                         dbo.Directorate ON dbo.Executive.ExecutiveCod = dbo.Directorate.ExecutiveCod ON dbo.BusinessUnit.DirectorateId = dbo.Directorate.DirectorateId INNER JOIN
                         dbo.FunctionalArea ON dbo.Unit.FunctionalAreaId = dbo.FunctionalArea.FuncationalAreaId AND 
                         dbo.Directorate.DirectorateId = dbo.FunctionalArea.DirectorateId LEFT OUTER JOIN
                         dbo.PositionInformation ON dbo.Position.PositionId = dbo.PositionInformation.PositionId
ORDER BY Division, [Business Unit], Team, [Position Number]
GO

EXEC sp_addextendedproperty N'MS_DiagramPane1', N'[0E232FF0-B466-11cf-A24F-00AA00A3EFFF, 1.00]
Begin DesignProperties = 
   Begin PaneConfigurations = 
      Begin PaneConfiguration = 0
         NumPanes = 4
         Configuration = "(H (1[50] 4[3] 2[3] 3) )"
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
         Begin Table = "BusinessUnit"
            Begin Extent = 
               Top = 117
               Left = 574
               Bottom = 341
               Right = 756
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "Position"
            Begin Extent = 
               Top = 6
               Left = 38
               Bottom = 415
               Right = 263
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "RolePositionDescription"
            Begin Extent = 
               Top = 104
               Left = 328
               Bottom = 270
               Right = 528
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "Unit"
            Begin Extent = 
               Top = 302
               Left = 306
               Bottom = 441
               Right = 488
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "Executive"
            Begin Extent = 
               Top = 6
               Left = 1143
               Bottom = 190
               Right = 1341
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "Directorate"
            Begin Extent = 
               Top = 233
               Left = 1374
               Bottom = 362
               Right = 1590
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "FunctionalArea"
            Begin Extent = 
               Top = 380
               Left = 578
               Bottom = 509
               Right = 763
            ', 'SCHEMA', N'dbo', 'VIEW', N'udv_positions', NULL, NULL
GO

EXEC sp_addextendedproperty N'MS_DiagramPane2', N'End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "PositionInformation"
            Begin Extent = 
               Top = 61
               Left = 845
               Bottom = 366
               Right = 1056
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
      Begin ColumnWidths = 10
         Width = 284
         Width = 1500
         Width = 1860
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 2280
         Width = 1500
         Width = 1500
         Width = 1500
      End
   End
   Begin CriteriaPane = 
      Begin ColumnWidths = 11
         Column = 2970
         Alias = 3945
         Table = 3720
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
      End
   End
End
', 'SCHEMA', N'dbo', 'VIEW', N'udv_positions', NULL, NULL
GO


DECLARE @xp int
SELECT @xp=2
EXEC sp_addextendedproperty N'MS_DiagramPaneCount', @xp, 'SCHEMA', N'dbo', 'VIEW', N'udv_positions', NULL, NULL
GO
