Imports Microsoft.VisualBasic
Imports DevExpress.XtraGrid
Imports DevExpress.XtraGrid.Views.Grid
Imports DevExpress.XtraGrid.Registrator
Imports DevExpress.XtraGrid.Views.Base
Imports System.ComponentModel
Imports DevExpress.XtraGrid.Columns
Imports DevExpress.Data

Namespace DXSample
	Public Class MyGridControl
		Inherits GridControl
		Public Sub New()
			MyBase.New()
		End Sub

		Protected Overrides Sub RegisterAvailableViewsCore(ByVal collection As InfoCollection)
			MyBase.RegisterAvailableViewsCore(collection)
			collection.Add(New MyGridViewInfoRegistrator())
		End Sub
	End Class

	Public Class MyGridView
		Inherits GridView
		Public Sub New()
			MyBase.New()
		End Sub
		Public Sub New(ByVal grid As GridControl)
			MyBase.New(grid)
		End Sub

		Friend Const MyGridViewName As String = "MyGridView"
		Protected Overrides ReadOnly Property ViewName() As String
			Get
				Return MyGridViewName
			End Get
		End Property

		Private fsFieldName As String
		Public Property SortFieldName() As String
			Get
				Return fsFieldName
			End Get
			Set(ByVal value As String)
				fsFieldName = value
			End Set
		End Property

		Protected Overrides Sub OnColumnSortInfoCollectionChanged(ByVal e As CollectionChangeEventArgs)
			MyBase.OnColumnSortInfoCollectionChanged(e)
			Dim col As GridColumn = Columns(fsFieldName)
			If col Is Nothing Then
				Return
			End If
			If col.SortIndex <> 0 Then
				SortInfo.BeginUpdate()
				Try
					If Columns(fsFieldName).SortIndex > 0 Then
						SortInfo.RemoveAt(Columns(fsFieldName).SortIndex)
					End If
					SortInfo.Insert(0, New GridColumnSortInfo(Columns(fsFieldName), ColumnSortOrder.Ascending))
				Finally
					SortInfo.EndUpdate()
				End Try
			End If

		End Sub

		Public Overrides Sub Assign(ByVal v As BaseView, ByVal copyEvents As Boolean)
			BeginUpdate()
			Try
				MyBase.Assign(v, copyEvents)
				Dim source As MyGridView = TryCast(v, MyGridView)
				If source Is Nothing Then
					Return
				End If
				SortFieldName = source.SortFieldName
			Finally
				EndUpdate()
			End Try
		End Sub
	End Class

	Public Class MyGridViewInfoRegistrator
		Inherits GridInfoRegistrator
		Public Sub New()
			MyBase.New()
		End Sub

		Public Overrides ReadOnly Property ViewName() As String
			Get
				Return MyGridView.MyGridViewName
			End Get
		End Property

		Public Overrides Function CreateView(ByVal grid As GridControl) As BaseView
			Return New MyGridView(grid)
		End Function
	End Class
End Namespace