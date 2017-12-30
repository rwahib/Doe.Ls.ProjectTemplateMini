SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
CREATE VIEW [dbo].[udv_PositionsWithChildren]
AS
SELECT        TOP (100) PERCENT PARENT.PositionId AS [Parent PositionId], PARENT.PositionNumber AS [Parent PositionNumber], PARENT.PositionTitle AS [Parent PositionTitle], 
                         PARENT.PositionPath AS [Parent PositionPath], PDirectorate.ExecutiveCod AS [Parent DIV], PARENT.StatusId AS [Parent StatusId], 
                         PStatusValue.StatusName AS [Parent Status], CHILD.PositionId AS [Child PositionId], CHILD.ReportToPositionId AS [Child ReportToPositionId], 
                         CHILD.PositionNumber AS [Child PositionNumber], CHILD.PositionTitle AS [Child PositionTitle], CHILD.PositionPath AS [Child PositionPath], 
                         dbo.Directorate.ExecutiveCod AS [Child DIV], CHILD.StatusId AS [Child StatusId], dbo.StatusValue.StatusName AS [Child Status]
FROM            dbo.Position AS CHILD INNER JOIN
                         dbo.Unit ON CHILD.UnitId = dbo.Unit.UnitId INNER JOIN
                         dbo.BusinessUnit ON dbo.Unit.BUnitId = dbo.BusinessUnit.BUnitId INNER JOIN
                         dbo.Directorate ON dbo.BusinessUnit.DirectorateId = dbo.Directorate.DirectorateId INNER JOIN
                         dbo.StatusValue ON CHILD.StatusId = dbo.StatusValue.StatusId RIGHT OUTER JOIN
                         dbo.StatusValue AS PStatusValue INNER JOIN
                         dbo.Position AS PARENT INNER JOIN
                         dbo.Unit AS PUnit ON PARENT.UnitId = PUnit.UnitId INNER JOIN
                         dbo.BusinessUnit AS PBusinessUnit ON PUnit.BUnitId = PBusinessUnit.BUnitId INNER JOIN
                         dbo.Directorate AS PDirectorate ON PBusinessUnit.DirectorateId = PDirectorate.DirectorateId ON PStatusValue.StatusId = PARENT.StatusId ON 
                         CHILD.ReportToPositionId = PARENT.PositionId
WHERE        (CHILD.PositionId IS NOT NULL) AND (PARENT.StatusId <> - 1) AND (CHILD.StatusId <> - 1)
ORDER BY LEN(PARENT.PositionPath)
GO
EXEC sp_addextendedproperty N'MS_DiagramPane1', N'[0E232FF0-B466-11cf-A24F-00AA00A3EFFF, 1.00]
Begin DesignProperties = 
   Begin PaneConfigurations = 
      Begin PaneConfiguration = 0
         NumPanes = 4
         Configuration = "(H (1[30] 4[40] 2[11] 3) )"
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
         Begin Table = "CHILD"
            Begin Extent = 
               Top = 6
               Left = 38
               Bottom = 391
               Right = 263
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "Unit"
            Begin Extent = 
               Top = 6
               Left = 301
               Bottom = 135
               Right = 483
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "BusinessUnit"
            Begin Extent = 
               Top = 6
               Left = 521
               Bottom = 135
               Right = 703
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "Directorate"
            Begin Extent = 
               Top = 6
               Left = 741
               Bottom = 135
               Right = 957
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "PARENT"
            Begin Extent = 
               Top = 6
               Left = 995
               Bottom = 368
               Right = 1220
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "PUnit"
            Begin Extent = 
               Top = 6
               Left = 1258
               Bottom = 135
               Right = 1440
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "PBusinessUnit"
            Begin Extent = 
               Top = 231
               Left = 1026
               Bottom = 360
               Right = 1208
            End
            DisplayFlags ', 'SCHEMA', N'dbo', 'VIEW', N'udv_PositionsWithChildren', NULL, NULL
GO
EXEC sp_addextendedproperty N'MS_DiagramPane2', N'= 280
            TopColumn = 0
         End
         Begin Table = "PDirectorate"
            Begin Extent = 
               Top = 263
               Left = 1151
               Bottom = 562
               Right = 1367
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "PStatusValue"
            Begin Extent = 
               Top = 354
               Left = 946
               Bottom = 466
               Right = 1127
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "StatusValue"
            Begin Extent = 
               Top = 323
               Left = 427
               Bottom = 435
               Right = 608
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
      Begin ColumnWidths = 15
         Width = 284
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1695
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
         Column = 3795
         Alias = 4515
         Table = 1845
         Output = 1890
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
', 'SCHEMA', N'dbo', 'VIEW', N'udv_PositionsWithChildren', NULL, NULL
GO
DECLARE @xp int
SELECT @xp=2
EXEC sp_addextendedproperty N'MS_DiagramPaneCount', @xp, 'SCHEMA', N'dbo', 'VIEW', N'udv_PositionsWithChildren', NULL, NULL
GO
